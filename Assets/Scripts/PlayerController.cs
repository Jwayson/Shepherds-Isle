using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float aimSpeed = 2.5f;
    public float rotationSpeed = 10f;
    private Vector2 moveInput;
    private GameObject mainCamera;
    public Animator anim;
    private bool moving;
    private Vector2 lookInput; 
    public LayerMask cursorMask;
    public float aimRotationSpeed = 5.0f;
    private bool aiming;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main.gameObject.transform.parent.gameObject; 
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        lookInput = new Vector2 (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetButton("Fire2"))
        {
           UpdateLookRotation();
           aiming = true; 
        }
        else
        {
            aiming = false;
        }

        UpdateAnimator();
        
    }

    // fixed update is called once per physics tick
    void FixedUpdate()
    {     

        Vector3 combinedMovement = mainCamera.transform.right * moveInput.x + mainCamera.transform.forward * moveInput.y;
        
        if (aiming)
        {
            transform.position += combinedMovement * aimSpeed * Time.fixedDeltaTime; 
        }
        else
        {
            transform.position += combinedMovement * speed * Time.fixedDeltaTime;
        }

        if (combinedMovement.sqrMagnitude > 0.0001f)
        {
            moving = true;

            if (!aiming)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-combinedMovement.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }


            
        }
        else
        {
            moving = false;
        }
    }

    void UpdateAnimator()
    {
        anim.SetBool("Running", moving);
    }

    void UpdateLookRotation()
    {
        Physics.Raycast(mainCamera.transform.GetChild(0).GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 500, cursorMask);

        if (hit.point != null)
        {
          
          Vector3 direction = hit.point - transform.position;
          direction.y = 0f;

          Quaternion lookAtRotation = Quaternion.LookRotation(-direction);

          transform.rotation = Quaternion.Slerp(transform.rotation, lookAtRotation, aimRotationSpeed * Time.deltaTime);
        }
    }
}

