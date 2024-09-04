using System;

namespace FluentTryCatch.Interfaces;

public interface IWillThrowComplete<TResult>
{
	IWillTry<TResult> And();

	IWillFinally<TResult> Finally(Action finalAction);

	Func<TResult?> Build();

	TResult? Run();
}

public interface IWillThrowComplete
{
	IWillTry And();

	IWillFinally Finally(Action finalAction);

	Action Build();

	void Run();
}