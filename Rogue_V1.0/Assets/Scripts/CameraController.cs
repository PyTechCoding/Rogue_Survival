using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;
    
    public Transform target;
    public PlayerController player;

    public Camera mainCamera, bigMapCamera;

    private bool bigMapActive;
    public bool isBossRoom;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "TitleMenu")
        {

            player = PlayerController.playerInstance;
            if(player != null)
            {
                //On start focus the camera on player instance if play is not null
                if(player.transform != null)
                {
                    target = player.transform;
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime); 
        }


        if (Input.GetKeyDown(KeyCode.M) && !isBossRoom)
        {
            if (!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;

            PlayerController.playerInstance.canMove = false;
        
            Time.timeScale = 0f;

            UIController.instance.mapDisplay.SetActive(false);
            UIController.instance.bigMapText.SetActive(true);
        }
    }

    public void DeactivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = false;
            
            bigMapCamera.enabled = false;
            mainCamera.enabled = true;

            PlayerController.playerInstance.canMove = true;
        
            Time.timeScale = 1f;

            UIController.instance.mapDisplay.SetActive(true);
            UIController.instance.bigMapText.SetActive(false);
        }
    }
}
