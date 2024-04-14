using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private List<GoalController> goalsInTrigger = new List<GoalController>();
    public static event Action OnWin;


    void Start()
    {
        GoalController.OnBoxEnterGoal += HandleBoxEnterGoal;
        GoalController.OnBoxExitGoal += HandleBoxExitGoal;
    }

    void OnDestroy()
    {
        GoalController.OnBoxEnterGoal -= HandleBoxEnterGoal;
        GoalController.OnBoxExitGoal -= HandleBoxExitGoal;
    }

    private void HandleBoxEnterGoal(GoalController goal)
    {
        if (!goalsInTrigger.Contains(goal))
        {
            goalsInTrigger.Add(goal);
            CheckWinCondition();
        }
    }

    private void HandleBoxExitGoal(GoalController goal)
    {
        if (goalsInTrigger.Contains(goal))
        {
            goalsInTrigger.Remove(goal);
        }
    }

    private void CheckWinCondition()
    {
        if (goalsInTrigger.Count == FindObjectsOfType<GoalController>().Length)
        {
            StartCoroutine(WaitAndInvokeWin());
        }
    }

    private IEnumerator WaitAndInvokeWin()
    {
        Debug.Log("Win");
        yield return new WaitForSeconds(0.5f);
        OnWin?.Invoke();
    }
}