using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the Player prefab
 * When main menu is displayed, the menu music plays. The music switches
 * to the fight music when the game is running.
 */

public class MusicController : MonoBehaviour {

    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip actionMusic;

    private AudioSource audioSource;
    private GameManager gameManager;
    
    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    void Update() {
        if(gameManager.gameRunning && audioSource.clip != actionMusic) {
            audioSource.clip = actionMusic;
            audioSource.Play();
        }
    }
}
