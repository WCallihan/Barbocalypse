using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] Transform leftSpawn;
    [SerializeField] Transform rightSpawn;
    [SerializeField] GameObject barbarianPrefab;
    [SerializeField] GameObject skeletonPrefab;
    [SerializeField] int maxEnemies;
    [SerializeField] GameObject[] powerupPrefabs;
    [SerializeField] int maxPowerups;

    private GameManager gameManager;
    private float minEnemyWait = 0.5f, maxEnemyWait = 3f;
    private bool waitingToSpawnEnemy;
    private bool waitingToSpawnPowerup;

    private float barbarianSpawnRate = 1;
    private float skeletonSpawnRate = 0;
    private float nextProbabilityChange = 10;
    private float nextSpawnRateChange = 20;

    // Start is called before the first frame update
    void Start() {
        gameManager = gameObject.GetComponent<GameManager>();
        waitingToSpawnEnemy = false;
        waitingToSpawnPowerup = false;
        InvokeRepeating("SpawnPowerup", 0, 30);
    }

    // Update is called once per frame
    void Update() {
        //every 10 seconds, the spawn rates are adjusted to spawn harder enemies
        if(gameManager.gameTime >= nextProbabilityChange) {
            if(barbarianSpawnRate > 0.33f) {
                AdjustSpawnRate(ref barbarianSpawnRate, 0.05f, ref skeletonSpawnRate); //decreases barbarian spawn rate and increases skeleton spawn rate by 5%
            }
            nextProbabilityChange += 10;
        }
        //every 20 seconds, the max enemies is increased by 1 and the maximum enemy wait is decreased by 0.25 seconds
        if(gameManager.gameTime >= nextSpawnRateChange) {
            if(maxEnemies < 20) {
                maxEnemies += 1;
            }
            if(maxEnemyWait > 1f) {
                maxEnemyWait -= 0.25f;
            }
            nextSpawnRateChange += 20;
        }

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
        GameObject enemyToSpawn;
        float probability = Random.Range(0f, 1f);
        if(probability <= barbarianSpawnRate) {
            enemyToSpawn = barbarianPrefab;
        } else if(probability <= barbarianSpawnRate + skeletonSpawnRate) {
            enemyToSpawn = skeletonPrefab;
        } else {
            enemyToSpawn = barbarianPrefab;
            Debug.Log("enemy chooser failed");
        }

        waitingToSpawnEnemy = true;
        yield return new WaitForSeconds(waitTimer);
        if(spawnSide == 0) { //spawn on left side
            Instantiate(enemyToSpawn, leftSpawn);
        } else if(spawnSide == 1) { //spawn on right side
            Instantiate(enemyToSpawn, rightSpawn);
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


    void AdjustSpawnRate(ref float enemy1Prob, float adjustment, ref float enemy2Prob) {
        enemy1Prob -= adjustment;
        enemy2Prob += adjustment;
    }
}