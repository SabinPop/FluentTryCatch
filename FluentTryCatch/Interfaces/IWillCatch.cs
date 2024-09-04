using System;

namespace FluentTryCatch.Interfaces;

public interface IWillCatch<TResult> : IWillCatchNoThrow<TResult>
{
	IWillThrow<TResult> Throw<TException>() where TException : Exception;

	IWillThrowComplete<TResult> Rethrow();
}

public interface IWillCatchNoThrow<TResult> : IWillFinally<TResult>
{
	IWillFinally<TResult> Finally(Action finalAction);
}

public interface IWillCatch : IWillFinally
{
	Type CatchedType { get; }

	IWillThrow Throw<TException>() where TException : Exception;

	IWillThrowComplete Rethrow();

	IWillFinally Finally(Action finalAction);
}