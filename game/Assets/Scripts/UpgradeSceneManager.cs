using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSceneManager : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance
    }

    // Update is called once per frame
    void Update()
    {
        gameManager.AddPoints(1);
    }

    
}
