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
    [Tooltip("Max Rate of Fire")]
    public float fireRate;
    public float shootRange;
    public float delay = 3f;
    private float fireCounter;

    [Header("Header Variables")]
    public static EnemyController instance;    
    public Animator anim;
    [Tooltip("Array of deathSplatter sprites to include on death")]
    public GameObject[] deathSplatters;
    public GameObject[] itemsToDrop;
    public GameObject slimeBlast;
    public HealthBarScript healthBar;
    public SpriteRenderer theBody;
    public int maxHealth = 150;
    public int currentHealth;
    [Tooltip("Determines whether the enemy is allowed to drop an item")]
    public bool shouldDropItem;
    [Tooltip("Chance an enemy will drop an item ")]
    [Range(0f,100f)]
    public float itemDropPercent;
    public float timeSincePlayerArrived;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {        
        //Set health bar UI
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
         }
    }

    // Update is called once per frame
    void Update()
    {
        //How long the room has been active 
        timeSincePlayerArrived = Room.timeSinceRoomActive; //TODO this needs to be adjusted as the Room becomes active once the level is loaded
        InvestigatePlayer(); //Determine enemy action based on player status and location
    }

    
       
    private void InvestigatePlayer()
    {
        PlayerController player = PlayerController.playerInstance; //Create an instance of the player
        if (theBody.isVisible && player.gameObject.activeInHierarchy) // If the enemyBody is visible and the player is not dead
        {
            moveDirection = Vector3.zero; //Don't move

            //Calculates the distance from the Enemy to the Player
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            //If the player is within chase radius and enemy is assigned shouldChasePlayer
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
                            //While wandering around. Pause for a random amount of time before moving
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    //If the pause counter is bigger than zero, begin timer
                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            //Calculates the new target direction
                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    //Calculates the distance between current transform and next patrol point in array
                    float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position);
                    
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                    if(distanceToPatrolPoint< .2f)
                    {
                        //If current distance to patrol point is within .2 then move to the next point
                        currentPatrolPoint++;

                        //Reset patrol points once the last point is reached
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            //If enemy is designated to run away and the player is within range, move direction is away from player
            if(shouldRunAway && distanceToPlayer < runawayRange)
            {
                moveDirection = transform.position - player.transform.position;
            }

            moveDirection.Normalize(); //Maintains the Vectors direction and changes the length to 1.0 unit

            theRB.velocity = moveDirection * moveSpeed; // Adjusts the vectors speed

            //If the enemy is moving activate inPursuit animation otherwise deactivate
            if (moveDirection != Vector3.zero)
            {
                anim.SetBool("inPursuit", true);
            }
            else
            {
                anim.SetBool("inPursuit", false);
            }

            //If the enemy is to the left of the player flip the sprite 
            if (transform.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);                
            }
            else
            {
                transform.localScale = Vector3.one; //Flip back to normal
            }

            //If the enemy is designated as a shooter and player's in range and the room has
            //been active for x amount of time. Shoot 
            if (shouldShoot && distanceToPlayer < shootRange && Room.timeSinceRoomActive <= 0)
            {
                Shoot();
                Room.timeSinceRoomActive = delay; //TODO : Play around with this to adjust enemy difficulty on room entry                               
            }

        }
        else
        {
            //If enemy or player is inactive/dead don't move
            theRB.velocity = Vector2.zero;
        }
    }


    //Once the Fire Counter hits zero, fire counter becomes a random
    // integer between 0 and the firerate. enemy will then shoot towards the player
    private void Shoot()
    {
        fireCounter -= Time.deltaTime; //Decrements the shot counter
        if (fireCounter <= 0)
        {
            fireCounter = Random.Range(0, fireRate);

            AudioController.instance.PlaySFX(12);

            Instantiate(bullet, shotPoint.transform.position, shotPoint.transform.rotation);
        }
    }

    //When called the enemy looses health equal to the damage amount
    //If the enemys health is at or below zero. Call the slimHit function
    // and has an 'itemDropPercent' chance of dropping a random item
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

    //On trigger collision with a player projectile, animation controller
    // will fire the tookDamage trigger, play the impact sound and then inflice
    // damage on the enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.CompareTag("PlayerProjectile"))
        {
            anim.SetTrigger("tookDamage");
            AudioController.instance.PlaySFX(2);            
            DamageEnemy(GameObject.FindObjectOfType<PlayerBullet>().damageToGive);
        }
        
    }

    //Loops through a list of death splatters and instantiates at random rotations.
    //Destroys itself after 3 seconds
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
