using System;

namespace FluentTryCatch.Scenarios;

public static class Scenarios
{
    public static Action TryCatch(Action tryAction, Action catchAction)
    {
        return TryCatch<Exception>(tryAction, catchAction);
    }

    public static Action TryCatch<TExceptionToCatch>(Action tryAction, Action catchAction)
        where TExceptionToCatch : Exception
    {
        return Try.To(tryAction).Catch<TExceptionToCatch>(catchAction).Build();
    }

    public static Action TryCatchFinally(Action tryAction, Action catchAction, Action finalAction)
    {
        return TryCatchFinally<Exception>(tryAction, catchAction, finalAction);
    }

    public static Action TryCatchFinally<TExceptionToCatch>(Action tryAction, Action catchAction, Action finalAction)
        where TExceptionToCatch : Exception
    {
        return Try.To(tryAction).Catch<TExceptionToCatch>(catchAction).Finally(finalAction).Build();
    }

    public static Action TryFinally(Action tryAction, Action finalAction)
    {
        return Try.To(tryAction).Finally(finalAction).Build();
    }

    public static Action TryCatchThrowWithMessageAndInner<TExceptionToCatch, TExceptionToRethrow>(Action tryAction, Action catchAction, string thrownExceptionMessage, bool includeInnerException = false)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithMessage(thrownExceptionMessage)
                .WithInnerException(includeInnerException)
            .Build();
    }

    public static Action TryCatchThrowWithMessageAndInnerFinally<TExceptionToCatch, TExceptionToRethrow>(Action tryAction, Action catchAction, Action finalAction, string thrownExceptionMessage, bool includeInnerException = false)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithMessage(thrownExceptionMessage)
                .WithInnerException(includeInnerException)
            .Finally(finalAction)
            .Build();
    }

    public static Action TryCatchThrow<TExceptionToCatch, TExceptionToRethrow>(Action tryAction, Action catchAction)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
            .Build();
    }

    public static Action TryCatchThrowFinally<TExceptionToCatch, TExceptionToRethrow>(Action tryAction, Action catchAction, Action finalAction)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
            .Finally(finalAction)
            .Build();
    }

    public static Action TryCatchThrowWithArguments<TExceptionToCatch, TExceptionToRethrow>(Action tryAction, Action catchAction, object[] args)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithArguments(args)
            .Build();
    }

    public static Action TryCatchThrowWithArgumentsFinally<TExceptionToCatch, TExceptionToRethrow>(Action tryAction, Action catchAction, Action finalAction, object[] args)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithArguments(args)
            .Finally(finalAction)
            .Build();
    }

    public static Action TryCatchRethrow<TExceptionToCatch>(Action tryAction, Action catchAction)
        where TExceptionToCatch : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Rethrow()
            .Build();
    }

    public static Action TryCatchRethrowFinally<TExceptionToCatch>(Action tryAction, Action catchAction, Action finalAction)
        where TExceptionToCatch : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Rethrow()
            .Finally(finalAction)
            .Build();
    }
}