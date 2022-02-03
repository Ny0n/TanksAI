
using UnityEngine;

public class GenericVariableSetter<T> : BaseVariableSetter
{
    [SerializeField] private GenericVariableSO<T> _variable;
    [SerializeField] private T _component;
    
    // Not using OnEnable and OnDisable because of potentially disabled game objects

    public override void Set()
    {
        _variable.Value = _component;
    }

    public override void Unset()
    {
        _variable.Value = default;
    }
}
