using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlatformLevelGeneration : MonoBehaviour
{
    public enum Direction { down, left, right}
    public Direction nextStartingDirection;
    public Transform[] startingPositions;
    [Tooltip("index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 -->LRBT")]
    public GameObject[] rooms;
    public LayerMask room;
 
    private int direction;
    public int downCounter;
    public float moveAmount;
    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;
    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration;
    // Start is called before the first frame update
    void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;

        Instantiate(rooms[(int)nextStartingDirection], transform.position, Quaternion.identity);
                
        nextStartingDirection = (Direction)Random.Range(0, 3);
       
    }

    private void Move()
    {
        Vector2 newPos;
        switch (nextStartingDirection) 
        {

            case Direction.right: //Move Right

                downCounter = 0;
                
                if (transform.position.x < maxX) 
                { 
                    newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                    transform.position = newPos;

                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    nextStartingDirection = (Direction)Random.Range(0, 3);
                    switch (nextStartingDirection)
                    {
                        case Direction.left:
                            nextStartingDirection = Direction.down;
                            break;
                    }
                }
                else
                {
                    nextStartingDirection = Direction.down;
                }
                break;
            case Direction.left: //Move Left!

                downCounter = 0;

                if(transform.position.x > minX)
                {
                    newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                    transform.position = newPos;

                    nextStartingDirection = (Direction)Random.Range(0, 3);
                    Instantiate(rooms[(int)nextStartingDirection],transform.position, Quaternion.identity);
                   switch (nextStartingDirection)
                    {
                        case Direction.right:
                            nextStartingDirection = Direction.left;
                            break;
                    }
                }
                else
                {
                    nextStartingDirection = Direction.down;
                }
                break;
            case Direction.down: //Move Down

                downCounter++;
                
                if(transform.position.y > minY)
                {
                    Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                    
                    if (roomDetection)
                    {
                        //If the room doesn't have a bottom opening, Destroy it and Instantiate one with all Directions
                        if( roomDetection.GetComponent<RoomType>().roomType == RoomType.Type.leftRightOpen ||
                            roomDetection.GetComponent<RoomType>().roomType == RoomType.Type.noBottom)
                        {                            
                            if(downCounter >= 2)
                            {
                                RoomType.instance.DestroyRoom();
                                Instantiate(rooms[(int)RoomType.Type.allDirections],transform.position, Quaternion.identity);
                            }
                            else
                            {
                                RoomType.instance.DestroyRoom();
                                Instantiate(rooms[(int)RoomType.Type.allDirections], transform.position, Quaternion.identity);
                            }

                        }
                    }
                    newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                    transform.position = newPos;

                    int rand = Random.Range(2, 4); //Rooms between indexes 2 and 3 have a top opening
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);

                    nextStartingDirection = (Direction)Random.Range(0, 3);
                }
                else
                {
                    //STOP LEVEL GENERATION
                    stopGeneration = true;
                }
                
                break;
        }       
        
        
        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }
}
