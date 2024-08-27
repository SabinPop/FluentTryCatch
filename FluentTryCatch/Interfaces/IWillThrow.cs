using System;

namespace FluentTryCatch.Interfaces;

public interface IWillThrow : IWillThrowWithArguments, IWillThrowComplete
{
    Type? ThrowableType { get; }

    IWillThrowWithMessage WithMessage(string message);
}

public interface IWillThrow<TResult> : IWillThrowWithArguments<TResult>, IWillThrowComplete<TResult>
{
    Type? ThrowableType { get; }

    IWillThrowWithMessage<TResult> WithMessage(string message);
}