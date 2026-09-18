

using UnityEngine;

public interface IControlsSettings
{
    KeyCode AimLeftKey { get; }
    KeyCode AimRightKey { get; }
    KeyCode AttackKey { get; }
}

[CreateAssetMenu(fileName = "Controls Settings", menuName = "Settings/Controls Settings")]
public class ControlsSettings : ScriptableObject, IControlsSettings
{
    [field: SerializeField] public KeyCode AimLeftKey { get; set; } = KeyCode.A;
    [field: SerializeField] public KeyCode AimRightKey { get; set; } = KeyCode.D;
    [field: SerializeField] public KeyCode AimUpKey { get; set; } = KeyCode.W;
    [field: SerializeField] public KeyCode AimDownKey { get; set; } = KeyCode.S;

    [field: SerializeField] public KeyCode AttackKey { get; set; } = KeyCode.J;
}

