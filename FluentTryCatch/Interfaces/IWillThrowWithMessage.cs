namespace FluentTryCatch.Interfaces;

public interface IWillThrowWithMessage : IWillThrowComplete
{
    IWillThrowComplete WithInnerException(bool includeInnerException = false);
}

public interface IWillThrowWithMessage<TResult> : IWillThrowComplete<TResult>
{
    IWillThrowComplete<TResult> WithInnerException(bool includeInnerException = false);
}