using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    private Vector2 mDestination;
    private bool reachedDestination = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(mDestination, transform.position) < 0.0001f && reachedDestination)
        {
            Destroy(gameObject);
        }
        else if(Vector2.Distance(mDestination, transform.position) < 0.0001f)
        {
            reachedDestination = true;
            mDestination = new Vector2(transform.position.x, 8.0f);
        }

         transform.Translate((mDestination - new Vector2(transform.position.x, transform.position.y)) * gameManager.GetEnemySpeed() * Time.deltaTime);

    }

    public void SetDesination(Vector2 destination)
    {
        mDestination = destination;
    }
}
