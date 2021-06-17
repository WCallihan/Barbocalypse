using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] Transform leftSpawn;
    [SerializeField] Transform rightSpawn;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxEnemies;
    [SerializeField] GameObject[] powerupPrefabs;
    [SerializeField] int maxPowerups;

    private GameManager gameManager;
    private float minEnemyWait = 1f, maxEnemyWait = 3f;
    private bool waitingToSpawnEnemy;
    private bool waitingToSpawnPowerup;
    
    // Start is called before the first frame update
    void Start() {
        gameManager = gameObject.GetComponent<GameManager>();
        waitingToSpawnEnemy = false;
        waitingToSpawnPowerup = false;
        InvokeRepeating("SpawnPowerup", 0, 30);
    }

    // Update is called once per frame
    void Update() {
        if(gameManager.gameRunning && !waitingToSpawnEnemy && GameObject.FindObjectsOfType<EnemyController>().Length < maxEnemies) { //only spawns more if the total number on the field don't exceed the max
            StartCoroutine(SpawnEnemy());
        }
        if(gameManager.gameRunning && !waitingToSpawnPowerup && GameObject.FindObjectsOfType<Powerup>().Length < maxPowerups) {
            StartCoroutine(SpawnPowerup());
        }
    }

    IEnumerator SpawnEnemy() {
        float waitTimer = Random.Range(minEnemyWait, maxEnemyWait);
        int spawnSide = Random.Range(0, 2);

        waitingToSpawnEnemy = true;
        yield return new WaitForSeconds(waitTimer);
        if(spawnSide == 0) { //spawn on left side
            Instantiate(enemyPrefab, leftSpawn);
        } else if(spawnSide == 1) { //spawn on right side
            Instantiate(enemyPrefab, rightSpawn);
        } else {
            Debug.Log("spawn failed");
        }
        waitingToSpawnEnemy = false;
    }

    IEnumerator SpawnPowerup() {
        int powerupIndex = Random.Range(0, powerupPrefabs.Length);
        Vector3 powerupSpawn = new Vector3(Random.Range(leftSpawn.transform.position.x, rightSpawn.transform.position.x), 0.7f, 0.5f);
        Instantiate(powerupPrefabs[powerupIndex], powerupSpawn, powerupPrefabs[powerupIndex].transform.rotation);
        waitingToSpawnPowerup = true;
        yield return new WaitForSeconds(30);
        waitingToSpawnPowerup = false;
    }
}
