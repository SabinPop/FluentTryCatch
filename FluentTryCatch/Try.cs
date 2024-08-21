using FluentTryCatch.Interfaces;
using System;
using System.Collections.Generic;

namespace FluentTryCatch;

public sealed class Try : IWillTry, IWillFinally
{
    private Action? _finalAction;
    private Action? _builtAction;

    private readonly Action _action;
    private readonly Dictionary<Type, CatchBlock> _catchBlocks = [];

    private Try(Action action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public static IWillTry To(Action action) => new Try(action);

    public IWillCatch Catch<TException>() where TException : Exception
    {
        return InternalCatch<TException>(null);
    }

    public IWillCatch Catch<TException>(Action catchAction) where TException : Exception
    {
        if (catchAction == null)
        {
            throw new ArgumentNullException(nameof(catchAction));
        }

        return InternalCatch<TException>(catchAction);
    }

    internal IWillCatch InternalCatch<TException>(Action? catchAction) where TException : Exception
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

    private void InternalRun()
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
            if (block.PreserveCatchedException == true)
            {
                throw;
            }

            block.ActionToExecute?.Invoke();

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

    public Action Build()
    {
        _builtAction = InternalRun;
        return _builtAction;
    }

    public void Run()
    {
        Build();
        _builtAction?.Invoke();
    }
}