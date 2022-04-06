using UnityEngine;

[CreateAssetMenu(menuName = "Event/Tank Death")]
public class TankDeathEventSO : GenericEventSO
{
    public TankManager Manager { get; private set; }
    
    public void SetAndRaise(TankManager tm)
    {
        Manager = tm;
        Raise();
    }
}
