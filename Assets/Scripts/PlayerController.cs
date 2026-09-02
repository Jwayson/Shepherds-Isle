using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    private Vector2 moveInput;
    private GameObject mainCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.gameObject.transform.parent.gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    // fixed update is called once per physics tick
    void FixedUpdate()
    {     

        Vector3 combinedMovement = mainCamera.transform.right * moveInput.x + mainCamera.transform.forward * moveInput.y;
        transform.position += combinedMovement * speed * Time.fixedDeltaTime; 
        if (combinedMovement.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-combinedMovement.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}

