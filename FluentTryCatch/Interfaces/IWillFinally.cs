using System;

namespace FluentTryCatch.Interfaces;

public interface IWillFinally
{
    Action Build();

    void Run();
}