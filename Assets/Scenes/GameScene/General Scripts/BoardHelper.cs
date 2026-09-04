using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class BoardHelper : ScriptableObject
{
    Board boardScript = Board.boardScript;
    public void RemoveCard(GameObject cardToRemove) {
        cardToRemove.GetComponent<CardScript>().cardAnimator.SetBool("removed", true);
        Destroy(cardToRemove, 0.5f);
    }

    public void UnitAttack(GameObject attacker, GameObject target, double damage) {
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

        attackerBehavior.unitAnimator.SetTrigger("attack");
        targetBehavior.unitAnimator.SetTrigger("damage");

        if (target.GetComponent<UnitBehavior>().damageThisUnit(damage) && target.GetComponent<UnitBehavior>().isVampire){
            boardScript.score2 += 1;
        }
    }
    public void MoveUnit(GameObject unitToMove, (int x, int y) pos) {
        if (unitToMove.GetComponent<UnitBehavior>().position.x>pos.x)
            unitToMove.GetComponent<UnitBehavior>().unitAnimator.SetBool("facingRight", false);
        else if (unitToMove.GetComponent<UnitBehavior>().position.x<pos.x)
            unitToMove.GetComponent<UnitBehavior>().unitAnimator.SetBool("facingRight", true);
        unitToMove.GetComponent<UnitBehavior>().movePosition(pos);
        unitToMove.GetComponent<UnitBehavior>().SetHasActed(true);

        boardScript.spawnedPieces[boardScript.selectedUnitPosition.x, boardScript.selectedUnitPosition.y].GetComponent<BoardButtonsScript>().setSelected(0);
        boardScript.selectedUnitPosition = (-1, -1);
        boardScript.UpdatePieceInteractability();
    }
    public void DeselectUnit() {
        boardScript.clearInteractabilityMatrix();
        boardScript.spawnedPieces[boardScript.selectedUnitPosition.x, boardScript.selectedUnitPosition.y].GetComponent<BoardButtonsScript>().setSelected(0);
        boardScript.selectedUnitPosition = (-1, -1);
        boardScript.UpdatePieceInteractability();
    }
}
