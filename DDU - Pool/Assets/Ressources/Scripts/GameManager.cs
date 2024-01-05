using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject pawnBallPrefab;
    public float force;
    public float minVelocityToStop;
    public float minAngularVelocityToStop;

    private void Start()
    {
        // Instantiate the ball prefab
        GameObject ballInstance = Instantiate(ballPrefab, new Vector3(0, 1, 0), Quaternion.identity);

        // Make the ball black
        ballInstance.GetComponent<Renderer>().material.color = Color.black;

        // Spawn another ball
        Instantiate(pawnBallPrefab, new Vector3(1, 1, 2), Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Push the ball forward
            PushBall();
        }

        //Stop the ball if its too slow
        GameObject ball = GameObject.FindGameObjectWithTag("Player");
        if (ball != null)
        {
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                if (ballRigidbody.velocity.magnitude < minVelocityToStop)
                {
                    ballRigidbody.velocity = Vector3.zero;
                }
                if (ballRigidbody.angularVelocity.magnitude < minVelocityToStop)
                {
                    ballRigidbody.angularVelocity = Vector3.zero;
                    print("velociy = 0");
                }
            }
        }

    }

    private void PushBall()
    {
        // Find the ball with the "Player" tag
        GameObject ball = GameObject.FindGameObjectWithTag("Player");

        if (ball != null)
        {
            // Ensure the ball has a Rigidbody component
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                // Get the camera's forward direction
                Vector3 cameraForward = Camera.main.transform.forward;

                // Ignore the vertical component to avoid pushing the ball up
                cameraForward.y = 0f;

                // Normalize the direction to maintain consistent force magnitude
                cameraForward.Normalize();

                // Add force to the ball in the camera's forward direction
                ballRigidbody.AddForce(cameraForward * force, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("The ball is missing a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("No object with the 'Player' tag found.");
        }
    }
}
