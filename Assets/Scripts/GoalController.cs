using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public static event Action<GoalController> OnBoxEnterGoal;
    public static event Action<GoalController> OnBoxExitGoal;


    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Box entered goal");

        if (other.gameObject.CompareTag("Box"))
        {
            Debug.Log("Box entered goal");
            // Si es una caja, emite el evento
            OnBoxEnterGoal?.Invoke(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            // Si es una caja, emite el evento
            OnBoxExitGoal?.Invoke(this);
        }
    }
}
