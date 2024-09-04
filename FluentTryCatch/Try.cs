using FluentTryCatch.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentTryCatch;

public class Try<TResult> : TryBase, IWillTry<TResult>, IWillFinally<TResult>
{
	private readonly Func<TResult>? _tryFunc;
	private Func<TResult?>? _builtFunc;

	private readonly Dictionary<Type, CatchBlock<TResult>> _catchBlocks = [];

	internal Try(Func<TResult> func)
	{
		_tryFunc = func ?? throw new ArgumentNullException(nameof(func));
	}

	public static IWillTry<TResult> To(Func<TResult> func) => new Try<TResult>(func);

	public IWillCatchNoThrow<TResult> Catch<TException>(Func<TResult> catchFunc) where TException : Exception
	{
		if (catchFunc == null)
		{
			throw new ArgumentNullException(nameof(catchFunc));
		}

		return InternalCatchFunc<TException>(catchFunc);
	}

	internal IWillCatchNoThrow<TResult> InternalCatchFunc<TException>(Func<TResult>? catchFunc) where TException : Exception
	{
		if (_catchBlocks.ContainsKey(typeof(TException)))
		{
			return _catchBlocks[typeof(TException)];
		}

		var data = new CatchBlock<TResult>(this, typeof(TException), catchFunc);

		_catchBlocks.Add(typeof(TException), data);

		return data;
	}

	public IWillCatch<TResult> Catch<TException>()
		where TException : Exception
	{
		return InternalCatchAction<TException>(null);
	}

	public IWillCatch<TResult> Catch<TException>(Action catchAction)
		where TException : Exception
	{
		if (catchAction == null)
		{
			throw new ArgumentNullException(nameof(catchAction));
		}

		return InternalCatchAction<TException>(catchAction);
	}

	internal IWillCatch<TResult> InternalCatchAction<TException>(Action? catchAction)
	{
		if (_catchBlocks.ContainsKey(typeof(TException)))
		{
			return _catchBlocks[typeof(TException)];
		}

		var data = new CatchBlock<TResult>(this, typeof(TException), catchAction);

		_catchBlocks.Add(typeof(TException), data);

		return data;
	}

	public IWillFinally<TResult> Finally(Action finalAction)
	{
		_finalAction = finalAction;
		return this;
	}

	protected TResult? InternalRun()
	{
		try
		{
			return (TResult?)_tryFunc?.DynamicInvoke();
		}
		catch (Exception ex)
		{
			var exToBeTreated = ex;
			if (!_catchBlocks.ContainsKey(ex.GetType()))
			{
				// This exception is thrown by DynamicInvoke() and contains
				// the actual thrown exception in ex.InnerException property.
				if (ex.GetType() == typeof(TargetInvocationException)
					&& ex.InnerException is not null)
				{
					if (!_catchBlocks.ContainsKey(ex.InnerException.GetType()))
					{
						throw;
					}
					else
					{
						exToBeTreated = ex.InnerException;
					}
				}
				else
				{
					throw;
				}
			}

			var block = _catchBlocks[exToBeTreated.GetType()];
			if (block.ActionToExecute is null)
			{
				return (TResult?)block.FuncToExecute?.DynamicInvoke();
			}

			block.ActionToExecute?.Invoke();

			if (block.PreserveCatchedException == true)
			{
				throw;
			}

			var exception = block.BuildException(exToBeTreated);
			if (exception != null)
			{
				throw exception;
			}

			return default;
		}
		finally
		{
			_finalAction?.Invoke();
		}
	}

	public Func<TResult?> Build()
	{
		_builtFunc = InternalRun;
		return _builtFunc;
	}

	public TResult? Run()
	{
		Build();
		return (TResult?)_builtFunc?.DynamicInvoke();
	}
}

public class Try : TryBase, IWillTry, IWillFinally
{
	protected Action? _builtAction;

	protected readonly Action? _action;
	private readonly Dictionary<Type, CatchBlock> _catchBlocks = [];

	protected Try() { }
	private Try(Action action)
	{
		_action = action ?? throw new ArgumentNullException(nameof(action));
	}

	public static IWillTry To(Action action) => new Try(action);

	public static IWillTry<TResult> To<TResult>(Func<TResult> func) => new Try<TResult>(func);

	public IWillCatch Catch<TException>() where TException : Exception
	{
		return InternalCatchAction<TException>(null);
	}

	public IWillCatch Catch<TException>(Action catchAction) where TException : Exception
	{
		if (catchAction == null)
		{
			throw new ArgumentNullException(nameof(catchAction));
		}

		return InternalCatchAction<TException>(catchAction);
	}

	internal IWillCatch InternalCatchAction<TException>(Action? catchAction) where TException : Exception
	{
		if (_catchBlocks.ContainsKey(typeof(TException)))
		{
			return _catchBlocks[typeof(TException)];
		}

		var data = new CatchBlock(this, typeof(TException), catchAction);

		_catchBlocks.Add(typeof(TException), data);

		return data;
	}

	public IWillFinally Finally(Action action)
	{
		_finalAction = action;
		return this;
	}

	protected void InternalRun()
	{
		try
		{
			_action?.Invoke();
		}
		catch (Exception ex)
		{
			if (!_catchBlocks.ContainsKey(ex.GetType()))
			{
				throw;
			}

			var block = _catchBlocks[ex.GetType()];
			block.ActionToExecute?.Invoke();

			if (block.PreserveCatchedException == true)
			{
				throw;
			}

			var exception = block.BuildException(ex);
			if (exception != null)
			{
				throw exception;
			}
		}
		finally
		{
			_finalAction?.Invoke();
		}
	}

	public void Run()
	{
		Build();
		_builtAction?.Invoke();
	}

	public Action Build()
	{
		_builtAction = InternalRun;
		return _builtAction;
	}
}