using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GenericEventListener
{
    #region SINGLE event for ONE unity event

    // [SerializeField]
    // private GenericEventSO _event;
    //
    // [SerializeField]
    // private UnityEvent<GenericEventSO> _onEventRaised;
    //
    // public void OnEventRaised(GenericEventSO evt)
    // {
    //     _onEventRaised.Invoke(evt);
    // }
    //
    // public void Enable()
    // {
    //     _event.RegisterListener(this);
    // }
    //
    // public void Disable()
    // {
    //     _event.UnregisterListener(this);
    // }
    
    #endregion

    #region MULTIPLE events for ONE unity event

    [SerializeField]
    private List<GenericEventSO> _events;
    
    [SerializeField]
    private UnityEvent<GenericEventSO> _onEventRaised;
    
    public void OnEventRaised(GenericEventSO evt)
    {
        _onEventRaised.Invoke(evt);
    }
    
    public void Enable()
    {
        foreach (var evt in _events)
        {
            evt.RegisterListener(this);
        }
    }
    
    public void Disable()
    {
        foreach (var evt in _events)
        {
            evt.UnregisterListener(this);
        }
    }
    
    #endregion
}
