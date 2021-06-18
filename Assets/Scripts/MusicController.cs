using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip actionMusic;

    private AudioSource audioSource;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update() {
        if(gameManager.gameRunning && audioSource.clip != actionMusic) {
            audioSource.clip = actionMusic;
            audioSource.Play();
        }
    }
}
