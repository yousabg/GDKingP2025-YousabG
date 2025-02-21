using TMPro;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Pins pinsDB;
    public static int selection = 0;
    public SpriteRenderer sprite;
    public TMP_Text nameLabel;

    void updateCharacter() {
        Pin current = pinsDB.getPin(selection);
        sprite.sprite = current.prefab.GetComponent<SpriteRenderer>().sprite;
        nameLabel.SetText(current.name);
    }
    public void next() {
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        audioSource.Play();
        int numberPins = pinsDB.getCount();
        if (selection < numberPins - 1) {
            selection = selection + 1;
        } else {
            selection = 0;
        }
        updateCharacter();
    }
    public void previous() {
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        audioSource.Play();
        if (selection > 0) {
            selection = selection - 1;
        } else {
            selection = pinsDB.getCount() - 1;
        }
        updateCharacter();
    }
    void Start()
    {
        updateCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
