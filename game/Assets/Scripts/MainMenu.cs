using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayClicked()
    {
         SceneManager.LoadScene("Level1");
    }

    public void OnOptionsClicked()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
