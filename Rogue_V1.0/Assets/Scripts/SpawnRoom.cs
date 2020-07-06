using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRoom : MonoBehaviour
{
    public GameObject[] rooms;
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, rooms.Length);
        GameObject instance = Instantiate(rooms[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }

}
