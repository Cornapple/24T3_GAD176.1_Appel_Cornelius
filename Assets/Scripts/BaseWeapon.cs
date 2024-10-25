using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{ //Evidence of commenting
    #region FUNCTIONS
    //references ingame object/prefab which is a bullet
    public GameObject bullet; // Evidence of using public functions and variables

    //the force of the bullet
    [SerializeField] public float //Evidence of using the [SerialiseField] keyword
        shootForce, //evidence of naming conventions
        upwardForce;

    //gun statistics
    [SerializeField] public float 
        reloadTime, 
        timeBetweenShots, 
        timeBetweenShooting, 
        spread;

    [SerializeField] public int //Evidence of using functions
        bulletsPerClick;

    protected int MagazineSize;

    [SerializeField] public bool 
        allowButtonHold;

    [SerializeField] protected int //Evidence of using the 'Protected' access type keyword
        bulletsInMagazine, 
        bulletsShot;

    public bool 
        shooting, 
        readyToShoot, 
        reloading;

    //reference to game camera and point where bullets will hit (centre of screen)
    public Camera fpsCamera;
    public Transform attackPoint;

    //graphic references
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;

    #endregion

    #region UPDATE
    private void Update()
    {
        MyInput(); //Evidence of calling a function from a base class

        //set up display Information
        if (ammunitionDisplay != null)
            Debug.Log("Ammunition is null"); // evidence of debugging code
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

    #endregion


    #region INPUT
    public void MyInput() // Evidence of classes
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

        if (bulletsInMagazine >= 0)
        {
            return;
        }
    }
    #endregion

    #region SHOOTING  
    public virtual void Shoot()
    {

        //I attempted to remove Invokes from a script, which I originally used to fix
        //a problem with the shooting via a tutorial, but never really understood them

        Debug.Log("Shoot function is active");

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
    // Evidence of following the Singe Responcibility Principle
    public void ResetShot()
    {
        Debug.Log("ResetShot function is active");
        readyToShoot = true;
        //allowInvoke = true;
    }

    public void Reload()
    {
        Debug.Log("Reload function is active");
        reloading = true;

        //Invoke("ReloadFinished", reloadTime);
        if (reloading == true)
        {
            ReloadingFinished();
        }
    }

    public void ReloadingFinished()
    {
        Debug.Log("ReloadingFinished function is active");
        //bulletsInMagazine = MagazineSize;
        reloading = false;
    }

    #endregion
}
