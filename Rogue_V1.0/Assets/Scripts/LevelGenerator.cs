using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public enum Direction { up, right, down, left};
    public Direction selectedDirection;
    public GameObject layoutRoom;
    public Transform generatorPoint;
    public RoomCenter centerStart, centerEnd, centerShop, centerGun;
    public RoomCenter[] potentialCenters;
    public RoomPrefabs rooms;
    public LayerMask whatIsRoom;
    public Color startColor, endColor, shopColor, gunColor;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();
    private List<GameObject> generatedOutlines = new List<GameObject>();
    private GameObject endRoom, shopRoom, gunRoom;
    
    public float xOffset = 18f, yOffset = 10f;
    public int distanceToEnd;
    public int minDistanceToShop, maxDistanceToShop, minDistanceToGun, maxDistanceToGun;
    private bool includeShop, includeGunRoom;

          
    // Start is called before the first frame update
    void Start()
    {
        CheckForSpecialRoom();

        //Generate Starting room

        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        //Select which direction the next room should be generated
        MoveGenerationPoint();

        //For each iteration up to the distance to the end room
        // generate a new room and add it to the list of layout rooms
        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            //if the next room is the last in the iteration it will be the end room
            if (i + 1 == distanceToEnd)
            {

                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            //Randomly choose one of the four cardinal directions to generate a point
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            //While there's a room overlappinging with the circle move the generator point
            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;

        }

        if (includeGunRoom)
        {
            int gunRoomSelector = Random.Range(minDistanceToGun, maxDistanceToGun + 1);
            if(gunRoomSelector < 0)
            {
                gunRoomSelector += 1;
                gunRoom = layoutRoomObjects[gunRoomSelector];
                gunRoom.GetComponent<SpriteRenderer>().color = gunColor;
                layoutRoomObjects.RemoveAt(gunRoomSelector);
            }
            else
            {
                gunRoom = layoutRoomObjects[gunRoomSelector];
                gunRoom.GetComponent<SpriteRenderer>().color = gunColor;
                layoutRoomObjects.RemoveAt(gunRoomSelector);
            }         
        }


        //create room outlines
        CreateRoomOutline(Vector3.zero);

        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        CreateRoomOutline(endRoom.transform.position);

        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }

        if (includeGunRoom)
        {
            CreateRoomOutline(gunRoom.transform.position);
        }

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                generateCenter = false;
            }

            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;

            }

            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if (includeGunRoom)
            {
                if (outline.transform.position == gunRoom.transform.position)
                {
                    Instantiate(centerGun, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

                    generateCenter = false;
                }
            }

            if (generateCenter == true)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();

            }
        }
    }

    private void CheckForSpecialRoom()
    {
        if (Random.Range(0, 10) % 3 == 0)
        {
            includeGunRoom = true;
            includeShop = false;
        }
        else if (Random.Range(0, 10) % 4 == 0)
        {
            includeGunRoom = false;
            includeShop = true;
        }
        else if (Random.Range(0, 10) % 5 == 0)
        {
            includeShop = true;
            includeGunRoom = true;
        }
        else
        {
            includeGunRoom = false;
            includeShop = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
            if(Input.GetKey(KeyCode.R))
            {
                Destroy(PlayerController.playerInstance.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);           
            }
        if (Input.GetKey(KeyCode.T))
        {
            if(CameraController.instance.isBossRoom)
            {
                Destroy(PlayerController.playerInstance.gameObject);
                Destroy(AudioController.instance.gameObject);
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                CharacterTracker.instance.currentCoins = LevelManager.instance.currentCoins;
                CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
                CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;
            }
        }
            
#endif
    }


    public void MoveGenerationPoint()
    {
        //Move the generator point based on the selected direction
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        Vector3 aboveLocation = new Vector3(0f, yOffset, 0f);
        Vector3 belowLocation = new Vector3(0f, -yOffset, 0f);
        Vector3 leftLocation =  new Vector3(-xOffset, 0f, 0f);
        Vector3 rightLocation = new Vector3(xOffset, 0f, 0f);
        
        //Is there an object of type<Room> that intersects the current and adjacent rooms
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + aboveLocation, .2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + belowLocation, .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + leftLocation, .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + rightLocation, .2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch(directionCount)
        {
            case 0:
                Debug.LogError("Found no room exists!!");
                break;

            case 1:
                if(roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }

                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }

                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }

                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;

            case 2:

                if(roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }

                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }

                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }

                if (roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }

                if (roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }

                if (roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }
                break;

            case 3:
                if(roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }

                if (roomRight && roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }

                if (roomAbove && roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }

                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }

                break;

            case 4:

                if (roomAbove && roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourWay, roomPosition, transform.rotation));
                }

                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown,
        doubleDownLeft, doubleLeftUp, tripleUpRightDown, tripleRightDownLeft,
        tripleDownLeftUp,tripleLeftUpRight, fourWay;
}
