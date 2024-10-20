using TMPro;
using UnityEngine;
using TMPro;

public class ProjectileScript : MonoBehaviour
{
    //references ingame object/prefab which is a bullet
    public GameObject projectile;

    //the force of the bullet
    public float shootForce, upwardForce;

    //gun statistics
    public float reloadTime, timeBetweenShots, timeBetweenShooting, spread;
    public int MagazineSize, bulletsPerClick;
    int bulletsInMagazine, bulletsShot;
    public bool allowButtonHold;

    bool shooting, readyToShoot, reloading;

    //reference to game camera and point where bullets will hit (centre of screen)
    public Camera fpsCamera;
    public Transform attackPoint;

    //graphic references
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;

    private void Update()
    {
        MyInput();

        //set up display Information
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsInMagazine / bulletsPerClick + " / " + MagazineSize / bulletsPerClick);
    }
    private void Awake()
    {
        bulletsInMagazine = MagazineSize;
        ReloadingFinished();
        readyToShoot = true;

    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting && !reloading && bulletsInMagazine > 0)
        {
            bulletsInMagazine = 0;

            Shoot();
        }

        //calls reloading function if magazine is no completely full
        if (Input.GetKeyDown(KeyCode.R) && bulletsInMagazine < MagazineSize && !reloading) Reload();

        //if magazine is completely empty and you try to shoot reload is called
        if (readyToShoot && shooting && !reloading && bulletsInMagazine <= 0) Reload();
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

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
        currentProjectile.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentProjectile.GetComponent<Rigidbody>().AddForce(fpsCamera.transform.up * upwardForce, ForceMode.Impulse);

        bulletsInMagazine--;
        bulletsShot++;

        //invokes the reset of shooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //for more than one bullet per click
        if (bulletsShot < bulletsPerClick && bulletsInMagazine > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadingFinished()
    {
        bulletsInMagazine = MagazineSize;
        reloading = false;
    }

}
