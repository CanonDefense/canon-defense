using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D collider;

    private bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (exploded) {
            return;
        }

        bool hasBigExplosion = GameManager.instance.HasUpgrade(GameManager.AvailableUpgrades.EXPLOSIVE_SHELLS);

        exploded = true;

        // Freeze position to avoid impact movement
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        // Prevent explosion collission
        if (!hasBigExplosion) {
            collider.enabled = false;
        }

        // Increate explosion scale
        gameObject.transform.localScale = hasBigExplosion ? new Vector3(10f, 10f, 10f) : new Vector3(5f, 5f, 5f);

        // Start explosion animation
        animator.enabled = true;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
