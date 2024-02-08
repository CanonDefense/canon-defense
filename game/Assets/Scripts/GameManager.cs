using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Prefabs
    public GameObject soldierPrefab;
    public GameObject tankPrefab;

    // Spawns config
    public Transform enemySpawn;
    private float spawnInterval = 3f;

    // Canvas
    public Canvas inGameMenu;
    public Canvas upgradeMenu;

    // Stats
    private int points = 0;
    private int wave = 1;

    // Upgrades

    [Serializable]
    public enum AvailableUpgrades {RAPID_RELOAD, EXPLOSIVE_SHELLS, ADVANCED_AMMO, EMP_BURST}
    private enum UpgradesCost {
        RAPID_RELOAD = 10,
        EXPLOSIVE_SHELLS = 30,
        ADVANCED_AMMO = 20,
        EMP_BURST = 40
    }
    private Dictionary<AvailableUpgrades, bool> upgrades = new() {
        {AvailableUpgrades.RAPID_RELOAD, false},
        {AvailableUpgrades.EXPLOSIVE_SHELLS, false},
        {AvailableUpgrades.ADVANCED_AMMO, false},
        {AvailableUpgrades.EMP_BURST, false},
    };

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }

    void SpawnEnemy()
    {
        GameObject prefab = soldierPrefab;

        if (UnityEngine.Random.value <= 0.15f) {
            prefab = tankPrefab;
        }

        // Instantiate the prefab at a specific position (you can modify this as needed)
        Instantiate(prefab, enemySpawn.position, Quaternion.identity);
    }

    public void OpenUpgradeMenu()
    {
        PauseGame();
        inGameMenu.gameObject.SetActive(false);
        upgradeMenu.gameObject.SetActive(true);
    }

    public bool BuyUpgrade(AvailableUpgrades upgrade)
    {
        // Control already upgraded
        if (upgrades[upgrade] == true) {
            return false;
        }

        //TODO Check if user has enought points
        if (true) {
            upgrades[upgrade] = true;
            return true;
        }

        return false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public bool IsGamePaused()
    {
        return Time.timeScale == 0;
    }

    public void ResumeGame()
    {
        inGameMenu.gameObject.SetActive(true);
        upgradeMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnGUI()
    {
        if (IsGamePaused()) {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;

        // Display Points
        GUI.Label(new Rect(10, 10, 200, 30), "Points: " + points, style);

        // Display Wave
        GUI.Label(new Rect(10, 50, 200, 30), "Wave: " + wave, style);
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            // Wait for 3 seconds
            yield return new WaitForSeconds(spawnInterval);

            // Spawn the prefab
            SpawnEnemy();
        }
    }
}
