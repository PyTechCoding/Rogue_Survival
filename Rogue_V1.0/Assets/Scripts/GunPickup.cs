using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public float waitToBeCollected = .5f;
    public Gun theGun;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            bool hasGun = false;
            foreach(Gun gunToCheck in PlayerController.playerInstance.availableGuns)
            {
                if(theGun.weaponName == gunToCheck.weaponName)
                {
                    hasGun = true;
                }
            }
            if (!hasGun)
            {
                Gun gunClone = Instantiate(theGun);
                gunClone.transform.parent = PlayerController.playerInstance.gunArm;
                gunClone.transform.position = PlayerController.playerInstance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = Vector3.one;

                PlayerController.playerInstance.availableGuns.Add(gunClone);
                PlayerController.playerInstance.currentGun = PlayerController.playerInstance.availableGuns.Count - 1;
                PlayerController.playerInstance.SwitchGun();
            }
            AudioController.instance.PlaySFX(7);
            Destroy(gameObject);
        }
    }
}

