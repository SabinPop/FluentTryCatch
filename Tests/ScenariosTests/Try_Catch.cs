using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests;

public class Try_Catch
{
    [Fact]
    public void Success()
    {
        var actionOrder = new List<int>();
        var tryAction = () =>
        {
            actionOrder.Add(1);
        };
        var catchAction = () => actionOrder.Add(2);

        var funcToTest = Scenarios.TryCatch(tryAction, catchAction);
        funcToTest.Should().NotBeNull();

        funcToTest();
        actionOrder.Should().HaveCount(1);
    }

    [Fact]
    public void Catches()
    {
        var actionOrder = new List<int>();
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatch(tryAction, catchAction);
        actionToTest.Should().NotBeNull();

        actionToTest();
        actionOrder.Should().HaveCount(2);
        actionOrder.Should().BeInAscendingOrder();
    }

    [Fact]
    public void DoesNotCatch()
    {
        var actionOrder = new List<int>();
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new NullReferenceException();
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatch<ArgumentNullException>(tryAction, catchAction);
        actionToTest.Should().NotBeNull();

        var exception = Assert.ThrowsAny<Exception>(actionToTest);
        exception.Should().BeOfType<NullReferenceException>();
        actionOrder.Should().HaveCount(1);
        actionOrder.Should().BeInAscendingOrder();
    }
}
