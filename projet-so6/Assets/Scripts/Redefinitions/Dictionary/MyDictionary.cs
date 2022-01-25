using System.Collections.Generic;

public class MyDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public event System.Action ValueChanged;

    public void NotifyChange() => ValueChanged?.Invoke();

    public new TValue this[TKey key] {
        get => base[key];
        set
        {
            base[key] = value;
            ValueChanged?.Invoke();
        }
    }

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        ValueChanged?.Invoke();
    }

    public new bool Remove(TKey key)
    {
        bool result = base.Remove(key);
        ValueChanged?.Invoke();
        return result;
    }

    public new void Clear()
    {
        base.Clear();
        ValueChanged?.Invoke();
    }
}
