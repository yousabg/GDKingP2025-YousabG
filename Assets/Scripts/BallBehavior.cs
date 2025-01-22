using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float minX = -9.75f;
    public float maxX = 9.86f;
    public float minY = -4.43f;
    public float maxY = 3.51f;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // secondsToMaxSpeed = 30;
        // minSpeed = 2.0f;
        // maxSpeed = 5.0f;
        targetPosition = getRandomPosition();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPos = gameObject.GetComponent<Transform>().position;
        float distance = Vector2.Distance(currentPos, targetPosition);
        if (distance > 0.1) {
            float difficulty = getDifficultyPercentage();
            float currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, difficulty);
            currentSpeed = currentSpeed * Time.deltaTime;
            Vector2 newPosition = Vector2.MoveTowards(currentPos, targetPosition, currentSpeed);
            transform.position = newPosition;
        } else {
            targetPosition = getRandomPosition();
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
        
    }
}
