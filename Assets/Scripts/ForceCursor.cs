using UnityEngine;

public class ForceCursor : MonoBehaviour
{
    void Start()
    {
        CrapCursor();
    }

    void Update()
    {
        CrapCursor();
    }

    private void CrapCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
