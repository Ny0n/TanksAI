using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Special/Team")]
public class TeamSO : ScriptableObject
{
    [Header("Properties")]
    
    [SerializeField] private string _name = "Base";
    public string Name => _name;
    
    [SerializeField] private Color _color = Color.white;
    public Color Color => _color;
    
    [SerializeField] private int _nbPlayers;
    public int NbPlayers => _nbPlayers;
    
    [Header("Scene data")]
    
    [SerializeField] private TransformVariableSO _spawnTransform;
    public Transform SpawnPoint => _spawnTransform.Value;
    
    [Header("Game data")]
    
    [SerializeField] private float _points;
    public float Points
    {
        get => _points;
        set => _points = value;
    }

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        _tanks = new List<TankManager>();
        _points = 0;
    }

    public void NotifyChange()
    {
        GlobalObservableManager.Instance.VarUpdated(this);
    }
    
    private List<TankManager> _tanks;
    public List<TankManager> Tanks => _tanks;
}
