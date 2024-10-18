using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public float spinSpeed = 50f; // Adjust the speed of rotation as needed

    void Update()
    {
        // Rotate the object around the Y axis
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
    }
}
