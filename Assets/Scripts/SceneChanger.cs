using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string targetSceneName = "NextScene"; // The name of the scene to change to

    [SerializeField]
    private float timeToWait = 5.0f; // The amount of time to wait before changing the scene

    private void OnEnable()
    {
        StartCoroutine(ChangeSceneAfterTime());
    }

    private void OnDisable()
    {
        StopCoroutine(ChangeSceneAfterTime());
    }

    private IEnumerator ChangeSceneAfterTime()
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(targetSceneName);
    }
}
