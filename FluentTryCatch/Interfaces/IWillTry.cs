using System;

namespace FluentTryCatch.Interfaces;

public interface IWillTry<TResult> : IWillTryMarker
{
    // Maybe add overload with default value to be returned in catch block
    IWillCatch<TResult> Catch<TException>() where TException : Exception;

    IWillCatch<TResult> Catch<TException>(Action catchAction) where TException : Exception;

    IWillCatchNoThrow<TResult> Catch<TException>(Func<TResult> catchFunc) where TException : Exception;

    IWillFinally<TResult> Finally(Action finalAction);
}

public interface IWillTry : IWillTryMarker
{
    IWillCatch Catch<TException>() where TException : Exception;

    IWillCatch Catch<TException>(Action catchAction) where TException : Exception;

    IWillFinally Finally(Action finalAction);
}

public interface IWillTryMarker { }