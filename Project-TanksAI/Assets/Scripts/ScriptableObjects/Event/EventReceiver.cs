using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    [SerializeField] private List<GenericEventListener> _eventListeners;

    private void OnEnable()
    {
        foreach (GenericEventListener _eventListener in _eventListeners)
        {
            _eventListener.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (GenericEventListener _eventListener in _eventListeners)
        {
            _eventListener.Disable();
        }
    }
}
