using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private DefaultInput defaultInput;
    [HideInInspector]
    public Vector2 input_Movement;
    [HideInInspector]
    public Vector2 input_View;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

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
    public WeaponController currentWeapon;

    public float weaponAnimationSpeed;

    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isFalling;

    [Header("ADS")]
    public bool isAimingDownSights;



    #region - Awake -
    private void Awake()
    {
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
        defaultInput.Weapon.Fire1Pressed.performed += e => FirePressed();


        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();
        cameraHeight = cameraHolder.localPosition.y;

        if (currentWeapon)
        {
            currentWeapon.Initialize(this);
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion

    #region - Update - 

    private void Update()
    {
        SetIsGrounded();
        SetIsFalling();  
        CalculateMovement();
        CalculateJump();   
        CalculateView();
        CalculateStance();
        CalculateADS();
    }
    private void FixedUpdate()
    {
    }


    #endregion

    #region - ADS -

    private void ADSPressed()
    {
        isAimingDownSights = true;
    }

    private void ADSReleased()
    {
        isAimingDownSights = false;
    }

    private void CalculateADS()
    {
        if (!currentWeapon)
        {
            return;
        }

        currentWeapon.isAimingDownSights = isAimingDownSights;
    }

    #endregion

    #region - Is Falling / IsGrounded -

    private void SetIsGrounded()
    {
        isGrounded = Physics.CheckSphere(feetTransform.position, playerSettings.isGroundedRadius, groundMask);
    }

    private void SetIsFalling()
    {
        Debug.Log(isFalling);

        isFalling = !isGrounded && characterController.velocity.magnitude > playerSettings.isFallingSpeed;
    }

    #endregion

    #region - View / Movement -
    private void CalculateView()
    {
        newCharacterRotation.y += (isAimingDownSights ? playerSettings.ViewXSensitivity * playerSettings.ViewADSSensitivityEffector : playerSettings.ViewXSensitivity) * (playerSettings.ViewXInverted ? -input_View.x : input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x += (isAimingDownSights ? playerSettings.ViewYSensitivity * playerSettings.ViewADSSensitivityEffector : playerSettings.ViewYSensitivity)* (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
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

        
        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }
        if (playerGravity < -0.0f && isGrounded)
        {
            playerGravity = -0.0f;
        }
        

        movementSpeed.y += playerGravity;
        movementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(movementSpeed);
    }
    #endregion

    #region - Jump - 
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
        if (playerStance == PlayerStance.Crouch && StanceCheck(playerStandStance.StanceCollider.height))
        {
            playerStance = PlayerStance.Stand;
            return;
        }

        jumpingForce = Vector3.up * playerSettings.JumpingHeight;
        playerGravity = 0;
        currentWeapon.TriggerJump();
    }
    #endregion

    #region - Stance -
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

    #endregion
    
    #region - Sprinting -
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
    #endregion

    #region - Firing -

    private void FirePressed()
    {
        if (currentWeapon)
        {
            currentWeapon.FireWeapon();
        }
    }

    #endregion

    #region - Gizmos -

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetTransform.position, playerSettings.isGroundedRadius);
    }

    #endregion
}
