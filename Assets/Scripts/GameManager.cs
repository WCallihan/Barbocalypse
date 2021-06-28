using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/* Used by the GameManager object
 * This script mostly deals with the User Interface and controlling the various
 * screens that the player can view. This manager sets the gameRunning variable used by
 * many other objects, and stops everything when it detects that the Player has died.
 * This scripts also controls the game timer displayed in the top right and the score displayed
 * in the top left. The background is gradually turned to a dark blue while the game is running.
 */

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject restartScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] TextMeshProUGUI scoreValueText;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] TextMeshProUGUI endHighScoreText;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] SpriteRenderer background;
    private Color originalColor;
    private Color finalColor = new Color(0, 0, 0.078f);
    private float backgroundTimer = 0;

    private PlayerController playerController;
    private HealthManager healthManager;

    public float gameTime = 0;
    public bool gameRunning;
    public int score;

    void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        healthManager = GameObject.Find("Player").GetComponent<HealthManager>();
        gameRunning = false;
        originalColor = background.color;
    }

    void Update() {
        //fades the background into a deep blue over 5 minutes
        backgroundTimer += Time.deltaTime / 300f; //hits 1 at 5 minutes
        background.color = Color.Lerp(originalColor, finalColor, backgroundTimer);

        //gametimer keeps track of how long the player has been going
        if(gameRunning) {
            gameTime += Time.deltaTime;
            int seconds = Mathf.RoundToInt(gameTime % 60);
            int minutes = Mathf.FloorToInt(gameTime / 60);
            string minutesString = null;
            string secondsString = Mathf.RoundToInt(seconds).ToString();
            if(minutes > 0) {
                minutesString = minutes.ToString() + ":";
                if(seconds < 10) {
                    secondsString = "0" + secondsString;
                }
            }
            timerText.text = minutesString + secondsString;
        }

        //stops the game when the player is dead
        if(playerController.isDead) {
            //sets high score if applicable
            if(score > PlayerPrefs.GetInt("HighScore", 0)) {
                PlayerPrefs.SetInt("HighScore", score);
            }
            endHighScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);

            //stop the game
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
        timerText.gameObject.SetActive(true);
        healthManager.SpawnLives();
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
        controlsScreen.SetActive(false);
        startScreen.SetActive(true);
    }
    //turns on the Options Screen and turns off all other possible screens
    public void OptionsScreen() {
        startScreen.SetActive(false);
        controlsScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
    //turns on the Controls Screen and turns off all other possible screens
    public void ControlsScreen() {
        optionsScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }
    //turns on the Credits Screen and turns off all other possible screens
    public void CreditsScreen() {
        startScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }
}
