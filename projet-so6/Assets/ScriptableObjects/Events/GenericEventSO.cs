using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class  GenericEventSO : ScriptableObject
{
    [SerializeField]
    private List<GenericEventListener> _listeners;

    public void Raise()
    {
        _answered = 0;
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i]?.OnEventRaised(this);
        }
    }

    public void RegisterListener(GenericEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(GenericEventListener listener)
    {
        _listeners.Remove(listener);
    }

    #region Answers System

    public void Answer() => _answered++;
    public bool HasEveryoneAnswered => _answered >= _listeners.Count;
    
    public IEnumerator RaiseAndWait()
    {
        this.Raise();
        while (!this.HasEveryoneAnswered)
            yield return null;
    }
    
    public IEnumerator WaitForSelfAnswers()
    {
        while (!this.HasEveryoneAnswered)
            yield return null;
    }

    public IEnumerator WaitForEventAnswers(GenericEventSO otherEvent)
    {
        while (!otherEvent.HasEveryoneAnswered)
            yield return null;
        this.Answer();
    }
    
    public IEnumerator WaitForEventsAnswers(List<GenericEventSO> otherEvents)
    {
        while (otherEvents.Any(evt => !evt.HasEveryoneAnswered))
            yield return null;
        this.Answer();
    }
    
    private int _answered;

    #endregion
}
