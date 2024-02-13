using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    public int rewardPoints;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isKilled = false;
    private bool stunned = false;

    public bool isTank = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKilled && !stunned) {
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

        if (GameManager.instance.HasUpgrade(GameManager.AvailableUpgrades.EMP_BURST)) {
            stunned = true;
            Invoke("RecoverFromEmp", 2.5f);
        }

        if (health <= 0) {
            Die();
        }
    }

    private void RecoverFromEmp()
    {
        stunned = false;
    }

    private void Die()
    {
        isKilled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("die", true);
        GameManager.instance.AddPoints(rewardPoints);

        // Play sound
        if (isTank) {
            GameManager.instance.PlayTankExplosionSound();
        } else {
            GameManager.instance.PlaySolderDyingSound();
        }
    }

    private void OnDieAnimationFinished()
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible() {
        if (gameObject.transform.position.x < -10) {
            Destroy(gameObject);
        }
    }
}
