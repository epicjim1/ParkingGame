using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CarController : MonoBehaviour
{
    private int retryCount = 0;
    public TMP_Text retryCountText;

    public static bool GameIsPaused = false;
    public static bool GameIsLost = false;
    public Animator canvasAnim;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    private VolumeProfile volume;
    private DepthOfField depthOfField;
    private float startingFocalLength;

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 30f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    private void Start()
    {
        GameIsPaused = false;
        GameIsLost = false;

        retryCount = PlayerPrefs.GetInt("RetryCount", 0);

        volume = virtualCamera.GetComponent<CinemachineVolumeSettings>().m_Profile;
        if (volume != null && volume.TryGet(out depthOfField))
        {
            Debug.Log("Depth of Field override found!");
        }
        else
        {
            Debug.LogError("Depth of Field override not found in the Volume profile.");
        }
        //depthOfField.active = false;
        depthOfField.focalLength.value = 1f;
        startingFocalLength = depthOfField.focalLength.value;
    }

    private void FixedUpdate()
    {
        if (!GameIsLost && !GameIsPaused)
        {
            //Get forward/reverse accleration from the vertical axis (w and s keys)
            currentAcceleration = acceleration * Input.GetAxis("Vertical");

            //If we're presssing space, give currentBreakingForce a value
            if (Input.GetKey(KeyCode.Space))
            {
                currentBreakForce = breakingForce;
                frontRight.motorTorque = 0f;
                frontLeft.motorTorque = 0f;
            }
            else
            {
                currentBreakForce = 0f;
                frontRight.motorTorque = currentAcceleration;
                frontLeft.motorTorque = currentAcceleration;
            }

            //Apply acceleration to front wheels
            frontRight.brakeTorque = currentBreakForce;
            frontLeft.brakeTorque = currentBreakForce;
            backRight.brakeTorque = currentBreakForce;
            backLeft.brakeTorque = currentBreakForce;

            // Smooth steering logic
            float targetTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

            // Smoothly interpolate the current steering angle to the target angle
            currentTurnAngle = Mathf.Lerp(currentTurnAngle, targetTurnAngle, Time.deltaTime * 15f);

            // Apply the smooth steering angle to the front wheels
            frontLeft.steerAngle = currentTurnAngle;
            frontRight.steerAngle = currentTurnAngle;

            //Update wheel meshes
            UpdateWheel(frontLeft, frontLeftTransform);
            UpdateWheel(frontRight, frontRightTransform);
            UpdateWheel(backLeft, backLeftTransform);
            UpdateWheel(backRight, backRightTransform);
        }
        else
        {
            frontRight.motorTorque = 0f;
            frontLeft.motorTorque = 0f;
        }
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        //Get wheel collider state
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        //Set wheel transform state
        trans.position = position;
        trans.rotation = rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Crashed();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (GameIsLost || GameIsPaused)
        {
            startingFocalLength = Mathf.Lerp(startingFocalLength, 13f, Time.deltaTime * 4f);
            depthOfField.focalLength.value = startingFocalLength;
        }
        else
        {
            startingFocalLength = Mathf.Lerp(startingFocalLength, 1f, Time.deltaTime * 4f);
            depthOfField.focalLength.value = startingFocalLength;
        }
    }

    public void Resume()
    {
        GameIsPaused = false;
        //Time.timeScale = 1f;
        canvasAnim.SetTrigger("PauseSlideDown");
    }

    public void Pause ()
    {
        GameIsPaused = true;
        //Time.timeScale = 0f;
        canvasAnim.SetTrigger("PauseSlideUp");
        retryCountText.text = "Retry Count: " + retryCount;
    }

    public void Crashed ()
    {
        retryCount += 1;
        PlayerPrefs.SetInt("RetryCount", retryCount);
        GameIsLost = true;
        canvasAnim.SetTrigger("LoseSlideUp");
    }
}
