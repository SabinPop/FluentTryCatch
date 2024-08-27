using FluentAssertions;
using FluentTryCatch.Scenarios;
using System.Reflection;

namespace Tests.ScenariosTests.Generic;

public class Try_Catch_Rethrow
{
    [Fact]
    public void Success_Func_Action()
    {
        var actionOrder = new List<int>();
        Func<int?> tryFunc = () =>
        {
            actionOrder.Add(1);
            return 1;
        };
        var catchAction = () => actionOrder.Add(2);

        var funcToTest = Scenarios.TryCatchRethrow<int?, ArgumentNullException>(tryFunc, catchAction);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        result.Should().Be(1);
        actionOrder.Should().BeEquivalentTo([1]);
    }

    [Fact]
    public void Catches_And_Rethrows_Func_Action()
    {
        var actionOrder = new List<int>();
        var exceptionToThrow = new ArgumentNullException("paramName", "message");
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw exceptionToThrow;
        };
        var catchAction = () => actionOrder.Add(2);
        var funcToTest = Scenarios.TryCatchRethrow<object?, ArgumentNullException>(tryFunc, catchAction);

        funcToTest.Should().NotBeNull();

        var exception = Assert.Throws<TargetInvocationException>(funcToTest);
        exception.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<ArgumentNullException>();
        var inner = exception.InnerException as ArgumentNullException;
        inner.Should().NotBeNull();
        inner!.Message.Should().Be(exceptionToThrow.Message);
        inner.ParamName.Should().Be(exceptionToThrow.ParamName);
        actionOrder.Should().BeEquivalentTo([1, 2]);
    }

    [Fact]
    public void DoesNotCatch_Func_Action()
    {
        var actionOrder = new List<int>();
        var exceptionToThrow = new ArgumentNullException("paramName", "message");
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw exceptionToThrow;
        };
        var catchAction = () => actionOrder.Add(2);
        var funcToTest = Scenarios.TryCatchRethrow<object?, InvalidOperationException>(tryFunc, catchAction);

        funcToTest.Should().NotBeNull();

        var exception = Assert.Throws<TargetInvocationException>(funcToTest);
        exception.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<ArgumentNullException>();
        var inner = exception.InnerException as ArgumentNullException;
        inner.Should().NotBeNull();
        inner!.Message.Should().Be(exceptionToThrow.Message);
        inner.ParamName.Should().Be(exceptionToThrow.ParamName);
        actionOrder.Should().BeEquivalentTo([1]);
    }
}
