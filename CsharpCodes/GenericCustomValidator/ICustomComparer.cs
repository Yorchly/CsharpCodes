using System.Linq.Expressions;

namespace GenericCustomComparer;

public interface ICustomComparer<T1, T2>
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>true if properties compared are all equal; false otherwise.</returns>
    public bool IsValid { get; }
    /// <summary>
    /// Set instances to compare. IMPORTANT: you must call this method first
    /// </summary>
    /// <param name="firstInstance"></param>
    /// <param name="secondInstance"></param>
    public void SetInstances(T1 firstInstance, T2 secondInstance);
    /// <summary>
    /// Compare if properties of instances are equal.
    /// IMPORTANT: you must call SetInstances method first.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="firstProperty"></param>
    /// <param name="secondProperty"></param>
    /// <returns></returns>
    public ICustomComparer<T1, T2> AreEqual<TProperty>(
        Expression<Func<T1, TProperty?>> firstProperty,
        Expression<Func<T2, TProperty?>> secondProperty
    );
    /// <summary>
    /// Compare if IEnumerable properties of instances are equal. 
    /// IMPORTANT: you must call SetInstances method first. 
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="firstProperty"></param>
    /// <param name="secondProperty"></param>
    /// <returns></returns>
    public ICustomComparer<T1, T2> AreEqualIEnumerable<TProperty>(
        Expression<Func<T1, IEnumerable<TProperty?>>> firstProperty,
        Expression<Func<T2, IEnumerable<TProperty?>>> secondProperty
    );
}
