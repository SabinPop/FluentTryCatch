using FluentAssertions;
using FluentTryCatch;

namespace Tests;

public class FunctionalTests
{
	[Fact]
	public void CatchExceptionAndThrowOtherTypeWithMessageAndInnerException()
	{
        var exceptionToCatch = typeof(ArgumentException);
        var exceptionToRethrow = typeof(Exception);
        var catchAction = () => { };
        var message = "messsage";
        var includeInnerException = true;

		var actionToTest = TestBuilder.Build(exceptionToCatch, exceptionToCatch, exceptionToRethrow, catchAction, message, includeInnerException);

		actionToTest.Should().NotBeNull();

        var exception = Assert.Throws(exceptionToRethrow, actionToTest!);

        exception.Should().NotBeNull();
        exception.Should().BeOfType(exceptionToRethrow);
		exception.Message.Should().Be(message);
		exception.InnerException.Should().NotBeNull();
        exception.InnerException.Should().BeOfType(exceptionToCatch);
    }

    [Fact]
    public void CatchExceptionAndThrowOtherTypeWithMessageAndWithoutInnerException()
    {
        var exceptionToCatch = typeof(ArgumentException);
        var exceptionToRethrow = typeof(Exception);
        var catchAction = () => { };
        var message = "messsage";
        var includeInnerException = false;

        var actionToTest = TestBuilder.Build(exceptionToCatch, exceptionToCatch, exceptionToRethrow, catchAction, message, includeInnerException);

        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws(exceptionToRethrow, actionToTest!);

        exception.Should().NotBeNull();
        exception.Should().BeOfType(exceptionToRethrow);
        exception.Message.Should().Be(message);
		exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void CatchExceptionAndThrowOtherTypeWithoutMessageAndWithoutInnerException()
    {
        var exceptionToCatch = typeof(ArgumentException);
        var exceptionToRethrow = typeof(Exception);
        var catchAction = () => { };

		var actionToTest = TestBuilder.Build(exceptionToCatch, exceptionToCatch, exceptionToRethrow, catchAction);

        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws(exceptionToRethrow, actionToTest!);

        exception.Should().NotBeNull();
        exception.Should().BeOfType(exceptionToRethrow);
        exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void CatchExceptionAndThrowOtherTypeWithArguments()
    {
        var exceptionToCatch = typeof(ArgumentException);
        var exceptionToRethrow = typeof(ArgumentNullException);
		var args = new object[] { "paramNameIsRandom", "messageIsAlsoRandom"};
        var catchAction = () => { };

        var actionToTest = TestBuilder.Build(exceptionToCatch, exceptionToCatch, exceptionToRethrow, catchAction, args);

        actionToTest.Should().NotBeNull();

        var exception = Assert.Throws(exceptionToRethrow, actionToTest!);

        exception.Should().NotBeNull();
		exception.Should().BeOfType(exceptionToRethrow);
        exception.InnerException.Should().BeNull();
    }

    [Fact]
    public void TryFinally()
    {
        var actionOrder = new List<int>();

		var tryAction = () => actionOrder.Add(1);
        var finalAction = () => actionOrder.Add(3);

        var actionToTest = TestBuilder.Build(tryAction, finalAction);

        actionToTest();

        actionToTest.Should().NotBeNull();
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

        var actionToTest = TestBuilder.Build(tryAction, catchAction, finalAction);

        actionToTest();

        // if there is no exception thrown in try block, then no action will be 
        // executed in the catch block
        actionToTest.Should().NotBeNull();
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

        var actionToTest = TestBuilder.Build(tryAction, catchAction, finalAction);

        actionToTest();

        actionToTest.Should().NotBeNull();
        actionOrder.Should().HaveCount(3);
        actionOrder.Should().BeInAscendingOrder();
    }

    private Action ActionToTest1()
	{
		return Try
				.To(() =>
				{
					throw new InvalidOperationException("test");
				})
				.Catch<InvalidOperationException>(() =>
				{
					Console.WriteLine("catch");
				})
				.Throw<InvalidOperationException>()
					.WithMessage("test")
					.WithInnerException(true)
				.And()
				.Catch<ArgumentException>(() =>
				{
					Console.WriteLine("catch arg exception");
				})
				.Rethrow()
				.Build();
	}

	private Action ActionToTest2()
	{
		return Try
				.To(() =>
				{
					throw new ArgumentException("test argument exception");
				})
				.Catch<InvalidOperationException>(() =>
				{
					Console.WriteLine("catch");
				})
				.Throw<InvalidOperationException>()
					.WithMessage("test")
					.WithInnerException(true)
				.And()
				.Catch<ArgumentException>(() =>
				{
					Console.WriteLine("catch arg exception");
				})
				.Rethrow()
				.Build();
	}

    private Action ActionToTest3()
    {
		return Try
				.To(() =>
				{
					throw new ArgumentException("test argument exception");
				})
				.Catch<InvalidOperationException>(() =>
				{
					Console.WriteLine("catch");
				})
				.Rethrow()
				.Build();
    }
}