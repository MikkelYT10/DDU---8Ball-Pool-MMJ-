using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Delcaring GameObjects used         
    public GameObject ballPrefab;
    public GameObject pawnBallPrefab;
    private GameObject ballInstance;

    // Declaring the players
    private Player player1;
    private Player player2;

    // Declaring the lists of balls
    public List<GameObject> fullBalls;
    public List<GameObject> stripedBalls;

    // Declaring the UI elements
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI type;

    // Declaring the variables used for the balls movement
    public float force;
    public float minVelocityToStop;
    public float minAngularVelocityToStop;
    private float str;

    // Declaring the variables used for the turns
    private bool switchTurnsCalled = false;

    // Declaring the variable used for the ball assignment
    public bool assignedBalls = false;

    private void Start()
    {
        // Instantiate the ball prefab
        ballInstance = Instantiate(ballPrefab, new Vector3(-165, -9, -20), Quaternion.identity);

        // Create two new players
        player1 = new Player();
        player1.name = "Player 1";
        player1.assignTurn(true);

        player2 = new Player();
        player2.name = "Player 2";
        player2.assignTurn(false);
    }

    private void Update()   // Function that's called every frame
    {
        if (Input.GetKeyDown(KeyCode.Space))    // If the space key is pressed
        {
            // Push the ball forward
            PushBall();
        }

    //Stop the ball if it's too slow
    GameObject ball = GameObject.FindGameObjectWithTag("Player");   // Find the ball with the "Player" tag
    if (ball != null)   // Check if the ball exists
    {
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();   // Ensure the ball has a Rigidbody component
        if (ballRigidbody != null)  // Check if the ball has a Rigidbody component
        {
               
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
        Debug.Log("Game has been quitted.");

    }

    public  void pocketedque()
    {
        ballInstance.transform.position = new Vector3(0, 1, 0);
        Debug.Log("Que has been pocketed");
    }

    public void removeBallFromPlayerList(GameObject ball)
    {
        // if the ball is striped
        if (ball.tag == "Striped")
        {
            // If it's player 1's turn
            if (player1.isTurn == true)
            {
                // If player 1's ball type is striped
                if (player1.pBallType == "Striped")
                {
                    // Remove the ball from player 1's list
                    player1.removeBall(ball);
                }
                else
                {
                    // Remove the ball from player 2's list
                    player2.removeBall(ball);
                }
            }
            // Else if its player 2's turn
            else if (player2.isTurn == true)
            {
                // If player 2's ball type is striped
                if (player2.pBallType == "Striped")
                {
                    // Remove the bal from player 2's list.   
                    player2.removeBall(ball);
                }
                else
                {
                    // Remove the ball from player 1's list
                    player1.removeBall(ball);
                }
            }
        }
        // Else if the ball is full
        else if (ball.tag == "Full")
        {
            // If it's player 1's turn
            if (player1.isTurn == true)
            {
                // If player 1's ball type is full
                if (player1.pBallType == "Full")
                {
                    // Remove the ball from player 1's list
                    player1.removeBall(ball);
                }
                else
                {
                    // Remove the ball from player 2's list
                    player2.removeBall(ball);
                }
            }
            // Else if its player 2's turn
            else if (player2.isTurn == true)
            {
                // If player 2's ball type is full
                if (player2.pBallType == "Full")
                {
                    // Remove the bal from player 2's list.   
                    player2.removeBall(ball);
                }
                else
                {
                    // Remove the ball from player 1's list
                    player1.removeBall(ball);
                }
            }
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

    public void checkSkipTurn()
    {
        if (player1.isTurn == true)
        {
            player1.assignTurn(true);
            player2.assignTurn(false);
        }
        else
        {
            player1.assignTurn(false);
            player2.assignTurn(true);
        }
    }

    public void switchturns()
    {

        if (player1.isTurn == true)
        {
            player1.assignTurn(false);
            player2.assignTurn(true);
            changeText(player2);

        }
        else
        {
            player1.assignTurn(true);
            player2.assignTurn(false);
            if (player2.pBallType != "")
            changeText(player1);

        }
    }

    public void changeText(Player player)
    {
        if (player.pBallType != "")
        {
            playerText.text = $"{player.name}'s Turn!";
            type.text = $"Type: {player.pBallType}!";
        }
        else
        {
            playerText.text = $"{player.name}'s Turn!";
            type.text = $"Type: None!";
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
                SceneManager.LoadScene("Start Menu");
            }
            else
            {
                // Player 1 loses
                SceneManager.LoadScene("Start Menu");
            }
        }
        else
        {
            // Check if the player has pocketed all of their balls
            if (player2.balls.Count == 0)
            {
                // Player 2 wins
                SceneManager.LoadScene("Start Menu");
            }
            else
            {
                // Player 2 loses
                SceneManager.LoadScene("Start Menu");
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
        if (isTurn == true)
        {
            Debug.Log(name);
        }
    }

    public void removeBall(GameObject ball)
    {
        balls.Remove(ball);
        Debug.Log("Ball " + ball + " has been removed from " + name + "'s list of balls");
    }
}
