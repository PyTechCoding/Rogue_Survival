using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerInstance;
    public Animator anim;
    public Rigidbody2D theRB;    
    /*public GameObject bulletToFire;
    public Transform shotPoint;
    public float timeBetweenShots;
    private float shotCounter;*/
    public Transform gunArm;
    public SpriteRenderer bodySR;

    //private Camera theCam;
    private Vector2 moveInput;
    
    
    public float moveSpeed, dashSpeed = 8f, dashLength = .5f;
    public float dashCooldown = 1f, dashInvincibility = .5f;
    
    [HideInInspector]
    public float dashCounter;
    
    private float activeMoveSpeed,  dashCoolCounter;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    
    [HideInInspector]
    public int currentGun;

    public CharacterSelector selector;
    private void Awake()
    {       

        if(playerInstance == null || playerInstance == CharacterSelectManager.instance.activePlayer)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);
        }else if(playerInstance != this && !CharacterSelectManager.instance.activePlayer && !CharacterSelectManager.instance.activeCharSelect)
        {
            Destroy(gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //theCam = Camera.main;
        activeMoveSpeed = moveSpeed;
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName; 
    }

    // Update is called once per frame
    void Update()
    {
        CharacterSelectManager.instance.activeCharSelect = selector;
        if (canMove && !LevelManager.instance.isPaused)
        {

            MovePlayer();
            

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    AudioController.instance.PlaySFX(8);
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger("Dash");
                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);
                }
            }

            if(dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if(dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if(dashCooldown > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(availableGuns.Count > 0)
            {
                currentGun++;
                if(currentGun >= availableGuns.Count)
                {
                    currentGun = 0;
                }

                SwitchGun();
            }
            else
            {
                Debug.LogError("Player has no guns");
            }
        }

    }

  /*  private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletToFire, shotPoint.position, shotPoint.rotation);
            AudioController.instance.PlaySFX(15);
            shotCounter = timeBetweenShots;
        }

        if (Input.GetMouseButton(0))
        {
            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                Instantiate(bulletToFire, shotPoint.position, shotPoint.rotation);
                AudioController.instance.PlaySFX(15);

                shotCounter = timeBetweenShots;
            }
        }
    }*/

    private void MovePlayer()
    {
        /*moveInput.x = UltimateJoystick.GetHorizontalAxis( "Movement" );
        moveInput.y = UltimateJoystick.GetVerticalAxis( "Movement" );*/
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        //transform.position += new Vector3(moveInput.x, moveInput.y,0f) * Time.deltaTime * moveSpeed;

        theRB.velocity = moveInput * activeMoveSpeed;

        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition); 

        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunArm.localScale = Vector3.one;
        }

        //rotate gun arm        
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        //calculates rotation angle and converts from radians to degree
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        
        gunArm.rotation = Quaternion.Euler(0, 0, angle);

        if (moveInput != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void SwitchGun()
    {
        foreach(Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }

        availableGuns[currentGun].gameObject.SetActive(true);
        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);
    }
}
