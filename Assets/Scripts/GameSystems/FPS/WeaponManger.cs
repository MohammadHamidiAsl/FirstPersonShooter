using System;
using UnityEngine;
using System.Collections;
using GameSystem.Pool;


namespace GameSystems.FPS
{
    public class WeaponManager : MonoBehaviour
    {
        public Weapon currentWeapon;
        public AmmoViewModel AmmoViewModel;
        public ParticleSystem muzzleFlashEffect; 
        public GameObject bulletImpactEffectPrefab; 
        public Camera mainCamera;
        private GameObjectPool gameObjectPool;
        private WaitForSeconds w8;

        private void Start()
        {
            AmmoViewModel.AmmoData.CurrentBullets = currentWeapon.CurrentAmmo;
            AmmoViewModel.AmmoData.RemainingBullets = currentWeapon.CartridgeCapcity;
            AmmoViewModel.AmmoData.AmmoCount = currentWeapon.AmmoCount;
            gameObjectPool = new GameObjectPool(bulletImpactEffectPrefab, 10);
            mainCamera = Camera.main;
            w8 = new WaitForSeconds(1f);
        }

        public void ChangeWeapon(Weapon newWeapon)
        {
            currentWeapon = newWeapon;
            // Additional logic for changing weapons
        }

        public void FireWeapon(Action accAction)
        {
            if (currentWeapon != null && currentWeapon.CurrentAmmo > 0)
            {
                // Play muzzle flash
                muzzleFlashEffect.Play();

                HandleGunShot();

                // Update ammo count
                currentWeapon.CurrentAmmo--;
                currentWeapon.AmmoCount--;

                AmmoViewModel.UpdateAmmo(currentWeapon.CurrentAmmo, currentWeapon.CartridgeCapcity,
                    currentWeapon.AmmoCount);
            }
            else if (currentWeapon.AmmoCount > 0)
            {
                accAction?.Invoke();
            }
        }

        private void HandleGunShot()
        {
            RaycastHit hit;
            // Raycast from the center of the camera
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out hit))
            {
                // Instantiate bullet impact effect at the hit point

                GameObject bulletImpact = gameObjectPool.Get();
                bulletImpact.transform.position = hit.point;
                bulletImpact.transform.rotation = Quaternion.LookRotation(hit.normal);
                bulletImpact.SetActive(true);


                StartCoroutine(DeactivateBulletImpact(bulletImpact));
            }
        }

        public void ReloadWeapon()
        {
            if (currentWeapon != null)
            {
                currentWeapon.Reload();
                AmmoViewModel.UpdateAmmo(currentWeapon.CurrentAmmo, currentWeapon.CartridgeCapcity,
                    currentWeapon.AmmoCount);
            }
        }

        private IEnumerator DeactivateBulletImpact(GameObject bulletImpact)
        {
            yield return w8;
            gameObjectPool.Release(bulletImpact);
        }
    }
}