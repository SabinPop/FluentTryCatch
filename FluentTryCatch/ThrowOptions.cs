using FluentTryCatch.Interfaces;
using System;
using System.Linq;

namespace FluentTryCatch;

internal sealed class ThrowOptions : IWillThrowWithMessage
{
    private string _message = string.Empty;

    internal ThrowOptions(IWillTry parent)
    {
        Parent = parent;
    }

    public bool IncludesInnerException { get; internal set; }

    public bool HasMessage { get; private set; }

    public string Message
    {
        get => _message;
        internal set
        {
            _message = value;
            HasMessage = !string.IsNullOrEmpty(value);
        }
    }

    public IWillTry Parent { get; private set; }

    public IWillThrowComplete WithInnerException(bool includeInnerException = false)
    {
        IncludesInnerException = includeInnerException;
        return this;
    }

    internal Exception? BuildException(Type? exceptionType, Exception? inner = null)
    {
        // won't throw any exception, just catch it 
        if (exceptionType is null)
        {
            return null;
        }

        if (HasMessage && IncludesInnerException)
        {
            return (Exception)Activator.CreateInstance(exceptionType, Message, inner);
        }
        else if (HasMessage)
        {
            var constructorInfo = exceptionType.GetConstructor([typeof(string)]);

            if (constructorInfo.GetParameters().Any(x => x.Name == "message"))
            {
                return (Exception)constructorInfo.Invoke([Message]);
            }

            constructorInfo = exceptionType.GetConstructor([typeof(string), typeof(Exception)]);
            if (constructorInfo is not null)
            {
                return (Exception)constructorInfo.Invoke([Message, null]);
            }

            return (Exception)Activator.CreateInstance(exceptionType, (string?)Message, null);
        }
        else
        {
            return (Exception)Activator.CreateInstance(exceptionType);
        }
    }

    public IWillTry And()
    {
        return Parent;
    }

    public IWillFinally Finally(Action finalAction)
    {
        return (IWillFinally)Parent;
    }

    public Action Build()
    {
        return ((IWillFinally)Parent).Build();
    }

    public void Run()
    {
        ((IWillFinally)Parent).Run();
    }
}