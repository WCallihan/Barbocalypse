using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    [SerializeField] GameObject life1;
    [SerializeField] GameObject life2;
    [SerializeField] GameObject life3;

    private int maxLives = 3;
    public int currentLives;

    // Start is called before the first frame update
    void Start() {
        currentLives = maxLives;
    }

    // Update is called once per frame
    void Update() {
    }

    //called by GameManager when player looses a life
    public void UpdateLives(int damage) {
        currentLives -= damage;
        //deactivates heart sprites as the player looses lives
        if(currentLives == 2) {
            life1.SetActive(false);
        }
        if(currentLives == 1) {
            life2.SetActive(false);
        }
        if(currentLives == 0) {
            life3.SetActive(false);
        }
    }
}
