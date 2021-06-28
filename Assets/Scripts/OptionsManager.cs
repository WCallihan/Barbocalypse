using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Used by the OptionsManager object
 * The options uses PlayerPrefs to set the options as the player
 * has set them and updates the PlayerPrefs when they are changed.
 * These settings and highscore are saved between play throughs.
 */

public class OptionsManager : MonoBehaviour {

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Toggle bloodToggle;
    [SerializeField] Scrollbar livesScrollbar;
    [SerializeField] TextMeshProUGUI highScoreText;

    public int highScore;
    
    void Start() {
        //sets initial values of the sliders and toggle to the playerprefs saved
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1);
        bloodToggle.isOn = (PlayerPrefs.GetInt("BloodToggle", 1) == 1);
        livesScrollbar.value = PlayerPrefs.GetFloat("LivesDifficulty", 0.5f);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    void Update() {
        //changes the playerprefs when the values are changed
        if(musicSlider.value != PlayerPrefs.GetFloat("MusicVolume")) {
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        }
        if(effectsSlider.value != PlayerPrefs.GetFloat("EffectsVolume")) {
            PlayerPrefs.SetFloat("EffectsVolume", effectsSlider.value);
        }
        if(bloodToggle.isOn != (PlayerPrefs.GetInt("BloodToggle") == 1)) {
            PlayerPrefs.SetInt("BloodToggle", (bloodToggle.isOn ? 1 : 0));
        }
        if(livesScrollbar.value != PlayerPrefs.GetFloat("LivesDifficulty")) {
            PlayerPrefs.SetFloat("LivesDifficulty", livesScrollbar.value);
        }
        if(highScore != PlayerPrefs.GetInt("HighScore", 0)) {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "High Score: " + highScore;
        }
    }

    //called by a button on the options menu to reset the high score
    public void ResetHighScore() {
        PlayerPrefs.SetInt("HighScore", 0);
        highScore = 0;
        highScoreText.text = "High Score: " + highScore;
    }
}
