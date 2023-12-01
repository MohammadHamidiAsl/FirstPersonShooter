using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Setting",fileName = "PlayerSetting")]
public class PlayerSettingData : ScriptableObject
{
    public float speed = 5.0f;
    public float runSpeed = 10.0f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 2.0f;
    public float mouseSensitivity = 2.0f;

}