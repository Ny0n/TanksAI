using UnityEngine;

[CreateAssetMenu(menuName = "Special/Cooldown")]
public class CooldownSO : ScriptableObject
{
    [SerializeField] [Tooltip("Length in s")]
    private float _cooldownDuration;

    private void OnEnable()
    {
        _nextCooldownDate = Time.time;
    }

    public void StartCooldown()
    {
        _nextCooldownDate = Time.time + _cooldownDuration;
    }

    public bool IsCooldownDone()
    {
        return Time.time > _nextCooldownDate;
    }
    
    private float _nextCooldownDate;
}
