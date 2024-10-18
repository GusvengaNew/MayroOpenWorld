using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spaceship : MonoBehaviour
{
    private Rigidbody rb;

    public float forwardSpeed = 50f;
    public float rotationSpeed = 100f;
    public float liftForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveForward();
        HandleRotation();
        ApplyLift();
    }

    private void MoveForward()
    {
        Vector3 forwardMovement = transform.forward * forwardSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }

    private void HandleRotation()
    {
        float yaw = Input.GetAxis("Horizontal");
        float pitch = -Input.GetAxis("Vertical");  // Negative to invert pitch

        Quaternion yawRotation = Quaternion.Euler(0f, yaw * rotationSpeed * Time.deltaTime, 0f);
        Quaternion pitchRotation = Quaternion.Euler(pitch * rotationSpeed * Time.deltaTime, 0f, 0f);

        rb.MoveRotation(rb.rotation * yawRotation * pitchRotation);
    }

    private void ApplyLift()
    {
        Vector3 lift = transform.up * liftForce;
        rb.AddForce(lift, ForceMode.Force);
    }
}
