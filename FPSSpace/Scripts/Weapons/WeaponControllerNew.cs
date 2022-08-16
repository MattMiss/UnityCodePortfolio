using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class WeaponControllerNew : MonoBehaviour
{
    private CharacterControllerNew characterController;

    [Header("References")]
    public Animator weaponAnimator;
    public Camera fpsCamera;
    public Transform muzzlePoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    [Header("Settings")]
    public WeaponSettingsModel settings;

    bool isInitialized;

    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;

    private bool isGroundedTrigger;
    public float fallingDelay;

    [Header("Weapon Sway")]
    public Transform weaponSwayObject;
    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 600f;
    public float swayLerpSpeed = 14f;
    private float swayTime;
    private Vector3 swayPosition;

    [Header("Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float adsTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;
    //[HideInInspector]
    public bool isAimingDownSights;

    [Header("Firing")]
    public Transform weaponFireObject;
    private Vector3 weaponFirePosition;
    private Vector3 weaponFirePositionVelocity;
    private Vector3 weaponAfterRecoilVelocity;
    public float fireTime;
    public float afterRecoilTime;
    [SerializeField]
    private GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        //newWeaponRotation  = transform.localRotation.eulerAngles;
    }

    public void Initialize(CharacterControllerNew CharacterController)
    {
        characterController = CharacterController;
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        if (!GameManager.instance.gamePaused)
        {
            SetWeaponAnimations();
            CalculateWeaponSway();
            CalculateWeaponRotation();
            CalculateADS();
        }
    }

    private void CalculateWeaponRotation()
    {
        // Set view rotation sway
        targetWeaponRotation.y += (isAimingDownSights ? settings.SwayAmount / 5 : settings.SwayAmount) * (settings.SwayXInverted ? -characterController.input_View.x : characterController.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += (isAimingDownSights ? settings.SwayAmount / 5 : settings.SwayAmount) * (settings.SwayYInverted ? characterController.input_View.y : -characterController.input_View.y) * Time.deltaTime;
        
        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);
        targetWeaponRotation.z = isAimingDownSights ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothing);

        // set movement rotation sway
        targetWeaponMovementRotation.z = settings.MovementSwayX * (settings.MovementSwayXInverted ? -characterController.input_Movement.x : characterController.input_Movement.x);
        targetWeaponMovementRotation.x = settings.MovementSwayY * (settings.MovementSwayYInverted ? -characterController.input_Movement.y : characterController.input_Movement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);

        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }

    private void SetWeaponAnimations()
    {
        
        if (isGroundedTrigger)
        {
            fallingDelay = 0;
        }
        else
        {
            fallingDelay += Time.deltaTime;
        }
        
        if (characterController.isGrounded && !isGroundedTrigger && fallingDelay > 0.1f)
        {
            weaponAnimator.SetTrigger("Land");
            isGroundedTrigger = true;
        }
        else if (!characterController.isGrounded && isGroundedTrigger)
        {
            weaponAnimator.SetTrigger("Falling");
            isGroundedTrigger = false;
        }
        weaponAnimator.SetBool("IsSprinting", characterController.isSprinting);
        weaponAnimator.SetFloat("WeaponAnimationSpeed", isAimingDownSights ? 0f : characterController.weaponAnimationSpeed);
    }

    public void TriggerJump()
    {
        weaponAnimator.SetTrigger("Jump");
        isGroundedTrigger = false;
    }

    private void CalculateWeaponSway()
    {
        var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAimingDownSights ? swayScale * 200 : swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 12.6f)
        {
            swayTime = 0;
        }
    }

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }

    private void CalculateADS()
    {
        var targetPosition = transform.position;
        if (isAimingDownSights)
        {
            targetPosition = characterController.cameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (characterController.cameraHolder.transform.forward * sightOffset);
        }
        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, adsTime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
    }

    public void PlayWeaponSwapAnim()
    {
        weaponAnimator.SetTrigger("Swap");
    }

}
