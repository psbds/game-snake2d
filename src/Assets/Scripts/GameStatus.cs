using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{

    private bool gameOver = false;
    [SerializeField] GameObject gameOverCanvas;

    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void NewPoint()
    {
        var foodSpawner = GameObject.FindObjectOfType<FoodSpawner>();
        foodSpawner.SpawnFood();
        score += 39;

        var scoreUI = GameObject.FindGameObjectWithTag("score");
        scoreUI.GetComponent<TextMeshProUGUI>().SetText($"Score: {score}");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            Instantiate(gameOverCanvas, new Vector3(0, 0, 0), Quaternion.identity);
            gameOver = true;
        }
    }

    void Update()
    {

    }
}
