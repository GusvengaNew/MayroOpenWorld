using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class LoadGame : MonoBehaviour
{
    private string saveFilePath;
    private List<CharacterController> characterControllers;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.txt");
        characterControllers = new List<CharacterController>();
    }

    public void LoadLastSave()
    {
        if (File.Exists(saveFilePath))
        {
            Debug.Log("Save file found, loading...");
            string[] saveData = File.ReadAllLines(saveFilePath);
            string sceneToLoad = saveData[0];
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Save file not found!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid repeated calls

        // Temporarily disable all CharacterControllers in the scene
        DisableAllCharacterControllers();

        string[] saveData = File.ReadAllLines(saveFilePath);
        for (int i = 1; i < saveData.Length; i++)
        {
            string[] data = saveData[i].Split('|');
            string fullPath = data[0];
            string name = data[1];
            Vector3 localPosition = new Vector3(
                float.Parse(data[2]),
                float.Parse(data[3]),
                float.Parse(data[4])
            );
            Quaternion localRotation = new Quaternion(
                float.Parse(data[5]),
                float.Parse(data[6]),
                float.Parse(data[7]),
                float.Parse(data[8])
            );
            Vector3 localScale = new Vector3(
                float.Parse(data[9]),
                float.Parse(data[10]),
                float.Parse(data[11])
            );
            bool isActive = bool.Parse(data[12]);

            GameObject obj = FindObjectByPath(fullPath);
            if (obj != null)
            {
                obj.transform.localPosition = localPosition;
                obj.transform.localRotation = localRotation;
                obj.transform.localScale = localScale;
                obj.SetActive(isActive);

                Animator animator = obj.GetComponent<Animator>();
                if (animator != null && data[13] != "null" && data[14] != "null")
                {
                    int animHash = int.Parse(data[13]);
                    float normalizedTime = float.Parse(data[14]);
                    animator.Play(animHash, 0, normalizedTime);
                }

                AudioSource audioSource = obj.GetComponent<AudioSource>();
                if (audioSource != null && data[15] != "null" && data[16] != "null" && data[17] != "null" && data[18] != "null")
                {
                    audioSource.volume = float.Parse(data[15]);
                    audioSource.pitch = float.Parse(data[16]);
                    audioSource.time = float.Parse(data[17]);
                    audioSource.mute = bool.Parse(data[18]);
                }

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null && data[19] != "null" && data[20] != "null" && data[21] != "null" && data[22] != "null" && data[23] != "null" && data[24] != "null")
                {
                    Vector3 velocity = new Vector3(
                        float.Parse(data[19]),
                        float.Parse(data[20]),
                        float.Parse(data[21])
                    );
                    Vector3 angularVelocity = new Vector3(
                        float.Parse(data[22]),
                        float.Parse(data[23]),
                        float.Parse(data[24])
                    );
                    rb.velocity = velocity;
                    rb.angularVelocity = angularVelocity;
                }

                Debug.Log($"Loaded object: {name} with path: {fullPath}");
            }
            else
            {
                Debug.LogWarning($"Object with path {fullPath} and name {name} not found.");
            }
        }

        // Re-enable all CharacterControllers in the scene
        EnableAllCharacterControllers();

        Debug.Log("Game Loaded!");
    }

    private void DisableAllCharacterControllers()
    {
        characterControllers.Clear();
        CharacterController[] controllers = FindObjectsOfType<CharacterController>();
        foreach (CharacterController controller in controllers)
        {
            controller.enabled = false;
            characterControllers.Add(controller);
        }
    }

    private void EnableAllCharacterControllers()
    {
        foreach (CharacterController controller in characterControllers)
        {
            controller.enabled = true;
        }
        characterControllers.Clear();
    }

    private GameObject FindObjectByPath(string path)
    {
        string[] names = path.Split('/');
        Transform current = null;
        foreach (string name in names)
        {
            if (string.IsNullOrEmpty(name))
                continue;
            if (current == null)
            {
                current = GameObject.Find(name)?.transform;
            }
            else
            {
                current = current.Find(name);
            }
            if (current == null)
                return null;
        }
        return current?.gameObject;
    }
}
