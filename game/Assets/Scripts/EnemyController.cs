using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    public int rewardPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Hit();
        }
    }

    private void Hit()
    {
        health--;

        if (health <= 0) {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameManager.instance.AddPoints(rewardPoints);
    }
}
