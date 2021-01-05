using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        var gameStatus = GameObject.FindObjectOfType<GameStatus>();
        gameStatus.Restart();
    }
}
