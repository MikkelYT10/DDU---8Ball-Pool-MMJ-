using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject pawnBallPrefab;
    private GameObject ballInstance;

    private Player player1;
    private Player player2;

    public List<GameObject> fullBalls;
    public List<GameObject> stripedBalls;

    public float force;
    public float minVelocityToStop;
    public float minAngularVelocityToStop;
    private float str;

    private bool switchTurnsCalled = false;
    public bool assignedBalls = false;

    private void Start()
    {
        // Instantiate the ball prefab
        ballInstance = Instantiate(ballPrefab, new Vector3(-165, -9, -20), Quaternion.identity);

        // Create a new player
        player1 = new Player();
        player1.name = "Player 1";
        player1.assignTurn(true);
        player2 = new Player();
        player2.name = "Player 2";
        player2.assignTurn(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Push the ball forward
            PushBall();
        }

    //Stop the ball if it's too slow
    GameObject ball = GameObject.FindGameObjectWithTag("Player");
    if (ball != null)
    {
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        if (ballRigidbody != null)
        {
                // Other than the velocity, check if the ball is in contact with the ground, as it falls very slowly
            if (ballRigidbody.velocity.magnitude < minVelocityToStop)
                {
                    stopMoving(ballRigidbody);
                // Call switchturns function only once when the ball is standing still
                if (!switchTurnsCalled)
                {
                    switchturns();
                    switchTurnsCalled = true;
                }
            }
            else
            {
                // Reset the flag when the ball is not standing still
                switchTurnsCalled = false;
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

    public void assigningBalls(int player, string ballType)
    {
        if (player == 1)
        {
            if (ballType == "Full")
            {
                player1.assignBallList(fullBalls);
                player1.pBallType = "Full";
            }
            else if (ballType == "Striped")
            {
                player1.assignBallList(stripedBalls);
                player1.pBallType = "Striped";
            }
        }
        else if (player == 2)
        {
            if (ballType == "Full")
            {
                player2.assignBallList(fullBalls);
                player2.pBallType = "Full";
            }
            else if (ballType == "Striped")
            {
                player2.assignBallList(stripedBalls);
                player2.pBallType = "Striped";
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

    public void stopMoving(Rigidbody ballRigidbody)
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
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
        Debug.Log("Removing ball " + ball + " from player list" + "Ballstipe: " + isstriped);
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

    public bool switchturns()
    {
        if (player1.isTurn == true)
        {
            player1.assignTurn(false);
            player2.assignTurn(true);
            return true;
        }
        else
        {
            player1.assignTurn(true);
            player2.assignTurn(false);
            return false;
        }
    }

    public void EightBallPocketed()
    {
        // Check for each player
        if (player1.isTurn == true)
        {
            // Check if the player has pocketed all of their balls
            if (player1.balls.Count == 0)
            {
                // Player 1 wins
                Debug.Log("Player 1 wins!");
            }
            else
            {
                // Player 1 loses
                Debug.Log("Player 1 loses!");
            }
        }
        else
        {
            // Check if the player has pocketed all of their balls
            if (player2.balls.Count == 0)
            {
                // Player 2 wins
                Debug.Log("Player 2 wins!");
            }
            else
            {
                // Player 2 loses
                Debug.Log("Player 2 loses!");
            }
        }
    }
}

public class Player
{
    public List<GameObject> balls;
    public string pBallType;
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
        Debug.Log(name + "'S TURN IS NIOW " + isTurn);
    }

    public void removeBall(GameObject ball)
    {
        balls.Remove(ball);
        Debug.Log("Ball " + ball + " has been removed from " + name + "'s list of balls");
    }
}
