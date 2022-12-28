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
        Expression<Func<T1, TProperty>> firstProperty,
        Expression<Func<T2, TProperty>> secondProperty
    )
    {
        CheckInstances();

        if (!IsValid)
            return this;
        if (firstProperty is null)
            throw new ArgumentNullException(nameof(firstProperty));
        if (secondProperty is null)
            throw new ArgumentNullException(nameof(secondProperty));

        TProperty prop1Value = firstProperty
            .Compile().Invoke(_firstInstance);
        TProperty prop2Value = secondProperty
            .Compile().Invoke(_secondInstance);

        IsValid = prop1Value.Equals(prop2Value);

        return this;
    }

    public void CheckInstances()
    {
        if (_firstInstance is null || _secondInstance is null)
            throw new ArgumentNullException(
                "Instances are null, you must call first SetInstances method"
            );
    }

    public ICustomComparer<T1, T2> AreEqualIEnumerable<TProperty>(
        Expression<Func<T1, IEnumerable<TProperty>>> firstProperty,
        Expression<Func<T2, IEnumerable<TProperty>>> secondProperty
    )
    {
        CheckInstances();

        if (!IsValid)
            return this;
        if (firstProperty is null)
            throw new ArgumentException("First property cannot be null");
        if (secondProperty is null)
            throw new ArgumentException("Second property cannot be null");

        TProperty[] prop1Value = firstProperty
            .Compile().Invoke(_firstInstance).ToArray();
        TProperty[] prop2Value = secondProperty
            .Compile().Invoke(_secondInstance).ToArray();

        if (prop1Value.Length != prop2Value.Length)
            IsValid = false;

        for (int i = 0; i < prop1Value.Length; i++)
            if (!prop1Value[i].Equals(prop2Value[i]))
            {
                IsValid = false;
                break;
            }

        return this;
    }
}
