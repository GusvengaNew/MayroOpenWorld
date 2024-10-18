using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuArrow : MonoBehaviour
{
    public string targetSceneName;    // The name of the scene where the object should be activated.
    public string objectName;         // The name of the GameObject to activate.

    private static bool objectActivated = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            GameObject targetObject = GameObject.Find(objectName);
            if (targetObject != null && !objectActivated)
            {
                targetObject.SetActive(true);
                objectActivated = true;
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
