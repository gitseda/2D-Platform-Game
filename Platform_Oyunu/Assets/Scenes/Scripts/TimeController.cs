using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] Text timeValue;                 
    [SerializeField] float time;                     
    private bool gameActive;                         

    void Start()
    {
        gameActive = true;                           
        timeValue.text = time.ToString();           
    }

    void Update()
    {
        if(gameActive == true)                       
        {
            time -= Time.deltaTime;                  
            timeValue.text = ((int)time).ToString(); 
        }

        if(time<0)                                   
        {
            gameActive = false;                      
            GetComponent<PlayerController>().Die();  
        }
    }
}
