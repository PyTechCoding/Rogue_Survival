using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource levelMusic, gameOverMusic, winMusic, levelWinMusic, titleMusic, bossMusic, bossDeathMusic;
    public AudioSource[] sfx;    
    
    void Awake()
    {

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "TitleMenu")
        {            
            titleMusic.Play();
        }
   
    }

    // Update is called once per frame
    void Update()
    {
        
            
    }

    public void PlayGameOver()
    {
        if (CameraController.instance.isBossRoom)
        {
            bossMusic.Stop();
        }
        else
        {
            levelMusic.Stop();
        }
        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {        
        bossDeathMusic.Stop();
        levelMusic.Stop();
        titleMusic.Stop();
        levelWinMusic.Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
