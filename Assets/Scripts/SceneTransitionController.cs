using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    private Image theImage;
    public float transSpeed = 2f;

    [SerializeField]
    private bool shouldReveal;
    private bool goToMainMenu = false; // New flag to decide if we go to the main menu

    public bool isMainMenu = false;

    void Start()
    {
        theImage = GetComponentInChildren<Image>();
        if (!isMainMenu)
        {
            theImage.material.SetFloat("_Cutoff", -0.1f - theImage.material.GetFloat("_EdgeSmoothing"));
        }
        else
        {
            theImage.material.SetFloat("_Cutoff", 1.1f);
        }
        shouldReveal = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            shouldReveal = !shouldReveal;
        }

        if (shouldReveal)
        {
            theImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(theImage.material.GetFloat("_Cutoff"), 1.1f, transSpeed * Time.deltaTime));
        }
        else
        {
            theImage.material.SetFloat("_Cutoff", Mathf.MoveTowards(theImage.material.GetFloat("_Cutoff"), -0.1f - theImage.material.GetFloat("_EdgeSmoothing"), transSpeed * Time.deltaTime));

            if (theImage.material.GetFloat("_Cutoff") == -0.1f - theImage.material.GetFloat("_EdgeSmoothing"))
            {
                if (goToMainMenu)
                {
                    SceneManager.LoadScene("StartMenu");
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next level
                }
            }
        }
    }

    public void GoToMainMenu()
    {
        goToMainMenu = true;
        shouldReveal = false; // Start the transition effect
    }

    public void GoToNextLevel ()
    {
        goToMainMenu = false; //not nesscary
        shouldReveal = false;
    }
}
