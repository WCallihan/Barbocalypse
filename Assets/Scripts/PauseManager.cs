using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the PauseManager object
 * This manages whether or not the game is paused and pauses and unpauses
 * the game when the pause key is pressed. When the game is paused, everything
 * stops and no input for the Player is taken.
 */

public class PauseManager : MonoBehaviour {

    [SerializeField] GameObject pauseScreen;

    private GameManager gameManager;

    public bool isPaused;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isPaused = false;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPaused && gameManager.gameRunning) {
            PauseGame();
        } else if(Input.GetKeyDown(KeyCode.Escape) && isPaused) {
            ResumeGame();
        }
    }

    //pauses everything in the game and shows the pause screen
    public void PauseGame() {
        Time.timeScale = 0; 
        isPaused = true;
        gameManager.gameRunning = false;
        pauseScreen.SetActive(true);
    }

    //resumes everything in the game and deactivates the pause screen
    public void ResumeGame() {
        Time.timeScale = 1;
        isPaused = false;
        gameManager.gameRunning = true;
        pauseScreen.SetActive(false);
    }
}
