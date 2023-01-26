using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class Globals
{
    public static bool initialized = false;
    public static bool gameOver = false;
}

public class GlobalManager : MonoBehaviour
{

    public void ClosePanel(GameObject p)
    {
        p.SetActive(false);
    }

    public void ActivatePanel(GameObject p)
    {
        p.SetActive(true);
    }

    public void LoadScene(int x)
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(x); 
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}