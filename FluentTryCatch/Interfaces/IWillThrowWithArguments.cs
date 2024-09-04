namespace FluentTryCatch.Interfaces;

public interface IWillThrowWithArguments
{
	IWillThrowComplete WithArguments(params object[] args);
}

public interface IWillThrowWithArguments<TResult>
{
	IWillThrowComplete<TResult> WithArguments(params object[] args);
}