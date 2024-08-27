using FluentTryCatch.Interfaces;
using System;

namespace FluentTryCatch;

internal class CatchBlock<TResult> 
    : CatchBlockBase<IWillTry<TResult>, ThrowOptions<TResult>>, 
    IWillCatch<TResult>,
    IWillThrow<TResult>,
    IWillThrowComplete<TResult>
{
    internal CatchBlock(IWillTry<TResult> tryData, Type catchedType, Func<TResult>? funcToExecute)
        : base(tryData, catchedType, null)
    {
        FuncToExecute = funcToExecute;
    }

    internal CatchBlock(IWillTry<TResult> tryData, Type catchedType, Action? actionToExecute)
        : base(tryData, catchedType, actionToExecute) { }

    public Func<TResult>? FuncToExecute { get; private set; }

    public IWillThrow<TResult> Throw<TException>() where TException : Exception
    {
        ThrowableType = typeof(TException);
        return this;
    }

    public IWillThrowComplete<TResult> Rethrow()
    {
        PreserveCatchedException = true;
        return this;
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

    public IWillThrowWithMessage<TResult> WithMessage(string message)
    {
        ThrowOptions.Message = message;
        return ThrowOptions;
    }

    public IWillThrowComplete<TResult> WithArguments(params object[] args)
    {
        Arguments = args;
        return this;
    }

    public IWillTry<TResult> And()
    {
        return Parent;
    }
}

internal class CatchBlock
    : CatchBlockBase<IWillTry, ThrowOptions>, 
    IWillCatch, 
    IWillThrow, 
    IWillThrowComplete
{
    public CatchBlock(IWillTry parent, Type catchedType, Action? actionToExecute)
        : base(parent, catchedType, actionToExecute) { }

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

internal abstract class CatchBlockBase<TParent, TOptions>
    where TParent : IWillTryMarker
    where TOptions : ThrowOptionsBase<TParent>
{
    private object[] _arguments = [];

    internal CatchBlockBase(TParent parent, Type catchedType, Action? actionToExecute)
    {
        Parent = parent;
        CatchedType = catchedType;
        ActionToExecute = actionToExecute;
        ThrowOptions = (TOptions?)Activator.CreateInstance(typeof(TOptions), parent)!;
    }

    public TParent Parent { get; }

    public Type CatchedType { get; protected set; }

    public Type? ThrowableType { get; protected set; }

    public Exception? InnerException { get; private set; }

    public bool HasArguments { get; private set; }

    public object[] Arguments
    {
        get => _arguments;
        protected set
        {
            _arguments = value;
            HasArguments = value.Length > 0;
        }
    }

    public Action? ActionToExecute { get; private set; }

    public bool PreserveCatchedException { get; protected set; }

    public TOptions ThrowOptions { get; protected set; }

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
}