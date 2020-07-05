using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static  CharacterSelectManager   instance;
    [Tooltip("Displays which player character is currently active.")]
    public         CharacterSelector        activeCharSelect;    
    public         CharacterSelector[]      potentialCharacters;
    public         PlayerController         activePlayer;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject); //GameObject remains active after changing scenes
    }

    private void Start()
    {
        PlayerController currentPlayer = PlayerController.playerInstance;
        //Sets the current player as the active player
        if(currentPlayer != null)
        {
            activePlayer = currentPlayer;
        }
    }

}
