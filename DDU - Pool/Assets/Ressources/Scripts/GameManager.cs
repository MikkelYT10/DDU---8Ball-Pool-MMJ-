using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject pawnBallPrefab;
    public float force;
    public float minVelocityToStop;
    public float minAngularVelocityToStop;

    private Player player1;
    private Player player2;

    public List<GameObject> fullBalls;
    public List<GameObject> stripedBalls;

    private GameObject ballInstance;

    private void Start()
    {
        // Instantiate the ball prefab
        ballInstance = Instantiate(ballPrefab, new Vector3(0, 1, 0), Quaternion.identity);

        // Create a new player
        player1 = new Player();
        player1.name = "Player 1";
        player2 = new Player();
        player2.name = "Player 2";

        // Assign the player a list of balls
        player1.assignBallList(fullBalls);
        player2.assignBallList(stripedBalls);
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
                // Other than the velocity, check if the ball iss in contact with the ground, as it falls very slowly
                if (ballRigidbody.velocity.magnitude < minVelocityToStop)
                {
                    ballRigidbody.velocity = Vector3.zero;
                    ballRigidbody.angularVelocity = Vector3.zero;
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

    public void quit()
    {
        Application.Quit();
        Debug.Log("Spillet er afsluttet");

    }

    public  void pocketedque()
    {
        ballInstance.transform.position = new Vector3(0, 1, 0);
        Debug.Log("Que has been pocketed");
    }
}

public class Player
{
    public List<GameObject> balls;
    public string name;

    public void assignBallList(List<GameObject> ballList)
    {
        // Asssign the incomming list's elements to the player's list
        balls = ballList;

        Debug.Log(name + " has been assigned the following balls: " + balls);
        for(int i = 0; i < balls.Count; i++)
        {
            Debug.Log("Ball " + i + ": " + balls[i]);
        }
    }

}
