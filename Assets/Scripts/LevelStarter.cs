using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStarter : MonoBehaviour
{
    private int NumPlayersReady = 0;
    public static LevelStarter Instance { get; private set; }
    // Start is called before the first frame update
    private void Start()
    {
        Instance = this;
    }

    public void ReadyPlayer()
    {
        NumPlayersReady++;
        if(NumPlayersReady == 2)
        {
            SceneManager.LoadScene(1);
        }
    }

}
