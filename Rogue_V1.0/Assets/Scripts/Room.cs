using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered;
    public bool roomActive;

    public GameObject doors;
    public GameObject mapHider;
    //public GameObject enemyParent;
    
    [HideInInspector]
    public static float timeSinceRoomActive = 3f;
    //public List<GameObject> enemies = new List<GameObject>();

  
    
    void Update()

    {
        if (roomActive)
        {
            timeSinceRoomActive -= Time.deltaTime;
        }
        else
        {
            timeSinceRoomActive = 0f;
        }
    }

    

    public void OpenDoors()
    {
        doors.SetActive(false);
        closeWhenEntered = false;        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {

            CameraController.instance.ChangeTarget(transform);
            if (closeWhenEntered)
            {
                
                CloseDoors();
            }
            roomActive = true;
            
            
            mapHider.SetActive(false);

        }
        
    }

    private void CloseDoors()
    {
        doors.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && roomActive == true)
        {
            roomActive = false;            
            closeWhenEntered = false;
        }
        
    }



    public void KillEnemy(GameObject enemy)
    {         
        Destroy(enemy);        
    }
}
