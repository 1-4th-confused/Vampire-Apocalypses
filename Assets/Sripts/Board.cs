using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Net.Http.Headers;
using System.Collections;
using System.IO;

/// <summary>
/// Manages the game board, including unit placement, tile interaction, and deck management.
/// </summary>
public class Board : MonoBehaviour
{
    /// <summary>
    /// List of player units on the board.
    /// </summary>
    public List<GameObject> units = new List<GameObject>();

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

    /// <summary>
    /// Initializes the board, units, tiles, and deck.
    /// </summary>
    void Start()
    {
        boardScript = this;

        CardManager.ReadUnitsJSON();
        // Temporary unit initialization
        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units[0].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[0]);
        units[0].GetComponent<UnitBehavior>().position = (3, 3);
        units[1].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[0]);
        units[1].GetComponent<UnitBehavior>().position = (1, 1);

        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, unitsParrent));
        units[2].GetComponent<UnitBehavior>().unitdata = ((CardData)CardManager.unitTypes[0]);
        units[2].GetComponent<UnitBehavior>().position = (1,3);

        CreateTiles();

        // Initialize deck data
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
    public void UpdatePeiceInteractability()
    {
        if(CardScript.playerHandScript.SelectedCard != null){
            cardSpecification = CardScript.playerHandScript.SelectedCard.GetComponent<CardScript>().cardType.range;
        }else{
            cardSpecification = null;
        }
        if (selectedUnitPostion == (-1, -1) && CardScript.playerHandScript.SelectedCard == null)
        {
            clearInterabilityMatrix();
            for (int i = 0; i < units.Count; i++)
            {
                SetPieceInteractable(units[i].GetComponent<UnitBehavior>().position, true);
            }
        }
        else if(CardScript.playerHandScript.SelectedCard == null){
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
        }else if (selectedUnitPostion == (-1,-1) && CardScript.playerHandScript.SelectedCard != null){
            clearInterabilityMatrix();
            for (int i = 0; i < units.Count; i++)
            {
                SetPieceInteractable(units[i].GetComponent<UnitBehavior>().position, true);
            }
        }else if(cardSpecification == "melee"){
            clearInterabilityMatrix();
            SelectMeleeUnitForCardApplication();
            Debug.Log("Melee?");
        }else if(cardSpecification == "ranged"){
            clearInterabilityMatrix();
            SelectRangeUnitForCardApplication();
            Debug.Log("Range?");
        }
        else{
            clearInterabilityMatrix();
            spawnedPieces[selectedUnitPostion.x,selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(1);
            SelectSelfUnitForCardApplication();
            Debug.Log("Self?");
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
        if (selectedUnitPostion == pos && CardScript.playerHandScript.SelectedCard == null)
        {
            clearInterabilityMatrix();
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(0);
            selectedUnitPostion = (-1, -1);
            UpdatePeiceInteractability();
        }
        else if(CardScript.playerHandScript.SelectedCard != null){
            if(selectedUnitPostion == pos)
            {
                SetPieceInteractable(pos,false);
            }
            UpdatePeiceInteractability();
        }
        else if (selectedUnitPostion == (-1, -1))
        {
            // Select unit
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(0);
            selectedUnitPostion = pos;
            spawnedPieces[pos.x, pos.y].GetComponent<BoardButtonsScript>().setSelected(1);
            UpdatePeiceInteractability();
        }
        else
        {
            // Move selected unit to new position
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].GetComponent<UnitBehavior>().position == selectedUnitPostion)
                {
                    units[i].GetComponent<UnitBehavior>().movePosition(pos);
                    break;
                }
            }
            spawnedPieces[selectedUnitPostion.x, selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(0);
            selectedUnitPostion = (-1, -1);
            UpdatePeiceInteractability();
        }
    }

    /// <summary>
    /// Handles deck click to draw a card.
    /// </summary>
    public void ClickDeck()
    {   
        if(CardScript.playerHandScript.currentCards.Count < 5){
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
    }

    /// <summary>
    /// Coroutine to wait before activating the next deck card and adding drawn card to hand.
    /// </summary>
    /// <param name="script">The table card script to activate.</param>
    /// <param name="data">The drawn card data.</param>
    IEnumerator WaitToActivate(TableCardScript script, CardData data)
    {
        yield return new WaitForSeconds(0.2f);
        CardScript.playerHandScript.AddCardToHand(data);
        yield return new WaitForSeconds(0.2f);
        script.Activate();
    }

    public void clearInterabilityMatrix(){
        for (int i = 0; i < spawnedPieces.GetLength(0); i++)
        { //7
            for (int j = 0; j < spawnedPieces.GetLength(1); j++)
            { //5
                spawnedPieces[i,j].GetComponent<BoardButtonsScript>().setSelected(0);
                SetPieceInteractable((i, j), false);
            }
        }
    }

    public void SelectMeleeUnitForCardApplication(){
        clearInterabilityMatrix();
        if(selectedUnitPostion != (-1,-1)){
            for(int i = -1; i <= 1; i++){
                for(int j = -1; j <= 1; j++){
                    if(i != 0 || j != 0){
                        if(IsSpaceOccupied((selectedUnitPostion.x+i,selectedUnitPostion.y+j))){
                            spawnedPieces[selectedUnitPostion.x+i,selectedUnitPostion.y+j].GetComponent<BoardButtonsScript>().setSelected(3);
                            SetPieceInteractable((selectedUnitPostion.x+i,selectedUnitPostion.y+j),true);
                        }else{
                        spawnedPieces[selectedUnitPostion.x+i,selectedUnitPostion.y+j].GetComponent<BoardButtonsScript>().setSelected(0);
                        }
                    }else{
                        spawnedPieces[selectedUnitPostion.x+i,selectedUnitPostion.y+j].GetComponent<BoardButtonsScript>().setSelected(1);
                    }
                }
            }
        }
        if(CardScript.playerHandScript.SelectedCard == null){
            clearInterabilityMatrix();
        }
    }
    public void SelectRangeUnitForCardApplication(){
        clearInterabilityMatrix();
        for(int i = 0; i < units.Count; i++){
            if(units[i].GetComponent<UnitBehavior>().position!= selectedUnitPostion){
                spawnedPieces[units[i].GetComponent<UnitBehavior>().position.x,units[i].GetComponent<UnitBehavior>().position.y].GetComponent<BoardButtonsScript>().setSelected(3);
                SetPieceInteractable(units[i].GetComponent<UnitBehavior>().position,true);
            }
        }
        if(selectedUnitPostion != (-1,-1)){
            spawnedPieces[selectedUnitPostion.x,selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(1);
        }
        if(CardScript.playerHandScript.SelectedCard == null){
            clearInterabilityMatrix();
        }
    }

    public void SelectSelfUnitForCardApplication(){
        clearInterabilityMatrix();
        if(selectedUnitPostion != (-1,-1)){
            spawnedPieces[selectedUnitPostion.x,selectedUnitPostion.y].GetComponent<BoardButtonsScript>().setSelected(3);
            SetPieceInteractable((selectedUnitPostion.x,selectedUnitPostion.y),true);
        }
        if(CardScript.playerHandScript.SelectedCard == null){
            clearInterabilityMatrix();
        }
    }
}