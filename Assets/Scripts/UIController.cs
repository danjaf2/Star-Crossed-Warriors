using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text; //Speed
    [SerializeField] public TextMeshProUGUI text2; //Altitude
    [SerializeField] public Rigidbody planeRB;
    [SerializeField] public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Speed: " + (int)planeRB.velocity.magnitude + " units/second"; 
        text2.text = "Altitude: " + (int)planeRB.transform.position.y + " units";
        CheckForGameOver();
    }

    public void Restart()
    {
        if (Globals.gameOver)
        {
            Time.timeScale = 1;
            gameOverPanel.SetActive(false);
            Globals.gameOver = false;
            SceneManager.LoadScene(0); //Restarts the simulation

        }
    }


    public void CheckForGameOver()
    {
        if (Globals.gameOver && !gameOverPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
    }
}
