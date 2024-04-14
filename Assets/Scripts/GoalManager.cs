using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private List<GoalController> goalsInTrigger = new List<GoalController>();

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
            Debug.Log("You Win");
        }
    }
}