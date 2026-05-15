using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Http.Headers;
using System.Collections;

public class Board : MonoBehaviour
{
    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> vampireUnits = new List<GameObject>();
    public GameObject unitPrefab;
    public Transform commonParent;
    public Transform unitsParrent;
    public static Board boardScript;

    public List<GameObject> deckObjs;
    public List<CardData> deckData;
    public List<CardData> deckDataRemaining;
    public GameObject deckCardPrefab;
    public GameObject deckCardParent;


    [SerializeField] private Transform bottomLeftTransform;
    [SerializeField] private GameObject place;

    private GameObject[,] spawnedPieces;
    public (int x, int y) selectedUnitPostion = (-1, -1);
    void Start()
    {
        boardScript = this;


        CardManager.ReadUnitsJSON();
        //temporary
        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units[0].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[0]);
        units[0].GetComponent<UnitBehavior>().position = (3, 3);
        units[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[0]);
        units[1].GetComponent<UnitBehavior>().position = (1, 1);

        CreateTiles();

        deckData = new List<CardData>();
        deckData.Add((CardData)CardManager.cardTypes[0]);
        deckData.Add((CardData)CardManager.cardTypes[1]);
        deckData.Add((CardData)CardManager.cardTypes[0]);
        deckData.Add((CardData)CardManager.cardTypes[0]);
        deckData.Add((CardData)CardManager.cardTypes[1]);
        deckData.Add((CardData)CardManager.cardTypes[0]);
        deckData.Add((CardData)CardManager.cardTypes[0]);
        deckData.Add((CardData)CardManager.cardTypes[0]);
        deckData.Add((CardData)CardManager.cardTypes[1]);

        deckDataRemaining = new List<CardData>();
        foreach (CardData data in deckData)
            deckDataRemaining.Add(data);
        CreateDeck();

