using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Collectables : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Text timeTextMasked;
    [SerializeField] private GameObject collectable;
    [SerializeField] private AudioClip m_collect;
    private double timeAlive; // Time remaining
    private Vector3 position;
    private AudioSource m_AudioSource;
    // Reference to GenerateTerrain script
    private GenerateTerrain generateTerrain;

    void Start()
    {
        timeAlive = GenerateTerrain.radius / 2; // Set initial timer
        SpawnCollectable();
        m_AudioSource = GetComponent<AudioSource>();
        // Find the GenerateTerrain instance in the scene
        generateTerrain = FindObjectOfType<GenerateTerrain>(); // Ensure there is only one instance in the scene
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
            generateTerrain.KillPlayer();
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
            PlayCollectSound();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        HandleCollectablePickup(col);
    }

    private void PlayCollectSound()
    {
        m_AudioSource.PlayOneShot(m_collect);
    }
}
