using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [Header("Chase Player")]
    private Vector3 moveDirection;

    public Rigidbody2D theRB;
    public bool shouldChasePlayer;
    public float moveSpeed;
    public float rangeToChasePlayer;
    public float rangeToEndChase;

    [Header("Runaway")]
    public bool shouldRunAway;
    public float runawayRange;

    [Header("Wandering")]
    private Vector3 wanderDirection;
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;

    [Header("Patrolling")]
    public Transform[] patrolPoints;
    public bool shouldPatrol;
    private int currentPatrolPoint;


    [Header("Shooting")]
    public GameObject bullet;
    public Transform shotPoint,Parent;    
    public bool shouldShoot;
    public float fireRate;
    public float shootRange;
    public float delay = 3f;
    private float fireCounter;

    [Header("Header Variables")]
    RoomCenter room;
    public Animator anim;
    public static EnemyController instance;
    public GameObject[] deathSplatters;
    public GameObject slimeBlast;
    public SpriteRenderer theBody;
    public int maxHealth = 150;
    public int currentHealth;
    public HealthBarScript healthBar;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    public float timeSincePlayerArrived;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        room = FindObjectOfType<RoomCenter>();
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
         }
    }

    // Update is called once per frame
    void Update()
    {
        timeSincePlayerArrived = Room.timeSinceRoomActive;
        PursuePlayer();
    }

    
       
    private void PursuePlayer()
    {
        PlayerController player = PlayerController.playerInstance;
        if (theBody.isVisible && player.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > rangeToEndChase && distanceToPlayer < rangeToChasePlayer && shouldChasePlayer)
            {               
                    moveDirection = player.transform.position - transform.position;             
            }
            else
            {
                if (shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection;

                    if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position);
                    
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                    if(distanceToPatrolPoint< .2f)
                    {
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            if(shouldRunAway && distanceToPlayer < runawayRange)
            {
                moveDirection = transform.position - player.transform.position;
            }



            /*else
            {
                moveDirection = Vector3.zero;
            }*/

            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;

            if (moveDirection != Vector3.zero)
            {
                anim.SetBool("inPursuit", true);
            }
            else
            {
                anim.SetBool("inPursuit", false);
            }

            if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);                
            }
            else
            {
                transform.localScale = Vector3.one;
            }

            if (shouldShoot && distanceToPlayer < shootRange && Room.timeSinceRoomActive <= 0)
            {
                Shoot();
                Room.timeSinceRoomActive = delay;                                
            }

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }
    }

    private void Shoot()
    {
        fireCounter -= Time.deltaTime;
        if (fireCounter <= 0)
        {
            fireCounter = Random.Range(0, fireRate);

            AudioController.instance.PlaySFX(12);

            Instantiate(bullet, shotPoint.transform.position, shotPoint.transform.rotation);
        }
    }

    public void DamageEnemy(int damage)
    {        
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {                       
            Destroy(gameObject);
            SlimeHit();                        
            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
        anim.ResetTrigger("tookDamage");                
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.CompareTag("PlayerProjectile"))
        {
            anim.SetTrigger("tookDamage");
            AudioController.instance.PlaySFX(2);
            DamageEnemy(PlayerBullet.instance.damageToGive);
        }
        
    }

    void SlimeHit()
    {
        int selectedSplatter = Random.Range(0, deathSplatters.Length);
        int rotation = Random.Range(0, 4); // rotates the death splatter in random directions 0,90, 180 and 270

        Instantiate(slimeBlast, transform.position, transform.rotation);
        
        AudioController.instance.PlaySFX(1);
        GameObject splatter = Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f,0f,rotation * 90)) ;

        Destroy(splatter, 3f);
    }

    

}
