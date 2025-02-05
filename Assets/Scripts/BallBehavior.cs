using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float minX = -9.45f;
    public float maxX = 9.41f;
    public float minY = -3.52f;
    public float maxY = 3.5f;
    public float minSpeed;
    public float maxSpeed;
    public Vector2 targetPosition;
    public int secondsToMaxSpeed;

    public GameObject target;
    public float minLaunchSpeed;
    public float maxLaunchSpeed;
    public float minTimeToLaunch;
    public float maxtimeToLaunch;
    public float cooldown;
    public bool launching;
    public float launchDuration;
    public float timeLastLaunch;
    public float timeLaunchStart;

    Rigidbody2D body;
    public bool rerouting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // secondsToMaxSpeed = 30;
        // minSpeed = 2.0f;
        // maxSpeed = 5.0f;
        initialPosition();

    }

    public void initialPosition() {
        body = GetComponent<Rigidbody2D>();
        body.position = getRandomPosition();
        targetPosition = getRandomPosition();
        launching = false;
        rerouting = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {
        body = GetComponent<Rigidbody2D>();
        if (onCooldown() == false) {
            launch();
        }
        Vector2 currentPos = body.position;
        float distance = Vector2.Distance(currentPos, targetPosition);
        if (distance > 0.1) {
            float difficulty = getDifficultyPercentage();
            float currentSpeed;
            if (launching == true) {
                float launchingForHowLong = Time.time - timeLaunchStart;
                if (launchingForHowLong > launchDuration) {
                    startCooldown();
                }
                currentSpeed = Mathf.Lerp(minLaunchSpeed, maxLaunchSpeed, difficulty);
            } else {
                currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, difficulty);
            }

            currentSpeed = currentSpeed * Time.deltaTime;
            Vector2 newPosition = Vector2.MoveTowards(currentPos, targetPosition, currentSpeed);
            body.MovePosition(newPosition);
        } else {
            if (launching == true) {
                startCooldown();
            }
            targetPosition = getRandomPosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(this + " Collided with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Wall") {
            targetPosition = getRandomPosition();
        }
        if (collision.gameObject.tag == "Ball") {
            Reroute(collision);
        }
    }

    public void Reroute(Collision2D collision) {
        GameObject otherBall = collision.gameObject;
        if (rerouting == true) {
            otherBall.GetComponent<BallBehavior>().rerouting = false;
            Rigidbody2D ballBody = otherBall.GetComponent<Rigidbody2D>();
            Vector2 contact = collision.GetContact(0).normal;
            targetPosition = Vector2.Reflect(targetPosition, contact).normalized;
            launching = false;
            float separationDistance = 0.1f;
            ballBody.position += contact * separationDistance;
        } else {
            rerouting = true;
        }
    }

    Vector2 getRandomPosition() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector2 v = new Vector2(randomX, randomY);

        return v;
    }

    private float getDifficultyPercentage(){
        float difficulty = Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxSpeed );
        return difficulty;
    }

    public void launch() {
        Rigidbody2D targetBody = target.GetComponent<Rigidbody2D>();
        targetPosition = targetBody.position;

        if (launching == false) {
            timeLaunchStart = Time.time;
            launching = true;
        }
    }

    public bool onCooldown() {
        bool result = false;

        float timeSinceLastLaunch = Time.time - timeLastLaunch;

        if (timeSinceLastLaunch < cooldown) {
            result = true;
        }

        return result;
    }

    public void startCooldown() {
        timeLastLaunch = Time.time;
        launching = false;
    }
}
