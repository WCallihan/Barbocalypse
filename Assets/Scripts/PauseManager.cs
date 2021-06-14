using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

    [SerializeField] GameObject pauseScreen;

    private GameManager gameManager;

    public bool isPaused;

    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isPaused = false;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.P) && !isPaused && gameManager.gameRunning) {
            PauseGame();
        } else if(Input.GetKeyDown(KeyCode.P) && isPaused && gameManager.gameRunning) {
            ResumeGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0; //pauses everything in the game
        isPaused = true;
        gameManager.gameRunning = false;
        pauseScreen.SetActive(true); //shows paused screen
    }

    public void ResumeGame() {
        Time.timeScale = 1; //resumes everything in the game
        isPaused = false;
        gameManager.gameRunning = true;
        pauseScreen.SetActive(false);
    }
}
