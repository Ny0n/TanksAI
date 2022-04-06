using UnityEngine;

public class ObservableVariableSO<T> : ScriptableObject
{
    [SerializeField] private T _value;
    
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
    
    public void SetValueWithoutNotify(T value)
    {
        _value = value;
    }
}
