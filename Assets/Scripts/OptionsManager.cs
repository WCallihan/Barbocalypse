using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour {

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Toggle bloodToggle;
    [SerializeField] TextMeshProUGUI highScoreText;

    public int highScore;
    
    // Start is called before the first frame update
    void Start() {
        //sets initial values of the sliders and toggle to the playerprefs saved
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 1);
        bloodToggle.isOn = (PlayerPrefs.GetInt("BloodToggle", 1) == 1);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    // Update is called once per frame
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
