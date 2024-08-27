using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests;

public class Try_Finally
{
    [Fact]
    public void Success()
    {
        var actionOrder = new List<int>();
        var tryAction = () => actionOrder.Add(1);
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = Scenarios.TryFinally(tryAction, finalAction);
        actionToTest.Should().NotBeNull();

        actionToTest();

        actionOrder.Should().HaveCount(2);
        actionOrder.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Throws()
    {
        var actionOrder = new List<int>();
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = Scenarios.TryFinally(tryAction, finalAction);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<Exception>(actionToTest);
        exception.Should().NotBeNull();
        actionOrder.Should().HaveCount(2);
        actionOrder.Should().BeInAscendingOrder();
    }
}
