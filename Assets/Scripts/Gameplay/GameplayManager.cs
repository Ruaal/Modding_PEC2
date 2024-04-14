using System.IO;
using UnityEngine;
using System.Diagnostics;
using TMPro;
using JetBrains.Annotations;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private LevelEditorManager levelEditorManager;
    [SerializeField]
    private TMP_Text moveCountText;
    [SerializeField]
    private TMP_Text pushCountText;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text levelText;

    private int currentLevel = 0;
    private int moveCount = 0;
    private int pushCount = 0;
    private string time;
    private Stopwatch stopwatch = new Stopwatch();
    private string streamingAssetsPath;


    void Start()
    {
        GoalManager.OnWin += HandleWin;
        PlayerController.OnPlayerMove += HandlePlayerMove;
        BoxController.OnBoxMove += HandleBoxMove;
        streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, "Levels");
        LoadNextLevel();
    }

    void HandleWin()
    {
        stopwatch.Stop();
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        moveCount = 0;
        pushCount = 0;
        stopwatch.Reset();
        stopwatch.Start();

        string levelFileName = $"level_{currentLevel}.json";
        if (!File.Exists(Path.Combine(streamingAssetsPath, levelFileName)))
        {
            currentLevel = 1;
        }
        levelEditorManager.LoadMap(currentLevel);
    }

    public void ResetLevel()
    {
        moveCount = 0;
        pushCount = 0;
        stopwatch.Reset();
        stopwatch.Start();
        levelEditorManager.LoadMap(currentLevel);
    }

    void OnDestroy()
    {
        GoalManager.OnWin -= HandleWin;
    }

    void HandlePlayerMove()
    {
        moveCount++;
    }

    void HandleBoxMove()
    {
        pushCount++;
    }

    void Update()
    {
        moveCountText.text = $"{moveCount}";
        pushCountText.text = $"{pushCount}";
        time = string.Format("{0:00}:{1:00}:{2:00}",
                    stopwatch.Elapsed.Hours,
                    stopwatch.Elapsed.Minutes,
                    stopwatch.Elapsed.Seconds);
        timeText.text = $"{time}";
        levelText.text = $"{currentLevel}";
    }
}
