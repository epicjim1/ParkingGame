using System.Collections;
using System.Collections.Generic;
//using TreeEditor;
using UnityEngine;

public class ParkedDetector : MonoBehaviour
{
    public GameObject player;  // Assign the player object in the inspector
    private Renderer objRenderer;
    private Material originalMaterial;
    public Material greenMaterial;  // Assign the green material in the inspector
    private BoxCollider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        originalMaterial = objRenderer.material;  // Store the original material
        myCollider = objRenderer.GetComponent<BoxCollider>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (myCollider.bounds.Contains(other.bounds.min) && myCollider.bounds.Contains(other.bounds.max))
            {
                //Debug.Log("player fully in");
                objRenderer.material = greenMaterial;  // Change to green material
            }
            else
            {
                //Debug.Log("player not fully in");
            }
        }
    }

    // When the player exits the collider, reset the material
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            objRenderer.material = originalMaterial;  // Reset to the original material
        }
    }
}
