using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests;

public class Try_Catch_Throw_With_Args
{
    [Fact]
    public void Success()
    {
        var actionOrder = new List<int>();
        var args = new string[] { "paramNameIsRandom", "messageIsAlsoRandom" };
        var tryAction = () =>
        {
            actionOrder.Add(1);
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatchThrowWithArguments<Exception, ArgumentNullException>(tryAction, catchAction, args);
        actionToTest.Should().NotBeNull();

        actionToTest();
        actionOrder.Should().BeEquivalentTo([1]);
    }

    [Fact]
    public void Catches()
    {
        var actionOrder = new List<int>();
        var args = new string[] { "paramNameIsRandom", "messageIsAlsoRandom" };
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatchThrowWithArguments<Exception, ArgumentNullException>(tryAction, catchAction, args);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        exception.ParamName.Should().Be(args[0]);
        exception.Message.Should().Be($"{args[1]} (Parameter '{args[0]}')");
        actionOrder.Should().BeEquivalentTo([1, 2]);
    }

    [Fact]
    public void DoesNotCatch()
    {
        var actionOrder = new List<int>();
        var args = new string[] { "paramNameIsRandom", "messageIsAlsoRandom" };
        var tryAction = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentNullException();
        };
        var catchAction = () => actionOrder.Add(2);

        var actionToTest = Scenarios.TryCatchThrowWithArguments<Exception, ArgumentNullException>(tryAction, catchAction, args);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        actionOrder.Should().BeEquivalentTo([1]);
    }
}
