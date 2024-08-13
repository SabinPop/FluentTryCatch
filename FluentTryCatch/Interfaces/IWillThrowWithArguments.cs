namespace FluentTryCatch.Interfaces;

public interface IWillThrowWithArguments
{
    IWillThrowComplete WithArguments(params object[] args);
}