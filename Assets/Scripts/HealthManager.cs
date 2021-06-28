using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the Player prefab
 * This script manages the Player's current and maximum lives and portrays
 * them with the hearts displayed in the top left of the screen.
 */

public class HealthManager : MonoBehaviour {

    [SerializeField] GameObject lifePrefab;

    private int maxLives;
    private GameObject livesDisplay;
    private List<GameObject> lives = new List<GameObject>();

    public int currentLives;

    void Start() {
        livesDisplay = GameObject.Find("Lives");
    }

    //called by GameManager when the game starts
    public void SpawnLives() {
        //sets the max lives depending on the lives scrollbar value
        float livesScrollbarVal = PlayerPrefs.GetFloat("LivesDifficulty", 0.5f);
        if(livesScrollbarVal <= 0.25f) {
            maxLives = 5;
        } else if(livesScrollbarVal > 0.25f && livesScrollbarVal < 0.75f) {
            maxLives = 3;
        } else if(livesScrollbarVal >= 0.75f) {
            maxLives = 1;
        }
        currentLives = maxLives;
        //spawns number of hearts equal to number of max lives just below the score text
        Vector3 spawnOffset = new Vector3(0, 0, 0);
        for(int i = 0; i < maxLives; i++) {
            GameObject life = Instantiate(lifePrefab, livesDisplay.transform.position + spawnOffset, lifePrefab.transform.rotation, livesDisplay.transform) as GameObject;
            lives.Add(life);
            spawnOffset += new Vector3(27.2f, 0, 0);
        }
    }

    //called by PlayerController when player looses a life
    public void UpdateLives(int damage) {
        currentLives -= damage;
        Destroy(lives[currentLives].gameObject);
    }
}
