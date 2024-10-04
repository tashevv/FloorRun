using UnityEngine;

public class MenuCameraMove : MonoBehaviour
{
    private Rigidbody _rigidbody;

    void Start()
    {
        // Cache the Rigidbody reference
        _rigidbody = GetComponent<Rigidbody>();

        // Apply a torque to rotate the camera
        if (_rigidbody != null) _rigidbody.AddTorque(Vector3.up * 5f);
        else Debug.LogWarning("Rigidbody component not found on the camera object.");
    }
}
