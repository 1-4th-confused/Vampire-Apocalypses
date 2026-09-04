using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class BoardHelper : ScriptableObject
{
    Board boardScript = Board.boardScript;
    public void RemoveCard(GameObject cardToRemove) {
        CardScript.playerHandScript.currentCards.Remove(cardToRemove.GetComponent<CardScript>().cardType);
        CardScript.playerHandScript.currentCardObjs.Remove(cardToRemove);

        cardToRemove.GetComponent<CardScript>().cardAnimator.SetBool("removed", true);
        Destroy(cardToRemove, 0.5f);

        CardScript.playerHandScript.rehandTheHand();
    }

    public void UnitAttack(GameObject attacker, GameObject target, double damage, bool ranged) {
        // Iterate backwards to safely remove during iteration
        UnitBehavior attackerBehavior = attacker.GetComponent<UnitBehavior>();       
        UnitBehavior targetBehavior = target.GetComponent<UnitBehavior>();

        if (attackerBehavior.position.x>targetBehavior.position.x){
            attackerBehavior.unitAnimator.SetBool("facingRight", false);
            targetBehavior.unitAnimator.SetBool("facingRight", true);
        }
        else if (attackerBehavior.position.x<targetBehavior.position.x){
            attackerBehavior.unitAnimator.SetBool("facingRight", true);
            targetBehavior.unitAnimator.SetBool("facingRight", false);
        }
        if (ranged) {
            attackerBehavior.unitAnimator.SetTrigger("rangedAttack");
            playDamageAnimation(target, 33f/60f, damage);
        }
        else {
            attackerBehavior.unitAnimator.SetTrigger("attack");
            playDamageAnimation(target, 8f/60f, damage);
        }
    }
    public void MoveUnit(GameObject unitToMove, (int x, int y) pos) {
        if (unitToMove.GetComponent<UnitBehavior>().position.x>pos.x)
            unitToMove.GetComponent<UnitBehavior>().unitAnimator.SetBool("facingRight", false);
        else if (unitToMove.GetComponent<UnitBehavior>().position.x<pos.x)
            unitToMove.GetComponent<UnitBehavior>().unitAnimator.SetBool("facingRight", true);
        unitToMove.GetComponent<UnitBehavior>().movePosition(pos);
        unitToMove.GetComponent<UnitBehavior>().SetHasActed(true);

        // if it was moved by the player selected unit should be deselected
        if (!unitToMove.GetComponent<UnitBehavior>().isVampire) {
            DeselectUnit();
        }
    }
    public void DeselectUnit() {
        boardScript.clearInteractabilityMatrix();
        boardScript.spawnedPieces[boardScript.selectedUnitPosition.x, boardScript.selectedUnitPosition.y].GetComponent<BoardButtonsScript>().setSelected(0);
        boardScript.selectedUnitPosition = (-1, -1);
        boardScript.UpdatePieceInteractability();
    }

    public void playDamageAnimation(GameObject target, float delay, double damage) { // animations play at 60fps
        boardScript.playDamageAnimation(target, delay, damage); // coroutine in board.cs because scriptable objects cannot use coroutines
    }
    public GameObject UnitAt((int x, int y) pos) {
        for(int i = 0; i < boardScript.units.Count; i++)
        {
            if (boardScript.units[i].GetComponent<UnitBehavior>().position == pos)
            {
                return boardScript.units[i];
            }
        }
        Debug.LogError("No unit found at position: " + pos);
        return null;
    }
}
