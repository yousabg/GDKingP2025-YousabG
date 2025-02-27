using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using TMPro;
using System.Threading;

public class PinBehaviour : MonoBehaviour
{
    public float speed;
    public float start;
    public float baseSpeed = 15.0f;
    public float dashSpeed = 25.0f;
    public float dashDuration = 0.3f;
    public float invincibleDuration = 10.0f;

    public bool dashing;
    public bool invincibility;

    public static float cooldownRate = 1.0f;
    public static float invincibilityCooldownRate = 10.0f;

    public float endLastDash;
    public static float cooldown = 0.0f;
    public static float invincibilityCooldown = 0.0f;

    public Vector2 newPosition;
    public Vector3 mousePosG;
    Camera cam;

    Rigidbody2D body;
    public AudioSource[] audioSources;
    private SpriteRenderer spriteRenderer;
    public Color normalColor = Color.white;
    public Color invincibleColor = Color.yellow;
    public float invincibilityGlowIntensity = 2f;
    
    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();
        dashing = false;
        audioSources = GetComponents<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        invincibilityCooldown = 0.0f;
        cooldown = 0.0f;
    }

    private void Invincibility() {
        if (invincibility == true) {
            spriteRenderer.color = invincibleColor * invincibilityGlowIntensity;
            float currenttime = Time.time;
            float timeInvincible = currenttime - start;
            Debug.Log(invincibleDuration);
            if (timeInvincible > invincibleDuration) {
                invincibility = false;
                spriteRenderer.color = normalColor;
                invincibilityCooldown = invincibilityCooldownRate;
            } 
        } else {
            invincibilityCooldown = invincibilityCooldown - Time.deltaTime;
            if (invincibilityCooldown < 0.0) {
                invincibilityCooldown = 0.0f;
            }
            if (invincibilityCooldown == 0.0 && Input.GetMouseButtonDown(1)) {
                invincibility = true;
                //invincibility stuff here
                start = Time.time;
                if (audioSources[1].isPlaying) {
                    audioSources[1].Stop();
                }
                audioSources[1].Play();
            }
        }
    }
    private void Dash() {
        if (dashing == true) {
            float currenttime = Time.time;
            float timeDashing = currenttime - start;
            if (timeDashing > dashDuration) {
                dashing = false;
                speed = baseSpeed;
                cooldown = cooldownRate;
            } 
        } else {
            cooldown = cooldown - Time.deltaTime;
            if (cooldown < 0.0) {
                cooldown = 0.0f;
            }
            if (cooldown == 0.0 && Input.GetMouseButtonDown(0)) {
                dashing = true;
                speed = dashSpeed;
                start = Time.time;
                if (audioSources[1].isPlaying) {
                    audioSources[1].Stop();
                }
                audioSources[1].Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TextBehaviour.countdownFinished)
        {
            Dash();
            Invincibility();
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (TextBehaviour.countdownFinished) {
            mousePosG = cam.ScreenToWorldPoint(Input.mousePosition);
            newPosition = Vector2.MoveTowards(body.position, mousePosG, speed * Time.fixedDeltaTime);
            body.MovePosition(newPosition);
        } else {
            transform.position = Vector3.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (invincibility) {
            return;
        }
        string collided = collision.gameObject.tag;
        Debug.Log("Collided with" + collided);
        if (collided == "Ball") {
            BallBehavior ballBehavior = collision.gameObject.GetComponent<BallBehavior>();
            if (ballBehavior.canCollide)
            {
                StartCoroutine(WaitForSoundAndTransition("GameOver"));
            }
        } else if (collided == "Wall") {
            StartCoroutine(WaitForSoundAndTransition("GameOver"));
        }
    }

    private IEnumerator WaitForSoundAndTransition(string sceneName) {
        audioSources[0].Play();
        yield return new WaitForSeconds(audioSources[0].clip.length);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
