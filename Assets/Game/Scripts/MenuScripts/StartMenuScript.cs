﻿using UnityEngine;

public class StartMenuScript : MonoBehaviour
{
    [SerializeField]
    GameObject menuPanel;

    public void Lobby()
    {
        menuPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Return()
    {
        gameObject.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}