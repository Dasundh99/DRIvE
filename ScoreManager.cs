// ScoreManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public float Score { get; private set; }
    public float HighScore;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    private CarController carController;
    private Vector3 lastPosition;

    private void Start()
    {
        carController = CarController.Instance;

        if (carController == null)
        {
            Debug.LogError("CarController not found. Make sure it is attached or accessible.");
            return;
        }

        lastPosition = carController.transform.position;

        // Load the high score from PlayerPrefs
        HighScore = PlayerPrefs.GetFloat("HighScore", 0);

        // Display the previous high score when starting the game
        DisplayHighScore();
        UpdateUI();
    }

    private void Update()
    {
        if (carController == null)
            return;

        if (!carController.alive)
        {
            StoreHighScore();
            DisplayHighScore();
            return;
        }

        UpdateScore();
        UpdateUI();
    }

    private void UpdateScore()
    {
        float distanceTraveled = carController.transform.position.z - lastPosition.z;

        // Check if the car is moving forward (positive distance)
        if (distanceTraveled > 0)
        {
            Score += distanceTraveled;
        }
        // Check if the car is moving backward (negative distance)
        else if (distanceTraveled < 0)
        {
            Score += distanceTraveled; // This will decrease the score
        }

        lastPosition = carController.transform.position;
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(Score);
        }
    }

    // Increase score 
    public void IncreaseScore(int amount)
    {
        Score += amount;
    }

    public void StoreHighScore()
    {
        // Check if the current score is higher than the stored high score
        if (Score > HighScore)
        {
            HighScore = Score;

            // Save the new high score to PlayerPrefs
            PlayerPrefs.SetFloat("HighScore", HighScore);
            PlayerPrefs.Save();

            Debug.Log("New High Score: " + HighScore);
        }
    }

    public void DisplayHighScore()
    {
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + Mathf.RoundToInt(HighScore);
        }
    }

    private void RestartScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
