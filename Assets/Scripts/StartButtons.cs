using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtons : MonoBehaviour
{
    public bool isLeft;
    private bool hasRotated = false;
    public GameObject topText, bottomText;
    public void ReadyPlayer()
    {
        if (!hasRotated)
        {
            hasRotated = true;
            transform.Rotate(isLeft ? new Vector3(35f, 0f, 0f) : new Vector3(-35f, 0f, 0f));
            topText.SetActive(false);
            bottomText.SetActive(true);
            LevelStarter.Instance.ReadyPlayer();
        }
    }
}
