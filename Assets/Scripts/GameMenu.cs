using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void Retry ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu ()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Quit()
    {
        Debug.Log("Quit game");
    }
}
