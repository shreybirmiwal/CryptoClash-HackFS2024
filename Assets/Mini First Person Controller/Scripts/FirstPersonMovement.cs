using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    PhotonView PV;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PV.IsMine)
        {
            Camera cameraComponent = GetComponentInChildren<Camera>();
            if (cameraComponent != null)
            {
                // Delete the Camera component
                Destroy(cameraComponent);

                // Delete the AudioListener component if it exists
                AudioListener audioListener = cameraComponent.GetComponent<AudioListener>();
                if (audioListener != null)
                {
                    Destroy(audioListener);
                }

                // Delete the FirstPersonLook script if it exists
                FirstPersonLook firstPersonLook = cameraComponent.GetComponent<FirstPersonLook>();
                if (firstPersonLook != null)
                {
                    Destroy(firstPersonLook);
                }

                // Delete the PostProcessingBehavior component if it exists
                PostProcessingBehavior postProcessing = cameraComponent.GetComponent<PostProcessingBehavior>();
                if (postProcessing != null)
                {
                    Destroy(postProcessing);
                }

                // Delete the CullDistances component if it exists
                CullDistances cullDistances = cameraComponent.GetComponent<CullDistances>();
                if (cullDistances != null)
                {
                    Destroy(cullDistances);
                }

                // Delete the FlareLayer component if it exists
                FlareLayer flareLayer = cameraComponent.GetComponent<FlareLayer>();
                if (flareLayer != null)
                {
                    Destroy(flareLayer);
                }
            }

            // Assuming you also want to destroy the Rigidbody component from the GameObject this script is attached to
            Rigidbody rigidbodyComponent = GetComponent<Rigidbody>();
            if (rigidbodyComponent != null)
            {
                Destroy(rigidbodyComponent);
            }

            return;
        }
    }

    void FixedUpdate()
    {

        if (!PV.IsMine)
        {
            return;
        }

        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }
}