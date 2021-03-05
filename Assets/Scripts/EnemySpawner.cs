using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Timers
    [SerializeField]
    private float intervalMultiplier = 1.0f;
    private float maxTime;
    private float witchTimer = 0.0f;
    public float witchInterval; // set in Start()
    private float spiderTimer = 0.0f;
    public float spiderInterval = 5.0f;
    private float pumpkinTimer = 0.0f;
    public float pumpkinInterval = 5.0f;
    private float ghostTimer = 0.0f;
    public float ghostInterval = 5.0f;

    // Witchy Stuff
    private int numberOfWitches;
    
    // Prefabs
    public GameObject witchPrefab;
    public GameObject spiderPrefab;
    public GameObject pumpkinPrefab;
    public GameObject ghostPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SetUp(); // also called in GameManager when we change rounds.
    }

    // Update is called once per frame
    void Update()
    {
        // Timers
        witchTimer += Time.deltaTime;
        spiderTimer += Time.deltaTime;
        pumpkinTimer += Time.deltaTime;
        ghostTimer += Time.deltaTime;

        if(witchTimer >= witchInterval)
        {
            SpawnWitch();
            witchTimer = 0.0f;
        }

        if (spiderTimer >= spiderInterval * intervalMultiplier)
        {
            SpawnSpider();
            spiderTimer = 0.0f;
        }

        if (pumpkinTimer >= pumpkinInterval * intervalMultiplier)
        {
            SpawnPumpkin();
            pumpkinTimer = 0.0f;
        }

        if (ghostTimer >= ghostInterval * intervalMultiplier)
        {
            SpawnGhost();
            ghostTimer = 0.0f;
        }
    }

    public void SetUp()
    {
        maxTime = GetComponent<GameManager>().GetMaxTime();
        numberOfWitches = Random.Range(3, 5); // minimum of 2, 3rd spawns at 0 sec left of level
        witchInterval = maxTime / (float)numberOfWitches;
    }

    private void SpawnWitch()
    {
        GameObject witch = Instantiate(witchPrefab);
    }

    private void SpawnSpider()
    {
        Vector2 destination = new Vector2(Random.Range(-4, 4), Random.Range(3, 6));
        GameObject spider = Instantiate(spiderPrefab, new Vector2(destination.x, 8.0f), Quaternion.identity);
        spider.GetComponent<Spider>().SetDesination(destination);
    }

    private void SpawnPumpkin()
    {
        GameObject pumpkin = Instantiate(pumpkinPrefab);
        Destroy(pumpkin, 5.0f);
    }

    private void SpawnGhost()
    {
        Vector2 startPos = new Vector2(Random.Range(-5, 5), -4);
        GameObject ghost = Instantiate(ghostPrefab, startPos, Quaternion.identity);
    }

    public float GetIntervalMultiplier()
    {
        return intervalMultiplier;
    }

    public void SetIntervalMuliplier(float newMultiplier)
    {
        intervalMultiplier = newMultiplier;
    }
}
