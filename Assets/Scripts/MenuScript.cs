using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gotoGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    public void gotoMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void gotoCharacterSelection() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelectionMenu");
    }
}
