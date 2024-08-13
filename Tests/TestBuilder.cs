using FluentTryCatch;

namespace Tests;

internal static class TestBuilder
{
    internal static Action Build(Action tryAction, Action catchAction, Action finalAction)
    {
        return Try.To(tryAction).Catch<Exception>(catchAction).Finally(finalAction).Build();
    }

    internal static Action Build(Action tryAction, Action finalAction)
    {
        return Try.To(tryAction).Finally(finalAction).Build();
    }
    
    internal static Action? Build(Type exceptionToThrow, Type exceptionToCatch, Type exceptionToRethrow, Action catchAction, string thrownExceptionMessage, bool includeInnerException = false)
    {
        return typeof(TestBuilder).GetMethod(nameof(InternalBuildWithMessageAndOptionalInclude), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?
            .MakeGenericMethod(exceptionToThrow, exceptionToCatch, exceptionToRethrow)
            .Invoke(null, [catchAction, thrownExceptionMessage, includeInnerException]) as Action;
    }

    internal static Action? Build(Type exceptionToThrow, Type exceptionToCatch, Type exceptionToRethrow, Action catchAction, object[] args)
    {
        return typeof(TestBuilder).GetMethod(nameof(InternalBuildWithArguments), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?
            .MakeGenericMethod(exceptionToThrow, exceptionToCatch, exceptionToRethrow)
            .Invoke(null, [catchAction, args]) as Action;
    }

    internal static Action? Build(Type exceptionToThrow, Type exceptionToCatch, Type exceptionToRethrow, Action catchAction)
    {
        return typeof(TestBuilder).GetMethod(nameof(InternalBuild), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?
            .MakeGenericMethod(exceptionToThrow, exceptionToCatch, exceptionToRethrow)
            .Invoke(null, [catchAction]) as Action;
    }

    private static Action InternalBuildWithMessageAndOptionalInclude<TExceptionToThrow, TExceptionToCatch, TExceptionToRethrow>(Action catchAction, string thrownExceptionMessage, bool includeInnerException = false)
        where TExceptionToThrow : Exception, new()
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(() => throw new TExceptionToThrow())
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithMessage(thrownExceptionMessage)
                .WithInnerException(includeInnerException)
            .Build();
    }

    private static Action InternalBuild<TExceptionToThrow, TExceptionToCatch, TExceptionToRethrow>(Action catchAction)
        where TExceptionToThrow : Exception, new()
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(() => throw new TExceptionToThrow())
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
            .Build();
    }

    private static Action InternalBuildWithArguments<TExceptionToThrow, TExceptionToCatch, TExceptionToRethrow>(Action catchAction, object[] args)
        where TExceptionToThrow : Exception, new()
        where TExceptionToCatch : Exception
        where TExceptionToRethrow : Exception
    {
        return Try
            .To(() => throw new TExceptionToThrow())
            .Catch<TExceptionToCatch>(catchAction)
            .Throw<TExceptionToRethrow>()
                .WithArguments(args)
            .Build();
    }
}
