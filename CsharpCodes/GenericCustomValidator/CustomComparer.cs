using System.Linq.Expressions;

namespace GenericCustomComparer;

public class CustomComparer<T1, T2> : ICustomComparer<T1, T2>
{
    public bool IsValid 
    { 
        get
        {
            CheckInstances();

            return _isValid;
        }
        private set => _isValid = value; 
    }

    private T1 _firstInstance;
    private T2 _secondInstance;
    private bool _isValid = true;

    public void SetInstances(T1 firstInstance, T2 secondInstance)
    {
        if (firstInstance is null)
            throw new ArgumentNullException(nameof(firstInstance));

        if (secondInstance is null)
            throw new ArgumentNullException(nameof(secondInstance));

        _firstInstance = firstInstance;
        _secondInstance = secondInstance;
    }

    public ICustomComparer<T1, T2> AreEqual<TProperty>(
        Expression<Func<T1, TProperty?>> firstProperty,
        Expression<Func<T2, TProperty?>> secondProperty
    )
    {
        CheckInstances();
        CheckProperties(firstProperty, secondProperty);

        if (!IsValid)
            return this;

        TProperty? prop1Value = firstProperty
            .Compile().Invoke(_firstInstance);
        TProperty? prop2Value = secondProperty
            .Compile().Invoke(_secondInstance);

        if (!ArePropertiesNullOrHasValue(prop1Value, prop2Value))
        {
            IsValid = false;
            return this;
        }

        if (prop1Value is null && prop2Value is null)
            return this;

        IsValid = prop1Value!.Equals(prop2Value);

        return this;
    }

    public void CheckInstances()
    {
        if (_firstInstance is null || _secondInstance is null)
            throw new ArgumentNullException(
                "Instances are null, you must call first SetInstances method"
            );
    }

    private static void CheckProperties<TProperty>(
        Expression<Func<T1, TProperty?>> firstProperty, 
        Expression<Func<T2, TProperty?>> secondProperty)
    {
        if (firstProperty is null)
            throw new ArgumentNullException($"{nameof(firstProperty)} cannot be null");
        if (secondProperty is null)
            throw new ArgumentNullException($"{nameof(secondProperty)} cannot be null");
    }

    private static bool ArePropertiesNullOrHasValue<TProperty>(
        TProperty? firstProp, TProperty? secondProp)
    {
        if (firstProp is null && secondProp is not null ||
            firstProp is not null && secondProp is null)
            return false;

        return true;
    }

    public ICustomComparer<T1, T2> AreEqualIEnumerable<TProperty>(
        Expression<Func<T1, IEnumerable<TProperty>>> firstProperty,
        Expression<Func<T2, IEnumerable<TProperty>>> secondProperty
    )
    {
        CheckInstances();
        CheckProperties(firstProperty, secondProperty);

        if (!IsValid)
            return this;

        IEnumerable<TProperty> prop1Value = firstProperty
            .Compile().Invoke(_firstInstance);
        IEnumerable<TProperty> prop2Value = secondProperty
            .Compile().Invoke(_secondInstance);

        if (!ArePropertiesNullOrHasValue(prop1Value, prop2Value))
        {
            IsValid = false;
            return this;
        }
        else if (prop1Value is null && prop2Value is null)
            return this;

        TProperty[] prop1ValueArray = prop1Value!.ToArray();
        TProperty[] prop2ValueArray = prop2Value!.ToArray();

        if (prop1ValueArray.Length != prop2ValueArray.Length)
            IsValid = false;

        for (int i = 0; i < prop1ValueArray.Length; i++)
            if (!prop1ValueArray[i]!.Equals(prop2ValueArray[i]))
            {
                IsValid = false;
                break;
            }

        return this;
    }

    private static bool ArePropertiesNullOrHasValue<TProperty>(
        IEnumerable<TProperty> firstProp, 
        IEnumerable<TProperty> secondProp)
    {
        if (firstProp is null && secondProp is not null ||
            firstProp is not null && secondProp is null)
            return false;

        return true;
    }

    private static void CheckProperties<TProperty>(
        Expression<Func<T1, IEnumerable<TProperty>>> firstProperty,
        Expression<Func<T2, IEnumerable<TProperty>>> secondProperty)
    {
        if (firstProperty is null)
            throw new ArgumentNullException($"{nameof(firstProperty)} cannot be null");
        if (secondProperty is null)
            throw new ArgumentNullException($"{nameof(secondProperty)} cannot be null");
    }
}
