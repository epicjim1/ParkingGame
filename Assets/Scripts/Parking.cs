using System.Collections;
using System.Collections.Generic;
using TMPro;
//using TreeEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parking : MonoBehaviour
{
    public int secondsTillWin = 3;
    public GameObject secondsTillWinText;
    private Coroutine winCoroutine = null;
    private int nextSceneToLoad;
    private bool hasTriggeredWin = false;

    public GameObject unparkedChild;
    public GameObject parkedChild;

    private BoxCollider myCollider;
    public Animator canvasAnim;

    void Start()
    {
        myCollider = GetComponent<BoxCollider>();
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        secondsTillWinText.SetActive(false);
        unparkedChild.SetActive(true);
        parkedChild.SetActive(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !hasTriggeredWin)
        {
            //if (myCollider.bounds.Contains(other.bounds.min) && myCollider.bounds.Contains(other.bounds.max))
            if (IsFullyInside(other.bounds, myCollider.bounds))
            {
                Debug.Log("cars inside");
                unparkedChild.SetActive(false);
                parkedChild.SetActive(true);

                if (winCoroutine == null) // Start coroutine if not already running
                {
                    secondsTillWinText.SetActive(true);
                    winCoroutine = StartCoroutine(WaitAndTriggerWin());
                }
            }
            else
            {
                secondsTillWinText.SetActive(false);
                unparkedChild.SetActive(true);
                parkedChild.SetActive(false);

                if (winCoroutine != null) // Stop coroutine if player leaves bounds
                {
                    StopCoroutine(winCoroutine);
                    winCoroutine = null;
                }
            }
        }
    }

    private IEnumerator WaitAndTriggerWin()
    {
        float remainingTime = secondsTillWin; // Start with the total time

        while (remainingTime > 0)
        {
            // Update the countdown text
            secondsTillWinText.GetComponent<TextMeshProUGUI>().text = $"Stay parked for \"{remainingTime:F1}\" seconds";

            yield return null; // Wait for the next frame
            remainingTime -= Time.deltaTime; // Decrement the remaining time
        }

        secondsTillWinText.SetActive(false);

        if (nextSceneToLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneToLoad);
        }

        canvasAnim.SetTrigger("WinSlideUp");
        CarController.GameIsPaused = true;
        winCoroutine = null; // Reset the coroutine reference after completion
        hasTriggeredWin = true;
    }

    bool IsFullyInside(Bounds innerBounds, Bounds outerBounds)
    {
        // Get all 8 corners of the inner bounds
        Vector3[] corners = new Vector3[8];

        Vector3 min = innerBounds.min;
        Vector3 max = innerBounds.max;

        corners[0] = new Vector3(min.x, min.y, min.z); // Bottom-front-left
        corners[1] = new Vector3(min.x, min.y, max.z); // Bottom-front-right
        corners[2] = new Vector3(min.x, max.y, min.z); // Top-front-left
        corners[3] = new Vector3(min.x, max.y, max.z); // Top-front-right
        corners[4] = new Vector3(max.x, min.y, min.z); // Bottom-back-left
        corners[5] = new Vector3(max.x, min.y, max.z); // Bottom-back-right
        corners[6] = new Vector3(max.x, max.y, min.z); // Top-back-left
        corners[7] = new Vector3(max.x, max.y, max.z); // Top-back-right

        // Check if all corners are inside the outer bounds
        foreach (var corner in corners)
        {
            if (!outerBounds.Contains(corner))
            {
                return false; // If any corner is outside, return false
            }
        }

        return true; // All corners are inside
    }
}
