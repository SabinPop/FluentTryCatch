using System;

namespace FluentTryCatch.Interfaces;

public interface IWillCatch
{
    Type CatchedType { get; }

    IWillThrow Throw<TException>() where TException : Exception;

    IWillThrowComplete Rethrow();

    IWillFinally Finally(Action finalAction);
}