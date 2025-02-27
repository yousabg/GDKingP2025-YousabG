using System.Collections;
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
    public bool canCollide = false;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // secondsToMaxSpeed = 30;
        // minSpeed = 2.0f;
        // maxSpeed = 5.0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition();
        StartCoroutine(OpacityTimer());
    }

        IEnumerator OpacityTimer()
    {
        float timer = 0f;
        float fadeDuration = 5f;
        float opacity = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            opacity = Mathf.PingPong(timer, 1f); 
            Color color = spriteRenderer.color;
            color.a = opacity;
            spriteRenderer.color = color;

            yield return null;
        }

        canCollide = true;
    }

    public void initialPosition() {
        body = GetComponent<Rigidbody2D>();
        //body.position = getRandomBallPosition();
        targetPosition = getRandomPosition();
        launching = false;
        rerouting = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {
        if (TextBehaviour.countdownFinished) {
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
    }

    private void OnCollisionEnter2D(Collision2D collision) {
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

    Vector2 getRandomBallPosition() {
        float randomX, randomY;
        Vector2 randomPosition;
        Rigidbody2D targetBody = target.GetComponent<Rigidbody2D>();

        Vector2 targetPosition = targetBody.position;

        bool targetOnLeft = targetPosition.x < (minX + maxX) / 2;

        float minDistanceFromZero = 3.0f;

        if (targetOnLeft) {
            randomX = Random.Range((minX + maxX) / 2, maxX);
        } else {
            randomX = Random.Range(minX, (minX + maxX) / 2);
        }
        randomY = Random.Range(minY, maxY);
        randomPosition = new Vector2(randomX, randomY);

        do {
            targetOnLeft = targetPosition.x < (minX + maxX) / 2;

            if (targetOnLeft) {
                randomX = Random.Range((minX + maxX) / 2, maxX);
            } else {
                randomX = Random.Range(minX, (minX + maxX) / 2);
            }

            randomY = Random.Range(minY, maxY);
            randomPosition = new Vector2(randomX, randomY);

        } while (Mathf.Abs(randomX) < minDistanceFromZero && Mathf.Abs(randomY) < minDistanceFromZero);


        return randomPosition;
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


    public void setBounds(float miX, float maX, float miY, float maY) {
        minX = miX;
        maxX = maX;
        minY = miY;
        maxY = maY;
    }

    public void setTarget(GameObject pin) {
        target = pin;
    }
}
