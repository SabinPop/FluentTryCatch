using System;

namespace FluentTryCatch.Interfaces;

public interface IWillThrowComplete
{
    IWillTry And();

    // IWillFinally Finally();

    Action Build();

    void Run();
}