using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    private float str;

    private void Start()
    {
        // Instantiate the ball prefab
        ballInstance = Instantiate(ballPrefab, new Vector3(0, 1, 0), Quaternion.identity);

        // Create a new player
        player1 = new Player();
        player1.name = "Player 1";
        player1.assignTurn(true);
        player2 = new Player();
        player2.name = "Player 2";
        player2.assignTurn(false);

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Print the balls in the player's list
            Debug.Log(player1.name + "'s balls:");
            for (int i = 0; i < player1.balls.Count; i++)
            {
                Debug.Log("Ball " + i + ": " + player1.balls[i]);
            }
            Debug.Log(player2.name + "'s balls:");
            for (int i = 0; i < player2.balls.Count; i++)
            {
                Debug.Log("Ball " + i + ": " + player2.balls[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            str = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            str = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            str = 1;
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
                ballRigidbody.AddForce(cameraForward * force * str, ForceMode.Impulse);
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

    public void removeBallFromPlayerList(bool isstriped, GameObject ball)
    {
        // Remove the ball from the player's list and destroy it
        if (isstriped == true)
        {
            player1.removeBall(ball);
        }
        else
        {
            player2.removeBall(ball);
        }
        
    }

    public bool getPlayer1IsTurn()
    {
        if (player1.isTurn == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool getPlayer2IsTurn()
    {
           if (player2.isTurn == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class Player
{
    public List<GameObject> balls;
    public string name;
    public bool isTurn;

    public void assignBallList(List<GameObject> ballList)
    {
        // Asssign the incomming list's elements to the player's list
        balls = ballList;
    }
    public void assignTurn(bool turn)
    {
        isTurn = turn;
    }

    public void removeBall(GameObject ball)
    {
        balls.Remove(ball);
        Debug.Log("Ball " + ball + " has been removed from " + name + "'s list of balls");
    }
}
