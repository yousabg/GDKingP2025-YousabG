    using UnityEngine;

public class SpawningBehavior : MonoBehaviour
{
    public GameObject[] ballVariants;
    public GameObject targetObject;
    GameObject newObject;
    public float startTime;
    public float spawnRatio = 1.0f;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public Pins pinsDB;
    public GameObject originalBall; 
    void Start()
    {
        spawnPin();
        BallBehavior orig = originalBall.GetComponent<BallBehavior>();
        orig.setTarget(targetObject);
        spawnBall();
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        float timeElapsed = currentTime - startTime;
        if (timeElapsed > spawnRatio) {
            spawnBall();
        }
    }

    void spawnBall()
    {
        int numVariants = ballVariants.Length;
        if (numVariants > 0) {
            int selection = Random.Range(0, numVariants);
            Vector2 spawnPosition = getRandomBallPosition();
            newObject = Instantiate(ballVariants[selection], new Vector3(spawnPosition.x, spawnPosition.y, 0.0f), Quaternion.identity);
            BallBehavior ballBehavior = newObject.GetComponent<BallBehavior>();
            ballBehavior.setBounds(minX, maxX, minY, maxY);
            ballBehavior.setTarget(targetObject);
            ballBehavior.initialPosition();
        }
        startTime = Time.time;
    }

        Vector2 getRandomBallPosition() {
        float randomX, randomY;
        Vector2 randomPosition;
        Rigidbody2D targetBody = targetObject.GetComponent<Rigidbody2D>();

        Vector2 targetPosition = targetBody.position;

        bool targetOnLeft = targetPosition.x < (minX + maxX) / 2;

        float minDistanceFromCenter = 3.0f;

        if (targetOnLeft) {
        randomX = Random.Range((minX + maxX) / 2 + minDistanceFromCenter, maxX); // Spawn on the right
        } else {
            randomX = Random.Range(minX, (minX + maxX) / 2 - minDistanceFromCenter); // Spawn on the left
        }

        randomY = Random.Range(minY, maxY);
        randomPosition = new Vector2(randomX, randomY);

        while (Mathf.Abs(randomX) < minDistanceFromCenter && Mathf.Abs(randomY) < minDistanceFromCenter) {
            if (targetOnLeft) {
                randomX = Random.Range((minX + maxX) / 2 + minDistanceFromCenter, maxX); // Spawn on the right
            } else {
                randomX = Random.Range(minX, (minX + maxX) / 2 - minDistanceFromCenter); // Spawn on the left
            }
            randomY = Random.Range(minY, maxY);
            randomPosition = new Vector2(randomX, randomY);
        }

        Debug.Log($"Ball Spawn Position: {randomPosition}");

        return randomPosition;
    }

    void spawnPin()
    {
        targetObject = Instantiate(pinsDB.getPin(CharacterManager.selection).prefab,
            new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        PinBehaviour pinBehaviour = targetObject.GetComponent<PinBehaviour>();
    }
}
