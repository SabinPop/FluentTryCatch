using System;

namespace FluentTryCatch.Interfaces;

public interface IWillThrow : IWillThrowWithArguments, IWillThrowComplete
{
    Type? ThrowableType { get; }

    IWillThrowWithMessage WithMessage(string message);
}