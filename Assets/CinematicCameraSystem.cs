using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicCameraSystem : MonoBehaviour
{
    #region Instance
    private static CinematicCameraSystem s_Instance = null;
    public static CinematicCameraSystem instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(CinematicCameraSystem)) as CinematicCameraSystem;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("CinematicCameraSystem");
                s_Instance = obj.AddComponent(typeof(CinematicCameraSystem)) as CinematicCameraSystem;
                Debug.Log("Could not locate an CinematicCameraSystem object. CinematicCameraSystem was Generated Automaticly.");
            }

            return s_Instance;
        }
    }
    #endregion
    public GameObject cinematicCam1;
    public Vector3 cam1StartPostion;
    public Vector3 cam1EndPosition;
    public float cam1MovementSpeed;
    public float cam1ToCam2transitionTime;

    [Space]
    public GameObject cinematicCam2;
    public Vector3 cam2StartPostion;
    public Vector3 cam2EndPosition;
    public float cam2MovementSpeed;
    public float cam2ToCam3transitionTime;

    [Space]
    public GameObject cinematicCam3;
    public Vector3 cam3StartPostion;
    public Vector3 cam3EndPosition;
    public float cam3MovementSpeed;
    public float cam3ToLevelNametransitionTime;

    [Space]
    public GameObject levelName;
    public Fade fadeScript;

    public delegate void CinematicFinished();
    public static event CinematicFinished OnCinematicFinished;

    bool moveCam1;
    bool moveCam2;
    bool moveCam3;

    public IEnumerator StartCinematic()
    {
        fadeScript.FadeToBlack();
        cinematicCam1.transform.localPosition = cam1StartPostion;
        cinematicCam2.transform.localPosition = cam2StartPostion;
        cinematicCam3.transform.localPosition = cam3StartPostion;

        //Camera 1 Transition
        moveCam1 = true;
        fadeScript.StartFadingIn();
        yield return new WaitForSeconds(cam1ToCam2transitionTime);
        //Camera 2 Transition
        fadeScript.StartFadingIn();
        yield return new WaitForSeconds(.5f);
        cinematicCam2.SetActive(true);
        cinematicCam1.SetActive(false);
        moveCam1 = false;
        moveCam2 = true;
        yield return new WaitForSeconds(cam2ToCam3transitionTime);

        //Camera 3 Transition
        fadeScript.StartFadingIn();
        yield return new WaitForSeconds(.5f);
        cinematicCam3.SetActive(true);
        cinematicCam2.SetActive(false);
        moveCam2 = false;
        moveCam3 = true;
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(cam3ToLevelNametransitionTime);
        levelName.SetActive(true);
        yield return new WaitForSeconds(6.5f);
        fadeScript.StartFadingIn();
        yield return new WaitForSeconds(.5f);
        cinematicCam3.SetActive(false);
        moveCam3 = false;
        OnCinematicFinished();
    }

    private void Update()
    {
        if(moveCam1)
            cinematicCam1.transform.localPosition = Vector3.Lerp(cinematicCam1.transform.localPosition, cam1EndPosition, cam1MovementSpeed * Time.deltaTime);
        else if(moveCam2)
            cinematicCam2.transform.localPosition = Vector3.Lerp(cinematicCam2.transform.localPosition, cam2EndPosition, cam2MovementSpeed * Time.deltaTime);
        else if(moveCam3)
            cinematicCam3.transform.localPosition = Vector3.Lerp(cinematicCam3.transform.localPosition, cam3EndPosition, cam3MovementSpeed * Time.deltaTime);
    }
}
