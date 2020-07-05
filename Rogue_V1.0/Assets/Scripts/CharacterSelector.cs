using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    
    public      PlayerController playerToSpawn;
    public      GameObject       message;
    public      bool             shouldUnlock;
    private     bool             canSelect;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldUnlock)
        {
            //If the player character was previously unlocked
            if (PlayerPrefs.HasKey(playerToSpawn.name))
            {
                //Locate that player and set as active
                if(PlayerPrefs.GetInt(playerToSpawn.name) == 1)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If the character has been unlocked
        if (canSelect)
        {
            SwapPlayer(); //Change player character
        }
    }

    private void SwapPlayer()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Store the active players position
            Vector3 playerPos = PlayerController.playerInstance.transform.position;

            //Destroy the active player game object
            Destroy(PlayerController.playerInstance.gameObject);

            //Instantiate the locked player(playerToSpawn) as the new active player
            PlayerController newPlayer = Instantiate(playerToSpawn, playerPos, playerToSpawn.transform.rotation);

            //Assign the new player as the new player controller instance
            PlayerController.playerInstance = newPlayer;

            //Disable the character selector gameObject
            gameObject.SetActive(false);

            //Set player as new camera target
            CameraController.instance.target = newPlayer.transform;

            //Set player game object as the activePlayer gameObject
            CharacterSelectManager.instance.activePlayer = newPlayer;
            CharacterSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
            CharacterSelectManager.instance.activeCharSelect = this;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Once the player enters the trigger.Message becomes active and player can select
        if(other.CompareTag("Player"))
        {
            canSelect = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Once player leaves the trigger. Message is deactivated and player cannot select
        if (other.CompareTag("Player"))
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}
