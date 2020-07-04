using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;
    public PlayerController activePlayer;
    public CharacterSelector activeCharSelect;
    public CharacterSelector[] potentialCharacters;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerController currentPlayer = PlayerController.playerInstance;
        if(currentPlayer != null)
        {
            activePlayer = currentPlayer;
        }
    }

}
