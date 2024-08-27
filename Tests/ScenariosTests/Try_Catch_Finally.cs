using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests;

public class Try_Catch_Finally
{
    [Fact]
    public void Success()
    {
        var actionOrder = new List<int>();
        var tryAction = () => actionOrder.Add(1);
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = Scenarios.TryCatchFinally(tryAction, catchAction, finalAction);
        actionToTest.Should().NotBeNull();

        actionToTest();

        // if there is no exception thrown in try block, then no action will be 
        // executed in the catch block
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void Success_TException()
    {
        var actionOrder = new List<int>();
        var tryAction = () => actionOrder.Add(1);
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = Scenarios.TryCatchFinally<ArgumentNullException>(tryAction, catchAction, finalAction);
        actionToTest.Should().NotBeNull();

        actionToTest();

        // if there is no exception thrown in try block, then no action will be 
        // executed in the catch block
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void CatchesException()
    {
        var actionOrder = new List<int>();
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = Scenarios.TryCatchFinally(tryAction, catchAction, finalAction);
        actionToTest.Should().NotBeNull();

        actionToTest();

        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void CatchesException_TException()
    {
        var actionOrder = new List<int>();
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentNullException();
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = Scenarios.TryCatchFinally<ArgumentNullException>(tryAction, catchAction, finalAction);
        actionToTest.Should().NotBeNull();

        actionToTest();

        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
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
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = Scenarios.TryCatchFinally<InvalidOperationException>(tryAction, catchAction, finalAction);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }
}
