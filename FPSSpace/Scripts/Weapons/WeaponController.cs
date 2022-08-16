using UnityEngine;
using static Models;

public class WeaponController : MonoBehaviour
{
    private PlayerController playerController;

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
    public float swayTime;
    public Vector3 swayPosition;

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




    private void Start()
    {
        //newWeaponRotation = transform.localRotation.eulerAngles;
    }


    public void Initialize(PlayerController PlayerController)
    {
        playerController = PlayerController;
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        if (!GameManager.instance.gamePaused)
        {
            SetWeaponAnimations();  
            CalculateWeaponSway(); 
            CalculateADS();     
            CalculateWeaponRotation();
        }
    }

    private void CalculateADS()
    {
        var targetPosition = transform.position;
        if (isAimingDownSights)
        {
            targetPosition = playerController.cameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (playerController.cameraHolder.transform.forward * sightOffset);
        }
        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, adsTime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
    }

    public void TriggerJump()
    {
        weaponAnimator.SetTrigger("Jump");
        isGroundedTrigger = false;
    }

    public void FireWeapon()
    {
        if (!GameManager.instance.gamePaused)
        {
            Debug.Log("FIRE!");

            // Calculate direction
            Vector3 direction = fpsCamera.transform.forward;

            // Raycast
            RaycastHit hit;
            if (Physics.Raycast(fpsCamera.transform.position, direction, out hit, 100f))
            {
                if (Vector3.Distance(fpsCamera.transform.position, hit.point) > 2f)
                {
                    muzzlePoint.LookAt(hit.point);
                }
            }
            else
            {
                muzzlePoint.LookAt(fpsCamera.transform.position + (direction * 30f));
            }

            Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
        }
    }

    private void CalculateWeaponRotation()
    {
        // Set view rotation sway
        targetWeaponRotation.y += (isAimingDownSights ? settings.SwayAmount / 5 : settings.SwayAmount) * (settings.SwayXInverted ? -playerController.input_View.x : playerController.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += (isAimingDownSights ? settings.SwayAmount / 5 : settings.SwayAmount) * (settings.SwayYInverted ? playerController.input_View.y : -playerController.input_View.y) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);
        targetWeaponRotation.z = isAimingDownSights ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothing);

        // set movement rotation sway
        targetWeaponMovementRotation.z = (isAimingDownSights ? settings.MovementSwayX / 3 : settings.MovementSwayX) * (settings.MovementSwayXInverted ? -playerController.input_Movement.x : playerController.input_Movement.x);
        targetWeaponMovementRotation.x = (isAimingDownSights ? settings.MovementSwayY / 3 : settings.MovementSwayY) * (settings.MovementSwayYInverted ? -playerController.input_Movement.y : playerController.input_Movement.y);

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

        if (playerController.isGrounded && !isGroundedTrigger && fallingDelay > 0.1f)
        {
            weaponAnimator.SetTrigger("Land");
            isGroundedTrigger = true;
        }
        else if (!playerController.isGrounded && isGroundedTrigger)
        {
            weaponAnimator.SetTrigger("Falling");
            isGroundedTrigger = false;
        }
        weaponAnimator.SetBool("IsSprinting", playerController.isSprinting);
        weaponAnimator.SetFloat("WeaponAnimationSpeed", playerController.weaponAnimationSpeed);
    }

    private void CalculateWeaponSway()
    {
        var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAimingDownSights ? swayScale * 4 : swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }

    }

    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }

    
    
}

