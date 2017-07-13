using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerCreateTimer : MonoBehaviour
{
    public static bool countdownActive;
    public Button createButton;
    public Text countdownTimer;

    int waitTime = 30;
    int timer;

    void Start()
    {
        timer = waitTime;
    }

    public void StartTimer()
    {
        print("Start Timer called");
        if(!countdownActive)
        {
            countdownActive = true;
            StartCoroutine(Countdown());
        }
    }

    IEnumerator Countdown()
    {
        createButton.interactable = false;
        
        for(int i = waitTime; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            print(i);
            countdownTimer.text = "Please wait " + i + " seconds to create another match.";
        }


        countdownTimer.text = "Create A Game";
        createButton.interactable = true;
        countdownActive = false;
    }
}
