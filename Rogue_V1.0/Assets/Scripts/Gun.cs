using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletToFire;
    public Transform shotPoint;    
    public float timeBetweenShots;
    private float shotCounter;

    public string weaponName;
    public Sprite gunUI;

    public int itemCost;
    public Sprite gunShopSprite;

  

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.playerInstance != null)
        {
            if(PlayerController.playerInstance.canMove && !LevelManager.instance.isPaused)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if(shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }
        else
        {

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) 
            {

                PlayWeaponSound();

                Instantiate(bulletToFire, shotPoint.position, shotPoint.rotation);

                
                shotCounter = timeBetweenShots;
            }

            /*if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    Instantiate(bulletToFire, shotPoint.position, shotPoint.rotation);
                    AudioController.instance.PlaySFX(15);

                    shotCounter = timeBetweenShots;
                }
            }*/
        }
    }

    private void PlayWeaponSound()
    {
        switch (bulletToFire.name)
        {
            case "LaserBeam":
                {
                    //Debug.Log("Laser sound");
                    AudioController.instance.PlaySFX(13);
                }
                break;
            case "Light Machine Gun Bullet":
                {
                    //Debug.Log("Play Maching Gun Sound");
                    AudioController.instance.PlaySFX(23);
                }
                break;
            case "pistolBullet":
                {
                    //Debug.Log("Play pistol sound");
                    AudioController.instance.PlaySFX(26);
                }
                break;
            case "Sub_Machine Bullet":
                {
                    //Debug.Log("Play machine gun sound");
                    AudioController.instance.PlaySFX(24);
                }
                break;
            case "ShotGunBullet":
                {
                    //Debug.Log("Play shot gun sound and reload");
                    AudioController.instance.PlaySFX(27);
                }
                break;
            case "RevolverBullet":
                {
                    AudioController.instance.PlaySFX(22);
                }
                break;
            case "machineGunBullet":
                {
                    AudioController.instance.PlaySFX(25);
                }
                break;
        }
    }
}
