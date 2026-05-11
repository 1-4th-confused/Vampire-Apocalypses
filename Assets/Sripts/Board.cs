using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Board : MonoBehaviour
{   
    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> vampireUnits = new List<GameObject>();
    public GameObject unitPrefab;
    void Start()
    {
        CardManager.ReadUnitsJSON();
        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform));
        units[0].GetComponent<UnitBehavior>().unitdata = ((CardData) CardManager.unitTypes[0]);
    [SerializeField] private List<Row> cardRows = new List<Row>();
    [SerializeField] private Transform bottomLeftTransform;
    [SerializeField] private GameObject place;

    private GameObject[,] spawnedPieces; 

    void Start()
    {
        if (bottomLeftTransform == null) return;

        int rows = 7;
        int cols = 5;
        spawnedPieces = new GameObject[rows, cols];
        InitializeGrid(rows, cols);

        // Get the Board's parent (The Canvas)
        Transform commonParent = transform.parent;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                
                float x = bottomLeftTransform.position.x + ((i - 3) * 0.32f);
                float y = 0.0f;//bottomLeftTransform.position.y + 0.5f;
                float z = bottomLeftTransform.position.z + ((j - 2) * 0.32f);

                Vector3 newPos = new Vector3(x, y, z);
                Vector3 newRot = new Vector3(90, 0, 0);
                
                // Instantiate as a sibling (sharing the same parent as Board)
                GameObject currentObject = Instantiate(place, newPos, Quaternion.Euler(newRot), commonParent);
                
                currentObject.layer = LayerMask.NameToLayer("units");
                currentObject.name = $"Piece_{i}_{j}";

                spawnedPieces[i, j] = currentObject;
            }
        }
        SetPieceInteractable(2,3,false);
    }

    void InitializeGrid(int rows, int cols)
    {
        cardRows.Clear();
        for (int i = 0; i < rows; i++)
        {
            Row newRow = new Row();
            for (int j = 0; j < cols; j++)
            {
                newRow.columns.Add(new Transformish());
            }
            cardRows.Add(newRow);
        }
    }

    public void SetPieceInteractable(int x, int y, bool state){
        GameObject piece = spawnedPieces[x, y];
        if (piece != null)
        {
            Button btn = piece.GetComponent<Button>();
            if (btn != null) btn.interactable = state;
        }
    }
}