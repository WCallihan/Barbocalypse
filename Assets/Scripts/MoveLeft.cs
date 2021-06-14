using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float leftBound;
    [SerializeField] float rightBound;

    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        //constantly moves left while game is running
        transform.Translate(Vector2.left * Time.deltaTime * speed);

        //teleports the object back to the right bound when it reaches the left bound
        if(transform.position.x <= leftBound) {
            transform.position = new Vector2(rightBound, transform.position.y);
        }
    }
}
