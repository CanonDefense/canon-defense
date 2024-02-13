using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    public int rewardPoints;
    private Animator animator;
    private bool isKilled = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKilled) {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
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
        isKilled = true;
        animator.SetBool("die", true);
        GameManager.instance.AddPoints(rewardPoints);
    }

    private void OnDieAnimationFinished()
    {
        Destroy(gameObject);
    }
}
