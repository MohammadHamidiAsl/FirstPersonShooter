using UnityEngine;

namespace GameSystems.FPS
{
    [CreateAssetMenu(menuName = "Data/Player/WeaponSway",fileName = "SwaySetting")]
    public class WeaponSwaySettingData : ScriptableObject
    {
        public float swayAmount = 0.02f;
        public float maxSwayAmount = 0.06f;
        public float swaySmoothValue = 4.0f;
    
    }
}