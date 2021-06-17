using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject restartScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] TextMeshProUGUI scoreValueText;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] GameObject livesDisplay;
    [SerializeField] TextMeshProUGUI timerText;

    private PlayerController playerController;
    private int gameTime = 60;
    
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
            scoreDisplay.SetActive(false);
            timerText.gameObject.SetActive(false);
        }
    }

    //triggered when the start button is pressed; spawns the player and begins spawning enemies
    public void StartGame() {
        gameRunning = true;
        startScreen.SetActive(false);
        scoreDisplay.SetActive(true);
        livesDisplay.SetActive(true);
        StartCoroutine("GameTimer");
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

    //not the same as RestartGame(); to be used for the various menu screens only, not to restart the game
    public void MenuReturn() {
        creditsScreen.SetActive(false);
        optionsScreen.SetActive(false);
        startScreen.SetActive(true);
    }
    public void OptionsScreen() {
        startScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
    public void CreditsScreen() {
        startScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    IEnumerator GameTimer() {
        timerText.gameObject.SetActive(true);
        timerText.text = "" + gameTime;
        while(gameTime > 0 && gameRunning) {
            yield return new WaitForSeconds(1);
            gameTime--;
            timerText.text = "" + gameTime;
        }
    }
}
