using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class ObservableListSO<T> : ScriptableObject
{
    [SerializeField] private GenericEventListener _evtListener;
    
    [SerializeField] public ObservableList2<T> _value;
    
    public ObservableList2<T> Value
    {
        get => _value;
        set => SetValue(value);
    }

    public void AddObserver(Action action)
    {
        Debug.Log("Add observer");
        if (_value == null)
            Debug.Log("what");
        _value.ValueChanged += action;
    }
    
    public void RemoveObserver(Action action)
    {
        _value.ValueChanged -= action;
    }

    public void NotifyChange() => _value.NotifyChange();

    public void SetValue(ObservableList2<T> value)
    {
        var oldValue = _value;
        _value = value;

        if (!Equals(oldValue, _value))
            NotifyChange();
    }

    public void Init()
    {
        Debug.Log("NANI");
        _value = new ObservableList2<T>(); // replaces everything, including ValueChanged!
    }

    private void OnEnable()
    {
        _evtListener.Enable();
    }
    
    private void OnDisable()
    {
        _evtListener.Disable();
    }

    public void OnVarUpdated(GenericEventSO evt) // Called by the evt
    {
        VarUpdatedEventSO e = (VarUpdatedEventSO) evt;
        if (e.Var == _value) // base var
        {
            Debug.Log("Observed List Changed");
            NotifyChange();
        }
        else // list items
        {
            if (e.Type == typeof(T))
            {
                if (_value.Contains((T) e.Var))
                {
                    Debug.Log(typeof(T).Name);
                    Debug.Log(e.Type.Name);
                    Debug.Log("Observed Content of List Changed");
                    NotifyChange();
                }
            }
        }
    }
    
}
