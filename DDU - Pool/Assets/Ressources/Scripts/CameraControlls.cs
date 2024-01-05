using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform sphereTransform; // Reference to the sphere's transform
    public Vector3 offset; // Offset of the camera relative to the sphere

    public float rotationSpeed; // Rotation speed when holding right click
    private float mouseMultiplier = 10;

    private bool isRotating = false;
    private Vector3 lastMousePosition;

    void Start()
    {
        Debug.Log(GameObject.FindWithTag("Player"));
        sphereTransform = GameObject.FindWithTag("Player").gameObject.transform; // Assuming the tag "Player" is assigned to the sphere
    }

    void Update()
    {
        Debug.Log(sphereTransform.gameObject.GetComponent<Rigidbody>().velocity);
        if (sphereTransform.gameObject.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            HandleZoomInput();
            HandleRotationInput();
        }


        // Follow the sphere's position but not its rotation
        transform.position = sphereTransform.position + offset;

        // You can optionally add code to make the camera look at a specific point, for example, the center of the sphere
        transform.LookAt(sphereTransform.position);
    }

    void HandleRotationInput()
    {
        // Check if right mouse button is held down
        if (Input.GetMouseButtonDown(1))
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }

        // Check if right mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            isRotating = false;
        }

        // Rotate the camera around the sphere if right mouse button is held down
        if (isRotating)
        {
            Vector3 currentMousePosition = Input.mousePosition;

            // Rotate around the sphere based on a fixed increment per frame
            float rotationIncrementX = Input.GetAxis("Mouse X") * mouseMultiplier;
            float rotationIncrementY = Input.GetAxis("Mouse Y") * mouseMultiplier;

            transform.RotateAround(sphereTransform.position, Vector3.up, rotationSpeed * Time.deltaTime * rotationIncrementX);
            transform.RotateAround(sphereTransform.position, -transform.right, rotationSpeed * Time.deltaTime * rotationIncrementY);

            // Update the camera offset to maintain the same distance from the sphere
            offset = transform.position - sphereTransform.position;

            lastMousePosition = currentMousePosition;
        }
    }

    void HandleZoomInput()
    {
        // Get the scroll wheel delta
        float scrollDelta = Input.mouseScrollDelta.y;

        // Limit the offset's y value to restrict zooming in/out
        offset.y = Mathf.Clamp(offset.y, 1, 5);


        // Zoom in or out by the scroll delta amount
        offset -= offset.normalized * scrollDelta;

    }
}
