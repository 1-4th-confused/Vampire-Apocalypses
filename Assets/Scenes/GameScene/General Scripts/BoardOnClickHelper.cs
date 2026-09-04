using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class BoardOnClickHelper : ScriptableObject
{
    Board boardScript = Board.boardScript;

    public void PlayDefend((int x, int y) pos) {
        // Iterate backwards to safely remove during iteration
            for (int i = 0; i < boardScript.units.Count; i++)
            {
                if (boardScript.units[i].GetComponent<UnitBehavior>().position == pos)
                {
                    boardScript.units[i].GetComponent<UnitBehavior>().defendThisUnit(CardScript.playerHandScript.SelectedCard.GetComponent<CardScript>().cardType.defense);
                    GameObject cardToRemove = CardScript.playerHandScript.SelectedCard;
                    CardData cardData = cardToRemove.GetComponent<CardScript>().cardType;

                    CardScript.playerHandScript.currentCards.Remove(cardData);
                    CardScript.playerHandScript.currentCardObjs.Remove(cardToRemove);

                    Board.helper.RemoveCard(cardToRemove);
                    CardScript.playerHandScript.SelectedCard = null;

                    CardScript.playerHandScript.rehandTheHand();
                    boardScript.units[i].GetComponent<UnitBehavior>().SetHasActed(true);
                    break;
                }
            }
            boardScript.UpdatePieceInteractability();
    }
    
    public void PlayStandardAttack((int x, int y) pos, double multipler) {
        GameObject playedCard = CardScript.playerHandScript.SelectedCard;
        CardData cardData = playedCard.GetComponent<CardScript>().cardType;

        // search for the vampire and unit gameObjects 
        GameObject vampAttacked = null;
        GameObject unitAttacking = null;
        for (int i = boardScript.vampireUnits.Count - 1; i >= 0; i--)
            if (i < boardScript.vampireUnits.Count && boardScript.vampireUnits[i] != null &&
                boardScript.vampireUnits[i].GetComponent<UnitBehavior>().position == pos)
            {
                vampAttacked = boardScript.vampireUnits[i];
                break;
            }

        for(int i = 0; i < boardScript.units.Count; i++)
            if (boardScript.units[i].GetComponent<UnitBehavior>().position == boardScript.selectedUnitPosition) {
                unitAttacking = boardScript.units[i];
                break;
            }

        if (unitAttacking != null && vampAttacked != null)
            Board.helper.UnitAttack(unitAttacking, vampAttacked, cardData.damage * multipler); //TODO: add Damage Multiplyer for Ranged/mele
        else
            Debug.LogError("Either unitAttacking or vampAttacked is null. Cannot perform attack.");

        CardScript.playerHandScript.currentCards.Remove(cardData);
        CardScript.playerHandScript.currentCardObjs.Remove(playedCard);

        Board.helper.RemoveCard(playedCard);
        CardScript.playerHandScript.SelectedCard = null;

        CardScript.playerHandScript.rehandTheHand();
        // set the unit that performed the attack to have acted
        for (int i = 0; i < boardScript.units.Count; i++)
        {
            if (boardScript.selectedUnitPosition == boardScript.units[i].GetComponent<UnitBehavior>().position)
            {
                boardScript.units[i].GetComponent<UnitBehavior>().SetHasActed(true);
            }
        }
        boardScript.UpdatePieceInteractability();
    }

    public void SelectUnitPosition((int x, int y) pos) {
        boardScript.selectedUnitPosition = pos;
        boardScript.spawnedPieces[boardScript.selectedUnitPosition.x, boardScript.selectedUnitPosition.y].GetComponent<BoardButtonsScript>().setSelected(1);
        boardScript.UpdatePieceInteractability();
    }
}
