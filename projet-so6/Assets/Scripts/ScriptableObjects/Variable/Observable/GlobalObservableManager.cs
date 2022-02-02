using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObservableManager : MySingleton<GlobalObservableManager>
{
    // protected override bool DoDestroyOnLoad { get; set; } = false;

    [SerializeField] private VarUpdatedEventSO _varUpdatedEvent;

    public void VarUpdated(object var)
    {
        _varUpdatedEvent.SetAndRaise(var);
    }
}
