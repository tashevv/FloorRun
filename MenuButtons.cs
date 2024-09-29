using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeSettings();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Material to set random color
    public Material materialColor;

    // Set a random color using Random.ColorHSV for better randomness and simplicity
    public void SetRandomColor()
    {
        materialColor.color = Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f);  // Full random range for colors
    }

    // Initializes settings like terrain radius and lifetime
    public void InitializeSettings()
    {
        GenerateTerrain.radius = Random.Range(20, 50); // Random radius between 20 and 50
        GenerateTerrain.floorLifeTime = GenerateTerrain.radius / 10f; // Set floor lifetime based on radius
    }

    // Load the main game scene
    public void LoadLevel()
    {
        SetRandomColor();
        SceneManager.LoadScene("Scene1");
    }

    // Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }
}
