using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorManager : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject playerPrefab;
    public GameObject wallPrefab;

    private GameObject currentPrefab;
    private GameObject currentPlayer;
    public TMP_InputField inputField;

    private string directoryPath;
    private int saveCounter = 1;
    [SerializeField]
    private bool isEditing = false;
    private int editorFileIndex = 0;
    private string streamingAssetsPath;

    void Start()
    {
        directoryPath = Path.Combine(Application.dataPath, "Levels");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        int savedCounter = PlayerPrefs.GetInt("SaveCounter", 1);
        saveCounter = savedCounter;
        streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, "Levels");
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("SaveCounter", saveCounter);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (isEditing)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector2Int cellPosition = GetCellPosition();
                PlacePrefab(cellPosition);
            }

            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector2Int cellPosition = GetCellPosition();
                RemovePrefab(cellPosition);
            }
        }
    }

    public void SelectPrefab(string prefabName)
    {
        switch (prefabName)
        {
            case "Box":
                currentPrefab = boxPrefab;
                break;
            case "Goal":
                currentPrefab = goalPrefab;
                break;
            case "Player":
                currentPrefab = playerPrefab;
                break;
            case "Wall":
                currentPrefab = wallPrefab;
                break;
        }
    }

    Vector2Int GetCellPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = new Vector3Int(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y), 0);
        return new Vector2Int(cellPosition.x, cellPosition.y);
    }

    void PlacePrefab(Vector2Int position)
    {
        if (IsCellOccupied(position))
        {
            Debug.Log("¡No se puede colocar un prefab en una casilla ocupada!");
            return;
        }

        if (currentPrefab != null)
        {
            if (currentPrefab == playerPrefab)
            {
                if (currentPlayer != null)
                {
                    Destroy(currentPlayer);
                }
                currentPlayer = Instantiate(currentPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(currentPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
            }
        }
    }

    bool IsCellOccupied(Vector2Int position)
    {
        return GetObjectAtPosition(position) != null;
    }

    void RemovePrefab(Vector2Int position)
    {
        GameObject objectToRemove = GetObjectAtPosition(position);
        if (objectToRemove != null)
        {
            Destroy(objectToRemove);
        }
    }

    public GameObject GetObjectAtPosition(Vector2Int position)
    {
        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true
        };

        RaycastHit2D[] hits = new RaycastHit2D[1];
        if (Physics2D.Raycast(position, Vector2.zero, filter, hits) > 0)
        {
            return hits[0].collider.gameObject;
        }

        return null;
    }

    public void SaveMap()
    {
        if (editorFileIndex != 0)
        {
            saveCounter = editorFileIndex;
        }
        string filePath = Path.Combine(streamingAssetsPath, $"level_{saveCounter}.json");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        List<LevelObjectData> mapData = new List<LevelObjectData>();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("EditorObject");
        foreach (GameObject obj in objects)
        {
            LevelObjectData data = new LevelObjectData
            {
                prefabName = obj.name,
                position = new Vector2Int((int)obj.transform.position.x, (int)obj.transform.position.y)
            };
            mapData.Add(data);
        }

        string jsonData = JsonHelper.ToJson<LevelObjectData>(mapData.ToArray(), true);
        File.WriteAllText(filePath, jsonData);

        saveCounter++;
    }

    public void LoadMap(int fileIndex)
    {
        if (fileIndex == 0 && editorFileIndex != 0)
        {
            fileIndex = editorFileIndex;
        }
        string fileName = $"level_{fileIndex}.json";
        ClearMap();

        string filePath = Path.Combine(streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            List<LevelObjectData> mapData = JsonHelper.FromJson<LevelObjectData>(jsonData).ToList();

            foreach (LevelObjectData data in mapData)
            {
                GameObject prefab = GetPrefabByName(data.prefabName);
                if (prefab != null)
                {
                    Instantiate(prefab, new Vector3(data.position.x, data.position.y, 0), Quaternion.identity);
                }
            }

            Debug.Log("Mapa cargado correctamente.");
        }
        else
        {
            Debug.LogError($"No se encontró el archivo de mapa: {fileName}");
        }
    }

    void ClearMap()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("EditorObject");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    GameObject GetPrefabByName(string prefabName)
    {
        switch (prefabName)
        {
            case "Box":
            case "Box(Clone)":
                return boxPrefab;
            case "Goal":
            case "Goal(Clone)":
                return goalPrefab;
            case "Player":
            case "Player(Clone)":
                return playerPrefab;
            case "Wall":
            case "Wall(Clone)":
                return wallPrefab;
            default:
                return null;
        }
    }

    public void readInputLevel()
    {
        editorFileIndex = int.Parse(inputField.text);
    }
}
