using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeySpawner : MonoBehaviour
{
    [SerializeField] private bool spawnOnATimer = false;
    [SerializeField] private float secondsPerSpawn = 1f;
    [SerializeField] private int numToSpawn = 3;
    [SerializeField] private GameObject monkey = null;

    public static MonkeySpawner instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one MonkeySpawner in scene!");
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(spawnOnATimer)
            StartCoroutine(SpawnMonkeysLoop());
    }

    private IEnumerator SpawnMonkeysLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(secondsPerSpawn);
            SpawnMonkeys(numToSpawn);
        }
    }

    public void SpawnMonkeys(int num)
    {
        for(int i = 0; i < num; i++)
        {
            GameObject go = Instantiate(monkey, transform.position, Quaternion.identity);
        }
    }
}
