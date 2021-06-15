using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupType { None, Sword }

public class Powerup : MonoBehaviour {

    public PowerupType powerupType;

    private float startPos;
    private float topPos;
    private float bottomPos;
    private float speed = 0.2f;
    private bool movingUp = true;

    // Start is called before the first frame update
    void Start() {
        startPos = transform.position.y;
        topPos = startPos + 0.08f;
        bottomPos = startPos - 0.08f;
    }

    // Update is called once per frame
    void Update() {
        //constantly bobs up and down slightly
        if(movingUp) {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
        } else {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
        if(transform.position.y >= topPos) {
            movingUp = false;
        } else if(transform.position.y <= bottomPos) {
            movingUp = true;
        }
    }
}
