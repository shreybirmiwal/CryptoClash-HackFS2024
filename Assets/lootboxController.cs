using UnityEngine;

public class LootboxController : MonoBehaviour
{
    public GameObject box;

    public float bobbingAmplitude = 1f; // How high the crate bobs
    public float bobbingFrequency = 1f;  // How fast the crate bobs
    public float rotationSpeedY = 35f;   // Speed of rotation around Y-axis
    public float rotationSpeedZ = 10f;    // Speed of rotation around Z-axis

    private Vector3 initialPosition;

    void Start()
    {
        if (box != null)
        {
            initialPosition = box.transform.position;
        }
    }

    void Update()
    {
        if (box != null)
        {
            HandleBobbing();
            HandleRotation();
        }
    }

    void HandleBobbing()
    {
        // Calculate the new Y position based on a sine wave
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        box.transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }

    void HandleRotation()
    {
        // Apply a constant rotation over time
        box.transform.Rotate(Vector3.up, rotationSpeedY * Time.deltaTime, Space.World);
        box.transform.Rotate(Vector3.forward, rotationSpeedZ * Time.deltaTime, Space.Self);
    }
}
