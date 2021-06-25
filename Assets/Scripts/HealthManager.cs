using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    [SerializeField] GameObject lifePrefab;
    [SerializeField] int maxLives;

    private GameObject livesDisplay;
    private List<GameObject> lives = new List<GameObject>();

    public int currentLives;

    // Start is called before the first frame update
    void Start() {
        livesDisplay = GameObject.Find("Lives");
        currentLives = maxLives;
    }

    // Update is called once per frame
    void Update() {
    }

    //called by GameManager when the game starts
    public void SpawnLives() {
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
