using UnityEngine;

namespace GameSystems.FPS
{
    [CreateAssetMenu(menuName = "Data/Weapons",fileName = "NewWeapon")]
    public class Weapon:ScriptableObject
    {
        public string Name;
        public int AmmoCount;
        public int CartridgeCapcity;
        public int CurrentAmmo;
        public int MaxAmmo;
        
        public float FireRate;
        public float ReloadTime;

        public void Reload()
        {
            CurrentAmmo = AmmoCount>=CartridgeCapcity ? CartridgeCapcity : AmmoCount;
        }
    }
}