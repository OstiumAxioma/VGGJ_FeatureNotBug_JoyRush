using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab; // Assign this in the inspector
    [SerializeField] private GameObject player;
    [SerializeField] private int enemyCount;
    [SerializeField] private GameObject logicManager;
    public float spawnInterval = 2f; // Time between each spawn
    private float timer;
    [SerializeField] private Vector2 mapMin;
    [SerializeField] private Vector2 mapMax;
    [SerializeField] private float spawnDistanceFromPlayer;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyAttackedCD;
    [SerializeField] private float enemyStatusDegradeCD; 

    private List<GameObject> enemies = new List<GameObject>();

    void Update()
    {
        GameManager managerScript = logicManager.GetComponent<GameManager>();
        if (managerScript.isGameOver) return;
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            if (enemies.Count < enemyCount)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }
        UpdateGameScore();
    }

    void UpdateGameScore ()
    {
        int currScore = 0;
        foreach (GameObject o in enemies) {
            Enemy enemyScript = o.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // Now you can interact with the Enemy script
                currScore += enemyScript.GetScore();
            }
        }

        GameManager gameManagerScript = logicManager.GetComponent<GameManager>();
        if (gameManagerScript != null)
        {
            gameManagerScript.updateScore(currScore);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = Vector3.zero;
        bool positionFound = false;

        while (!positionFound)
        {
            spawnPosition = new Vector3(
                Random.Range(mapMin.x, mapMax.x),
                Random.Range(mapMin.y, mapMax.y),
                0); // Assuming a 2D game; for 3D, adjust accordingly

            if (Vector3.Distance(spawnPosition, player.transform.position) >= spawnDistanceFromPlayer)
            {
                positionFound = true;
            }
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetPlayer(player);
            enemyScript.SetSpeed(enemySpeed);
            enemyScript.SetAttackedCD(enemyAttackedCD);
            enemyScript.SetStatusDegradeCD(enemyStatusDegradeCD);
        }
        enemies.Add(newEnemy);
    }

}
