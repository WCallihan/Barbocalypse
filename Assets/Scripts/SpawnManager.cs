using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] Transform leftSpawn;
    [SerializeField] Transform rightSpawn;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxEnemies;

    private GameManager gameManager;
    private float minWait = 1f, maxWait = 3f;
    private bool waitingToSpawnLeft, waitingToSpawnRight;
    
    // Start is called before the first frame update
    void Start() {
        gameManager = gameObject.GetComponent<GameManager>();
        waitingToSpawnLeft = false;
        waitingToSpawnRight = false;
    }

    // Update is called once per frame
    void Update() {
        if(gameManager.gameRunning && GameObject.FindObjectsOfType<EnemyController>().Length < maxEnemies-1) { //only spawns more if the total number on the field don't exceed the max
            if(!waitingToSpawnLeft) {
                float timer = Random.Range(minWait, maxWait);
                waitingToSpawnLeft = true;
                StartCoroutine(SpawnLeftEnemy(enemyPrefab, timer));
            }
            if(!waitingToSpawnRight) {
                float timer = Random.Range(minWait, maxWait);
                waitingToSpawnRight = true;
                StartCoroutine(SpawnRightEnemy(enemyPrefab, timer));
            }
        }
    }

    IEnumerator SpawnLeftEnemy(GameObject enemyToSpawn, float waitTimer) {
        yield return new WaitForSeconds(waitTimer);
        Instantiate(enemyToSpawn, leftSpawn);
        waitingToSpawnLeft = false;
    }
    IEnumerator SpawnRightEnemy(GameObject enemyToSpawn, float waitTimer) {
        yield return new WaitForSeconds(waitTimer);
        Instantiate(enemyToSpawn, rightSpawn);
        waitingToSpawnRight = false;
    }
}
