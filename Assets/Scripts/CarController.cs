using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
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

    private void FixedUpdate()
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

        //Take care of steering
        //currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        //frontLeft.steerAngle = currentTurnAngle;
        //frontRight.steerAngle = currentTurnAngle;

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
            Debug.Log("hit");
        }
    }
}
