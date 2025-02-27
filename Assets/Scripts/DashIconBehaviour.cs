using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashIconBehaviour : MonoBehaviour
{
    TextMeshProUGUI label;
    float cooldown;
    float cooldownRate;
    float dashCooldownRate;
    Image overlay;
    void Start()
    {
        label = GetComponentInChildren<TextMeshProUGUI>();
        cooldownRate = PinBehaviour.cooldownRate;
        dashCooldownRate = PinBehaviour.invincibilityCooldownRate;
        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].tag == "overlay")
            {
                overlay = images[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Dash")
        {
            cooldown = PinBehaviour.cooldown;
            string message = "";
            if (cooldown > 0.0)
            {
                float fill = cooldown / cooldownRate;
                message = string.Format("{0:0.0}", cooldown);
                overlay.fillAmount = fill;
            }
            label.SetText(message);
        }
        else if (gameObject.tag == "Invincibility")
        {
            cooldown = PinBehaviour.invincibilityCooldown;
            string message = "";
            if (cooldown > 0.0)
            {
                float fill = cooldown / dashCooldownRate;
                message = string.Format("{0:0.0}", cooldown);
                overlay.fillAmount = fill;
            }
            label.SetText(message);
        }
    }
}
