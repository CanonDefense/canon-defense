using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float bulletSpeed = 10f;
    public Transform firePoint;
    private readonly float rotationOffsetDegrees = -90f;
    public GameObject bulletPrefab;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 10f;
    private bool isReloading = false;
    public float shotReloadTime = 2f;
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

        yield return new WaitForSeconds(reloadTime);
        
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    IEnumerator ShotReload()
    {
        isShotReloading = true;

        yield return new WaitForSeconds(shotReloadTime);
        
        isShotReloading = false;
    }
    private void MoveCanon()
    {
        transform.rotation = GetRotationTowardsMouse(rotationOffsetDegrees);
    }

    private void FireCanon()
    {
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
        // Set the GUIStyle for larger text
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;

        // Display Ammo
        float ammoLabelWidth = style.CalcSize(new GUIContent("Ammo: " + currentAmmo)).x;
        GUI.Label(new Rect(Screen.width - ammoLabelWidth - 10, 10, ammoLabelWidth, 30), "Ammo: " + currentAmmo, style);

        if (isReloading) {
            GUIStyle blinkingStyle = new GUIStyle(GUI.skin.label);
            blinkingStyle.fontSize = 40;
            blinkingStyle.normal.textColor = Color.red;

            // Calculate the position to center the text
            float reloadingTextWidth = 200;  // Adjust the width of the text box as needed
            float reloadingTextHeight = 50;  // Adjust the height of the text box as needed
            float x = (Screen.width - reloadingTextWidth) / 2;
            float y = (Screen.height - reloadingTextHeight) / 2;

            GUI.Label(new Rect(x, y, reloadingTextWidth, reloadingTextHeight), "RELOADING", blinkingStyle);
        }
    }
}
