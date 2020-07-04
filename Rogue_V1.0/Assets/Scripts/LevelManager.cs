using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform startPoint;

    private PlayerController player;
    private CharacterTracker charTracker;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentCoins;
    private void Awake()
    {
        instance = this;
                
    }
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.playerInstance;
        charTracker = CharacterTracker.instance;

        if(player != null)
        {
            currentCoins = charTracker.currentCoins;
            player.transform.position = startPoint.position;
            player.canMove = true;
            UIController.instance.coinText.text = currentCoins.ToString();

        }
                
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd()
    {
        if(CameraController.instance.isBossRoom)
        {
            AudioController.instance.bossMusic.Stop();                        
            AudioController.instance.levelWinMusic.Play();
            AudioController.instance.winMusic.PlayDelayed(3f);
        }
        else
        {
            AudioController.instance.titleMusic.Stop();
            AudioController.instance.levelWinMusic.Play();
            AudioController.instance.levelMusic.PlayDelayed(3f);
        }
        
        UIController.instance.StartFadeToBlack();
        player.canMove = false;           

        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);
            isPaused = true;

            //Stop time
            Time.timeScale = 0f;
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);
            isPaused = false;

            //resume time
            Time.timeScale = 1f;
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;        
        charTracker.currentCoins = currentCoins;
        UIController.instance.coinText.text = currentCoins.ToString();
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if(currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.instance.coinText.text = currentCoins.ToString();
    }
}
