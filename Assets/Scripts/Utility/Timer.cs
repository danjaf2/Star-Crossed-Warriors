using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float timeLeft = 5 * 60;
    public float timeDefault = 5 * 60;
    public bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.unscaledDeltaTime;
            }
            else
            {
                timeLeft = 0;
                running = false;
            }
        }
        
    }

    public void Reset()
    {
        timeLeft = timeDefault;
        running = true;
    }
}
