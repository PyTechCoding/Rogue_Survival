using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{

    public GameObject[] brokenPieces;
    public GameObject[] itemsToDrop;
    public int maxPieces = 5;
    public bool shouldDropItem;
    public float itemDropPercent;


    private void OnTriggerEnter2D(Collider2D other)
    {    
        //If hit by a player's bullet execute smash
        if(other.CompareTag("PlayerProjectile"))
        {
            Smash();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //If the player dashes into the breakable, execute smash
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerController.playerInstance.dashCounter > 0)
            {
                Smash();
            }
        } 
    }

    //Destroys the initial box GameObject, instantiating broken pieces 
    //in different locations and rotations with the chance to drop
    //and item
    private void Smash()
    {
        Destroy(gameObject);
        AudioController.instance.PlaySFX(0);

        //show the  broken pieces
        int piecesToDrop = Random.Range(1, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

        //Drop item
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
}
