using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public static class BasicUtilities
{
    static List<string> onlyOnceValues = new List<string>();

    //==============Resets Transform=================
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    //===============Reloads the Current Scene===========
    public static void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    //========Loads the Next Scene in the BuildIndex=====
    public static void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        currentScene++;
        SceneManager.LoadScene(currentScene);
    }

    //====Allows you to perform an action only once, without the need to create another bool in script==========
    public static bool onlyOnce(string checkValue)
    {
        if (!onlyOnceValues.Contains(checkValue))
        {
            onlyOnceValues.Add(checkValue);
            return true;
        }
        else
            return false;
    }

    public static void resetOnce(string checkValue)
    {
        if (onlyOnceValues.Contains(checkValue))
            onlyOnceValues.Remove(checkValue);
        else
            Debug.LogError("You tried to remove something that hasn't been added.");
    }

    //==========Returns the Gameobject that is the closest of a given Vector3=======
    public static GameObject findNearest(Vector3 checkDistance, GameObject[] objectChecking)
    {
        int closeIndex = 0;
        float testDistance = Mathf.Infinity;

        for (int index = 0; index < objectChecking.Length; index++)
        {
            Vector3 objectPosition = objectChecking[index].transform.position;
            float distance = Vector3.Distance(checkDistance, objectPosition);

            if (distance < testDistance)
            {
                testDistance = distance;
                closeIndex = index;
            }
        }

        return objectChecking[closeIndex];
    }

    //==========Returns the Gameobject that is the closest of a given Vector3=======
    public static GameObject findNearest(Vector3 checkDistance, List<GameObject> objectChecking)
    {
        int closeIndex = 0;
        float testDistance = Mathf.Infinity;

        for (int index = 0; index < objectChecking.Count; index++)
        {
            Vector3 objectPosition = objectChecking[index].transform.position;
            float distance = Vector3.Distance(checkDistance, objectPosition);

            if (distance < testDistance)
            {
                testDistance = distance;
                closeIndex = index;
            }
        }

        return objectChecking[closeIndex];
    }


    //============Convert Into Time Based Format==========
    public static string textTime(float seconds)
    {
        int n = 0;
        string outString = "error";
        if (seconds < 0)
        {
            return "0 seconds";
        }
        if (seconds < 60)
        {
            n = (int)Mathf.Floor(seconds);
            outString = n + " second" + s(n);
            return outString;
        }
        if (seconds < 60 * 60)
        {
            n = (int)Mathf.Floor(seconds / 60);
            outString = n + " minute" + s(n);
            return outString;
        }
        if (seconds < 60 * 60 * 24)
        {
            n = (int)Mathf.Floor(seconds / 60 / 60);
            outString = n + " hour" + s(n);
            return outString;
        }
        if (seconds < 60 * 60 * 24 * 7)
        {
            n = (int)Mathf.Floor(seconds / 60 / 60 / 24);
            outString = n + " day" + s(n);
            return outString;
        }
        if (seconds < 60 * 60 * 24 * 31)
        {
            n = (int)Mathf.Floor(seconds / 60 / 60 / 24 / 7);
            outString = n + " week" + s(n);
            return outString;
        }
        if (seconds < 60 * 60 * 24 * 365)
        {
            n = (int)Mathf.Floor(seconds / 60 / 60 / 24 / 31);
            outString = n + " month" + s(n);
            return outString;
        }
        n = (int)Mathf.Floor(seconds / 60 / 60 / 24 / 365);
        outString = n + " year" + s(n);
        return outString;
    }

    static string s(int n)
    {
        if (n == 1)
        {
            return (" ");
        }
        else
        {
            return ("s");
        }
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

    ///public static void updateCoroutine(float waitTime)
}
