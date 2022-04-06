using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Decision : ScriptableObject
{
    public abstract bool DecideTransition(StateManager controller);
}
