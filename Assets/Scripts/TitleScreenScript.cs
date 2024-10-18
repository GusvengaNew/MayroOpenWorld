using UnityEngine;
using UnityEngine.UI;

public class TitleScreenScript : MonoBehaviour
{
    public Button loadLastSaveButton;
    private LoadGame loadGameScript;

    void Start()
    {
        loadGameScript = GetComponent<LoadGame>();
        loadLastSaveButton.onClick.AddListener(loadGameScript.LoadLastSave);
    }
}
