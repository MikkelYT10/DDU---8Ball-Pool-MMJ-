using UnityEngine;

public class CornerScripts : MonoBehaviour
{
    //Identify the corner
    public int cornerNumber;
    public GameObject player;
    public GameManager GameManager;
    public Transform middle;

    private void Awake()
    {
        GameManager = FindObjectOfType<GameManager>();
        if (GameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            // Handle this situation appropriately
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Corner " + cornerNumber + " entered by " + other.gameObject);

        if (other.gameObject.tag == "Striped")
        {
            if (GameManager.assignedBalls == false)
            {
                if (GameManager.getPlayer1IsTurn() == true)
                {
                    GameManager.assigningBalls(1, "Striped");
                    GameManager.assigningBalls(2, "Full");
                }
                else
                {
                    GameManager.assigningBalls(2, "Striped");
                    GameManager.assigningBalls(1, "Full");

                }
                GameManager.assignedBalls = true;

            }

            GameManager.removeBallFromPlayerList(true, other.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Full")
        {
            if (GameManager.assignedBalls == false)
            {
                if (GameManager.getPlayer1IsTurn() == true)
                {
                    GameManager.assigningBalls(1, "Full");
                    GameManager.assigningBalls(2, "Striped");

                }
                else
                {
                    GameManager.assigningBalls(2, "Full");
                    GameManager.assigningBalls(1, "Striped");

                }
                GameManager.assignedBalls = true;
            }

            GameManager.removeBallFromPlayerList(false, other.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "8Ball")
        {
            GameManager.EightBallPocketed();
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Player")
        {
            other.transform.position = middle.position;
            GameManager.stopMoving(other.gameObject.GetComponent<Rigidbody>());
        }
        else
        {
            Debug.LogError("Unknown ball type entered the corner!");
            // Handle this situation appropriately
        }
    }
}
