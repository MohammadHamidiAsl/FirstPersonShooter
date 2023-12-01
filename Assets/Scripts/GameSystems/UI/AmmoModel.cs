using UnityEngine;

[CreateAssetMenu(menuName = "Data/UI/AmmoModel")]
public class AmmoModel:ScriptableObject
{
    public int CurrentBullets { get; set; }
    public int RemainingBullets { get; set; }
    public int AmmoCount { get; set; }
    
}