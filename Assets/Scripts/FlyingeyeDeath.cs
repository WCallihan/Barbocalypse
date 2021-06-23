using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingeyeDeath : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        //constrains the enemy's y position
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update() {
        //unconstrains the enemy's y position so it falls when it dies
        if(gameObject.GetComponent<EnemyController>().m_isDead) {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
