using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenEnemiesCleared;
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject enemyParent;

    public Room theRoom;
    public RoomCenter instance;

  
    // Start is called before the first frame update
    void Start()
    {
        LoadEnemies();

        if (openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ClearRoom();
    }

    private void ClearRoom()
    {
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            //Remove dead enemies from enemy list
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            //once enemy list is empty open doors
            if (enemies.Count == 0)
            {                
                //openWhenEnemiesCleared = true;
                theRoom.OpenDoors();
            }

        }
    }

    private void LoadEnemies()
     {
        if(enemyParent.transform.childCount > 0)
        {
             if (theRoom.roomActive )
             {
                 enemyParent.SetActive(true);

                 for (int i = 0; i < enemyParent.transform.childCount; i++)
                 {
                     enemies.Add(enemyParent.transform.GetChild(i).gameObject);
                 }
             }
             else
             {
                 enemyParent.SetActive(false);
             }
        }
     }

 

}
