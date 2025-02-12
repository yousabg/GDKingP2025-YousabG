using UnityEngine;
using UnityEngine.XR;

public class PinBehaviour : MonoBehaviour
{
    public float speed;
    public float start;
    public float baseSpeed = 15.0f;
    public float dashSpeed = 25.0f;
    public float dashDuration = 0.3f;
    public bool dashing;

    public static float cooldownRate = 1.0f;
    public float endLastDash;
    public static float cooldown = 0.0f;

    public Vector2 newPosition;
    public Vector3 mousePosG;
    Camera cam;

    Rigidbody2D body;

    void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody2D>();
        dashing = false;
        Vector2 start = new Vector2(0, 0);
        body.MovePosition(start);
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
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
    }

    void FixedUpdate()
    {
        mousePosG = cam.ScreenToWorldPoint(Input.mousePosition);
        newPosition = Vector2.MoveTowards(body.position, mousePosG, speed * Time.fixedDeltaTime);
        body.MovePosition(newPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        string collided = collision.gameObject.tag;
        Debug.Log("Collided with " + collided);
        if (collided == "Ball" || collided == "Wall") {
            Debug.Log("Game Over");
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }
}
