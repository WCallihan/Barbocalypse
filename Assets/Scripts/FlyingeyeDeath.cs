using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the Flyingeye enemy prefab
 * Freezes the flyingeye's y position to give it the effect of
 * flying and hovering. Once the flyingeye dies, the y constraint
 * is unlocked to give effect of falling from the sky.
 */

public class FlyingeyeDeath : MonoBehaviour {
    void Start() {
        //constrains the enemy's y position
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    void Update() {
        //unconstrains the enemy's y position so it falls when it dies
        if(gameObject.GetComponent<EnemyController>().m_isDead) {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
