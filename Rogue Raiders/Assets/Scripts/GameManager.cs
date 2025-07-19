using System;
using UnityEngine;

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
        SpawnGrid();

        // Move camera to the center of the grid
        CenterCamera();
    }

    private void SpawnGrid()
    {
        float width = gridPoint.transform.localScale.x;
        float length = gridPoint.transform.localScale.z;

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridLength; j++)
            {
                SpawnNode(i * (width + 1), j * (length + 1));
            }
        }

        centerX = (gridWidth * (width + 1)) / 2;
        centerY = (gridLength * (length + 1)) / 2;
        float w = gridWidth * width;
        float l = gridLength * length;
        cameraHeight = w > l ? w : l;
    }

    private void SpawnNode(float x, float y)
    {
        Debug.Log("Spawning node at (" + x + ", " + y + ")");
        Instantiate(gridPoint, new Vector3(x, 0, y), Quaternion.identity);
    }

    private void CenterCamera()
    {
        mainCamera.transform.position = new Vector3(centerX, cameraHeight, centerY);
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
    }
}
