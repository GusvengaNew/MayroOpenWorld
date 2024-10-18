using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.txt");
        pauseMenuUI.SetActive(false); // Ensure the pause menu is initially inactive
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                LeaveToTitleScreen();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveGame();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LeaveToTitleScreen()
    {
        Time.timeScale = 1f; // Ensure time is running normally when changing scenes
        SceneManager.LoadScene("MainMenu"); // Replace with your actual title screen scene name
    }

    public void SaveGame()
    {
        List<GameObject> allObjects = new List<GameObject>(FindObjectsOfType<GameObject>());
        List<string> saveData = new List<string>();
        saveData.Add(SceneManager.GetActiveScene().name);

        foreach (var obj in allObjects)
        {
            if (ShouldExcludeFromSave(obj)) // Skip unnecessary objects
                continue;

            string objData = GetFullPath(obj) + "|" + obj.name + "|" +
                obj.transform.localPosition.x + "|" + obj.transform.localPosition.y + "|" + obj.transform.localPosition.z + "|" +
                obj.transform.localRotation.x + "|" + obj.transform.localRotation.y + "|" + obj.transform.localRotation.z + "|" + obj.transform.localRotation.w + "|" +
                obj.transform.localScale.x + "|" + obj.transform.localScale.y + "|" + obj.transform.localScale.z + "|" +
                obj.activeSelf;

            Animator animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
                objData += "|" + animState.fullPathHash + "|" + animState.normalizedTime;
            }
            else
            {
                objData += "|null|null";
            }

            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                objData += "|" + audioSource.volume + "|" + audioSource.pitch + "|" + audioSource.time + "|" + audioSource.mute;
            }
            else
            {
                objData += "|null|null|null|null";
            }

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                objData += "|" + rb.velocity.x + "|" + rb.velocity.y + "|" + rb.velocity.z + "|" +
                           rb.angularVelocity.x + "|" + rb.angularVelocity.y + "|" + rb.angularVelocity.z;
            }
            else
            {
                objData += "|null|null|null|null|null|null";
            }

            saveData.Add(objData);
        }

        File.WriteAllLines(saveFilePath, saveData.ToArray());
        Debug.Log("Game Saved!");
    }

    private bool ShouldExcludeFromSave(GameObject obj)
    {
        // Exclude objects whose names contain "Wall", "Cube", or "Floor"
        string[] excludeKeywords = { "Wall", "Cube", "Floor", "Road", "hinge", "bed" };
        foreach (var keyword in excludeKeywords)
        {
            if (obj.name.Contains(keyword))
            {
                return true;
            }
        }
        return false;
    }

    private string GetFullPath(GameObject obj)
    {
        string path = "/" + obj.name;
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            path = "/" + parent.name + path;
            parent = parent.parent;
        }
        return path;
    }
}
