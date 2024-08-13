namespace FluentTryCatch.Interfaces;

public interface IWillThrowWithMessage : IWillThrowComplete
{
    IWillThrowComplete WithInnerException(bool includeInnerException = false);
}