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
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] TextMeshProUGUI scoreValueText;
    [SerializeField] TextMeshProUGUI endScoreText;
    [SerializeField] TextMeshProUGUI endHighScoreText;
    [SerializeField] GameObject livesDisplay;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] SpriteRenderer background;
    private Color originalColor;
    private Color finalColor = new Color(0, 0, 0.078f);
    private float backgroundTimer = 0;

    private PlayerController playerController;

    public float gameTime = 0;
    public bool gameRunning;
    public int score;

    // Start is called before the first frame update
    void Start() {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameRunning = false;
        originalColor = background.color;
    }

    // Update is called once per frame
    void Update() {
        //fades the background into a deep blue over 10 minutes
        backgroundTimer += Time.deltaTime / 600f; //hits 1 at 10 minutes
        background.color = Color.Lerp(originalColor, finalColor, backgroundTimer);

        //gametimer keeps track of how long the player has been going
        if(gameRunning) {
            gameTime += Time.deltaTime;
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

    //not the same as RestartGame(); to be used for the various menu screens only, not to restart the game
    public void MenuReturn() {
        creditsScreen.SetActive(false);
        optionsScreen.SetActive(false);
        controlsScreen.SetActive(false);
        startScreen.SetActive(true);
    }
    public void OptionsScreen() {
        startScreen.SetActive(false);
        controlsScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
    public void ControlsScreen() {
        optionsScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }
    public void CreditsScreen() {
        startScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }
}
