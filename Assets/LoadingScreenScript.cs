using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour {
    [SerializeField]
    GameObject loadingScreenObj;
    public Slider loadingSlider;
    private AsyncOperation async;

    public string path;
    public void LoadScreen()
    {
        StartCoroutine(TheLoadingScreenCoroutine());
    }
    private IEnumerator TheLoadingScreenCoroutine()
    {
        loadingScreenObj.SetActive(true);
        async = SceneManager.LoadSceneAsync(path);
        async.allowSceneActivation = false;
        while(async.isDone == false)
        {
            loadingSlider.value = async.progress;
            if (async.progress == 0.9f)
            {

                MainMenuOptions mmo = GetComponent<MainMenuOptions>();
                mmo.ActivationOfButtons(mmo.mainMenuBtns, false);
                mmo.ActivationOfButtons(mmo.mainOptionsSubBtns, false);
                mmo.ActivationOfButtons(mmo.mainMenuBtns, false);
                mmo.ActivationOfButtons(mmo.menuPanel, false);
                loadingSlider.value = 1;
                async.allowSceneActivation = true;
                yield return true;

            }
            yield return null;
        }
    }
}
