using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Fade : MonoBehaviour
{
    public bool fadeAtStart;
    List<GameObject> nextMenuItems = new List<GameObject>();
    List<GameObject> thisMenuItems = new List<GameObject>();

    Image image;
    float fadeInTimer;
    float fadeOutTimer;

    Coroutine fadeIn;
    Coroutine fadeOut;
    bool fadingOut;


    void Start()
    {
        image = GetComponent<Image>();
        if (fadeAtStart)
            StartFadingIn();
    }

    public void PassMenuItems(List<GameObject> thisMenu, List<GameObject> nextMenu)
    {
        thisMenuItems = thisMenu;
        nextMenuItems = nextMenu;
    }

    void Update()
    {
        if (fadeInTimer >= .5f)
        {
            StopCoroutine(fadeIn);
            image.color = Color.black;
            fadeInTimer = 0;

            for (int i = 0; i < nextMenuItems.Count; i++)
                nextMenuItems[i].SetActive(true);

            for (int i = 0; i < thisMenuItems.Count; i++)
                thisMenuItems[i].SetActive(false);

            if (!fadingOut)
            {
                fadingOut = true;
                fadeOut = StartCoroutine(FadeOut());
            }
        }

        if (fadeOutTimer >= 2.5f)
        {
            StopCoroutine(fadeOut);
            image.color = Color.clear;
            fadeOutTimer = 0;
            fadingOut = false;
        }
    }

    public void StartFadingIn()
    {
        fadeIn = StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitUntil(FadingIn);
        print("Done");
    }

    bool FadingIn()
    {
        image.color = Color.Lerp(image.color, Color.black, Time.deltaTime * 10f);
        fadeInTimer += 1 / 60.0f;
        if (image.color == Color.clear)
            return true;
        else
            return false;
    }

    IEnumerator FadeOut()
    {
        yield return new WaitUntil(FadingOut);
        print("Done");
    }

    bool FadingOut()
    {
        image.color = Color.Lerp(image.color, Color.clear, Time.deltaTime * 1f);
        fadeOutTimer += 1 / 60.0f;
        if (image.color == Color.clear)
            return true;
        else
            return false;
    }
}

