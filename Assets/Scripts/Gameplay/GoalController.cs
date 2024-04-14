using System;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public static event Action<GoalController> OnBoxEnterGoal;
    public static event Action<GoalController> OnBoxExitGoal;

    private int boxLayer;

    private void Start()
    {
        boxLayer = LayerMask.NameToLayer("Box");
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Object entered goal");

        if (other.gameObject.layer == boxLayer)
        {
            Debug.Log("Box entered goal");
            OnBoxEnterGoal?.Invoke(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == boxLayer)
        {
            OnBoxExitGoal?.Invoke(this);
        }
    }
}
