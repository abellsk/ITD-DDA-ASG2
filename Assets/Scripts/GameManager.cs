using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOver;
    public static GameManager instance;

    public int taskCounter = 0;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void TaskTracker()
    {
        taskCounter++;
        if (taskCounter >= 3)
        {
            DayEnd();
        }
        Debug.Log(taskCounter);
    }

    public void DayEnd()
    {
        gameOver.SetActive(true);

    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
