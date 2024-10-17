using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UniversalWeaponScript : MonoBehaviour
{
    #region VARIABLES
    //weapon stats (defines the aspects of the weapon and how it will work)
    public int damage;
    public float range, reloadTime, timeBetweenShots, timeBetweenShooting;
    public int MagazineSize, bulletsPerClick;
    int bulletsInMagazine, bulletsShot;

    bool reloading, readyToShoot, shooting;
  
    public Camera fpsCam;
    public RaycastHit rayHit;
    public LayerMask WhatIsEnemy;
    #endregion

    #region AWAKE AND UPDATE
    private void Awake()
    {
        //sets the players magazine to full and allows them to begin play without reloading
        bulletsInMagazine = MagazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }
    #endregion

    #region INPUT MANAGEMENT
    private void MyInput() //defines how the script will mechanically interact with the player (via the mouse and left click for shooting)
    {
        shooting = Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsInMagazine < MagazineSize && !reloading) Reload();



        //shooting 
        if (readyToShoot && shooting && !reloading && bulletsInMagazine > 0)
        {
            Shoot();
        }
    }
    #endregion

    #region SHOOTING AND RELOADING MECHANIC
    private void Shoot()
    {
        readyToShoot = false;

        // raycast (an invisible ray that draws a line from the camera to the target)

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, WhatIsEnemy))
        {
            //checks to make sure up to this step are correctly working in engine
            Debug.Log(rayHit.collider.name);

            //if (rayHit.collider.CompareTag("Enemy"))
            //    rayHit.collider.GetComponent<ShootingAi> /* this will depend on the name given to the enemies in engine */ ().TageDamage(damage);
        }
        bulletsInMagazine--;
        Invoke("ResetShot", timeBetweenShooting);
    }
    
    private void ResetShot()
    {

        readyToShoot = true;
    }

    private void Reload()
    {
        Invoke ("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsInMagazine = MagazineSize;
        reloading = false;
    }
    #endregion
}
