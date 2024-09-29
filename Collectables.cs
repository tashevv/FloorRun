using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Collectables : MonoBehaviour
{
    private double timeAlive; // Time remaining
    public Text timeText;
    public Text timeTextMasked;
    public Text scoreText;
    public GameObject collectable;
    private Vector3 position;

    void Start()
    {
        timeAlive = GenerateTerrain.radius / 2; // Set initial timer
        SpawnCollectable();
    }

    void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        timeAlive -= Time.deltaTime; // Decrease time every frame

        string timeStr = Mathf.CeilToInt((float)timeAlive).ToString(); // Round up to int
        timeText.text = timeStr;
        timeTextMasked.text = timeStr; // Sync masked text

        if (timeAlive <= 0)
        {
            Time.timeScale = 0; // Pause game
            timeText.text = "Game over";
            timeTextMasked.text = "Game over";
            if (Input.anyKeyDown)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Menu");
            }
        }
    }

    private void CheckCollectablePos()
    {
        // Ensure collectable is spawned within a circular area
        do
        {
            position = new Vector3(
                Random.Range(-GenerateTerrain.radius, GenerateTerrain.radius),
                2,
                Random.Range(-GenerateTerrain.radius, GenerateTerrain.radius)
            );
        }
        while (position.sqrMagnitude > GenerateTerrain.radius * GenerateTerrain.radius);
    }

    private void SpawnCollectable()
    {
        CheckCollectablePos();
        Instantiate(collectable, position, Quaternion.identity); // Spawn collectable at new position
    }

    private void HandleCollectablePickup(Collider collider)
    {
        if (collider.CompareTag("Collectable"))
        {
            timeAlive += GenerateTerrain.radius / 8; // Add to the timer
            Destroy(collider.gameObject); // Destroy the collected item
            SpawnCollectable(); // Spawn the next collectable
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        HandleCollectablePickup(col);
    }
}
