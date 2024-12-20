using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;



public class Pistol : BaseWeapon
{
    private int pistolMagazineSize;


    private void Start()
    {
        pistolMagazineSize = 20;
    }
    #region UPDATE AND AWAKE
    private void Update()
    {
        //Call a function within the base class
        MyInput();

        //set up display Information
        if (ammunitionDisplay != null) // Evidence of null checks
            Debug.Log(" Pistol Ammunition is null");

        ammunitionDisplay.SetText(bulletsInMagazine + " / " + pistolMagazineSize);

        if (bulletsInMagazine <= pistolMagazineSize && reloading)
        {
            bulletsInMagazine = pistolMagazineSize;
        }
        else if (bulletsInMagazine >= 1)
        {
            MyInput();
        }

    }
    private void Awake()
    {
 
        ReloadingFinished();
        readyToShoot = true;
    }

    public override void Shoot() //Evidence of Virtual/Overide Keywords
    {
        Debug.Log("pistol Shoot function is active");
        readyToShoot = false;

        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(100);

        //direction from attackpoint to targetpoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position; // Evidence of Subtraction

        //direction from attackpoint to targetpoint with spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        //instantiate projectile
        GameObject pistolBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);


        //rotates object to the direction of shooting
        pistolBullet.transform.forward = directionWithoutSpread.normalized;

        //Add force to projectile



        // normal projectiles do not need upward force
        pistolBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse); // I assume this is the problem with lack of force


        pistolBullet.GetComponent<Rigidbody>().AddForce(fpsCamera.transform.up * upwardForce, ForceMode.Impulse); //Evidence of magnitude

        bulletsInMagazine = bulletsInMagazine - bulletsShot; 
        bulletsShot++;
    }
}

#endregion