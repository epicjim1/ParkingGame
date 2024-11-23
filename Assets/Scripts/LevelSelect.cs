using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public float speed = 0.5f;

    public Transform[] levelBlocks; // Assign the level block transforms in the Inspector
    public float moveSpeed = 5f;    // Speed at which the car moves
    public int currentLevelIndex = 0; // Index of the selected level block
    private Transform targetLevel; // The target level to move to

    public Button[] lvlButtons;

    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 2);

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 2 > levelAt)
            {
                lvlButtons[i].interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.E))
        {
            this.gameObject.transform.position += new Vector3(speed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            this.gameObject.transform.position -= new Vector3(speed, 0, 0);
        }*/

        Vector3 targetPosition = new Vector3(levelBlocks[currentLevelIndex].position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // Smoothly move the car towards the target level's position
        //Vector3 targetPosition = new Vector3(targetLevel.position.x, transform.position.y, transform.position.z);
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // Stop moving when close enough to the target
        /*if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetLevel = null;
        }*/
    }

    // Call this function to move the car to the next level
    public void MoveToNextLevel()
    {
        if (currentLevelIndex < levelBlocks.Length - 1)
        {
            currentLevelIndex++;
        }
    }

    // Call this function to move the car to the previous level
    public void MoveToPreviousLevel()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
        }
    }

    // Call this function when a level block is clicked
    public void MoveToLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelBlocks.Length)
        {
            targetLevel = levelBlocks[levelIndex];
            currentLevelIndex = levelIndex;
        }
        else
        {
            Debug.LogWarning("Invalid level index!");
        }
    }
}
