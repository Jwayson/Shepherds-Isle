using UnityEngine;

public class ShiipController : MonoBehaviour
{
    public float speed = 5f;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private GameObject mainCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.gameObject.transform.parent.gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        //left-right movement
        // detect if the A key is being pressed
        if (Input.GetAxis("Horizontal") == -1)
        {
            isMovingLeft = true;
        }

        // detect if the D key is being pressed
        if (Input.GetAxis("Horizontal") == 1)
        {
            isMovingRight = true;
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            isMovingLeft = false;
            isMovingRight = false;
        }

        // forward and back movement
        // detect if the W key is being pressed
        if (Input.GetAxis("Vertical") == 1)
        {
            isMovingForward = true;
        }

        // detect if the S key is being pressed
        if (Input.GetAxis("Vertical") == -1)
        {
            isMovingBackward = true;
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            isMovingForward = false;
            isMovingBackward = false;
        }
    }

    // fixed update is called once per physics tick
    void FixedUpdate()
    {
        
        if (isMovingLeft)
        {
            transform.Translate(-mainCamera.transform.right * speed * Time.fixedDeltaTime);
        }

        if (isMovingRight)
        {
            transform.Translate(mainCamera.transform.right * speed * Time.fixedDeltaTime);
        }

        if (isMovingForward)
        {
            transform.Translate(mainCamera.transform.forward * speed * Time.fixedDeltaTime);
        }

        if (isMovingBackward)
        {
            transform.Translate(-mainCamera.transform.forward * speed * Time.fixedDeltaTime);
        }

    }
}
