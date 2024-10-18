using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float swingAmplitude = 30f; // Amplitude of the swing (maximum angle)
    public float swingFrequency = 1f;   // Frequency of the swing (speed of oscillation)
    public float launchForce = 10000f; // Stronger force to launch the player

    void Update()
    {
        // Calculate the swing angle using a sine wave
        float angle = swingAmplitude * Mathf.Sin(Time.time * swingFrequency);

        // Apply the rotation to the hammer
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision);

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Get the collision normal, but only apply force in the x-direction
            ContactPoint contact = collision.contacts[0];
            Vector3 forceDirection = contact.normal;

            // Zero out the y and z components to apply force only along the x-axis
            forceDirection.x = -forceDirection.x;
            forceDirection.y = 0.7f;
            forceDirection.z = 0f;

            //Vector3 forceDirection = new Vector3(1, 0.5f, 0);
            Debug.DrawRay(collision.contacts[0].point, forceDirection * 10f, Color.red, 2f);
            rb.AddForce(launchForce * forceDirection, ForceMode.Impulse);
        }
    }
}
