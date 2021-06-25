using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] Transform leftSpawn;
    [SerializeField] Transform rightSpawn;
    [SerializeField] GameObject barbarianPrefab;
    [SerializeField] GameObject skeletonPrefab;
    [SerializeField] GameObject goblinPrefab;
    [SerializeField] GameObject flyingeyePrefab;
    [SerializeField] int maxEnemies;
    [SerializeField] GameObject[] powerupPrefabs;
    [SerializeField] int maxPowerups;

    private GameManager gameManager;
    private float minEnemyWait = 0.5f, maxEnemyWait = 3f;
    private bool waitingToSpawnEnemy;
    private bool waitingToSpawnPowerup;
    private float powerupSpawnTime = 30;

    private float barbarianSpawnRate = 1;
    private float skeletonSpawnRate = 0;
    private float goblinSpawnRate = 0;
    private float flyingeyeSpawnRate = 0;

    private float spawnRateTimer;
    private float maxEnemyTimer;
    private float maxEnemyWaitTimer;
    private float maxPowerupTimer;
    private float powerupWaitTimer;

    // Start is called before the first frame update
    void Start() {
        gameManager = gameObject.GetComponent<GameManager>();
        waitingToSpawnEnemy = false;
        waitingToSpawnPowerup = false;
        InvokeRepeating("SpawnPowerup", 0, 30);
    }

    // Update is called once per frame
    void Update() {
        //changes enemy spawnrates over certain intervals
        if(gameManager.gameRunning) {
            //over 100 seconds, barbarian = 100% -> 50%, skeleton = 0% -> 50%
            if(gameManager.gameTime <= 100f) {
                spawnRateTimer += Time.deltaTime / 100f;
                barbarianSpawnRate = Mathf.Lerp(1, 0.5f, spawnRateTimer);
                skeletonSpawnRate = 1 - barbarianSpawnRate;
                if(spawnRateTimer >= 100f) {
                    spawnRateTimer = 0;
                }
            //over 50 seconds, barbarian = 50% -> 35%, skeleton = 50% -> 40%, goblin = 0% -> 25%
            } else if(gameManager.gameTime <= 150f) {
                spawnRateTimer += Time.deltaTime / 50f; //hits 1 at 50 seconds
                barbarianSpawnRate = Mathf.Lerp(0.5f, 0.35f, spawnRateTimer);
                skeletonSpawnRate = Mathf.Lerp(0.5f, 0.4f, spawnRateTimer);
                goblinSpawnRate = 1 - skeletonSpawnRate - barbarianSpawnRate;
                if(spawnRateTimer >= 50f) {
                    spawnRateTimer = 0;
                }
            //over 30 seconds, barbarian = 35% -> 20%, goblin = 25% -> 30%, flyingeye = 0% -> 10%
            } else if(gameManager.gameTime <= 180f) {
                spawnRateTimer += Time.deltaTime / 30f; //hits 1 at 30 seconds
                barbarianSpawnRate = Mathf.Lerp(0.35f, 0.2f, spawnRateTimer);
                goblinSpawnRate = Mathf.Lerp(0.25f, 0.3f, spawnRateTimer);
                flyingeyeSpawnRate = 1 - barbarianSpawnRate - skeletonSpawnRate - goblinSpawnRate;
            }

            //over 200 seconds, maximum enemies = 5 -> 15
            if(maxEnemies < 15) {
                maxEnemyTimer += Time.deltaTime / 200f; //hits 1 at 200 seconds
                maxEnemies = (int)Mathf.Lerp(5, 15, maxEnemyTimer);
            }

            //over 260 seconds, maximum enemy wait time = 3 sec -> 1 sec
            if(maxEnemyWait > 1f) {
                maxEnemyWaitTimer += Time.deltaTime / 260f;
                maxEnemyWait = Mathf.Lerp(3f, 1f, maxEnemyWaitTimer);
            }

            //over 180 seconds, maximum powerups = 2 -> 6
            if(maxPowerups < 6) {
                maxPowerupTimer += Time.deltaTime / 180f; //hits 1 at 180 seconds
                maxPowerups = (int)Mathf.Lerp(2, 6, maxPowerupTimer);
            }

            //over 180 seconds, powerup wait timer = 30 sec -> 10 sec
            if(powerupSpawnTime > 10) {
                powerupWaitTimer += Time.deltaTime / 180f; //hits 1 at 180 seconds
                powerupSpawnTime = Mathf.Lerp(30, 10, powerupWaitTimer);
            }
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
        float spawnRateProb = Random.Range(0f, 1f);
        if(spawnRateProb <= barbarianSpawnRate) {
            enemyToSpawn = barbarianPrefab;
        } else if(spawnRateProb <= barbarianSpawnRate + skeletonSpawnRate) {
            enemyToSpawn = skeletonPrefab;
        } else if(spawnRateProb <= barbarianSpawnRate + skeletonSpawnRate + goblinSpawnRate) {
            enemyToSpawn = goblinPrefab;
        } else if(spawnRateProb <= barbarianSpawnRate + skeletonSpawnRate + goblinSpawnRate + flyingeyeSpawnRate) {
            enemyToSpawn = flyingeyePrefab;
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
        yield return new WaitForSeconds(powerupSpawnTime);
        waitingToSpawnPowerup = false;
    }
}