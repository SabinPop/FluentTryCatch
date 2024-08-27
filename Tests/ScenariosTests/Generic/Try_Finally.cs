using FluentAssertions;
using FluentTryCatch.Scenarios;
using System.Reflection;

namespace Tests.ScenariosTests.Generic;

public class Try_Finally
{
    [Fact]
    public void Success()
    {
        var actionOrder = new List<int>();

        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            return 1;
        };
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryFinally(tryFunc, finalAction);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        result.Should().Be(1);
        actionOrder.Should().HaveCount(2);
        actionOrder.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Throws()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryFinally(tryFunc, finalAction);
        funcToTest.Should().NotBeNull();

        var exception = Assert.Throws<TargetInvocationException>(funcToTest);
        exception.Should().NotBeNull();
        exception.InnerException.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<Exception>();
        actionOrder.Should().HaveCount(2);
        actionOrder.Should().BeInAscendingOrder();
    }
}
