using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests;

public class Try_Catch_Rethrow_Finally
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
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchRethrowFinally<ArgumentNullException>(tryAction, catchAction, finalAction);
        funcToTest.Should().NotBeNull();

        funcToTest();
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void Catches_And_Rethrows()
    {
        var actionOrder = new List<int>();
        var exceptionToThrow = new ArgumentNullException("paramName", "message");
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw exceptionToThrow;
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);
        var actionToTest = Scenarios.TryCatchRethrowFinally<ArgumentNullException>(tryAction, catchAction, finalAction);

        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        exception.Message.Should().Be(exceptionToThrow.Message);
        exception.ParamName.Should().Be(exceptionToThrow.ParamName);
        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void DoesNotCatch()
    {
        var actionOrder = new List<int>();
        var exceptionToThrow = new ArgumentNullException("paramName", "message");
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw exceptionToThrow;
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);
        var funcToTest = Scenarios.TryCatchRethrowFinally<InvalidOperationException>(tryAction, catchAction, finalAction);

        funcToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(funcToTest);
        exception.Should().NotBeNull();
        exception.Message.Should().Be(exceptionToThrow.Message);
        exception.ParamName.Should().Be(exceptionToThrow.ParamName);
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }
}
