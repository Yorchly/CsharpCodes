namespace CsharpCodes.Tests.UnitTests;

public class GenericCustomCompareTest
{
    private ICustomComparer<Test1, Test2> _comparer;

    [Fact]
    public void IsValid_InstancesAreNull_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();

        Assert.Throws<ArgumentNullException>(() => _comparer.IsValid);
    }

    [Fact]
    public void SetInstances_FirstInstanceIsNull_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();
        var test2 = new Test2();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.SetInstances(null, test2)
            );
    }

    [Fact]
    public void SetInstances_SecondInstanceIsNull_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();
        var test1 = new Test1();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.SetInstances(test1, null)
            );
    }

    [Fact]
    public void AreEqual_CallingThisMethodBeforeSetInstances_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.AreEqual(t1 => t1.Prop1, t2 => t2.Prop1)
            );
    }

    [Fact]
    public void AreEqual_FirstPropertyIsNull_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.AreEqual(null, t2 => t2.Prop1)
            );
    }

    [Fact]
    public void AreEqual_SecondPropertyIsNull_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.AreEqual(t1 => t1.Prop1, null)
            );
    }

    [Fact]
    public void AreEqual_OnePropHasValueWhileOtherIsNull_IsValidReturnsFalse()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        test2.Prop2 = null;
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);

        bool result = _comparer
            .AreEqual(t1 => t1.Prop2, t2 => t2.Prop2)
            .IsValid;

        Assert.False(result);
    }

    private (Test1 test1, Test2 test2) GetTestsInstances()
    {
        var test1 = new Test1
        {
            Prop1 = 1,
            Prop2 = "Test",
            Prop3 = new List<int> { 1, 2, 3 }
        };
        var test2 = new Test2
        {
            Prop1 = 1,
            Prop2 = "Test",
            Prop3 = new List<int> { 1, 2, 3 }
        };

        return (test1, test2);
    }

    [Fact]
    public void AreEqual_BothPropertiesAreNull_IsValidReturnsTrue()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        test1.Prop2 = null;
        test2.Prop2 = null;
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);

        bool result = _comparer
            .AreEqual(t1 => t1.Prop2, t2 => t2.Prop2)
            .IsValid;

        Assert.True(result);
    }

    [Fact]
    public void AreEqual_FirstPropertyAreNotEqualButSecondAre_IsValidReturnsFalse()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        test1.Prop1 = 999;
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);

        bool result = _comparer
            .AreEqual(t1 => t1.Prop1, t2 => t2.Prop1)
            .AreEqual(t1 => t1.Prop2, t2 => t2.Prop2)
            .IsValid;

        Assert.False(result);
    }

    [Fact]
    public void AreEqual_AllPropertiesAreEqual_IsValidReturnsTrue()
    {
        InitComparerWithEqualInstances();

        bool result = _comparer
            .AreEqual(t1 => t1.Prop1, t2 => t2.Prop1)
            .AreEqual(t1 => t1.Prop2, t2 => t2.Prop2)
            .IsValid;

        Assert.True(result);
    }

    private void InitComparerWithEqualInstances()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);
    }

    [Fact]
    public void AreEqualIEnumerable_CallingThisMethodBeforeSetInstances_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3)
            );
    }

    [Fact]
    public void AreEqualIEnumerable_FirstPropertyIsNull_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.AreEqualIEnumerable(null, t2 => t2.Prop3)
            );
    }

    [Fact]
    public void AreEqualIEnumerable_SecondPropertyIsNull_ThrowsArgumentNullException()
    {
        _comparer = new CustomComparer<Test1, Test2>();

        Assert
            .Throws<ArgumentNullException>(() =>
                _comparer.AreEqualIEnumerable(t1 => t1.Prop3, null)
            );
    }

    [Fact]
    public void AreEqualIEnumerable_OnePropHasValueWhileOtherIsNull_IsValidReturnsFalse()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        test2.Prop3 = null;
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);

        bool result = _comparer
            .AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3)
            .IsValid;

        Assert.False(result);
    }

    [Fact]
    public void AreEqualIEnumerable_BothPropertiesAreNull_IsValidReturnsTrue()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        test1.Prop3 = null;
        test2.Prop3 = null;
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);

        bool result = _comparer
            .AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3)
            .IsValid;

        Assert.True(result);
    }

    [Fact]
    public void AreEqualIEnumerable_ArraysHaveDifferentLength_IsValidReturnsFalse()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        test2.Prop3.Add(999);
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);

        bool result = _comparer
            .AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3)
            .IsValid;

        Assert.False(result);
    }

    [Fact]
    public void AreEqualIEnumerable_ArraysHaveOneOrMoreDifferentValue_IsValidReturnsFalse()
    {
        (Test1 test1, Test2 test2) = GetTestsInstances();
        test2.Prop3[1] = 999;
        _comparer = new CustomComparer<Test1, Test2>(test1, test2);

        bool result = _comparer
            .AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3)
            .IsValid;

        Assert.False(result);
    }

    [Fact]
    public void AreEqualIEnumerable_ArraysHaveTheSameValues_IsValidReturnsTrue()
    {
        InitComparerWithEqualInstances();

        bool result = _comparer
            .AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3)
            .IsValid;

        Assert.True(result);
    }
}
