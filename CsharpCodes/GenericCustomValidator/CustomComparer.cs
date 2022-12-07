namespace GenericCustomComparer
{
    public class CustomComparer<T1, T2>
    {
        private readonly T1 _firstInstance;
        private readonly T2 _secondInstance;
        private bool isValid = true;

        public CustomComparer(T1 instance1, T2 instance2)
        {
            if (instance1 is null)
                throw new ArgumentNullException(nameof(instance1));

            if (instance2 is null)
                throw new ArgumentNullException(nameof(instance2));

            _firstInstance = instance1;
            _secondInstance = instance2;
        }
        public CustomComparer<T1, T2> AreEqual<TProperty>(
            Func<T1, TProperty> firstProperty,
            Func<T2, TProperty> secondProperty
        )
        {
            if (!isValid)
                return this;

            if (firstProperty is null)
                throw new ArgumentNullException(nameof(firstProperty));
            if (secondProperty is null)
                throw new ArgumentNullException(nameof(secondProperty));

            TProperty prop1Value = firstProperty.Invoke(_firstInstance);
            TProperty prop2Value = secondProperty.Invoke(_secondInstance);

            isValid = prop1Value.Equals(prop2Value);

            return this;
        }

        public CustomComparer<T1, T2> AreEqualIEnumerable<TProperty>(
            Func<T1, IEnumerable<TProperty>> firstProperty,
            Func<T2, IEnumerable<TProperty>> secondProperty
        )
        {
            if (!isValid)
                return this;

            if (firstProperty is null)
                throw new ArgumentException("First property cannot be null");
            if (secondProperty is null)
                throw new ArgumentException("Second property cannot be null");

            TProperty[] prop1Value = firstProperty.Invoke(_firstInstance).ToArray();
            TProperty[] prop2Value = secondProperty.Invoke(_secondInstance).ToArray();

            if (prop1Value.Length != prop2Value.Length)
                isValid = false;

            for (int i = 0; i < prop1Value.Length; i++)
                if (!prop1Value[i].Equals(prop2Value[i]))
                {
                    isValid = false;
                    break;
                }

            return this;
        }

        public bool Isvalid() => isValid;
    }
}
