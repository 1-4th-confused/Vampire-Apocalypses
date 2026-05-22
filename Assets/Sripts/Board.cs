using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Http.Headers;
using System.Collections;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;

/// <summary>
/// Manages the game board, including unit placement, tile interaction, and deck management.
/// </summary>
public class Board : MonoBehaviour
{
    /// <summary>
    /// List of player units on the board.
    /// </summary>
    public List<GameObject> units = new List<GameObject>();

    [SerializeField]
    public int score2 = 0;


    /// <summary>
    /// List of vampire units on the board.
    /// </summary>
    public List<GameObject> vampireUnits = new List<GameObject>();

    /// <summary>
    /// Prefab for instantiating units.
    /// </summary>
    public GameObject unitPrefab;

    /// <summary>
    /// Common parent transform for board elements.
    /// </summary>
    public Transform commonParent;

    /// <summary>
    /// Parent transform for units.
    /// </summary>
    public Transform unitsParrent;

    /// <summary>
    /// Static reference to the board script instance.
    /// </summary>
    public static Board boardScript;
    public static string cardSpecification = null;

    /// <summary>
    /// List of deck card game objects.
    /// </summary>
    public List<GameObject> deckObjs;

    /// <summary>
    /// List of card data for the deck.
    /// </summary>
    public List<CardData> deckData;

    /// <summary>
    /// List of remaining card data in the deck.
    /// </summary>
    public List<CardData> deckDataRemaining;

    /// <summary>
    /// Prefab for deck cards.
    /// </summary>
    public GameObject deckCardPrefab;

    /// <summary>
    /// Parent transform for deck cards.
    /// </summary>
    public GameObject deckCardParent;

    /// <summary>
    /// Bottom-left transform for tile positioning.
    /// </summary>
    [SerializeField] private Transform bottomLeftTransform;

    /// <summary>
    /// Prefab for board tiles.
    /// </summary>
    [SerializeField] private GameObject place;

    /// <summary>
    /// 2D array of spawned tile pieces.
    /// </summary>
    private GameObject[,] spawnedPieces;

    /// <summary>
    /// Position of the currently selected unit.
    /// </summary>
    public (int x, int y) selectedUnitPostion = (-1, -1);

    public Animator hourglassAnimator;
    public int turnNumber;
    public Text textTurnNumber;
    public (int x, int y) hoveredTile = (-1, -1);
    public GameObject endPanelObject;
    public GameObject endPanelCanvas;
    private bool createdEndPanel = false;
    public int score = 0;
    public float timer = -1f;

    /// <summary>
    /// Initializes the board, units, tiles, and deck.
    /// </summary>
    void Start()
    {
        boardScript = this;

        CardManager.ReadUnitsJSON();
        // Temporary unit initialization
        // for (int i = 0; i < CardManager.unitTypes.Count; i++)
        // {
        //     Debug.Log("unit:" + ((CardData)CardManager.unitTypes[i]).name + " : " + i);
        // }
        // for (int i = 0; i < CardManager.cardTypes.Count; i++)
        // {
        //     Debug.Log("card:" + ((CardData)CardManager.cardTypes[i]).name + " : " + i);
        // }

        //pidgion : 4
        //soldier : 7
        //gladiator : 2

        //majician : 3
        //vampire : 8




        vampireUnits.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        vampireUnits[0].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[3]);
        vampireUnits[0].GetComponent<UnitBehavior>().position = (2, 4);

