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
            if (myCollider.bounds.Contains(other.bounds.min) && myCollider.bounds.Contains(other.bounds.max))
            {
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
}
