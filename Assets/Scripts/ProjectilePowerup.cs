using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePowerup : MonoBehaviour {

    private PlayerController player;
    private Vector3 moveVector;
    private float speed = 10;

    public Vector3 spawnOffset;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        //assigns which way the sprite will move
        if(player.m_facingDirection == -1) {
            GetComponent<SpriteRenderer>().flipX = true; //flip sprite if the player is facing left
            moveVector = Vector3.left;
        } else {
            moveVector = Vector3.right;
        }
    }

    // Update is called once per frame
    void Update() {
        //moves in assigned direction at speed for as long as its alive
        transform.Translate(moveVector * Time.deltaTime * speed);
        if(Mathf.Abs(transform.position.x) > 9.25f) {
            Destroy(gameObject);
        }
    }

    //detects and damages enemies
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Enemy")) {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(1);
        }
    }
}
