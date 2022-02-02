using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/VarUpdated")]
public class VarUpdatedEventSO : GenericEventSO
{
    private object _var;

    public object Var => _var;
    
    public Type Type => _var.GetType();

    public void SetAndRaise(object var)
    {
        _var = var;
        base.Raise();
    }
}
