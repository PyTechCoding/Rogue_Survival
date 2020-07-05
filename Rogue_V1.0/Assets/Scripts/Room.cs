using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [HideInInspector]
    public static       float        timeSinceRoomActive = 3f;
    public              GameObject   doors;
    public              GameObject   mapHider;
    [Tooltip("If the room has enemies this should be true")]
    public              bool         closeWhenEntered;
    public              bool         roomActive;
                  
    void Update()

    {
        //If the player is in the room begin count down
        if (roomActive)
        {
            timeSinceRoomActive -= Time.deltaTime;
        }
        else
        {
            timeSinceRoomActive = 0f;
        }
    }
    

    //Disables the doors within the room outlines
    public void OpenDoors()
    {
        doors.SetActive(false);
        closeWhenEntered = false;        
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Executes if player walks into the room    
        if(other.CompareTag("Player"))
        {
            //Move the camera to the center of the room
            CameraController.instance.ChangeTarget(transform);
         
            //If the room contains enemies lock down room
            if (closeWhenEntered)
            {                
                CloseDoors();
            }
            //Room is identified as the active room
            roomActive = true;            

            //Current room becomes visible on mini map
            mapHider.SetActive(false);
        }        
    }

    //Activate the rooms doors
    private void CloseDoors()
    {
        doors.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Once the player leaves the room, the room is no longer active and will not lockdown on entry
        if(other.CompareTag("Player") && roomActive == true)
        {
            roomActive = false;            
            closeWhenEntered = false;
        }        
    }

    //Destroys the enemy gameObject
    public void KillEnemy(GameObject enemy)
    {         
        Destroy(enemy);        
    }
}
