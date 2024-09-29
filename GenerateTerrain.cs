using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenerateTerrain : MonoBehaviour
{
    private List<GameObject> terrain = new List<GameObject>();
    private int terrainMaxCount;
    public GameObject floorPrefab;
    public GameObject mask;
    public Material colors;
    public static int radius = 25;
    public static float floorLifeTime;

    private static int pickTurn = 0;
    private Renderer playerRenderer;
    private bool isGamePaused = true;

    public Text timeText;
    public Text timeTextMasked;

    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        StartColor();
        InstantiateTerrain();
        MaskStartValue();

        // Start with the game paused
        Time.timeScale = 0;
        isGamePaused = true;
        StartCoroutine(WaitForAnyKey());
    }

    private void FixedUpdate()
    {
        if (!isGamePaused)
        {
            DemageAtPosition();
        }
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            KillPlayer();
        }
    }

    // Coroutine to wait for any key press to start the game
    private IEnumerator WaitForAnyKey()
    {
        while (!Input.anyKeyDown)
        {
            yield return null; // Wait for the next frame
        }

        // Resume the game
        Time.timeScale = 1;
        isGamePaused = false;
    }

    private void InstantiateTerrain()
    {
        float radiusSquared = radius * radius;

        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                if (x * x + z * z <= radiusSquared)
                {
                    Vector3 position = new Vector3(x, 0, z);
                    var newFloor = Instantiate(floorPrefab, position, Quaternion.identity);
                    terrain.Add(newFloor);
                }
            }
        }
        terrainMaxCount = terrain.Count;
    }

    private void DemageAtPosition()
    {
        Vector3 playerPos = transform.position;

        for (int i = terrain.Count - 1; i >= 0; i--)
        {
            Vector3 tilePos = terrain[i].transform.position;

            if (Vector3.Distance(new Vector3(tilePos.x, 0, tilePos.z), new Vector3(playerPos.x, 0, playerPos.z)) < 1)
            {
                float verticalDistance = Mathf.Abs(playerPos.y - tilePos.y);
                float multiplier = verticalDistance;
                ChangeColor(i, multiplier);
            }
        }
    }

    private void ChangeColor(int i, float multiplier)
    {
        Renderer tileRenderer = terrain[i].GetComponent<Renderer>();
        Color tileColor = tileRenderer.material.color;
        Color playerColor = playerRenderer.material.color;

        tileRenderer.material.color = Color.Lerp(tileColor, playerColor, floorLifeTime / 50f * multiplier);

        if (tileRenderer.material.color.r - 0.1f <= playerColor.r &&
            tileRenderer.material.color.g - 0.1f <= playerColor.g &&
            tileRenderer.material.color.b - 0.1f <= playerColor.b) 
        {
            if (transform.position.y < 1.5f || transform.position.y > 3f) // Only destroy if the player is touching the tiles or is above certain jump height
            {
                DestroyTile(i);
            }
        }
    }

    private void DestroyTile(int i)
    {
        Destroy(terrain[i].gameObject);
        terrain.RemoveAt(i);
        MaskProgress(terrain.Count, terrainMaxCount);
    }

    private void MaskStartValue()
    {
        RectTransform maskComponent = mask.GetComponent<RectTransform>();
        Vector2 sizeDelta = maskComponent.sizeDelta;
        sizeDelta.y = 0;  // Set the height to 0
        maskComponent.sizeDelta = sizeDelta;
    }

    private void MaskProgress(int current, int max)
    {
        RectTransform maskComponent = mask.GetComponent<RectTransform>();
        Vector2 sizeDelta = maskComponent.sizeDelta;
        float ratio = (float)current / max;
        sizeDelta.y = 100 - ratio * 100;  // Set the height
        maskComponent.sizeDelta = sizeDelta;
    }

    private void StartColor()
    {
        Color32[] colorsArray = new Color32[] {
            new Color32(148, 0, 211, 255),
            new Color32(75, 0, 130, 255),
            new Color32(0, 0, 255, 255),
            new Color32(0, 255, 0, 255),
            new Color32(255, 255, 0, 255),
            new Color32(255, 127, 0, 255),
            new Color32(255, 0, 0, 255)
        };

        colors.color = colorsArray[Random.Range(0, colorsArray.Length)];
        pickTurn = (pickTurn + 1) % colorsArray.Length;
    }

    private void KillPlayer()
    {
        if (transform.position.y < -20)
        {
            Time.timeScale = 0;
            timeText.text = "Game over";
            timeTextMasked.text = "Game over";
            if (Input.anyKeyDown)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Menu");
            }
            
        }
    }
}
