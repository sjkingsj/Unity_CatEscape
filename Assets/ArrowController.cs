using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        // Drop to constant velocity per frame
        transform.Translate(0, -0.1f, 0);

        // Destroy object when locate at outside of screen
        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }

        // Collision Decision
        Vector2 p1 = transform.position; // Center coordinate of Arrow
        Vector2 p2 = this.player.transform.position; // Center coordinate of Player
        Vector2 dir = p1 - p2;
        float d = dir.magnitude;
        float r1 = 0.5f; // Radius of Arrow
        float r2 = 1.0f; // Radius of Player

        if (d < r1 + r2)
        {
            // Transmit to Director Script that player and arrow collided
            GameObject director = GameObject.Find("GameDirector");
            director.GetComponent<GameDirector>().DecreaseHp();

            // Remove the Arrow in the event of Collision
            Destroy(gameObject);
        }
    }
}
