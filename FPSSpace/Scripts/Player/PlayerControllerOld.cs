using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour
{
    public static PlayerControllerOld instance;

    [SerializeField]
    private float moveSpeed, mouseSensitivity, gravityModifier, jumpPower, runSpeed = 12f;
    [SerializeField]
    private bool invertX, invertY;
    private Vector3 moveInput;
    private bool canJump, canDoubleJump;
    [SerializeField]
    private float maxViewAngle = 60f;


    [SerializeField]
    private Transform adsPoint, gunHolder;
    private Vector3 gunStartPosition;
    [SerializeField]
    private MuzzleFlash muzzleFlashFX;
    [SerializeField]
    private float adsSpeed = 2f;

    // References
    private CharacterController characterController;
    private Animator anim;
    [SerializeField]
    private Transform cameraPointTrans, groundCheckPoint, muzzlePoint;
    [SerializeField]
    private LayerMask whatIsGround;


    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGunIndex;

    public AudioSource footStepSlow, footstepFast;

    private float bounceAmount;
    private bool isBouncing;

    void Awake()
    {
        instance = this;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentGunIndex--;
        SwitchGun();

        gunStartPosition = gunHolder.localPosition;
    }

    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy && !GameManager.instance.levelEnding)
        {
            CalculateMovement();
            CalculateJump();
            CalculateBounce();

            
            characterController.Move(moveInput * Time.deltaTime);

            CalculateView();
            HandleShooting();
            SetAnimSettings();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGun();
            }
            CalculateADS();
            CalculateFOV();
        }
    }

    private void CalculateMovement()
    {
        float storedY = moveInput.y;

        // Control Movement
        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horzMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = vertMove + horzMove;
        moveInput.Normalize();

    }

    private void CalculateJump()
    {
        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;

        // Handle Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(canJump)
            {
                moveInput.y = jumpPower;
                canDoubleJump = true;
                AudioManager.instance.PlaySFX(8);
            }
            else if (canDoubleJump == true)
            {
                moveInput.y = jumpPower;
                canDoubleJump = false;
                AudioManager.instance.PlaySFX(8);
            }
        }

    }

    public void CalculateBounce()
    {
        if (isBouncing)
        {
            isBouncing = false;
            moveInput.y = bounceAmount;

            canDoubleJump = true;
        }
    }

    private void CalculateView()
    {
        // Control camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (invertX ? -mouseInput.x : mouseInput.x), transform.rotation.eulerAngles.z);
        cameraPointTrans.rotation = Quaternion.Euler(cameraPointTrans.rotation.eulerAngles + new Vector3(invertY ? mouseInput.y : -mouseInput.y, 0f, 0f));
        // clamp up and down view angle
        if (cameraPointTrans.rotation.eulerAngles.x > maxViewAngle && cameraPointTrans.rotation.eulerAngles.x < 180f)
        {
            cameraPointTrans.rotation = Quaternion.Euler(maxViewAngle, cameraPointTrans.rotation.eulerAngles.y, cameraPointTrans.rotation.eulerAngles.z);
        }
        else if (cameraPointTrans.rotation.eulerAngles.x > 180f && cameraPointTrans.rotation.eulerAngles.x < 360f - maxViewAngle)
        {
            cameraPointTrans.rotation = Quaternion.Euler(-maxViewAngle, cameraPointTrans.rotation.eulerAngles.y, cameraPointTrans.rotation.eulerAngles.z);
        }
    }

    private void SetAnimSettings()
    {
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraPointTrans.position, cameraPointTrans.forward, out hit, 100f))
            {
                if (Vector3.Distance(cameraPointTrans.position, hit.point) > 2f)
                {
                    muzzlePoint.LookAt(hit.point);
                }
            }
            else
            {
                muzzlePoint.LookAt(cameraPointTrans.position + (cameraPointTrans.forward * 30f));
            }

            FireShot();
        }

        // Repeats shot if autofire
        if (Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            if (activeGun.fireCounter <= 0)
            {
                FireShot();
            }
        }
    }

    private void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;

            Instantiate(activeGun.bullet, muzzlePoint.position, muzzlePoint.rotation);

            UpdateUIAmmo();

            activeGun.fireCounter = activeGun.fireRate;
            activeGun.PlayFireAnim();

            muzzleFlashFX.gameObject.SetActive(true);
            StartCoroutine(EndMuzzleFlashFX());
            //StartCoroutine(CameraShake.instance.Shake(.1f, .05f));
        }
    }

    private void UpdateUIAmmo()
    {
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
    }

    private void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGunIndex++;
        if (currentGunIndex > allGuns.Count - 1)
        {
            currentGunIndex = 0;
        }

        activeGun = allGuns[currentGunIndex];
        activeGun.gameObject.SetActive(true);
        muzzlePoint.position = activeGun.muzzlePointHolder.position;
        muzzleFlashFX.SetFlashColor(activeGun.muzzleFlashColor);
        if (activeGun)
        {
            UpdateUIAmmo();
        }
    }

    private void CalculateFOV()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CameraController.instance.ZoomIn(activeGun.zoomAmount);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            CameraController.instance.ZoomOut();
        }
    }

    private void CalculateADS()
    {
        if (Input.GetMouseButton(1))
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
        }
        else
        {
            gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPosition, adsSpeed * Time.deltaTime);
        }
    }

    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;
        if (unlockableGuns.Count > 0)
        {
            for(int i = 0; i < unlockableGuns.Count; i++)
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;
                    allGuns.Add(unlockableGuns[i]);
                    unlockableGuns.RemoveAt(i);
                    i = unlockableGuns.Count;
                }
            }
        }
        if (gunUnlocked)
        {
            currentGunIndex = allGuns.Count - 2;
            SwitchGun();
        }
    }

    private IEnumerator EndMuzzleFlashFX()
    {
        yield return new WaitForSeconds(.05f);

        muzzleFlashFX.gameObject.SetActive(false);
    }

    public void Bounce(float bounceForce)
    {
        bounceAmount = bounceForce;
        isBouncing = true;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
