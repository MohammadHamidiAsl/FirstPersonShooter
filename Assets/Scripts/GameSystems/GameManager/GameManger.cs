using System;
using System.Threading;
using GameSystem.Core;
using GameSystem.Core.ServiceLocator;
using GameSystems.AssetLoader;
using GameSystems.FPS;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameSystems.GameManager
{
    public class GameManager : MonoBehaviour
    {
       
        
        
        [SerializeField] private Transform playerSpawnPoint;
        [SerializeField] private Camera DefaultCamera;
        [SerializeField] private GameObject courser;
        private FPSController player;
        private AssetLoaderService addressableManager;

        private void Start()
        {
            
            
            Initialization();
        }

        void Initialization()
        {
            addressableManager = new AssetLoaderService();
            addressableManager.InstantiateAddressablePrefab("audiomanager", OnAudioManagerInstantiated);
        }

        private void OnAudioManagerInstantiated(GameObject instantiatedPrefab)
        {
            if (instantiatedPrefab != null)
            {
                var audioManagerObject = Instantiate(instantiatedPrefab, transform.position, Quaternion.identity);
                var audioManager = audioManagerObject.GetComponent<AudioManagementService>();
                addressableManager.LoadAddressableAsset<AudioData>("AudioData", (r) =>
                {
                    audioManager.Initialize(r);
                    ServiceLocator.Instance.Register<IAudioManagementService, AudioManagementService>(audioManager);
                    CreatePlayer();
                });
            }
            else
            {
                Debug.LogError("Failed to instantiate AudioManager.");
            }
        }

        private void OnPlayerInstantiate(GameObject instantiatedPrefab)
        {
            if (instantiatedPrefab != null)
            {
                var playerObject = Instantiate(instantiatedPrefab);

                playerObject.transform.position = playerSpawnPoint.position;

                player = playerObject.GetComponent<FPSController>();


                 player.Initialize();
                 DisableDefaultCamera();
                 ShowCrosshair();
                
            }
            else
            {
                Debug.LogError("Failed to instantiate AudioManager.");
            }
        }

        private void CreatePlayer()
        {
            addressableManager.InstantiateAddressablePrefab("player", OnPlayerInstantiate);
        }

        private void DisableDefaultCamera()
        {
            DefaultCamera.gameObject.SetActive(false);
        }

        private void ShowCrosshair()
        {
            courser.SetActive(true);
        }
        private void HideCrosshair()
        {
            courser.SetActive(false);
        }
    }
}