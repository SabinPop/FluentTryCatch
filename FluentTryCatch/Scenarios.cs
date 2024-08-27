using System;

namespace FluentTryCatch.Scenarios;

public static class Scenarios
{
    #region Try - Catch

    // Could add a method overload for something like
    // TryCatchWhen...(Action tryAction, Action catchAction, Expression<TExceptionToCatch> expression)

    public static Func<TResult?> TryCatch<TResult>(Func<TResult> tryFunc, Func<TResult> catchFunc)
    {
        return TryCatch<TResult, Exception>(tryFunc, catchFunc);
    }

    public static Func<TResult?> TryCatch<TResult, TExceptionToCatch>(Func<TResult> tryFunc, Func<TResult> catchFunc)
        where TExceptionToCatch : Exception
    {
        return Try.To(tryFunc).Catch<TExceptionToCatch>(catchFunc).Build();
    }

    public static Func<TResult?> TryCatch<TResult>(Func<TResult> tryFunc, Action catchAction)
    {
        return TryCatch<TResult, Exception>(tryFunc, catchAction);
    }

    public static Func<TResult?> TryCatch<TResult, TExceptionToCatch>(Func<TResult> tryFunc, Action catchFunc)
        where TExceptionToCatch : Exception
    {
        return Try.To(tryFunc).Catch<TExceptionToCatch>(catchFunc).Build();
    }

    public static Action TryCatch(Action tryAction, Action catchAction)
    {
        return TryCatch<Exception>(tryAction, catchAction);
    }

    public static Action TryCatch<TExceptionToCatch>(Action tryAction, Action catchAction)
        where TExceptionToCatch : Exception
    {
        return Try.To(tryAction).Catch<TExceptionToCatch>(catchAction).Build();
    }
    #endregion

    #region Try - Finally

    public static Action TryFinally(Action tryAction, Action finalAction)
    {
        return Try.To(tryAction).Finally(finalAction).Build();
    }

    public static Func<TResult?> TryFinally<TResult>(Func<TResult> tryFunc, Action finalAction)
    {
        return Try.To(tryFunc).Finally(finalAction).Build();
    }

    #endregion

    #region Try - Catch - Finally

    public static Func<TResult?> TryCatchFinally<TResult>(Func<TResult> tryFunc, Action catchAction, Action finalAction)
    {
        return TryCatchFinally<TResult, Exception>(tryFunc, catchAction, finalAction);
    }

    public static Func<TResult?> TryCatchFinally<TResult, TExceptionToCatch>(Func<TResult> tryFunc, Action catchAction, Action finalAction)
        where TExceptionToCatch : Exception
    {
        return Try.To(tryFunc).Catch<TExceptionToCatch>(catchAction).Finally(finalAction).Build();
    }

    public static Func<TResult?> TryCatchFinally<TResult>(Func<TResult> tryFunc, Func<TResult> catchFunc, Action finalAction)
    {
        return TryCatchFinally<TResult, Exception>(tryFunc, catchFunc, finalAction);
    }

    public static Func<TResult?> TryCatchFinally<TResult, TExceptionToCatch>(Func<TResult> tryFunc, Func<TResult> catchFunc, Action finalAction)
        where TExceptionToCatch : Exception
    {
        return Try.To(tryFunc).Catch<TExceptionToCatch>(catchFunc).Finally(finalAction).Build();
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

    #endregion

    #region Try - Catch - Rethrow

    public static Action TryCatchRethrow<TExceptionToCatch>(Action tryAction, Action catchAction)
        where TExceptionToCatch : Exception
    {
        return Try
            .To(tryAction)
            .Catch<TExceptionToCatch>(catchAction)
            .Rethrow()
            .Build();
    }

    public static Func<TResult?> TryCatchRethrow<TResult, TExceptionToCatch>(Func<TResult> tryFunc, Action catchAction)
        where TExceptionToCatch : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Rethrow()
            .Build();
    }

    #endregion

    #region Try - Catch - Rethrow - Finally

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

    public static Func<TResult?> TryCatchRethrowFinally<TResult, TExceptionToCatch>(Func<TResult> tryFunc, Action catchAction, Action finalAction)
        where TExceptionToCatch : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Rethrow()
            .Finally(finalAction)
            .Build();
    }

    #endregion

    #region Try - Catch - Throw

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

    public static Func<TResult?> TryCatchThrow<TResult, TExceptionToCatch, TExceptionToRethrow>(Func<TResult> tryFunc, Action catchAction)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
            .Build();
    }

    #endregion

    #region Try - Catch - Throw - Finally

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

    public static Func<TResult?> TryCatchThrowFinally<TResult, TExceptionToCatch, TExceptionToRethrow>(Func<TResult> tryFunc, Action catchAction, Action finalAction)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
            .Finally(finalAction)
            .Build();
    }

    #endregion

    #region Try - Catch - Throw with message and inner exception
    
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

    public static Func<TResult?> TryCatchThrowWithMessageAndInner<TResult, TExceptionToCatch, TExceptionToRethrow>(Func<TResult> tryFunc, Action catchAction, string thrownExceptionMessage, bool includeInnerException = false)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithMessage(thrownExceptionMessage)
                .WithInnerException(includeInnerException)
            .Build();
    }

    #endregion

    #region Try - Catch - Throw with message and inner exception - Finally

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

    public static Func<TResult?> TryCatchThrowWithMessageAndInnerFinally<TResult, TExceptionToCatch, TExceptionToRethrow>(Func<TResult> tryFunc, Action catchAction, Action finalAction, string thrownExceptionMessage, bool includeInnerException = false)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithMessage(thrownExceptionMessage)
                .WithInnerException(includeInnerException)
            .Finally(finalAction)
            .Build();
    }

    #endregion

    #region Try - Catch - Throw with arguments

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

    public static Func<TResult?> TryCatchThrowWithArguments<TResult, TExceptionToCatch, TExceptionToRethrow>(Func<TResult> tryFunc, Action catchAction, object[] args)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithArguments(args)
            .Build();
    }

    #endregion

    #region Try - Catch - Throw with arguments - Finally

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

    public static Func<TResult?> TryCatchThrowWithArgumentsFinally<TResult, TExceptionToCatch, TExceptionToRethrow>(Func<TResult> tryFunc, Action catchAction, Action finalAction, object[] args)
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(tryFunc)
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithArguments(args)
            .Finally(finalAction)
            .Build();
    }

    #endregion
}