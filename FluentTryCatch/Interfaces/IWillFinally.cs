using System;

namespace FluentTryCatch.Interfaces;

public interface IWillFinally<TResult>
{
	Func<TResult?> Build();

	TResult? Run();
}

public interface IWillFinally
{
	Action Build();

	void Run();
}