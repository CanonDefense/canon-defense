using UnityEngine;
using UnityEngine.UI;

public class UpgradeCanvasController : MonoBehaviour
{
    public Button rapidReloadButton;
    public Button explosiveShellsButton;
    public Button advancedAmmoButton;
    public Button empBurstButton;
    public Button backButton;
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start UpgradeCanvasController");

        gameManager = GameManager.instance;

        rapidReloadButton.onClick.AddListener(BuyRapidReload);
        explosiveShellsButton.onClick.AddListener(BuyExplosiveShells);
        advancedAmmoButton.onClick.AddListener(BuyAdvancedAmmo);
        empBurstButton.onClick.AddListener(BuyEMPBurst);
        backButton.onClick.AddListener(BackButton);
    }

    private void BuyRapidReload()
    {
        Debug.Log("BuyRapidReload");

        bool paid = gameManager.BuyUpgrade(GameManager.AvailableUpgrades.RAPID_RELOAD);
        if (paid) {
            rapidReloadButton.gameObject.SetActive(false);
        }
    }

    private void BuyExplosiveShells()
    {
        Debug.Log("BuyExplosiveShells");

        bool paid = gameManager.BuyUpgrade(GameManager.AvailableUpgrades.EXPLOSIVE_SHELLS);
        if (paid) {
            explosiveShellsButton.gameObject.SetActive(false);
        }
    }

    private void BuyAdvancedAmmo()
    {       
        Debug.Log("BuyAdvancedAmmo");

        bool paid = gameManager.BuyUpgrade(GameManager.AvailableUpgrades.ADVANCED_AMMO);
        if (paid) {
            advancedAmmoButton.gameObject.SetActive(false);
        }
    }

    private void BuyEMPBurst()
    {
        Debug.Log("BuyEMPBurst");

        bool paid = gameManager.BuyUpgrade(GameManager.AvailableUpgrades.EMP_BURST);
        if (paid) {
            empBurstButton.gameObject.SetActive(false);
        }
    }

    private void BackButton()
    {
        gameManager.ResumeGame();
    }
}
