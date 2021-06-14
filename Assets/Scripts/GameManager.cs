using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject restartScreen;
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] TextMeshProUGUI scoreValueText;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] GameObject livesDisplay;

    private PlayerController playerController;
    
    public bool gameRunning;
    public int score;

    // Start is called before the first frame update
    void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameRunning = false;
    }

    // Update is called once per frame
    void Update() {
        //stops the game when the player is dead
        if(playerController.isDead) {
            gameRunning = false;
            restartScreen.SetActive(true);
        }
    }

    //triggered when the start button is pressed; spawns the player and begins spawning enemies
    public void StartGame() {
        gameRunning = true;
        startScreen.SetActive(false);
        scoreDisplay.SetActive(true);
        livesDisplay.SetActive(true);
    }
    //triggered when the restart button is pressed
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //called by EnemyController to update the score value and display
    public void UpdateScore(int addScore) {
        score += addScore;
        scoreValueText.text = "" + score;
        endScoreText.text = "Score: " + score;
    }
}
