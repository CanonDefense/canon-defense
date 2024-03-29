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
    public GameObject bulletPrefab;

    // Spawns config
    public Transform enemySpawn;
    private float spawnInterval = 3f;

    // Canvas
    public Canvas inGameMenu;
    public Canvas upgradeMenu;

    // Background.
    public SpriteRenderer spriteRenderer;
    public Sprite[] backgrounds;

    // Stats
    private int points = 0;
    private int wave = 1;
    private int spawnedInRound = 0;
    private bool gameOver = false;

    // Sounds
    public AudioClip bulletExplosionSound;
    public AudioClip tankExplosionSound;
    public AudioClip soldierDyingSound;
    public AudioClip canonFireSound;

    // Upgrades
    [Serializable]
    public enum AvailableUpgrades {RAPID_RELOAD, EXPLOSIVE_SHELLS, ADVANCED_AMMO, EMP_BURST}
    private enum UpgradesCost {
        RAPID_RELOAD = 10,
        EXPLOSIVE_SHELLS = 30,
        ADVANCED_AMMO = 20,
        EMP_BURST = 40
    }

    private Dictionary<AvailableUpgrades, int> upgradesPrice = new() {
        {AvailableUpgrades.RAPID_RELOAD, 10},
        {AvailableUpgrades.EXPLOSIVE_SHELLS, 30},
        {AvailableUpgrades.ADVANCED_AMMO, 20},
        {AvailableUpgrades.EMP_BURST, 40},
    };

    private Dictionary<AvailableUpgrades, bool> upgrades = new() {
        {AvailableUpgrades.RAPID_RELOAD, false},
        {AvailableUpgrades.EXPLOSIVE_SHELLS, false},
        {AvailableUpgrades.ADVANCED_AMMO, false},
        {AvailableUpgrades.EMP_BURST, false},
    };

    private AudioSource audio;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();    
        spriteRenderer.sprite = backgrounds[UnityEngine.Random.Range(0, backgrounds.Length)];
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
        spawnedInRound++;
        GameObject prefab = soldierPrefab;

        if (UnityEngine.Random.value <= 0.25f) {
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

    public void CloseGame()
    {
        Application.Quit();
    }

    public bool BuyUpgrade(AvailableUpgrades upgrade)
    {
        // Control already upgraded
        if (upgrades[upgrade] == true) {
            return false;
        }

        //Check if user has enought points
        int upgradePrice = upgradesPrice[upgrade];
        if (points >= upgradePrice) {
            points -= upgradePrice;
            upgrades[upgrade] = true;
            return true;
        }

        return false;
    }

    public bool HasUpgrade(AvailableUpgrades upgrade)
    {
        return upgrades[upgrade];
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public bool IsGamePaused()
    {
        return Time.timeScale == 0;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void ResumeGame()
    {
        inGameMenu.gameObject.SetActive(true);
        upgradeMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnGUI()
    {
        if (gameOver) {
            GUIStyle textStyle = new GUIStyle(GUI.skin.label);
            textStyle.fontSize = 60;
            textStyle.normal.textColor = Color.red;

            // Calculate the position to center the text
            float reloadingTextWidth = 400;  // Adjust the width of the text box as needed
            float reloadingTextHeight = 70;  // Adjust the height of the text box as needed
            float x = (Screen.width - reloadingTextWidth) / 2;
            float y = (Screen.height - reloadingTextHeight) / 2;

            GUI.Box(new Rect(x, y, reloadingTextWidth - 30, reloadingTextHeight), "");
            GUI.Label(new Rect(x, y, reloadingTextWidth, reloadingTextHeight), "GAME OVER", textStyle);
            return;
        }
        if (IsGamePaused()) {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.red;

        // Display Points
        GUI.Label(new Rect(10, 10, 200, 30), "Points: " + points, style);

        // Display Wave
        GUI.Label(new Rect(10, 50, 200, 30), "Wave: " + wave, style);
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        while (!gameOver)
        {
            float random = Mathf.Clamp(UnityEngine.Random.value, 0f, 0.5f);

            // Increase difficulty based on wave
            float difficulty = 0.25f * wave;

            // Wait seconds before new spawn
            float seconds = (spawnInterval - difficulty) - random;
            yield return new WaitForSeconds(Mathf.Clamp(seconds, 1f, spawnInterval));

            // Spawn the prefab
            SpawnEnemy();

            if (spawnedInRound >= 10) {
                wave++;
                spawnedInRound = 0;
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;
        audio.mute = true;
        StartCoroutine(SpawnGameOver());
    }

    IEnumerator SpawnGameOver()
    {
        while(true) {
            for(float i = -10; i < 10; i++) {
                GameObject bullet = Instantiate(bulletPrefab, new Vector3(i, 4.5f, 0f), Quaternion.identity);
                yield return new WaitForSeconds(0.05f);
            }

            for(float i = 10; i > -10; i--) {
                GameObject bullet = Instantiate(bulletPrefab, new Vector3(i, 4.5f, 0f), Quaternion.identity);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public void PlayBulletExplosionSound()
    {
        audio.volume = 0.25f;
        audio.PlayOneShot(bulletExplosionSound);
    }

    public void PlayCanonFireSound()
    {
        audio.volume = 0.2f;
        audio.PlayOneShot(canonFireSound);
    }

    public void PlaySolderDyingSound()
    {
        audio.volume = 1f;
        audio.PlayOneShot(soldierDyingSound);
    }

    public void PlayTankExplosionSound()
    {
        audio.volume = 0.5f;
        audio.PlayOneShot(tankExplosionSound);
    }
}
