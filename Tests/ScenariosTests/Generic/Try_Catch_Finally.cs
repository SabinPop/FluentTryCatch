using FluentAssertions;
using FluentTryCatch.Scenarios;
using System.Reflection;

namespace Tests.ScenariosTests.Generic;

public class Try_Catch_Finally
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
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally(tryFunc, catchAction, finalAction);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        result.Should().Be(1);

        // if there is no exception thrown in try block, then no action will be 
        // executed in the catch block
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void Success_Func_Action_TException()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            return 1;
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally<object?, ArgumentNullException>(tryFunc, catchAction, finalAction);
        funcToTest.Should().NotBeNull();

        funcToTest();

        // if there is no exception thrown in try block, then no action will be 
        // executed in the catch block
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void Success_Func_Func()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            return 1;
        };
        Func<object?> catchFunc = () =>
        {
            actionOrder.Add(2);
            return 2;
        };
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally(tryFunc, catchFunc, finalAction);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        result.Should().Be(1);

        // if there is no exception thrown in try block, then no action will be 
        // executed in the catch block
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void Success_Func_Func_TException()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            return 1;
        };
        Func<object?> catchFunc = () =>
        {
            actionOrder.Add(2);
            return 2;
        };
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally<object?, ArgumentNullException>(tryFunc, catchFunc, finalAction);
        funcToTest.Should().NotBeNull();

        funcToTest();

        // if there is no exception thrown in try block, then no action will be 
        // executed in the catch block
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void CatchesException_Func_Action()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally(tryFunc, catchAction, finalAction);
        funcToTest.Should().NotBeNull();
        
        var result = funcToTest();
        result.Should().BeNull();
        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void CatchesException_Func_Action_TException()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentNullException();
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally<object?, ArgumentNullException>(tryFunc, catchAction, finalAction);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        result.Should().BeNull();
        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void CatchesException_Func_Func()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new Exception();
        };
        Func<object?> catchFunc = () =>
        {
            actionOrder.Add(2);
            return 2;
        };
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally(tryFunc, catchFunc, finalAction);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        result.Should().Be(2);
        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void CatchesException_Func_Func_TException()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentNullException();
        };
        Func<object?> catchFunc = () =>
        {
            actionOrder.Add(2);
            return 2;
        };
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally<object?, ArgumentNullException>(tryFunc, catchFunc, finalAction);
        funcToTest.Should().NotBeNull();

        var result = funcToTest();
        result.Should().Be(2);
        actionOrder.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void DoesNotCatch_Func_Action_TException()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentNullException();
        };
        var catchAction = () => actionOrder.Add(2);
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally<object?, InvalidOperationException>(tryFunc, catchAction, finalAction);
        funcToTest.Should().NotBeNull();

        var exception = Assert.Throws<TargetInvocationException>(funcToTest);
        exception.Should().NotBeNull();
        exception.InnerException.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<ArgumentNullException>();
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }

    [Fact]
    public void DoesNotCatch_Func_Func_TException()
    {
        var actionOrder = new List<int>();
        Func<object?> tryFunc = () =>
        {
            actionOrder.Add(1);
            throw new ArgumentNullException();
        };
        Func<object?> catchFunc = () =>
        {
            actionOrder.Add(2);
            return 2;
        };
        var finalAction = () => actionOrder.Add(3);

        var funcToTest = Scenarios.TryCatchFinally<object?, InvalidOperationException>(tryFunc, catchFunc, finalAction);
        funcToTest.Should().NotBeNull();

        var exception = Assert.Throws<TargetInvocationException>(funcToTest);
        exception.Should().NotBeNull();
        exception.InnerException.Should().NotBeNull();
        exception.InnerException.Should().BeOfType<ArgumentNullException>();
        actionOrder.Should().BeEquivalentTo([1, 3]);
    }
}
