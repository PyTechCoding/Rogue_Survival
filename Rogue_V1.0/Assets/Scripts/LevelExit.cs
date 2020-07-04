using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player" ))
        {
            if (CameraController.instance.isBossRoom)
            {
                AudioController.instance.bossDeathMusic.Stop();
                StartCoroutine(LevelManager.instance.LevelEnd());
            }
            else
            {
                StartCoroutine(LevelManager.instance.LevelEnd());
            }
        }
        
    }
}
