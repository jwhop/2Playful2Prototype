using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartTimer : MonoBehaviour
{
    public static StartTimer Instance { get; private set; }
    private TextMeshProUGUI message;
    public bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        message = GetComponent<TextMeshProUGUI>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            canMove = true;
            message.enabled = false;
        }
        else
        {

            StartCoroutine(StartTime());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(2f);
        message.text = "Go!";
        canMove = true;
        StartCoroutine(KillTimer());
            
    }

    IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
