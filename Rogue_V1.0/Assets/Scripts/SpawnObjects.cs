using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnObjects : MonoBehaviour
{
    public GameObject[] objects;        
    private RuleTile ruleTile;
    private Tilemap tileMap;


    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject instance = Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;

        tileMap = FindObjectOfType<LevelGenerator>().tileMap;
        ruleTile = FindObjectOfType<LevelGenerator>().ruleTile;
        
        //Vector3Int currentCell = tileMap.WorldToCell(transform.position);
        
        //tileMap.SetTile(currentCell, ruleTile);        

    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
}
