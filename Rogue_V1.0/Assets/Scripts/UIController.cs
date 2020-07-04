using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public GameObject deathScreen;
    public GameObject pauseMenu, mapDisplay, bigMapText, continueButton;
    
    public Slider healthSlider;
    public Slider bossHealthBar;
    
    public Text healthText, coinText, continueText ;
    public Text gunText, levelText;
           
    public Image currentGun;
    public Image fadeScreen;
    
    public float fadeSpeed;
    
    private bool fadeToBlack, fadeOutBlack;

    public string newGameScene, mainMenuScene, currentScene;



        
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        currentGun.sprite = PlayerController.playerInstance.availableGuns[PlayerController.playerInstance.currentGun].gunUI;
        gunText.text = PlayerController.playerInstance.availableGuns[PlayerController.playerInstance.currentGun].weaponName;
        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = SceneManager.GetActiveScene().name;

        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r,
                                         fadeScreen.color.g,
                                         fadeScreen.color.b,
                                         Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }
        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r,
                                         fadeScreen.color.g,
                                         fadeScreen.color.b,
                                         Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void Continue()
    {
        if(CharacterTracker.instance.remainingContinues > 0)
        {
            CharacterTracker.instance.remainingContinues--;

            PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
            healthSlider.maxValue = CharacterTracker.instance.maxHealth;
            healthSlider.value = CharacterTracker.instance.maxHealth;
            healthText.text = PlayerHealthController.instance.currentHealth.ToString() + " / " + PlayerHealthController.instance.maxHealth.ToString();
            
            SceneManager.LoadScene(currentScene);        
        
            PlayerController.playerInstance.gameObject.transform.position = LevelManager.instance.startPoint.position;
            PlayerController.playerInstance.gameObject.SetActive(true);
                    
            coinText.text = CharacterTracker.instance.currentCoins.ToString();            
        }       

    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Destroy(AudioController.instance.gameObject);
        Destroy(PlayerController.playerInstance.gameObject);
        Destroy(CharacterTracker.instance.gameObject);
        SceneManager.LoadScene(mainMenuScene);
        CharacterTracker.instance.remainingContinues = CharacterTracker.instance.maxContinues;
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
