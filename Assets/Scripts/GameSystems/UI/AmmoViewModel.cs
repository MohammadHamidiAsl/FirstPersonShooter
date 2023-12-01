using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Controller/UI/AmmoViewModel")]
public class AmmoViewModel : ScriptableObject
{
    public AmmoModel AmmoData;

    public Action OnUpdate;

    public void UpdateAmmo(int current, int remaining, int AmmoCount)
    {
        AmmoData.CurrentBullets = current;
        AmmoData.RemainingBullets = remaining;
        AmmoData.AmmoCount = AmmoCount;
        // Notify the view to update the UI
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        OnUpdate?.Invoke();
    }
}