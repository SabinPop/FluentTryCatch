using System;

namespace FluentTryCatch.Interfaces;

public interface IWillTry
{
    IWillCatch Catch<TException>() where TException : Exception;

    IWillCatch Catch<TException>(Action catchAction) where TException : Exception;

    IWillFinally Finally(Action action);
}