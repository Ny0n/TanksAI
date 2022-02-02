using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Special/Settings")]
public class SettingsSO : ScriptableObject
{
    public enum GameModeType
    {
        FirstToMaximumPointsWin,
        MostPointsWin
    }

    [field: Header("Default values")]
    
    [field: SerializeField]
    public GameModeType GameMode { get; private set; } = GameModeType.FirstToMaximumPointsWin;
    
    [field: Header("FirstToMaximumPointsWin Data")]
    
    [field: SerializeField]
    public float MaximumPoints { get; private set; } = 50f;
    
    [field: Header("MostPointsWin Data")]
    
    [field: SerializeField] [Description("Play time in min")]
    public float PlayTime { get; private set; } = 5.0f;
}
