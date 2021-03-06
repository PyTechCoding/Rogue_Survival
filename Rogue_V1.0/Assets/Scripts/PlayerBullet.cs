﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public static PlayerBullet  instance;
    public        GameObject    impactEffect;
    public        Rigidbody2D   theRB;
    public        int           damageToGive = 50;
    public        float         speed = 7.5f;

    // Update is called once per frame
    void Update()
    {
        //Each frame the bullet will move in the direction that it was 
        //instantiated at the given speed
        theRB.velocity = transform.right * speed;   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Once the trigger has been activated instantiate the bullets 
        //explosion effect and sound effect. Then destroy itself.
        Instantiate(impactEffect, transform.position, transform.rotation);
        AudioController.instance.PlaySFX(4);
        Destroy(gameObject);

        //If the other collider2D is tagged as being the Boss deal 1/3 of
        //the regular damage, instantiate the hit effect and play sound effect
        if (other.CompareTag("Boss"))
        {
            Debug.Log(gameObject.name + "collided with the: " +other.name); 
            Instantiate(impactEffect, transform.position, transform.rotation);
            AudioController.instance.PlaySFX(4);
            
            //Deal Damage to the boss            
            BossController.instance.TakeDamage(damageToGive / 3) ;
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);            
        }

        //If the bullet other collider2D is tagged as EnemyMinion instantiate
        //impact effect, player sound effect and destroy self
        if (other.CompareTag("EnemyMinion"))
        {
            Debug.Log(gameObject.name + "collided with the: " + other.name);
            Instantiate(impactEffect, transform.position, transform.rotation);
            AudioController.instance.PlaySFX(4);                                    
        }
    }

    //Once the bullet is out of screen view, destroy to conserve memory
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
