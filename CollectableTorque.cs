using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableTorque : MonoBehaviour
{
    // Adds torque to the collectable prefab
    [SerializeField] private Material playerMaterial;

    void Start()
    {
        // Cache the color conversion for reuse
        Color32 color = new Color32(
            (byte)(playerMaterial.color.r * 255),
            (byte)(playerMaterial.color.g * 255),
            (byte)(playerMaterial.color.b * 255),
            150 // Alpha value
        );

        // Apply torque and color to each child object
        for (int i = 0; i < 3; i++)
        {
            GameObject cube = transform.GetChild(i).gameObject;
            Rigidbody rb = cube.GetComponent<Rigidbody>();
            Renderer renderer = cube.GetComponent<Renderer>();

            // Apply different torque directions for alternate cubes
            Vector3 torque = (i % 2 == 0) ? new Vector3(100, 100, 100) : new Vector3(-100, -100, -100);
            rb.AddTorque(torque);

            // Set the material color
            renderer.material.color = color;
        }
    }
}
