using FluentAssertions;
using FluentTryCatch.Scenarios;
using System.Reflection;

namespace Tests.ScenariosTests.Generic;

public class Try_Catch_Throw_With_Args_Finally
{
    [Fact]
    public void Success()
    {
        var actionOrder = new List<int>();
        var args = new string[] { "paramNameIsRandom", "messageIsAlsoRandom" };
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            return 1;
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);
        var funcToTest = Scenarios.TryCatchThrowWithArgumentsFinally<object?, Exception, ArgumentNullException>(tryFunc, catchAction, finalAction, args);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void Catches()
    {
        var actionOrder = new List<int>();
        var args = new string[] { "paramNameIsRandom", "messageIsAlsoRandom" };
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);
        var actionToTest = Scenarios.TryCatchThrowWithArgumentsFinally<object?, Exception, ArgumentNullException>(tryFunc, catchAction, finalAction, args);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        exception.ParamName.Should().Be(args[0]);
        exception.Message.Should().Be($"{args[1]} (Parameter '{args[0]}')");
        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void DoesNotCatch()
    {
        var actionOrder = new List<int>();
        var args = new string[] { "paramNameIsRandom", "messageIsAlsoRandom" };
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new InvalidOperationException();
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);
        var actionToTest = Scenarios.TryCatchThrowWithArgumentsFinally<object?, InvalidCastException, ArgumentNullException>(tryFunc, catchAction, finalAction, args);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<TargetInvocationException>(actionToTest);
        exception.InnerException.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<InvalidOperationException>();
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }
}
