using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBaseController : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy") {
            GameManager.instance.GameOver();
        }
    }
}
