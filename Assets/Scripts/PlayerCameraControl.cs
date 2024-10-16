using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 cameraTargetRotation = Vector3.zero;
    public float cameraSpeed = 10f;
    public InputAction newControls; // Reference to the Look Action in the Input System

    private void OnEnable()
    {
        // Enable the input action when the script is enabled
        newControls.Enable();
    }

    private void OnDisable()
    {
        // Disable the input action when the script is disabled
        newControls.Disable();
    }

    private void Start()
    {
        // Set initial rotation
        cameraTargetRotation = virtualCamera.transform.localEulerAngles;
    }

    private void Update()
    {
        // Read input from the LookAction (assuming 2D Vector)
        Vector2 lookInput = newControls.ReadValue<Vector2>();

        // Update camera target rotation based on input
        cameraTargetRotation.x += lookInput.y * cameraSpeed * Time.deltaTime;
        cameraTargetRotation.y += lookInput.x * cameraSpeed * Time.deltaTime;

        // Optionally clamp rotation values to avoid unnatural camera behavior
        cameraTargetRotation.x = Mathf.Clamp(cameraTargetRotation.x, -30f, 60f); // For vertical rotation

        // Apply the rotation to the camera
        virtualCamera.transform.localEulerAngles = cameraTargetRotation;
    }
}
