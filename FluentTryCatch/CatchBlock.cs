using FluentTryCatch.Interfaces;
using System;

namespace FluentTryCatch;

internal sealed class CatchBlock : IWillCatch, IWillThrow, IWillThrowComplete
{
    private object[] _arguments = [];

    internal CatchBlock(IWillTry tryData, Type catchedType, Action? actionToExecute)
    {
        Parent = tryData;
        CatchedType = catchedType;
        ActionToExecute = actionToExecute;
        ThrowOptions = new ThrowOptions(tryData);
    }

    public IWillTry Parent { get; private set; }

    public Type CatchedType { get; private set; }

    public Type? ThrowableType { get; private set; }
    
    public Exception? InnerException { get; private set; }
    
    public bool HasArguments { get; private set; }
    
    public object[] Arguments
    {
        get => _arguments;
        private set
        {
            _arguments = value;
            HasArguments = value.Length > 0;
        }
    }
    
    public Action? ActionToExecute { get; private set; }
    
    public bool PreserveCatchedException { get; private set; }
    
    public ThrowOptions ThrowOptions { get; private set; }

    internal Exception? BuildException(Exception? exception = null)
    {
        if (HasArguments)
        {
            InnerException = (Exception)Activator.CreateInstance(ThrowableType, Arguments);
        }
        else
        {
            InnerException = ThrowOptions.BuildException(ThrowableType, exception);
        }

        return InnerException;
    }

    public IWillThrow Throw<TException>() where TException : Exception
    {
        ThrowableType = typeof(TException);
        return this;
    }

    public IWillThrowComplete Rethrow()
    {
        PreserveCatchedException = true;
        return this;
    }

    public IWillThrowComplete WithArguments(params object[] args)
    {
        Arguments = args;
        return this;
    }

    public IWillThrowWithMessage WithMessage(string message)
    {
        ThrowOptions.Message = message;
        return ThrowOptions;
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