using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public static PlayerBullet instance;
    public float speed = 7.5f;

    public Rigidbody2D theRB;

    public GameObject impactEffect;
    public int damageToGive = 50;


    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.right * speed;   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        AudioController.instance.PlaySFX(4);

        Destroy(gameObject);
        if (other.CompareTag("Boss"))
        {
            Debug.Log(gameObject.name + "collided with the: " +other.name); 
            Instantiate(impactEffect, transform.position, transform.rotation);
            AudioController.instance.PlaySFX(4);

            Destroy(gameObject);
            BossController.instance.TakeDamage(damageToGive / 3) ;

            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
            
        }
        if (other.CompareTag("EnemyMinion"))
        {
            Debug.Log(gameObject.name + "collided with the: " + other.name);
            Instantiate(impactEffect, transform.position, transform.rotation);
            AudioController.instance.PlaySFX(4);

            Destroy(gameObject);
                        

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(gameObject.name + "collided with the: " + other.gameObject.name);
        Instantiate(impactEffect, transform.position, transform.rotation);
        AudioController.instance.PlaySFX(4);

        Destroy(gameObject);

        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController.instance.DamageEnemy(damageToGive);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            BossController.instance.TakeDamage(damageToGive / 3);

            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);

        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
