using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStats.timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStats.gameRunning == true)
        {
            GameStats.timer += Time.deltaTime;
        }
    }
}
