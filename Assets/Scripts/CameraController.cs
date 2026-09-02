using UnityEngine;

public class CameraController : MonoBehaviour

{
    public Transform followTarget;
    private Vector3 initialPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followTarget.position.x + initialPosition.x, transform.position.y, followTarget.position.z + initialPosition.z);
    }

}
