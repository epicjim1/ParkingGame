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
    public float smoothTime = 0.1f;  // Time for smoothing the rotation
    public InputAction lookAction; // Reference to the Look Action in the Input System
    public InputAction mouseClickAction; // New input action for detecting mouse click (drag)
    public InputAction scrollAction; // Input action for mouse scroll

    public float zoomSpeed = 1f;  // Speed at which the camera zooms in/out
    public float minZoom = 2f;     // Minimum distance from the target
    public float maxZoom = 10f;    // Maximum distance from the target

    private Vector3 currentRotationVelocity;
    private Quaternion currentRotation;

    private bool isDragging = false;

    private void OnEnable()
    {
        // Enable the input action when the script is enabled
        lookAction.Enable();
        mouseClickAction.Enable();
        scrollAction.Enable(); // Enable the scroll action
    }

    private void OnDisable()
    {
        // Disable the input action when the script is disabled
        lookAction.Disable();
        mouseClickAction.Disable();
        scrollAction.Disable();
    }

    private void Start()
    {
        // Set initial rotation
        cameraTargetRotation = virtualCamera.transform.localEulerAngles;
        currentRotation = virtualCamera.transform.rotation; // Get initial rotation as a quaternion
    }

    private void Update()
    {
        // Check if the mouse button is pressed (for example, right-click for drag)
        if (mouseClickAction.ReadValue<float>() > 0) // Assuming mouseClickAction is bound to "Click"
        {
            isDragging = true;
        }
        else
        {
            isDragging = false;
        }

        // If the player is dragging (holding down the mouse button), update the camera rotation
        if (isDragging)
        {
            // Read input from the LookAction (assuming 2D Vector)
            Vector2 lookInput = lookAction.ReadValue<Vector2>();

            // Update camera target rotation based on input
            cameraTargetRotation.x -= lookInput.y * cameraSpeed * Time.deltaTime;
            cameraTargetRotation.y += lookInput.x * cameraSpeed * Time.deltaTime;

            // Optionally clamp rotation values to avoid unnatural camera behavior
            cameraTargetRotation.x = Mathf.Clamp(cameraTargetRotation.x, -30f, 60f); // For vertical rotation

            // Convert the target rotation to a quaternion
            Quaternion targetRotation = Quaternion.Euler(cameraTargetRotation);

            // Smoothly interpolate from the current rotation to the target rotation
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, smoothTime * cameraSpeed);

            // Apply the rotation to the camera
            virtualCamera.transform.rotation = currentRotation;
        }

        // Handle mouse scroll input for zooming in and out
        float scrollInput = scrollAction.ReadValue<Vector2>().y; // Get the vertical scroll value
        float zoomAmount = (scrollInput / 120) * zoomSpeed; // Calculate zoom amount

        Debug.Log("Scroll Input: " + scrollInput); // Log the scroll input value

        // Get the Framing Transposer component
        var framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        // Modify the existing camera distance instead of overwriting it
        framingTransposer.m_CameraDistance = Mathf.Clamp(framingTransposer.m_CameraDistance - zoomAmount, minZoom, maxZoom);
    }
}
