using System.Collections;
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
        TextBehaviour.countdownFinished = false;
        StartCoroutine(WaitForSoundAndTransition("MainGame"));
    }

    private IEnumerator WaitForSoundAndTransition(string sceneName) {
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void gotoMenu() {
        StartCoroutine(WaitForSoundAndTransition("MainMenu"));
    }

    public void gotoCharacterSelection() {
        StartCoroutine(WaitForSoundAndTransition("CharacterSelectionMenu"));
    }
}
