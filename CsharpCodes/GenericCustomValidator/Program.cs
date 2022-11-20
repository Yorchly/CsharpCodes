// See https://aka.ms/new-console-template for more information

var test1 = new Test1
{
    Prop1 = 1,
    Prop2 = "Test",
    Prop3 = new List<int> { 1, 2 },
};

var test2 = new Test2
{
    Prop1 = 1,
    Prop2 = "Test",
    Prop3 = new List<int> { 1, 2 },
};

CustomValidator<Test1, Test2> customValidator = new(test1, test2);

customValidator
    .AreEqual(t1 => t1.Prop1, t2 => t2.Prop1)
    .AreEqual(t1 => t1.Prop2, t2 => t2.Prop2)
    .AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3);

Console.WriteLine(customValidator.Isvalid());

public class Test1
{
    public int Prop1 { get; set; }
    public string Prop2 { get; set; }
    public List<int> Prop3 { get; set; }
}

public class Test2
{
    public int Prop1 { get; set; }
    public string Prop2 { get; set; }
    public List<int> Prop3 { get; set; }
}

public class CustomValidator<T1, T2>
{
    private readonly T1 _firstInstance;
    private readonly T2 _secondInstance;
    private readonly List<bool> isValid = new();

    public CustomValidator(T1 instance1, T2 instance2)
    {
        if (instance1 is null)
            throw new ArgumentException("Instance 1 cannot be null");

        if (instance2 is null)
            throw new ArgumentException("Instance 2 cannot be null");

        _firstInstance = instance1;
        _secondInstance = instance2;
    }
    public CustomValidator<T1, T2> AreEqual<TProperty>(
        Func<T1, TProperty> firstProperty, 
        Func<T2, TProperty> secondProperty
    )
    {
        if (firstProperty is null)
            throw new ArgumentException("First property cannot be null");
        if (secondProperty is null)
            throw new ArgumentException("Second property cannot be null");

        TProperty prop1Value = firstProperty.Invoke(_firstInstance);
        TProperty prop2Value = secondProperty.Invoke(_secondInstance);

        isValid.Add(prop1Value.Equals(prop2Value));

        return this;
    }

    public CustomValidator<T1, T2> AreEqualIEnumerable<TProperty>(
        Func<T1, IEnumerable<TProperty>> firstProperty, 
        Func<T2, IEnumerable<TProperty>> secondProperty
    )
    {
        if (firstProperty is null)
            throw new ArgumentException("First property cannot be null");
        if (secondProperty is null)
            throw new ArgumentException("Second property cannot be null");

        TProperty[] prop1Value = firstProperty.Invoke(_firstInstance).ToArray();
        TProperty[] prop2Value = secondProperty.Invoke(_secondInstance).ToArray();

        if (prop1Value.Length != prop2Value.Length)
            isValid.Add(false);

        for(int i = 0; i < prop1Value.Length; i++)
            if (!prop1Value[i].Equals(prop2Value[i]))
            {
                isValid.Add(false);
                break;
            }

        return this;
    }

    public bool Isvalid() =>
        isValid.All(element => element is true);
}
