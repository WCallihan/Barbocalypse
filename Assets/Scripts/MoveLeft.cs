using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the cloud objects
 * As long as the game is running, the clouds move left at a constant
 * speed and when they hit the left bound, are tleported back to the right bound.
 */

public class MoveLeft : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float leftBound;
    [SerializeField] float rightBound;

    private GameManager gameManager;
    
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        //constantly moves left while game is running
        transform.Translate(Vector3.left * Time.deltaTime * speed);

        //teleports the object back to the right bound when it reaches the left bound
        if(transform.position.x <= leftBound) {
            transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
        }
    }
}
