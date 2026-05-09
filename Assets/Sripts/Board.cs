using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Row {
    public List<Transformish> columns = new List<Transformish>();
}

[System.Serializable]
public class Transformish {
    public Vector3 position;
    public Vector3 rotation;
    
    // Constructor for specific values
    public Transformish(Vector3 pos, Vector3 rot) {
        this.position = pos;
        this.rotation = rot;
    }

    // Default constructor to prevent null errors during initialization
    public Transformish() {
        this.position = Vector3.zero;
        this.rotation = Vector3.zero;
    }
}

public class Board : MonoBehaviour
{   
    [SerializeField] private List<Row> cardRows = new List<Row>();
    [SerializeField] private Transform bottomLeftTransform;
    [SerializeField] private GameObject place;
 
    void Start()
    {   
        if (bottomLeftTransform == null) {
            Debug.LogError("Please assign Bottom Left Transform in the inspector!");
            return;
        }

        InitializeGrid(7, 5);

        for (int i = 0; i < 7; i++) {
            for (int j = 0; j < 5; j++) {
                // Simplified math: no need to cast to float if you use 'f' on your numbers
                float x = bottomLeftTransform.position.x + ((i-3) * 0.32f);
                float y = bottomLeftTransform.position.y + (0.5f);
                float z = bottomLeftTransform.position.z + ((j-2) * 0.32f);

                Vector3 newPos = new Vector3(x, y, z);
                
                // Assign the new data to the existing slot
                cardRows[i].columns[j] = new Transformish(newPos, Vector3.zero);
                Instantiate(place, cardRows[i].columns[j].position, Quaternion.identity);
            }
        }
    }

    void InitializeGrid(int rows, int cols)
    {
        cardRows.Clear();

        for (int i = 0; i < rows; i++)
        {
            Row newRow = new Row();
            for (int j = 0; j < cols; j++)
            {
                // CRITICAL: Use 'new Transformish()' instead of 'null' 
                // to ensure the list index is actually valid data.
                newRow.columns.Add(new Transformish());
            }
            cardRows.Add(newRow);
        }
        Debug.Log($"Initialized a {rows}x{cols} grid.");
    }
}