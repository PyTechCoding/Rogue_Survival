using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleMusic : MonoBehaviour
{
    public static TitleMusic instance;
    public AudioSource levelMusic;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    { }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
