using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModelSwitcher : MonoBehaviour
{
    public GameObject[] carModels;
    private int currentModelIndex = 0;

    private void Start()
    {
        // Select a random model index
        currentModelIndex = Random.Range(0, carModels.Length);

        // Activate the selected model
        for (int i = 0; i < carModels.Length; i++)
        {
            carModels[i].SetActive(i == currentModelIndex);
        }
    }

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
