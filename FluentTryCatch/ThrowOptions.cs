using FluentTryCatch.Interfaces;
using System;
using System.Linq;

namespace FluentTryCatch;

internal abstract class ThrowOptionsBase<TParent>
	where TParent : IWillTryMarker
{
	private string _message = string.Empty;

	internal ThrowOptionsBase(TParent parent)
	{
		Parent = parent ?? throw new ArgumentNullException(nameof(parent));
	}

	protected TParent Parent { get; }

	public bool IncludesInnerException { get; internal set; }

	public bool HasMessage { get; private set; }

	public string Message
	{
		get => _message;
		internal set
		{
			_message = value;
			HasMessage = !string.IsNullOrEmpty(value);
		}
	}

	internal Exception? BuildException(Type? exceptionType, Exception? inner = null)
	{
		// won't throw any exception, just catch it 
		if (exceptionType is null)
		{
			return null;
		}

		if (HasMessage && IncludesInnerException)
		{
			return (Exception)Activator.CreateInstance(exceptionType, Message, inner);
		}
		else if (HasMessage)
		{
			var constructorInfo = exceptionType.GetConstructor([typeof(string)]);

			if (constructorInfo.GetParameters().Any(x => x.Name == "message"))
			{
				return (Exception)constructorInfo.Invoke([Message]);
			}

			constructorInfo = exceptionType.GetConstructor([typeof(string), typeof(Exception)]);
			if (constructorInfo is not null)
			{
				return (Exception)constructorInfo.Invoke([Message, null]);
			}

			return (Exception)Activator.CreateInstance(exceptionType, (string?)Message, null);
		}
		else
		{
			return (Exception)Activator.CreateInstance(exceptionType);
		}
	}
}

internal sealed class ThrowOptions<TResult> : ThrowOptionsBase<IWillTry<TResult>>, IWillThrowWithMessage<TResult>
{
	public ThrowOptions(IWillTry<TResult> parent) : base(parent) { }

	public IWillThrowComplete<TResult> WithInnerException(bool includeInnerException = false)
	{
		IncludesInnerException = includeInnerException;
		return this;
	}

	public IWillTry<TResult> And()
	{
		return Parent;
	}

	public IWillFinally<TResult> Finally(Action finalAction)
	{
		return Parent.Finally(finalAction);
	}

	public Func<TResult?> Build()
	{
		return ((IWillFinally<TResult>)Parent).Build();
	}

	public TResult? Run()
	{
		return ((IWillFinally<TResult>)Parent).Run();
	}
}

internal sealed class ThrowOptions : ThrowOptionsBase<IWillTry>, IWillThrowWithMessage
{
	public ThrowOptions(IWillTry parent) : base(parent) { }

	public IWillThrowComplete WithInnerException(bool includeInnerException = false)
	{
		IncludesInnerException = includeInnerException;
		return this;
	}

	public IWillTry And()
	{
		return Parent;
	}

	public IWillFinally Finally(Action finalAction)
	{
		return Parent.Finally(finalAction);
	}

	public Action Build()
	{
		return ((IWillFinally)Parent).Build();
	}

	public void Run()
	{
		((IWillFinally)Parent).Run();
	}
}