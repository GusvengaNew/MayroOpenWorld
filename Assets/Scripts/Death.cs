using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{
    [SerializeField]
    private float timeToWait = 5.0f; // The amount of time to wait before quitting the game

    private void OnEnable()
    {
        StartCoroutine(QuitGameAfterTime());
    }

    private void OnDisable()
    {
        StopCoroutine(QuitGameAfterTime());
    }

    private IEnumerator QuitGameAfterTime()
    {
        yield return new WaitForSeconds(timeToWait);
        QuitGame();
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            // If running in the Unity editor, stop play mode
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running in a built application, quit the application
            Application.Quit();
        #endif
    }
}
