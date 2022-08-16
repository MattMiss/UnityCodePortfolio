using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    private DefaultInput defaultInput;
    [SerializeField] WeaponControllerNew weaponController;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Transform cameraPointTrans;
    [SerializeField] private MuzzleFlash muzzleFlashFX;
    [SerializeField] private Animator cameraAnims;
    [SerializeField] private CameraRecoil cameraRecoil;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGunIndex;

    private bool fireButtonPressed;
    private bool swappingWeapons = false;

    void Awake()
    {
        instance = this;

        defaultInput = new DefaultInput();

        defaultInput.Weapon.SwitchWeapon.performed += e => SwitchWeaponPressed();
        defaultInput.Weapon.Fire1Pressed.performed += e => FireButtonPressed();
        defaultInput.Weapon.Fire1Released.performed += e => FirButtonReleased();

        
        defaultInput.Enable();
    }

    void OnDisable()
    {
        defaultInput.Weapon.SwitchWeapon.performed -= e => SwitchWeaponPressed();
        defaultInput.Weapon.Fire1Pressed.performed -= e => FireButtonPressed();
        defaultInput.Weapon.Fire1Released.performed -= e => FirButtonReleased();
        defaultInput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGunIndex--;
        SwitchWeaponPressed();
    }

    void Update()
    {
        CheckForFiring();
    }

    private void SwitchWeaponPressed()
    {
        if (!GameManager.instance.gamePaused)
        {
            StartCoroutine(SwitchWeapon());
        }
    }

    private IEnumerator SwitchWeapon()
    {
        Debug.Log("Switch Gun!");
        weaponController.PlayWeaponSwapAnim();
        swappingWeapons = true;

        yield return new WaitForSeconds(.5f);

        activeGun.gameObject.SetActive(false);

        //currentGunIndex++;
        //if (currentGunIndex > allGuns.Count - 1)
        //{
        //    currentGunIndex = 0;
        //}

        currentGunIndex = (currentGunIndex + 1) % allGuns.Count;

        activeGun = allGuns[currentGunIndex];
        activeGun.gameObject.SetActive(true);
        muzzlePoint.position = activeGun.muzzlePointHolder.position;
        muzzleFlashFX.SetFlashColor(activeGun.muzzleFlashColor);
        if (activeGun)
        {
            UpdateUIAmmo();
        }
        StartCoroutine(FinishSwappingWeapon());
    }

    private IEnumerator FinishSwappingWeapon()
    {
        yield return new WaitForSeconds(.5f);
        swappingWeapons = false;
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
            SwitchWeaponPressed();
        }
    }

    private void UpdateUIAmmo()
    {
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
    }

    private void FireButtonPressed()
    {
        if (!GameManager.instance.gamePaused)
        {
            if (!CharacterControllerNew.instance.isSprinting && !swappingWeapons)
            {
                fireButtonPressed = true;

                if (activeGun.fireCounter <= 0)
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
            }
        }
    }

    private void FirButtonReleased()
    {
        fireButtonPressed = false;
        cameraRecoil.ResetRecoil();
    }

    private void CheckForFiring()
    {
        if (!CharacterControllerNew.instance.isSprinting && !swappingWeapons)
        {
            if (fireButtonPressed && activeGun.canAutoFire)
            {          
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }      
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
            cameraRecoil.StartRecoil(.2f, activeGun.recoilAmount, 10f);
            //CharacterControllerNew.instance.StartCameraRecoil();
            //cameraAnims.SetTrigger("PistolFire");

            muzzleFlashFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
            muzzleFlashFX.gameObject.SetActive(true);
            StartCoroutine(EndMuzzleFlashFX());
        }
    }

    private IEnumerator EndMuzzleFlashFX()
    {
        yield return new WaitForSeconds(.05f);

        muzzleFlashFX.gameObject.SetActive(false);
    }
}