        UpdatePeiceInteractability();
    }

    void CreateDeck()
    {
        for (int i = 0; i < deckObjs.Count; i++)
        {
            Destroy(deckObjs[i]);
        }
        StartCoroutine(CreateDeckInSequence());

    }

    IEnumerator CreateDeckInSequence()
    {
        for (int i = 0; i < deckData.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject tempCard = Instantiate(deckCardPrefab, deckCardParent.transform.position + new Vector3(0, 0.01f * i, 0), deckCardParent.transform.rotation, deckCardParent.transform);
            if (i != deckData.Count - 1)
                tempCard.gameObject.transform.GetChild(0).GetComponent<TableCardScript>().Deactivate();
            else
            {
                tempCard.gameObject.transform.GetChild(0).GetComponent<TableCardScript>().Activate();
            }
            deckObjs.Add(tempCard);
        }
    }

    void CreateTiles()
    {
        if (bottomLeftTransform == null) return;

        int cols = 7;
        int rows = 5;
        spawnedPieces = new GameObject[cols, rows];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                float x = bottomLeftTransform.position.x + ((i - 3) * 0.32f);
                float y = 0.0f;//bottomLeftTransform.position.y + 0.5f;
                float z = bottomLeftTransform.position.z + ((j - 2) * 0.32f);

                Vector3 newPos = new Vector3(x, y, z);
                Vector3 newRot = new Vector3(90, 0, 0);

                // Instantiate as a sibling (sharing the same parent as Board)
                GameObject currentObject = Instantiate(place, newPos, Quaternion.Euler(newRot), commonParent);

                currentObject.GetComponent<BoardButtonsScript>().position = (i, j);

                currentObject.layer = LayerMask.NameToLayer("units");
                currentObject.name = $"Piece_{i}_{j}";

                spawnedPieces[i, j] = currentObject;
            }
        }
        SetPieceInteractable((2, 3), false);
    }

    public void UpdatePeiceInteractability()
    {
        if (selectedUnitPostion == (-1, -1))
        {
            for (int i = 0; i < spawnedPieces.GetLength(0); i++)
            { //7
                for (int j = 0; j < spawnedPieces.GetLength(1); j++)
                { //5
                    SetPieceInteractable((i, j), false);
                }
            }

            for (int i = 0; i < units.Count; i++)
            {
                SetPieceInteractable(units[i].GetComponent<UnitBehavior>().position, true);
            }
        }
        else
        {
            for (int i = 0; i < spawnedPieces.GetLength(0); i++)
            { //7
                for (int j = 0; j < spawnedPieces.GetLength(1); j++)
                { //5
                    SetPieceInteractable((i, j), false);
                }
            }
            SetPieceInteractable(selectedUnitPostion, true);
            if (!IsSpaceOccupied((selectedUnitPostion.x + 1, selectedUnitPostion.y)))
            {
                SetPieceInteractable((selectedUnitPostion.x + 1, selectedUnitPostion.y), true);
            }
            if (!IsSpaceOccupied((selectedUnitPostion.x, selectedUnitPostion.y + 1)))
            {
                SetPieceInteractable((selectedUnitPostion.x, selectedUnitPostion.y + 1), true);
            }
            if (!IsSpaceOccupied((selectedUnitPostion.x, selectedUnitPostion.y - 1)))
            {
                SetPieceInteractable((selectedUnitPostion.x, selectedUnitPostion.y - 1), true);
            }
            if (!IsSpaceOccupied((selectedUnitPostion.x - 1, selectedUnitPostion.y)))
            {
                SetPieceInteractable((selectedUnitPostion.x - 1, selectedUnitPostion.y), true);
            }

        }
    }

    public bool IsSpaceOccupied((int x, int y) pos)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].GetComponent<UnitBehavior>().position == pos)
            {
                return true;
            }
        }
        for (int i = 0; i < vampireUnits.Count; i++)
        {
            if (vampireUnits[i].GetComponent<UnitBehavior>().position == pos)
            {
                return true;
            }
        }
        return false;
    }
    public void SetPieceInteractable((int x, int y) pos, bool state)
    {
        if (
            pos.x >= 0 && pos.x < spawnedPieces.GetLength(0) &&
            pos.y >= 0 && pos.y < spawnedPieces.GetLength(1))
        {
            GameObject piece = spawnedPieces[pos.x, pos.y];
            if (piece != null)
            {
                Button btn = piece.GetComponent<Button>();
                if (btn != null) btn.interactable = state;
            }
        }
    }

    public void ClickTile((int x, int y) pos)
    {
        if (selectedUnitPostion == pos)
        {
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(false);
            selectedUnitPostion = (-1, -1);
            UpdatePeiceInteractability();
        }
        else if (selectedUnitPostion == (-1, -1))
        {
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(false);
            selectedUnitPostion = pos;
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(true);
            UpdatePeiceInteractability();
        }
        else
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].GetComponent<UnitBehavior>().position == selectedUnitPostion)
                {
                    units[i].GetComponent<UnitBehavior>().movePosition(pos);
                    break;
                }
            }
            spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(false);
            selectedUnitPostion = (-1, -1);
            UpdatePeiceInteractability();
        }
    }

    public void ClickDeck()
    {
        deckObjs[deckObjs.Count - 1].transform.GetChild(0).GetComponent<TableCardScript>().RemoveCard();
        int randomIntToDraw = Random.Range(0, deckDataRemaining.Count);
        CardData drawnCard = deckDataRemaining[randomIntToDraw];
        deckDataRemaining.Remove(deckDataRemaining[randomIntToDraw]);
        deckObjs.Remove(deckObjs[deckObjs.Count - 1]);
        if (deckObjs.Count > 0)
        {
            StartCoroutine(WaitToActivate(deckObjs[deckObjs.Count - 1].transform.GetChild(0).GetComponent<TableCardScript>(),drawnCard));
        }
    }

    IEnumerator WaitToActivate(TableCardScript script, CardData data)
    {
        yield return new WaitForSeconds(0.2f);
        CardScript.playerHandScript.AddCardToHand(data);
        yield return new WaitForSeconds(0.2f);
        script.Activate();
    }
}