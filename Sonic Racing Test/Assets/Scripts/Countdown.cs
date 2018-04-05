using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private Manager gameManager;

    public void StartCountdown()
    {
        gameManager = GameObject.Find("Manager").GetComponent<Manager>();
        gameManager.countedDown = true;
    }
}
