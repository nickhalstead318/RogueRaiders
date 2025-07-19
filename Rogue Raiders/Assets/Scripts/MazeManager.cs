using System.IO;
using System;
using UnityEngine;

public static class MazeManager
{

    public static string[,] GetMap(int number)
    {
        string mapName = "map" + number + ".txt";
        string filepath = Path.Combine(Application.dataPath, "Maps", mapName);

        return Read2DArrayFromFile(filepath);
    }

    public static string[,] Read2DArrayFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        int rowCount = lines.Length;
        int colCount = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        string[,] array = new string[rowCount, colCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] values = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (values.Length != colCount)
                throw new InvalidDataException($"Row {i} does not have {colCount} columns.");

            for (int j = 0; j < colCount; j++)
            {
                array[i, j] = values[j];
            }
        }

        return array;
    }
}
