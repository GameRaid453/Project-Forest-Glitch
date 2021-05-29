using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonscript : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("The game would quit but no");
    }

    public void ChangeScene(int index)

    {
        SceneManager.LoadScene(index);
        Debug.Log("Index Change is a go!");
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene Change should be working!");
    }

}
