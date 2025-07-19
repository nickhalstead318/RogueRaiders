using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject gridPoint;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridLength;
    private float centerX = 0;
    private float centerY = 0;
    private float cameraHeight = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Error state checking
        CheckErrors();

        // Spawn them in a grid starting at (0, 0, 0)
        SpawnMap(1);

        // Move camera to the center of the grid
        CenterCamera();
    }

    private void SpawnMap(int id)
    {
        string[,] map = MazeManager.GetMap(id);
        float width = gridPoint.transform.localScale.x;
        float length = gridPoint.transform.localScale.z;
        gridWidth = map.GetLength(0);
        gridLength = map.GetLength(1);
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                if (map[i,j] == "01")
                {
                    SpawnNode(i * (width + 1), j * (length + 1));
                }
            }
        }

        centerX = ((gridWidth - 1) * (width + 1)) / 2;
        centerY = ((gridLength - 1) * (length + 1)) / 2;
        float w = gridWidth * width;
        float l = gridLength * length;
        cameraHeight = w > l ? w : l;
        cameraHeight *= 1.2f;
    }

    private void SpawnNode(float x, float y)
    {
        Debug.Log("Spawning node at (" + x + ", " + y + ")");
        Instantiate(gridPoint, new Vector3(x, 0, y), Quaternion.identity);
    }

    private void CenterCamera()
    {
        // mainCamera.transform.position = new Vector3(centerX, cameraHeight, centerY);
        MoveCameraTo(centerX, centerY);
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


    private void CheckErrors()
    {
        if (gridWidth < 1 || gridLength < 1)
        {
            gridWidth = gridWidth < 1 ? 1 : gridWidth;
            gridLength = gridLength < 1 ? 1 : gridLength;
            Debug.LogError("Grid length and width must be at least 1");
        }
        if (gridPoint == null)
        {
            Debug.LogError("No gridPoint object set");
        }
        if(mainCamera == null)
        {
            Debug.LogError("No camera object set; using Camera.main");
            mainCamera = Camera.main;
        }
    }
}
