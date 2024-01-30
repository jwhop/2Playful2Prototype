using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndScreen : MonoBehaviour
{
    public static EndScreen Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void EndGame(bool isLeft)
    {
        Debug.Log("ending game");
        GetComponent<TextMeshProUGUI>().enabled = true;
        GetComponent<TextMeshProUGUI>().text = isLeft ? "Right Monkey Wins!!!" : "Left Monkey Wins!!!";
    }
}
