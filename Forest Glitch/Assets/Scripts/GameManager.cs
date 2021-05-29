using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStats.timer = 0.0f;
        GameStats.score = 0;
        GameStats.gameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStats.score >= 5)
        {
            SceneManager.LoadScene("GameOver");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
