using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableOnScene : MonoBehaviour
{
    public string sceneName;
    public GameObject objectToActivate;

    void OnEnable()
    {
        SceneManager.activeSceneChanged += CheckScene;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= CheckScene;
    }

    void CheckScene(Scene scene1, Scene scene2)
    {
        if (scene2.Equals(sceneName))
        {
            if (objectToActivate.activeSelf)
                objectToActivate.SetActive(false);
        }
        else
        {
            if (!objectToActivate.activeSelf)
                objectToActivate.SetActive(true);
        }
    }
}
