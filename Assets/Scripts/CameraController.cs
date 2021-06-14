using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    [SerializeField] float leftBound;
    [SerializeField] float rightBound;
    [SerializeField] GameObject followPoint;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void LateUpdate() {
        //follows player at determined point above their head with set boundaries left and right
        transform.position = followPoint.gameObject.transform.position + new Vector3(0, 0, -10);
        if(transform.position.x <= leftBound) {
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        } else if(transform.position.x >= rightBound) {
            transform.position = new Vector3(rightBound, transform.position.y, transform.position.z);
        }
    }
}