        vampireUnits.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        vampireUnits[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[8]);
        vampireUnits[1].GetComponent<UnitBehavior>().position = (4, 4);

        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units[0].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[4]);
        units[0].GetComponent<UnitBehavior>().position = (2, 0);
        units[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[7]);
        units[1].GetComponent<UnitBehavior>().position = (3, 0);

        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units[2].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[2]);
        units[2].GetComponent<UnitBehavior>().position = (4, 0);

        CreateTiles();

        // Initialize deck data
        deckData = new List<CardData>();
        deckData.Add((CardData)CardManager.cardTypes[3]);//defend
        deckData.Add((CardData)CardManager.cardTypes[3]);//defend
        deckData.Add((CardData)CardManager.cardTypes[3]);//defend
        deckData.Add((CardData)CardManager.cardTypes[1]);//assassin
        deckData.Add((CardData)CardManager.cardTypes[1]);//assassin
        deckData.Add((CardData)CardManager.cardTypes[1]);//assassin
        deckData.Add((CardData)CardManager.cardTypes[1]);//assassin
        deckData.Add((CardData)CardManager.cardTypes[1]);//assassin
        deckData.Add((CardData)CardManager.cardTypes[1]);//assassin
        deckData.Add((CardData)CardManager.cardTypes[0]);//archer
        deckData.Add((CardData)CardManager.cardTypes[0]);//archer
        deckData.Add((CardData)CardManager.cardTypes[0]);//archer
        deckData.Add((CardData)CardManager.cardTypes[0]);//archer

        deckDataRemaining = new List<CardData>();
        foreach (CardData data in deckData)
            deckDataRemaining.Add(data);
        CreateDeck();

        textTurnNumber.text = "" + turnNumber;

        UpdatePieceInteractability();
    }

    /// <summary>
    /// Destroys existing deck objects and recreates the deck.
    /// </summary>
    void CreateDeck()
    {
        for (int i = 0; i < deckObjs.Count; i++)
        {
            Destroy(deckObjs[i]);
        }
        StartCoroutine(CreateDeckInSequence());
    }
    void SpawnEnemy(CardData unitType, (int x, int y) pos)
    {
        GameObject tempUnit = Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent);

        units.Add(tempUnit);
        units[2].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[0]);
        units[2].GetComponent<UnitBehavior>().position = (1, 3);
    }

    /// <summary>
    /// Coroutine to instantiate deck cards sequentially with delays.
    /// </summary>
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

    /// <summary>
    /// Creates the 7x5 grid of board tiles.
    /// </summary>
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
                float y = 0.0f;
                float z = bottomLeftTransform.position.z + ((j - 2) * 0.32f);

                Vector3 newPos = new Vector3(x, y, z);
                Vector3 newRot = new Vector3(90, 0, 0);

                // Instantiate tile as child of commonParent
                GameObject currentObject = Instantiate(place, newPos, Quaternion.Euler(newRot), commonParent);

                currentObject.GetComponent<BoardButtonsScript>().position = (i, j);

                currentObject.layer = LayerMask.NameToLayer("units");
                currentObject.name = $"Piece_{i}_{j}";

                spawnedPieces[i, j] = currentObject;
            }
        }
        SetPieceInteractable((2, 3), false);
    }

    /// <summary>
    /// Updates the interactability of board tiles based on unit selection and movement.
    /// </summary>
    public void UpdatePieceInteractability()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (selectedUnitPostion == units[i].GetComponent<UnitBehavior>().position && units[i].GetComponent<UnitBehavior>().hasActed)
            {
                selectedUnitPostion = (-1, -1);
                break;
            }
        }

        string cardName;
        if (CardScript.playerHandScript.SelectedCard != null)
        {
            cardName = CardScript.playerHandScript.SelectedCard.GetComponent<CardScript>().cardType.name;
            cardSpecification = CardScript.playerHandScript.SelectedCard.GetComponent<CardScript>().cardType.range;
        }
        else
        {
            cardName = null;
            cardSpecification = null;
        }

        if (selectedUnitPostion == (-1, -1) && CardScript.playerHandScript.SelectedCard == null)
        {
            clearInterabilityMatrix();
            for (int i = 0; i < units.Count; i++)
            {
                if (!units[i].GetComponent<UnitBehavior>().hasActed)
                    SetPieceInteractable(units[i].GetComponent<UnitBehavior>().position, true);
            }
        }
        else if (CardScript.playerHandScript.SelectedCard == null)
        {
            clearInterabilityMatrix();
            for (int i = 0; i < spawnedPieces.GetLength(0); i++)
            {
                for (int j = 0; j < spawnedPieces.GetLength(1); j++)
                {
                    SetPieceInteractable((i, j), false);

                }
            }
            spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(1);
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
        else if (selectedUnitPostion == (-1, -1) && CardScript.playerHandScript.SelectedCard != null)
        {
            clearInterabilityMatrix();
            for (int i = 0; i < units.Count; i++)
            {
                SetPieceInteractable(units[i].GetComponent<UnitBehavior>().position, true);
            }
        }
        else if (cardName == "bloodBolt")
        {
            clearInterabilityMatrix();
            SelectBloodBoltForCardApplication();
        }
        else if (cardSpecification == "melee")
        {
            clearInterabilityMatrix();
            SelectMeleeUnitForCardApplication();
        }
        else if (cardSpecification == "ranged")
        {
            clearInterabilityMatrix();
            SelectRangeUnitForCardApplication();
        }
        else
        {
            clearInterabilityMatrix();
            spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(1);
            SelectSelfUnitForCardApplication();
        }

        for (int i = 0; i < vampireUnits.Count; i++)
        {
            for (int j = 0; j < vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions.Length; j++)
                if (
                    vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions[j].x >= 0 &&
                    vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions[j].x < 7 &&
                    vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions[j].y >= 0 &&
                    vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions[j].y < 5
                )
                    spawnedPieces[
                        vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions[j].x,
                        vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions[j].y
                    ].GetComponent<BoardButtonsScript>().setSelected(4);
        }
    }

    /// <summary>
    /// Checks if a board position is occupied by any unit.
    /// </summary>
    /// <param name="pos">The position to check.</param>
    /// <returns>True if occupied, false otherwise.</returns>
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

    /// <summary>
    /// Sets the interactability of a board tile.
    /// </summary>
    /// <param name="pos">The tile position.</param>
    /// <param name="state">True to enable, false to disable.</param>
    public void SetPieceInteractable((int x, int y) pos, bool state)
    {
        if (
            pos.x >= 0 && pos.x < spawnedPieces.GetLength(0) &&
            pos.y >= 0 && pos.y < spawnedPieces.GetLength(1))
        {
            GameObject piece = spawnedPieces[pos.x, pos.y];
            if (piece != null)
            {
                Button btn = piece.transform.GetChild(0).gameObject.GetComponent<Button>();
                if (btn != null) btn.interactable = state;
            }
        }
    }

    /// <summary>
    /// Handles tile click events for unit selection and movement.
    /// </summary>
    /// <param name="pos">The clicked tile position.</param>
    public void ClickTile((int x, int y) pos)
    {
        if (CardScript.playerHandScript.SelectedCard != null && CardScript.playerHandScript.SelectedCard.GetComponent<CardScript>().cardType.name == "defend")
        {
            // Iterate backwards to safely remove during iteration
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].GetComponent<UnitBehavior>().position == pos)
                {
                    units[i].GetComponent<UnitBehavior>().defendThisUnit(CardScript.playerHandScript.SelectedCard.GetComponent<CardScript>().cardType.defense);
                    GameObject cardToRemove = CardScript.playerHandScript.SelectedCard;
                    CardData cardData = cardToRemove.GetComponent<CardScript>().cardType;

                    CardScript.playerHandScript.currentCards.Remove(cardData);
                    CardScript.playerHandScript.currentCardObjs.Remove(cardToRemove);

                    Destroy(cardToRemove, 0.5f);
                    CardScript.playerHandScript.SelectedCard = null;

                    CardScript.playerHandScript.rehandTheHand();
                    units[i].GetComponent<UnitBehavior>().SetHasActed(true);
                    break;
                }
            }
            UpdatePieceInteractability();
        }
        else if (selectedUnitPostion == pos)
        {
            clearInterabilityMatrix();
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(0);
            selectedUnitPostion = (-1, -1);
            UpdatePieceInteractability();
        }
        else if (CardScript.playerHandScript.SelectedCard != null)
        {
            if (selectedUnitPostion != (-1, -1))
            {
                spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(2);
                var cardToRemove = CardScript.playerHandScript.SelectedCard;
                var cardData = cardToRemove.GetComponent<CardScript>().cardType;
                var damage = cardData.damage;

                // Iterate backwards to safely remove during iteration
                for (int i = vampireUnits.Count - 1; i >= 0; i--)
                {
                    if (i < vampireUnits.Count && vampireUnits[i] != null &&
                        vampireUnits[i].GetComponent<UnitBehavior>().position == pos)
                    {
                        if(vampireUnits[i].GetComponent<UnitBehavior>().damageThisUnit(damage)){
                            score2 += 1;
                        }
                        break;
                    }
                }

                CardScript.playerHandScript.currentCards.Remove(cardData);
                CardScript.playerHandScript.currentCardObjs.Remove(cardToRemove);

                Destroy(cardToRemove, 0.5f);
                CardScript.playerHandScript.SelectedCard = null;

                CardScript.playerHandScript.rehandTheHand();
                for (int i = 0; i < units.Count; i++)
                {
                    if (selectedUnitPostion == units[i].GetComponent<UnitBehavior>().position)
                    {
                        units[i].GetComponent<UnitBehavior>().SetHasActed(true);
                    }
                }
                UpdatePieceInteractability();
            }
            else
            {
                spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(0);
                selectedUnitPostion = pos;
                spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(1);
                UpdatePieceInteractability();
            }
        }
        else if (selectedUnitPostion == (-1, -1))
        {
            // Select unit
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(0);
            selectedUnitPostion = pos;
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(1);
            UpdatePieceInteractability();
        }
        else
        {
            // Move selected unit to new position
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].GetComponent<UnitBehavior>().position == selectedUnitPostion)
                {
                    units[i].GetComponent<UnitBehavior>().movePosition(pos);
                    units[i].GetComponent<UnitBehavior>().SetHasActed(true);
                    break;
                }
            }
            spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(0);
            selectedUnitPostion = (-1, -1);
            UpdatePieceInteractability();
        }
    }

    /// <summary>
    /// Handles deck click to draw a card.
    /// </summary>
    // ...existing code...
    public void ClickDeck()
    {
        Debug.Log("deckDataRemaining"+deckDataRemaining.Count);
        if (CardScript.playerHandScript.currentCards.Count < 5)
        {
            deckObjs[deckObjs.Count - 1].transform.GetChild(0).GetComponent<TableCardScript>().RemoveCard();
            int randomIntToDraw = Random.Range(0, deckDataRemaining.Count);
            CardData drawnCard = deckDataRemaining[randomIntToDraw];
            deckDataRemaining.Remove(deckDataRemaining[randomIntToDraw]);
            deckObjs.Remove(deckObjs[deckObjs.Count - 1]);
            if (deckObjs.Count > 0)
            {
                StartCoroutine(WaitToActivate(deckObjs[deckObjs.Count - 1].transform.GetChild(0).GetComponent<TableCardScript>(), drawnCard));
            }
        }
        hourglassAnimator.SetTrigger("spin");
        turnNumber++;
        BeginTurn();
        textTurnNumber.text = "" + turnNumber;
    }
    public void ClickDeckRecursive()
    {
        if (CardScript.playerHandScript.currentCards.Count < 5)
        {
            deckObjs[deckObjs.Count - 1].transform.GetChild(0).GetComponent<TableCardScript>().RemoveCard();
            int randomIntToDraw = Random.Range(0, deckDataRemaining.Count);
            CardData drawnCard = deckDataRemaining[randomIntToDraw];
            deckDataRemaining.Remove(deckDataRemaining[randomIntToDraw]);
            deckObjs.Remove(deckObjs[deckObjs.Count - 1]);
            if (deckObjs.Count > 0)
            {
                StartCoroutine(WaitToActivate(deckObjs[deckObjs.Count - 1].transform.GetChild(0).GetComponent<TableCardScript>(), drawnCard));
            }
        }
    }

    IEnumerator WaitToActivate(TableCardScript script, CardData data)
    {
        yield return new WaitForSeconds(0.2f);

        try
        {
            CardScript.playerHandScript.AddCardToHand(data);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"AddCardToHand failed: {ex.Message}\n{ex.StackTrace}");
        }

        yield return new WaitForSeconds(0.2f);

        if (script != null && script.gameObject != null)
            script.Activate();

        if (CardScript.playerHandScript.currentCards.Count < 5)
        {
            ClickDeckRecursive();
        }
    }
    public void BeginTurn()
    {
        IterateVampAI();
        for (int i = 0; i < vampireUnits.Count; i++)
        {
            vampireUnits[i].GetComponent<UnitBehavior>().hasActed = false;
        }
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<UnitBehavior>().SetHasActed(false);
        }

    }
    public void Update()
    {
        if (timer == -1f)
        {
            timer = Time.time;
        }
        Debug.Log(timer + 5 < Time.time);
        Debug.Log("deckDataRemaining"+deckDataRemaining.Count);
        if (deckDataRemaining.Count == 0 && CardScript.playerHandScript.currentCards != null && timer + 5 < Time.time)
        {
            deckDataRemaining = new List<CardData>();
            foreach (CardData data in deckData)
            {
                // bool found = false;
                // for (int i = 0; i < CardScript.playerHandScript.currentCards.Count; i++)
                // {
                //     if (data == CardScript.playerHandScript.currentCards[i])
                //     {
                //         found = true;
                //         break;
                //     }
                // }
                // if (found == false)
                // {
                    deckDataRemaining.Add(data);
                // }
                
            }
            timer = Time.time;
            CreateDeck();
        }
    }
    public void IterateVampAI()
    {
        for (int i = 0; i < vampireUnits.Count; i++)
        {
            if (!vampireUnits[i].GetComponent<UnitBehavior>().hasActed && vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions.Length == 0)
            {
                //looking for units to select
                for (int j = 0; j < units.Count; j++)
                {
                    if (Mathf.Abs(units[j].GetComponent<UnitBehavior>().position.x - vampireUnits[i].GetComponent<UnitBehavior>().position.x) <= 1 && Mathf.Abs(units[j].GetComponent<UnitBehavior>().position.y - vampireUnits[i].GetComponent<UnitBehavior>().position.y) <= 1)
                    {
                        (int x, int y)[] temp = {
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x + 1, vampireUnits[i].GetComponent<UnitBehavior>().position.y + 1),
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x + 1, vampireUnits[i].GetComponent<UnitBehavior>().position.y),
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x + 1, vampireUnits[i].GetComponent<UnitBehavior>().position.y - 1),
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x, vampireUnits[i].GetComponent<UnitBehavior>().position.y + 1),
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x, vampireUnits[i].GetComponent<UnitBehavior>().position.y - 1),
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x - 1, vampireUnits[i].GetComponent<UnitBehavior>().position.y + 1),
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x - 1, vampireUnits[i].GetComponent<UnitBehavior>().position.y),
                            (vampireUnits[i].GetComponent<UnitBehavior>().position.x - 1, vampireUnits[i].GetComponent<UnitBehavior>().position.y - 1)
                        };
                        vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions = temp;
                        vampireUnits[i].GetComponent<UnitBehavior>().quedDamage = 5;
                        vampireUnits[i].GetComponent<UnitBehavior>().hasActed = true;
                    }
                }
            }
            else if (!vampireUnits[i].GetComponent<UnitBehavior>().hasActed)
            {
                for (int j = 0; j < vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions.Length; j++)
                {
                    for (int h = 0; h < units.Count; h++)
                    {
                        if (units[h].GetComponent<UnitBehavior>().position == vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions[j])
                        {
                            units[h].GetComponent<UnitBehavior>().damageThisUnit(vampireUnits[i].GetComponent<UnitBehavior>().quedDamage);
                            vampireUnits[i].GetComponent<UnitBehavior>().quedDamage = 0;

                        }
                    }
                }
                vampireUnits[i].GetComponent<UnitBehavior>().selectedPositions = new (int x, int y)[0];
                vampireUnits[i].GetComponent<UnitBehavior>().hasActed = true;
            }
            if (!vampireUnits[i].GetComponent<UnitBehavior>().hasActed)
            {
                GameObject closestUnit = null;
                for (int j = 0; j < units.Count; j++)
                {
                    if (
                        closestUnit == null
                        ||
                        (
                            Mathf.Abs(units[j].GetComponent<UnitBehavior>().position.x - vampireUnits[i].GetComponent<UnitBehavior>().position.x)
                            + Mathf.Abs(units[j].GetComponent<UnitBehavior>().position.y - vampireUnits[i].GetComponent<UnitBehavior>().position.y)
                            <
                            Mathf.Abs(closestUnit.GetComponent<UnitBehavior>().position.x - vampireUnits[i].GetComponent<UnitBehavior>().position.x)
                            + Mathf.Abs(closestUnit.GetComponent<UnitBehavior>().position.y - vampireUnits[i].GetComponent<UnitBehavior>().position.y)
                        )
                        )
                    {
                        closestUnit = units[j];
                    }
                }
                if (closestUnit != null)
                {
                    if (
                        Mathf.Abs(closestUnit.GetComponent<UnitBehavior>().position.y - vampireUnits[i].GetComponent<UnitBehavior>().position.y)
                        > Mathf.Abs(closestUnit.GetComponent<UnitBehavior>().position.x - vampireUnits[i].GetComponent<UnitBehavior>().position.x)
                    )
                    {
                        if (closestUnit.GetComponent<UnitBehavior>().position.y > vampireUnits[i].GetComponent<UnitBehavior>().position.y)
                        {
                            vampireUnits[i].GetComponent<UnitBehavior>().movePosition(
                                (
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.x,
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.y + 1
                                )
                            );
                        }
                        else
                        {
                            vampireUnits[i].GetComponent<UnitBehavior>().movePosition(
                                (
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.x,
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.y - 1
                                )
                            );
                        }
                    }
                    else
                    {
                        if (closestUnit.GetComponent<UnitBehavior>().position.x > vampireUnits[i].GetComponent<UnitBehavior>().position.x)
                        {
                            vampireUnits[i].GetComponent<UnitBehavior>().movePosition(
                                (
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.x + 1,
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.y
                                )
                            );
                        }
                        else
                        {
                            vampireUnits[i].GetComponent<UnitBehavior>().movePosition(
                                (
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.x - 1,
                                    vampireUnits[i].GetComponent<UnitBehavior>().position.y
                                )
                            );
                        }
                    }
                }
            }
        }

        UpdatePieceInteractability();
    }

    public void clearInterabilityMatrix()
    {
        for (int i = 0; i < spawnedPieces.GetLength(0); i++)
        { //7
            for (int j = 0; j < spawnedPieces.GetLength(1); j++)
            { //5
                spawnedPieces[i, j].GetComponent<BoardButtonsScript>().setSelected(0);
                spawnedPieces[i, j].GetComponent<BoardButtonsScript>().isAttackPosibility = false;
                SetPieceInteractable((i, j), false);
            }
        }
    }

    public bool whosThatVampire((int x, int y) pos)
    {
        for (int i = 0; i < vampireUnits.Count; i++)
        {
            if (vampireUnits[i].GetComponent<UnitBehavior>().position == pos)
            {
                return true;
            }
        }
        return false;
    }

    public void SelectMeleeUnitForCardApplication()
    {
        if (selectedUnitPostion != (-1, -1))
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        if (whosThatVampire((selectedUnitPostion.x + i, selectedUnitPostion.y + j)))
                        {
                            spawnedPieces[selectedUnitPostion.x + i, selectedUnitPostion.y + j].GetComponent<BoardButtonsScript>().setSelected(3);
                            SetPieceInteractable((selectedUnitPostion.x + i, selectedUnitPostion.y + j), true);
                        }
                        else
                        {
                            spawnedPieces[selectedUnitPostion.x + i, selectedUnitPostion.y + j].GetComponent<BoardButtonsScript>().setSelected(0);
                        }
                    }
                    else
                    {
                        spawnedPieces[selectedUnitPostion.x + i, selectedUnitPostion.y + j].GetComponent<BoardButtonsScript>().setSelected(1);
                    }
                }
            }
        }
        if (CardScript.playerHandScript.SelectedCard == null)
        {
            clearInterabilityMatrix();
        }
    }

    public void SelectBloodBoltForCardApplication()
    {
        for (int i = 0; i < 5; i++)
        {
            SetPieceInteractable((selectedUnitPostion.x, i), true);
            if (!spawnedPieces[selectedUnitPostion.x, i].GetComponent<BoardButtonsScript>().isSelected)
            {
                spawnedPieces[selectedUnitPostion.x, i].GetComponent<BoardButtonsScript>().isAttackPosibility = true;
            }
            if ((i == hoveredTile.y) && (selectedUnitPostion.x == hoveredTile.x) && (hoveredTile != selectedUnitPostion))
            {
                if (i > selectedUnitPostion.y)
                    for (int j = selectedUnitPostion.y; j < 5; j++)
                    {
                        spawnedPieces[selectedUnitPostion.x, j].GetComponent<BoardButtonsScript>().setSelected(2);
                    }
                else
                    for (int j = 0; j < selectedUnitPostion.y; j++)
                    {
                        spawnedPieces[selectedUnitPostion.x, j].GetComponent<BoardButtonsScript>().setSelected(2);
                    }
            }
        }
        for (int i = 0; i < 7; i++)
        {
            SetPieceInteractable((i, selectedUnitPostion.y), true);
            if (!spawnedPieces[i, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().isSelected)
            {
                spawnedPieces[i, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().isAttackPosibility = true;
            }
            if ((i == hoveredTile.x) && (selectedUnitPostion.y == hoveredTile.y) && (hoveredTile != selectedUnitPostion))
            {
                if (i > selectedUnitPostion.x)
                    for (int j = selectedUnitPostion.x; j < 7; j++)
                    {
                        spawnedPieces[j, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(2);
                    }
                else
                    for (int j = 0; j < selectedUnitPostion.x; j++)
                    {
                        spawnedPieces[j, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(2);
                    }
            }
        }
        SetPieceInteractable((selectedUnitPostion.x, selectedUnitPostion.y), true);
        spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(1);

    }
    public void SelectRangeUnitForCardApplication()
    {
        for (int i = 0; i < vampireUnits.Count; i++)
        {
            if (vampireUnits[i].GetComponent<UnitBehavior>().position != selectedUnitPostion)
            {
                spawnedPieces[vampireUnits[i].GetComponent<UnitBehavior>().position.x, vampireUnits[i].GetComponent<UnitBehavior>().position.y].GetComponent<BoardButtonsScript>().setSelected(3);
                SetPieceInteractable(vampireUnits[i].GetComponent<UnitBehavior>().position, true);
            }
        }
        if (selectedUnitPostion != (-1, -1))
        {
            spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(1);
        }
        if (CardScript.playerHandScript.SelectedCard == null)
        {
            clearInterabilityMatrix();
        }
    }

    public void SelectSelfUnitForCardApplication()
    {
        if (selectedUnitPostion != (-1, -1))
        {
            spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(3);
            SetPieceInteractable((selectedUnitPostion.x, selectedUnitPostion.y), true);
        }
        if (CardScript.playerHandScript.SelectedCard == null)
        {
            clearInterabilityMatrix();
        }
    }

    public void removeUnit(GameObject unit)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == unit)
            {
                Destroy(units[i], 0.5f);
                units.RemoveAt(i);
                return;
            }
        }
        for (int i = 0; i < vampireUnits.Count; i++)
        {
            score++;
            if (vampireUnits[i] == unit)
            {
                Destroy(vampireUnits[i], 0.5f);
                vampireUnits.RemoveAt(i);

                if (!IsSpaceOccupied((3, 3)))//breaks is a vamp is removed
                {
                    vampireUnits.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
                    if (Random.Range(0, 1) > 0.5f)
                    {
                        vampireUnits[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[3]);
                    }
                    else
                    {
                        vampireUnits[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[8]);
                    }
                    vampireUnits[1].GetComponent<UnitBehavior>().position = (3, 4);
                }
                else
                {
                    vampireUnits.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
                    if (Random.Range(0, 1) > 0.5f)
                    {
                        vampireUnits[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[3]);
                    }
                    else
                    {
                        vampireUnits[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[8]);
                    }
                    vampireUnits[1].GetComponent<UnitBehavior>().position = (4, 4);
                }

                return;
            }
        }
    }

    public void HoveringTileAttack((int x, int y) pos)
    {
        hoveredTile = pos;
        UpdatePieceInteractability();
    }
    public void UnHoveringTileAttack((int x, int y) pos)
    {
        hoveredTile = (-1, -1);
        UpdatePieceInteractability();
    }

    public void CheckIfUnitsAllDead()
    {
        if (units.Count == 0 && !createdEndPanel)
        {
            createdEndPanel = true;
            Instantiate(endPanelObject, endPanelCanvas.transform);
        }
    }
}