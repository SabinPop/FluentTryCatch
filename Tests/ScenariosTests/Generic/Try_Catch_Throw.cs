using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests.Generic;

public class Try_Catch_Throw
{

    [Fact]
    public void Success_Func_Action()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            return 1;
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatchThrow<object?, ArgumentException, Exception>(tryFunc, catchAction);
        actionToTest.Should().NotBeNull();

        actionToTest();

        actionOrder.Should().BeEquivalentTo([1]);
    }


    [Fact]
    public void Catches_And_Throws_Func_Action_TException()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentException();
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatchThrow<object?, ArgumentException, Exception>(tryFunc, catchAction);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<Exception>(actionToTest);
        exception.InnerException.Should().BeNull();
        actionOrder.Should().BeEquivalentTo([1, 2]);
    }

    [Fact]
    public void DoesNotCatch_TException()
    {
        var actionOrder = new List<int>();
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentNullException();
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatchThrow<InvalidCastException, Exception>(tryAction, catchAction);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        exception.InnerException.Should().BeNull();
        actionOrder.Should().BeEquivalentTo([1]);
    }
}
