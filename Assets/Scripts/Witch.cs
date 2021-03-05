using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    private int pointValue = 10; // in addition  to the 3*30 points for hitting
    private float speed = 5f;
    private Vector2 destination;
    private Vector2 position;
    private int health = 3;
    private GameManager gameManager;
    public GameObject bonus;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        destination = new Vector2(-transform.position.x, transform.position.y);
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            gameManager.UpdateScore(pointValue);
            GameObject bonusText = Instantiate(bonus, transform.position, Quaternion.identity);
            Destroy(bonusText, 1.0f);
            Destroy(gameObject);
        }

        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, destination) < 0.0001f)
        {
            Destroy(gameObject);
        }
    }

    public void DecrementHealth(int decrement)
    {
        health -= decrement;
    }
}
