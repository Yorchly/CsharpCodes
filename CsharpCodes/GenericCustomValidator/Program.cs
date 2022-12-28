// See https://aka.ms/new-console-template for more information

using GenericCustomComparer;

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

CustomComparer<Test1, Test2> customComparer = new();
customComparer.SetInstances(test1, test2);

customComparer
    .AreEqual(t1 => t1.Prop1, t2 => t2.Prop1)
    .AreEqual(t1 => t1.Prop2, t2 => t2.Prop2)
    .AreEqualIEnumerable(t1 => t1.Prop3, t2 => t2.Prop3);

Console.WriteLine(customComparer.IsValid);

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


