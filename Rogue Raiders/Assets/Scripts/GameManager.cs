using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject card;
    [SerializeField] private GameObject towerSlotNode;
    [SerializeField] private GameObject pathNode;
    [SerializeField] private float camScaleDist = 1.5f;
    [SerializeField] private Grid grid;

    private int gridWidth;
    private int gridLength;
    private float centerX = 0;
    private float centerY = 0;
    private float cameraHeight = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if(grid == null) grid = GetComponent<Grid>();
        if (grid == null)
        {
            grid = new Grid();
            grid.cellGap = new Vector3(1, 0, 1);
            grid.cellSize = new Vector3(4, 0, 4);
        }

        // Spawn a map at (0, 0, 0)
        SpawnMap(1);

        // Move camera to the center of the map
        CenterCamera();

        // Spawn some cards
        SpawnCards(5);
    }

    public void SpawnMap(int id)
    {
        string[,] map = MazeManager.GetMap(id);
        float width = grid.cellSize.x;
        float length = grid.cellSize.z;
        gridWidth = map.GetLength(0);
        gridLength = map.GetLength(1);
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                GameObject obj = MapLegendToObject(map[i, j]);
                obj.transform.localScale = grid.cellSize;
                SpawnNode(obj, i * (width + grid.cellGap.x), j * (length + grid.cellGap.z));
            }
        }

        centerX = ((gridWidth - 1) * (width + grid.cellGap.x)) / 2;
        centerY = ((gridLength - 1) * (length + grid.cellGap.z)) / 2;
        float w = gridWidth * width;
        float l = gridLength * length;
        cameraHeight = w > l ? w : l;
        cameraHeight *= camScaleDist;
    }

    public void CenterCamera()
    {
        // mainCamera.transform.position = new Vector3(centerX, cameraHeight, centerY);
        MoveCameraTo(centerX, centerY);
    }

    public void SpawnCards(int quantity)
    {
        for(int i = 0; i <quantity; i++)
        {
            GameObject obj = Instantiate(card);
            obj.transform.Rotate(new Vector3(0, 180.0f, 0));
            obj.GetComponent<CardBehavior>().cardIndex = i;

        }
    }


    private GameObject MapLegendToObject(string mapStr)
    {
        switch (mapStr)
        {
            case "01": return towerSlotNode;
            case "00": return pathNode;
            default: return new GameObject();
        }
    }

    private void SpawnNode(GameObject node, float x, float y)
    {
        Debug.Log("Spawning node at (" + x + ", " + y + ")");
        Instantiate(node, new Vector3(x, 0, y), Quaternion.identity);
    }

    private void MoveCameraTo(float x, float y)
    {
        StartCoroutine(MoveCamera(x, y));
    }

    private IEnumerator MoveCamera(float x, float y, float duration = 1.0f)
    {
        Vector3 startPos = mainCamera.transform.position;
        Vector3 endPos = new Vector3(x, cameraHeight, y);
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        mainCamera.transform.position = endPos;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
