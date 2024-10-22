using TMPro;
using UnityEngine;

/// <summary>
/// move all shared functions from pistol to baseweapon
/// 
/// </summary>

public class Pistol : BaseWeapon
{
    #region FUNCTIONS
    //references ingame object/prefab which is a bullet
    public GameObject projectile;

    //the force of the bullet
    [SerializeField] private float shootForce, upwardForce;

    //gun statistics
    [SerializeField] private float reloadTime, timeBetweenShots, timeBetweenShooting, spread;
    [SerializeField] private int MagazineSize, bulletsPerClick;
    [SerializeField] private bool allowButtonHold;
    [SerializeField] protected int bulletsInMagazine, bulletsShot;
    public bool shooting, readyToShoot, reloading;

    //reference to game camera and point where bullets will hit (centre of screen)
    public Camera fpsCamera;
    public Transform attackPoint;

    //graphic references
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;

    #endregion


    private void Start()
    {
        Debug.Log(magazineSize);

    }

    #region UPDATE AND AWAKE
    private void Update()
    {
        MyInput();

        //set up display Information
        if (ammunitionDisplay != null)
            Debug.Log("Ammunition is null");
        //ammunitionDisplay.SetText(bulletsInMagazine / bulletsPerClick + " / " + MagazineSize / bulletsPerClick);
        ammunitionDisplay.SetText(bulletsInMagazine + " / " + MagazineSize);

        

        if (bulletsInMagazine <= MagazineSize && reloading)
        {
            bulletsInMagazine = MagazineSize;
        }
        else if (bulletsInMagazine >= 1)
        {
            MyInput();
        }

    }
    private void Awake()
    {
        bulletsInMagazine = MagazineSize;
        ReloadingFinished();
        readyToShoot = true;

    }
    #endregion

    #region INPUT
    private void MyInput()
    {
        Debug.Log("MyInput function has been called");
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting && !reloading && bulletsInMagazine > 0)
        {
            //bulletsInMagazine = 20;

            Shoot();
            Debug.Log("Shoot function has been called");
        }

        //calls reloading function if magazine is no completely full
        if (Input.GetKeyDown(KeyCode.R) && bulletsInMagazine < MagazineSize && !reloading)
        {
            Reload();
            Debug.Log("Reload function has bee called");
        }

        //if magazine is completely empty and you try to shoot, reload is called
        if (readyToShoot && shooting && !reloading && bulletsInMagazine <= 0)
        {
            Reload();
            Debug.Log("Reload function has been called");
        }

        if (bulletsInMagazine >= 0 )
        {
            return;
        }
    }
    #endregion

    #region SHOOTING  
    private void Shoot()
    {
        Debug.Log("Shoot function is active");
        readyToShoot = false;

        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100);

        //direction from attackpoint to targetpoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //direction from attackpoint to targetpoint with spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3 (x, y, 0);

        //instantiate projectile
        GameObject currentProjectile = Instantiate(projectile, attackPoint.position, Quaternion.identity);
            

        //rotates object to the direction of shooting
        currentProjectile.transform.forward = directionWithoutSpread.normalized;

        //Add force to projectile



        // normal projectiles do not need upward force
        currentProjectile.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse); // I assume this is the problem with lack of force


        currentProjectile.GetComponent<Rigidbody>().AddForce(fpsCamera.transform.up * upwardForce, ForceMode.Impulse);

        bulletsInMagazine = bulletsInMagazine - bulletsShot; ///////
        bulletsShot++;

        //invokes the reset of shooting
        if (allowInvoke)
        {
            //Invoke("ResetShot", timeBetweenShooting);
            //allowInvoke = false;
            ResetShot();
        }

        //for more than one bullet per click
        if (bulletsShot < bulletsPerClick && bulletsInMagazine > 0)
        {
            //Invoke("Shoot", timeBetweenShots);
            Shoot();
        }
    }
    #endregion

    #region RELOADING   
    private void ResetShot()
    {
        Debug.Log("ResetShot function is active");
        readyToShoot = true;
        //allowInvoke = true;
    }

    private void Reload()
    {
        Debug.Log("Reload function is active");
        reloading = true;

        //Invoke("ReloadFinished", reloadTime);
        if (reloading == true)
        {
            ReloadingFinished();
        }
    }

    private void ReloadingFinished()
    {
        Debug.Log("ReloadingFinished function is active");
        bulletsInMagazine = MagazineSize;
        reloading = false;
    }

    #endregion
}
