using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LootboxController : MonoBehaviour
{
    public GameObject box;
    public Animator boxAnimator;
    public ParticleSystem confettiParticleSystem;
    public GameObject rewardUI;
    public RawImage rewardUIRawImage;
    public TextMeshProUGUI rewardUIText;
    public GameObject lootboxUI;
    public GameObject lootboxGameObjects;

    public float bobbingAmplitude = 1f; // How high the crate bobs
    public float bobbingFrequency = 1f;  // How fast the crate bobs
    public float rotationSpeedY = 35f;   // Speed of rotation around Y-axis
    public float rotationSpeedZ = 10f;    // Speed of rotation around Z-axis
    public float resetSpeed = 2f; // Speed at which the box resets position and rotation

    private Vector3 initialPosition;
    private Quaternion initialRotation;


    private bool continueRotation = true;
    private bool continueBobbing = true;

    void Start()
    {
        if (box != null)
        {
            initialPosition = box.transform.position;
            initialRotation = box.transform.rotation;
        }
    }

    void Update()
    {
        if (box != null)
        {
            if (continueBobbing)
                HandleBobbing();
            if (continueRotation)
                HandleRotation();

            if (Input.GetKeyDown(KeyCode.E))
            {
                ResetBox();
            }
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

    void ResetBox()
    {

        continueRotation = false;
        continueBobbing = false;


        box.transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        box.transform.rotation = initialRotation;


        // Play open box animation
        boxAnimator.SetTrigger("open");


        // Play confetti particle system
        confettiParticleSystem.Play();

        // Enable RewardUI and disable lootbox UI elements
        rewardUI.SetActive(true);
        lootboxUI.SetActive(false);
        lootboxGameObjects.SetActive(false);

        // You can set the reward image and text here if needed
        // rewardUIRawImage.texture = ...;
        rewardUIText.text = "you got a reward!";

        // Re-enable rotation and bobbing

    }
}
