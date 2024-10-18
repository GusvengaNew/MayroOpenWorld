using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    public GameObject carObject;
    public GameObject mainPlayerObject;
    public GameObject playerCarObject;
    public GameObject nonPlayerCarObject;

    private bool isInCar = false;
    private Vector3 carPosition;
    private Vector3 lastPlayerCarPosition;

    private void Start()
    {
        carPosition = carObject.transform.position;
        lastPlayerCarPosition = carPosition;
        mainPlayerObject.SetActive(true);
        playerCarObject.SetActive(false);
        nonPlayerCarObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInCar)
            {
                ExitCar();
            }
            else if (Vector3.Distance(mainPlayerObject.transform.position, carObject.transform.position) < 6.0f)
            {
                EnterCar();
            }
        }

        if (isInCar)
        {
            mainPlayerObject.transform.position = playerCarObject.transform.position + Vector3.left * 3f; // Adjust exit position
            nonPlayerCarObject.transform.position = playerCarObject.transform.position;
        }
    }

    private void EnterCar()
    {
        isInCar = true;
        carPosition = playerCarObject.transform.position;
        lastPlayerCarPosition = carPosition;
        mainPlayerObject.SetActive(false);
        playerCarObject.SetActive(true);
        nonPlayerCarObject.SetActive(false);
        playerCarObject.transform.position = carPosition;
        carObject.SetActive(false);
    }

    private void ExitCar()
    {
        isInCar = false;
        mainPlayerObject.SetActive(true);
        playerCarObject.SetActive(false);
        nonPlayerCarObject.SetActive(true);
        carObject.SetActive(true);
        lastPlayerCarPosition = playerCarObject.transform.position;
    }
}
