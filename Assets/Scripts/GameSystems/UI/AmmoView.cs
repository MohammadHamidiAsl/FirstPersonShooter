using TMPro;
using UnityEngine;


public class AmmoView : MonoBehaviour
{
    public AmmoViewModel viewModel;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reamingAmmo;
    public TextMeshProUGUI ammoCount;

    void Start()
    {
        if (viewModel == null)
        {
            Debug.LogError("ViewModel is not assigned");
            return;
        }

        viewModel.OnUpdate += UpdateAmmoUI;

        // Initialize or update the view
        UpdateAmmoUI();
    }


    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{viewModel.AmmoData.CurrentBullets}";
        }

        if (reamingAmmo != null)
        {
            reamingAmmo.text = $"{viewModel.AmmoData.RemainingBullets}";
        }

        if (ammoCount != null)
        {
            ammoCount.text = $"{viewModel.AmmoData.AmmoCount}";
        }
    }
}