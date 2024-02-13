using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float bulletSpeed = 125f;
    public Transform firePoint;
    private readonly float rotationOffsetDegrees = 0f;
    public GameObject bulletPrefab;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 5f;
    private bool isReloading = false;
    public float shotReloadTime = 0.5f;
    private bool isShotReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;   
    }

    // Update is called once per frame
    void Update()
    {
        if(isReloading) {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            FireCanon();
        }

        MoveCanon();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading");

        yield return new WaitForSeconds(GameManager.instance.HasUpgrade(GameManager.AvailableUpgrades.RAPID_RELOAD) ? reloadTime / 2 : reloadTime);
        
        currentAmmo = GameManager.instance.HasUpgrade(GameManager.AvailableUpgrades.ADVANCED_AMMO) ? maxAmmo * 2 : maxAmmo;
        isReloading = false;
    }

    IEnumerator ShotReload()
    {
        isShotReloading = true;

        yield return new WaitForSeconds(GameManager.instance.HasUpgrade(GameManager.AvailableUpgrades.RAPID_RELOAD) ? shotReloadTime / 2 : shotReloadTime);
        
        isShotReloading = false;
    }

    private void MoveCanon()
    {
        if (GameManager.instance.IsGameOver()) {
            return;
        }

        if (GameManager.instance.IsGamePaused()) {
            return;
        }

        transform.rotation = GetRotationTowardsMouse(rotationOffsetDegrees);
    }

    private void FireCanon()
    {
        if (GameManager.instance.IsGameOver()) {
            return;
        }
        
        if (isShotReloading) {
            return;
        }

        StartCoroutine(ShotReload());
        
        Vector3 bulletDirection = GetRotationTowardsMouse() * Vector3.right;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletDirection * bulletSpeed, ForceMode2D.Impulse);

        Destroy(bullet, 10);

        currentAmmo--;

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
    }

    private Quaternion GetRotationTowardsMouse(float offset = 0f)
    {
        // Get mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the angle in radians and convert it to degrees
        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        // Rotate the object towards the mouse
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + offset);

        return rotation;
    }

    void OnGUI()
    {
        if (GameManager.instance.IsGameOver()) {
            return;
        }

        if (GameManager.instance.IsGamePaused()) {
            return;
        }

        // Set the GUIStyle for larger text
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.red;

        // Display Ammo
        float ammoLabelWidth = style.CalcSize(new GUIContent("Ammo: " + currentAmmo)).x;
        GUI.Label(new Rect(Screen.width - ammoLabelWidth - 10, 10, ammoLabelWidth, 30), "Ammo: " + currentAmmo, style);

        if (isReloading) {
            GUIStyle textStyle = new GUIStyle(GUI.skin.label);
            textStyle.fontSize = 40;
            textStyle.normal.textColor = Color.red;

            // Calculate the position to center the text
            float reloadingTextWidth = 300;  // Adjust the width of the text box as needed
            float reloadingTextHeight = 50;  // Adjust the height of the text box as needed
            float x = (Screen.width - reloadingTextWidth) / 2;
            float y = (Screen.height - reloadingTextHeight) / 2;

            GUI.Box(new Rect(x, y, reloadingTextWidth - 60, reloadingTextHeight), "");
            GUI.Label(new Rect(x, y, reloadingTextWidth, reloadingTextHeight), "RELOADING", textStyle);
        }
    }
}
