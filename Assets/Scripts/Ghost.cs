using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Vector2 destination;
    private GameManager gameManager;
    private int eccentricity;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        destination = new Vector2(Random.Range(transform.position.x - 1, transform.position.x + 1), transform.position.y + 1);
    }

    // Update is called once per frame
    void Update()
    {
        FaceDirection();
        if(transform.position.y > 6.0f)
        {
            Destroy(gameObject);
        }
        else if(Vector2.Distance(transform.position, destination) < 0.0001f)
        {
            eccentricity = Random.Range(1, 3);
            destination = new Vector2(Random.Range(transform.position.x - eccentricity, transform.position.x + eccentricity), transform.position.y + 1);
        }

        transform.position = Vector2.MoveTowards(transform.position, destination, gameManager.GetEnemySpeed() * Time.deltaTime);
    }

    private void FaceDirection()
    {
        if(transform.position.x > destination.x)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
    }
}
