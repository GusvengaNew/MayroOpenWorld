using UnityEngine;

public class MCFallingBlock : MonoBehaviour
{
    private bool isFalling = false;

    void Update()
    {
        if (!isFalling)
        {
            CheckIfShouldFall();
        }
        else
        {
            Fall();
        }
    }

    private void CheckIfShouldFall()
    {
        Ray ray = new Ray(transform.position + Vector3.down * 0.51f, Vector3.down);
        if (!Physics.Raycast(ray, 1f))
        {
            isFalling = true;
        }
    }

    private void Fall()
    {
        Ray ray = new Ray(transform.position + Vector3.down * 0.51f, Vector3.down);
        if (!Physics.Raycast(ray, 1f))
        {
            transform.position += Vector3.down * Time.deltaTime * 5;
        }
        else
        {
            isFalling = false;
            transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), transform.position.z);
        }
    }
}
