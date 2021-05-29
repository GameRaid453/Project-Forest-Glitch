using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Script contains Animal Score and Time Limit
public class UIDetails : MonoBehaviour
{

    [SerializeField] Text TimelimitTEXT;
    [SerializeField] Text AnimalScoreTEXT;



    void Start()
    {
        AnimalScoreTEXT.text = GameStats.score.ToString();
        TimelimitTEXT.text = Mathf.Floor(GameStats.timer).ToString(); 
    }

  

    // Update is called once per frame
    void Update()
    {
        AnimalScoreTEXT.text = GameStats.score.ToString();
        TimelimitTEXT.text = Mathf.Floor(GameStats.timer).ToString();

        
    }
}
