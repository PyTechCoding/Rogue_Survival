using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public PlatformLevelGeneration levelGen;
    // Start is called before the first frame update
   
    void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);     
        if(roomDetection == null && levelGen.stopGeneration == true)
        {
            //Spawn Random Room
            int rand = Random.Range(0, levelGen.rooms.Length);
            Instantiate(levelGen.rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
