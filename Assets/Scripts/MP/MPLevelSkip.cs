using UnityEngine;
using UnityEngine.SceneManagement;

public class MPLevelSkip : MonoBehaviour
{
    // Name of the scene to skip
    public string sceneToSkip = "Death";

    void Update()
    {
        // Check if Left Control or Right Control is pressed once
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            ChangeToNextScene();
        }
    }

    void ChangeToNextScene()
    {
        // Get the current active scene's build index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the next scene index
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // Load the next scene, skipping the "Death" scene
        string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneIndex).name;

        // If the next scene is the "Death" scene, skip to the following one
        if (nextSceneName == sceneToSkip)
        {
            nextSceneIndex = (nextSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        }

        // Load the next scene after checking for "Death"
        SceneManager.LoadScene(nextSceneIndex);
    }
}
