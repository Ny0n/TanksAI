using UnityEngine;

public class BaseVariableGlobalSetter : MonoBehaviour
{
    private BaseVariableSetter[] _setters;
    
    private void Awake()
    {
        _setters = FindObjectsOfType<BaseVariableSetter>();
    }

    private void OnEnable()
    {
        foreach (var setter in _setters)
        {
            setter.Set();
        }
    }
    
    private void OnDisable()
    {
        foreach (var setter in _setters)
        {
            setter.Unset();
        }
    }
}
