using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public static BossController instance;
    public BossSequence[]        sequences;
    public BossAction[]          actions;
    public GameObject            deathEffect;
    public GameObject            levelExit; 
    public GameObject            hitEffect;
    public Rigidbody2D           theRB;
    public Animator              anim;
    private Vector2              moveDirection;
    public int                   currentHealth;
    public int                   currentSequence;
    private int                  currentAction;
    private float                shotCounter;
    private float                actionCounter;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        actions         = sequences[currentSequence].actions;
        actionCounter   = actions[currentAction].actionLength;
        
        //Set boss health bar values
        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value    = currentHealth;
        
        //If current scene is the Bosses Room and the Boss music isn't playing then player the boss music
        if (CameraController.instance.isBossRoom && !AudioController.instance.bossMusic.isPlaying)
        {
            AudioController.instance.levelMusic.Stop();
            AudioController.instance.bossMusic.Play();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;

            //handle movement

            moveDirection = Vector2.zero;

            if (actions[currentAction].shouldMove)
            {
                if (actions[currentAction].shouldChasePlayer)
                {
                    moveDirection = PlayerController.playerInstance.transform.position - transform.position;
                    moveDirection.Normalize();
                }

                if (actions[currentAction].moveToPoints && Vector3.Distance(transform.position, actions[currentAction].pointToMoveTo.position) > .5f)
                {
                    moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;
                    moveDirection.Normalize();
                }
            }



            theRB.velocity = moveDirection * actions[currentAction].moveSpeed;

            //handle shooting

            if (actions[currentAction].shouldShoot)
            {
                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0)
                {
                    shotCounter = actions[currentAction].timeBetweenShots;

                    foreach(Transform t in actions[currentAction].shotPoints)
                    {
                        Instantiate(actions[currentAction].itemToShoot, t.position, t.rotation);
                        AudioController.instance.PlaySFX(11);
                    }
                }
            }

        }
        else
        {
            currentAction++;
            if(currentAction >= actions.Length)
            {
                currentAction = 0;
            }

            actionCounter = actions[currentAction].actionLength;
        }


    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        anim.SetTrigger("tookDamage");

        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);

            Instantiate(deathEffect, transform.position, transform.rotation);
            AudioController.instance.bossMusic.Stop();
            AudioController.instance.bossDeathMusic.Play();

            if(Vector3.Distance(PlayerController.playerInstance.transform.position,levelExit.transform.position) < 2f)
            {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }

            UIController.instance.bossHealthBar.gameObject.SetActive(false);

            levelExit.SetActive(true);
        }
        else
        {
            if (currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionCounter = actions[currentAction].actionLength;
            }
        }
        
        UIController.instance.bossHealthBar.value = currentHealth;

    }

}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public Transform[]  shotPoints;
    public Transform    pointToMoveTo;
    public GameObject   itemToShoot;
    public bool         shouldChasePlayer;
    public bool         shouldMove;
    public bool         moveToPoints;
    public bool         shouldShoot;
    public float        actionLength;
    public float        moveSpeed;
    public float        timeBetweenShots;
}

[System.Serializable]
public class BossSequence
{
    [Header("This is a sequence")]
    public BossAction[] actions;
    public int endSequenceHealth;
}