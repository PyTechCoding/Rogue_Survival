using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnObjects : MonoBehaviour
{
    //public GameObject[] objects;
    public RuleTile rule;
    public Tilemap tileMap;

    // Start is called before the first frame update
    void Start()
    {
        /*
        int rand = Random.Range(0, objects.Length);
        GameObject instance = Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
        */
    }

    private void LateUpdate()
    {
        Vector3Int currentCell = tileMap.WorldToCell(transform.position);
        tileMap.SetTile(currentCell, rule);
        
    }

}
