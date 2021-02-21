using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private bool allowedToSpawn = true;
    [SerializeField] private float secondsPerSpawn = 1f;
    [SerializeField] private int numToSpawn = 3;
    [SerializeField] private GameObject enemy = null;

    // public static EnemySpawner instance;
    // private void Awake()
    // {
    //     if (instance != null)
    //     {
    //         Debug.LogError("More than one EnemySpawner in scene!");
    //         return;
    //     }
    //     instance = this;
    // }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // public void SetAllowedToSpawnStatus(bool b)
    // {
    //     allowedToSpawn = b;
    // }

    private IEnumerator SpawnEnemies()
    {
        while(true)
        {
            yield return new WaitForSeconds(secondsPerSpawn);
            if(!allowedToSpawn) continue; // don't spawn if not allowed to

            for(int i = 0; i < numToSpawn; i++)
            {
                GameObject go = Instantiate(enemy, transform.position, Quaternion.identity);
            }
        }
    }
}
