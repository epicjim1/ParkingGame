using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModelSwitcher : MonoBehaviour
{
    public GameObject[] carModels;
    private int currentModelIndex = 0;

    public void SwitchCarModel()
    {
        //Debug.Log("switch method working");

        // Deactivate current model
        carModels[currentModelIndex].SetActive(false);

        // Update model index
        currentModelIndex = (currentModelIndex + 1) % carModels.Length;

        // Activate next model
        carModels[currentModelIndex].SetActive(true);
    }
}
