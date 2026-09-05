using UnityEngine;

public class gamePhaseNavigator : MonoBehaviour
{
    Animator cameraAnimator;
    public int tablePosition = 2;
    public void moveToShop()
    {
        cameraAnimator.SetInteger("tablePosition", 1);
        tablePosition = 1;
    }

    public void moveToBattle()
    {
        cameraAnimator.SetInteger("tablePosition", 2);
        tablePosition = 2;
    }
}
