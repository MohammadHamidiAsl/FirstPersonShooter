using GameSystem.Core.ServiceLocator;
using UnityEngine;
using GameSystem.Factory;
using GameSystem.StateManager;
using GameSystem.StateManger;
using GameSystems.FPS;
using UnityEngine.Serialization;


namespace GameSystems.FPS
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSController : MonoBehaviour
    {
        [Header("Player Settings")] [SerializeField]
        private Transform playerCamera;

        [SerializeField] private PlayerSettingData playerSettingData;

        [SerializeField] private WeaponManager weaponManager;

        [Header("Weapon Sway Settings")] [SerializeField]
        private Transform weapon;

        [SerializeField] private SwaySetting swaySetting;

        [Header("Animation Component")] [SerializeField]
        private Animation characterAnimation;

        [SerializeField] private CharacterController characterController;
        private float pitch;
        private bool isCrouching;
        private float originalHeight;
        private Vector3 initialWeaponPosition;
        private StateFactoryServiceService _characterStateFactoryServiceService;
        private CharacterStateManagerService stateManager;
        private bool isBurstFiring;

        [FormerlySerializedAs("isInitilized")] [SerializeField]
        private bool isInitialized = false;

        public void Initialize()
        {
            SetupCharacterController();
            SetupWeapon();
            SetupWeaponSway();
            InitializeStateManager();
            isInitialized = true;
        }

        private void SetupWeapon()
        {
            weaponManager = GetComponent<WeaponManager>();
        }

        void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            HandleLook();
            HandleWeaponSway();

            Vector3 movement = HandleMovement();
            HandleActionInput();

            characterController.Move(movement * Time.deltaTime);
            stateManager.Update();
        }

        private void SetupCharacterController()
        {
            characterController = GetComponent<CharacterController>();
            originalHeight = characterController.height;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SetupWeaponSway()
        {
            if (weapon != null)
            {
                initialWeaponPosition = weapon.localPosition;
            }
        }

        private void InitializeStateManager()
        {
            _characterStateFactoryServiceService = new StateFactoryServiceService();
            _characterStateFactoryServiceService.RegisterCharacterState<IdleState>();
            _characterStateFactoryServiceService.RegisterCharacterState<WalkState>();
            _characterStateFactoryServiceService.RegisterCharacterState<RunningState>();
            _characterStateFactoryServiceService.RegisterCharacterState<ReloadState>();
            _characterStateFactoryServiceService.RegisterCharacterState<FireState>();
            _characterStateFactoryServiceService.RegisterCharacterState<BurstFire>();
            _characterStateFactoryServiceService.RegisterCharacterState<OutOfAmmo>();


            stateManager = new CharacterStateManagerService(characterAnimation, _characterStateFactoryServiceService);
            ServiceLocator.Instance.Register<IStateManagerService, CharacterStateManagerService>(stateManager);
            stateManager.ChangeState(_characterStateFactoryServiceService.CreateCharacterState<IdleState>());
        }

        private Vector3 HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            var characterTransform = transform;
            Vector3 movement = characterTransform.right * horizontal + characterTransform.forward * vertical;

            movement = Vector3.ClampMagnitude(movement, 1);

            float currentSpeed = isCrouching
                ? playerSettingData.crouchSpeed
                : (Input.GetKey(KeyCode.LeftShift) ? playerSettingData.runSpeed : playerSettingData.speed);
            movement *= currentSpeed;

            movement = ApplyGravityAndJump(movement);

            return movement;
        }

        private Vector3 ApplyGravityAndJump(Vector3 movement)
        {
            if (!characterController.isGrounded)
            {
                movement += Physics.gravity * Time.deltaTime;
            }
            else if (Input.GetButtonDown("Jump") && !isCrouching)
            {
                movement.y = Mathf.Sqrt(playerSettingData.jumpHeight * -2f * Physics.gravity.y);
            }

            return movement;
        }

        private void HandleActionInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ToggleCrouch();
            }

            // Add method calls for handling firing and reloading...
            HandleStateTransitions();
        }

        private void HandleLook()
        {
            var mouseX = Input.GetAxis("Mouse X") * playerSettingData.mouseSensitivity;
            var mouseY = Input.GetAxis("Mouse Y") * playerSettingData.mouseSensitivity;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
            transform.Rotate(Vector3.up * mouseX);
            playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);
        }

        private void HandleWeaponSway()
        {
            if (weapon == null) return;

            var movementX = -Input.GetAxis("Mouse X") * swaySetting.swayAmount;
            var movementY = -Input.GetAxis("Mouse Y") * swaySetting.swayAmount;
            movementX = Mathf.Clamp(movementX, -swaySetting.maxSwayAmount, swaySetting.maxSwayAmount);
            movementY = Mathf.Clamp(movementY, -swaySetting.maxSwayAmount, swaySetting.maxSwayAmount);

            Vector3 finalPosition = new Vector3(movementX, movementY, 0);
            weapon.localPosition = Vector3.Lerp(weapon.localPosition, initialWeaponPosition + finalPosition,
                Time.deltaTime * swaySetting.swaySmoothValue);
        }

        private void ToggleCrouch()
        {
            isCrouching = !isCrouching;
            characterController.height = isCrouching ? originalHeight / 2 : originalHeight;
        }

        private void HandleStateTransitions()
        {
            HandleFiring();
            HandleReloading();
            HandleMovementState();
        }

        private bool CanChangeState()
        {
            return stateManager.currentState is not ReloadState;
        }

        private void HandleFiring()
        {
            if (Input.GetButtonUp("Fire1"))
            {
                if (weaponManager.currentWeapon.AmmoCount <= 0)
                {
                    ChangeStateTo<OutOfAmmo>();
                    return;
                }

                weaponManager.FireWeapon(Reload);
                isBurstFiring = false;
                ChangeStateTo<FireState>();
            }
            else if (Input.GetButtonDown("Fire1") && !isBurstFiring)
            {
                isBurstFiring = true;
                ChangeStateTo<BurstFire>();
            }
        }

        public void HandleReloading()
        {
            if (Input.GetKeyDown(KeyCode.R) && !(stateManager.currentState is ReloadState))
            {
                Reload();
            }
        }


        private void HandleMovementState()
        {
            if (IsMoving())
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ChangeStateTo<RunningState>();
                }
                else
                {
                    ChangeStateTo<WalkState>();
                }
            }
            else
            {
                if (CanChangeState())
                {
                    ChangeStateTo<IdleState>();
                }
            }
        }

        private void ChangeStateTo<T>() where T : CharacterState, new()
        {
            var newState = stateManager.FactoryService.CreateCharacterState<T>();
            if (stateManager.currentState.GetType() != typeof(T))
            {
                stateManager.ChangeState(newState);
            }
        }

        private bool IsMoving()
        {
            return characterController.velocity.magnitude > 0.01f;
        }


        private void Reload()
        {
            stateManager.ChangeState(stateManager.FactoryService.CreateCharacterState<ReloadState>());
            weaponManager.ReloadWeapon();
        }
    }
}