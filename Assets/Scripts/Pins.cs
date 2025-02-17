using UnityEngine;

[CreateAssetMenu(fileName = "Pins", menuName = "Scriptable Objects/Pins", order = 1)]
public class Pins : ScriptableObject
{
    public Pin[] pins;
    public int getCount() {
        return pins.Length;
    }
    public Pin getPin(int index) {
        return pins[index];
    }
}
