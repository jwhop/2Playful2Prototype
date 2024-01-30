using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeHolder : MonoBehaviour
{
    private int numDead = -1;
    public Sprite DeadMonkey;
    public bool isLeft;
    public void Died()
    {
        numDead++;
        if(numDead == 2)
        {
            EndScreen.Instance.EndGame(isLeft);
            StartCoroutine(ResetGame());
        }
        
        transform.GetChild(numDead).GetComponent<Image>().sprite = DeadMonkey;
        transform.GetChild(numDead).GetComponent<Image>().color = new Color(.5f, .5f, .5f);
        
    }
    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
