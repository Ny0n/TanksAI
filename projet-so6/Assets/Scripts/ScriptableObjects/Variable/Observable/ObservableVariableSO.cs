using UnityEngine;

public class ObservableVariableSO<T> : ScriptableObject
{
    [SerializeField] private T _value; // simple type value, shown in the inspector
    
    public T Value
    {
        get => _value;
        set => SetValue(value);
    }

    public event System.Action ValueChanged;
    
    public void NotifyChange() => ValueChanged?.Invoke();

    public void SetValue(T value)
    {
        T oldValue = _value;
        _value = value;
        
        if (!Equals(oldValue, _value))
            ValueChanged?.Invoke();
    }
}
