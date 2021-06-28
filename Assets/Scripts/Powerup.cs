using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the Powerup prefabs
 * Defines a powerup type enumerator type to be used by the Player
 * and sets one of those types for each powerup. The powerup bobs
 * up and down until it is picked up and destroyed.
 */

public enum PowerupType { None, Sword, Shield, Projectile }

public class Powerup : MonoBehaviour {

    public PowerupType powerupType;

    private float startPos;
    private float topPos;
    private float bottomPos;
    private float speed = 0.2f;
    private bool movingUp = true;

    void Start() {
        startPos = transform.position.y;
        topPos = startPos + 0.08f;
        bottomPos = startPos - 0.08f;
    }

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
