using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests;

public class FunctionalTests
{
	[Fact]
	public void TryCatchThrowWithMessageAndInner()
	{
        var tryAction = () => { throw new ArgumentException(); };
        var catchAction = () => { };
        var message = "messsage";
        var includeInnerException = true;

        var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<ArgumentException, Exception>(tryAction, catchAction, message, includeInnerException);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<Exception>(actionToTest);
		exception.Message.Should().Be(message);
		exception.InnerException.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void TryCatchThrowWithMessageAndInnerFinally()
    {
        var tryAction = () => { throw new ArgumentException(); };
        var catchAction = () => { };
        var finalAction = () => { };
        var message = "messsage";
        var includeInnerException = true;

        var actionToTest = Scenarios.TryCatchThrowWithMessageAndInnerFinally<ArgumentException, Exception>(tryAction, catchAction, finalAction, message, includeInnerException);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<Exception>(actionToTest);
        exception.Message.Should().Be(message);
        exception.InnerException.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public void TryCatchThrowWithMessageAndNoInner()
    {
        var tryAction = () => {  throw new ArgumentException(); };
        var catchAction = () => { };
        var message = "messsage";
        var includeInnerException = false;

        var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<ArgumentException, Exception>(tryAction, catchAction, message, includeInnerException);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<Exception>(actionToTest);
        exception.Message.Should().Be(message);
		exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void TryCatchThrow()
    {
        var tryAction = () => { throw new ArgumentException(); };
        var catchAction = () => { };

		var actionToTest = Scenarios.TryCatchThrow<ArgumentException, Exception>(tryAction, catchAction);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<Exception>(actionToTest);
        exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void TryCatchThrowWithArguments()
    {
		var args = new object[] { "paramNameIsRandom", "messageIsAlsoRandom"};
        var tryAction = () => { throw new ArgumentException(); };
        var catchAction = () => { };

        var actionToTest = Scenarios.TryCatchThrowWithArguments<ArgumentException, ArgumentNullException>(tryAction, catchAction, args);
        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void TryFinally()
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
    public void TryCatchFinally_DoesNotCatchAnyException()
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
        actionOrder.Should().HaveCount(2);
        actionOrder.Should().BeInAscendingOrder();
    }

    [Fact]
    public void TryCatchFinally_CatchesException()
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

        actionOrder.Should().HaveCount(3);
        actionOrder.Should().BeInAscendingOrder();
    }

    [Fact]
    public void TryCatch_Catches()
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
    public void TryCatch_DoesNotCatch()
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

    [Fact]
    public void TryCatchRethrow_NoTryAction()
    {
        var tryAction = () => { throw new NullReferenceException(); };
        var catchAction = () => { };
        var actionToTest = Scenarios.TryCatchRethrow<NullReferenceException>(tryAction, catchAction);

        actionToTest.Should().NotBeNull();
        
        var exception = Assert.Throws<NullReferenceException>(actionToTest);
    }

    [Fact]
    public void TryCatchRethrow()
    {
        var exceptionToThrow = new ArgumentNullException("paramName", "message");
        var tryAction = () =>
        {
            throw exceptionToThrow;
        };
        var catchAction = () => { };
        var actionToTest = Scenarios.TryCatchRethrow<ArgumentNullException>(tryAction, catchAction);

        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws<ArgumentNullException>(actionToTest);
        exception.Message.Should().Be(exceptionToThrow.Message);
        exception.ParamName.Should().Be(exceptionToThrow.ParamName);
    }
}