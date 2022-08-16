using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class CharacterControllerNew : MonoBehaviour
{
    public static CharacterControllerNew instance;

    private CharacterController characterController;
    private DefaultInput defaultInput;
    [HideInInspector]
    public Vector2 input_Movement;
    [HideInInspector]
    public Vector2 input_View;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    private Vector3 recoilRotation;
    private float recoilAmount;


    [Header("References")]
    public Transform cameraHolder;
    public Transform feetTransform;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70, viewClampYMax = 80;
    public LayerMask playerMask;
    public LayerMask groundMask;

    [Header("Gravity")]
    private float playerGravity;
    public float gravityAmount;
    public float gravityMin;

    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    [Header("Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;
    public CharacterStance playerStandStance;
    public CharacterStance playerCrouchStance;
    public CharacterStance playerProneStance;
    private float stanceCheckErrorMargin = 0.05f;
    private float cameraHeight;
    private float cameraHeightVelocity;
    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;

    [HideInInspector]
    public bool isSprinting;

    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;

    [Header("Weapon")]
    public WeaponControllerNew currentWeapon;

    public float weaponAnimationSpeed;

    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isFalling;

    [Header("ADS")]
    public bool isAimingDownSights;

    void Awake()
    {
        instance = this;

        defaultInput = new DefaultInput();

        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();
        defaultInput.Character.Crouch.performed += e => Crouch();
        defaultInput.Character.Prone.performed += e => Prone();
        defaultInput.Character.Sprint.performed += e => ToggleSprint();
        defaultInput.Character.SprintReleased.performed += e => StopSprint();
        defaultInput.Weapon.Fire2Pressed.performed += e => ADSPressed();
        defaultInput.Weapon.Fire2Released.performed += e => ADSReleased();     

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        if (currentWeapon)
        {
            currentWeapon.Initialize(this);
        }

        characterController = GetComponent<CharacterController>();
        cameraHeight = cameraHolder.localPosition.y;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        defaultInput.Character.Movement.performed -= e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed -= e => input_View = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed -= e => Jump();
        defaultInput.Character.Crouch.performed -= e => Crouch();
        defaultInput.Character.Prone.performed -= e => Prone();
        defaultInput.Character.Sprint.performed -= e => ToggleSprint();
        defaultInput.Character.SprintReleased.performed -= e => StopSprint();
        defaultInput.Weapon.Fire2Pressed.performed -= e => ADSPressed();
        defaultInput.Weapon.Fire2Released.performed -= e => ADSReleased(); 
        defaultInput.Disable();
    }

    void Update()
    {
        if (!GameManager.instance.gamePaused)
        {
            CalculateADS();
            SetIsGrounded();
            SetIsFalling();
            CalculateView();
            CalculateMovement();
            CalculateJump();
            CalculateStance();
        }
    }

    private void CalculateView()
    {
        newCharacterRotation.y += (isAimingDownSights ? playerSettings.ViewXSensitivity * playerSettings.ViewADSSensitivityEffector : playerSettings.ViewXSensitivity) * (playerSettings.ViewXInverted ? -input_View.x : input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x += (isAimingDownSights ? playerSettings.ViewYSensitivity * playerSettings.ViewADSSensitivityEffector : playerSettings.ViewYSensitivity)* (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        if (recoilAmount > 0f)
        {
            recoilRotation.x = Mathf.Lerp(recoilRotation.x, -10f, 10f * Time.deltaTime);
            recoilAmount -= Time.deltaTime;
        }
        else
        {
            recoilRotation.x = Mathf.Lerp(recoilRotation.x, 0f, 5f * Time.deltaTime);
            
        }

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation + recoilRotation);
    }

    private void CalculateMovement()
    {
         if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
        }

        var verticalSpeed = playerSettings.WalkingForwardSpeed;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed;

        if (isSprinting)
        {
            verticalSpeed = playerSettings.RunningForwardSpeed;
            horizontalSpeed = playerSettings.RunningStrafeSpeed;
        }

        // Effectors
        if (!isGrounded)
        {
            playerSettings.SpeedEffector = playerSettings.FallingSpeedEffector;
        }
        else if (playerStance == PlayerStance.Crouch)
        {
            playerSettings.SpeedEffector = playerSettings.CrouchSpeedEffector;
        }
        else if (playerStance == PlayerStance.Prone)
        {
            playerSettings.SpeedEffector = playerSettings.ProneSpeedEffector;
        }
        else if (isAimingDownSights)
        {
            playerSettings.SpeedEffector = playerSettings.ADSSpeedEffector;
        }
        else{
            playerSettings.SpeedEffector = 1;
        }

        weaponAnimationSpeed = characterController.velocity.magnitude / (playerSettings.WalkingForwardSpeed * playerSettings.SpeedEffector);
        if (weaponAnimationSpeed > 1)
        {
            weaponAnimationSpeed = 1;
        }

        verticalSpeed *= playerSettings.SpeedEffector;
        horizontalSpeed *= playerSettings.SpeedEffector;

        newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, new Vector3(horizontalSpeed * input_Movement.x * Time.deltaTime, 0, verticalSpeed * input_Movement.y * Time.deltaTime), ref newMovementSpeedVelocity, isGrounded ? playerSettings.MovementSmoothing : playerSettings.FallingSmoothing);
        var movementSpeed = transform.TransformDirection(newMovementSpeed);

        if (playerGravity > gravityMin && jumpingForce.y < 0.1f)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }
        if (playerGravity < -1f && isGrounded)
        {
            playerGravity = -1f;
        }

        movementSpeed.y += playerGravity;
        movementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(movementSpeed);
    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
    }

    private void Jump()
    {
        if (!isGrounded || playerStance == PlayerStance.Prone)
        {
            return;
        }
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(playerStandStance.StanceCollider.height))
            {
                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        jumpingForce = Vector3.up * playerSettings.JumpingHeight;
        playerGravity = 0;
        currentWeapon.TriggerJump();
    }

    private void CalculateStance()
    {
        var currentStance = playerStandStance;
        if(playerStance == PlayerStance.Crouch)
        {
            currentStance = playerCrouchStance;
        }
        else if (playerStance == PlayerStance.Prone)
        {
            currentStance = playerProneStance;
        }

        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, currentStance.Cameraheight, ref cameraHeightVelocity, playerStanceSmoothing);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.StanceCollider.height, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.StanceCollider.center, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    private void Crouch()
    {
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(playerStandStance.StanceCollider.height))
            {
                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        playerStance = PlayerStance.Crouch;
    }

    private void Prone()
    {
        if (StanceCheck(playerCrouchStance.StanceCollider.height))
        {
            return;
        }
        playerStance = PlayerStance.Prone;
    }

    private bool StanceCheck(float stanceCheckHeight)
    {
        Vector3 start = new Vector3(feetTransform.position.x, feetTransform.position.y + characterController.radius + stanceCheckErrorMargin, feetTransform.position.z);
        Vector3 end = new Vector3(feetTransform.position.x, feetTransform.position.y - characterController.radius - stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);

        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }

    private void ToggleSprint()
    {
        if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
            return;
        }

        isSprinting = !isSprinting;
    }

    private void StopSprint()
    {
        if (playerSettings.HoldForSprint)
        {
            isSprinting = false;
        }
    }

    private void SetIsGrounded()
    {
        isGrounded = Physics.CheckSphere(feetTransform.position, playerSettings.isGroundedRadius, groundMask);
    }

    private void SetIsFalling()
    {
        isFalling = !isGrounded && characterController.velocity.magnitude > playerSettings.isFallingSpeed;
    }

    private void ADSPressed()
    {
        if (!isSprinting)
        {
            isAimingDownSights = true;
            CameraController.instance.ZoomIn(WeaponManager.instance.activeGun.zoomAmount);
        }
    }

    private void ADSReleased()
    {
        isAimingDownSights = false;
        CameraController.instance.ZoomOut();
    }

    private void CalculateADS()
    {
        if (!currentWeapon)
        {
            return;
        }

        currentWeapon.isAimingDownSights = isAimingDownSights;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SpringUpward(float springForce)
    {
        jumpingForce = Vector3.up * springForce;
        playerGravity = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetTransform.position, playerSettings.isGroundedRadius);
    }
}
