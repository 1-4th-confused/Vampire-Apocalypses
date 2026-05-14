using UnityEngine;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour
{
    public CardData cardType;
    public Animator cardAnimator;
    public GameObject cardObject;
    public int index;
    public static PlayerHand playerHandScript;
    public bool selected = false;
    public bool moved;
    private double timeMoved;
    public Vector3 CurrentPos;
    public Vector3 CurrentRot;
    public Vector3 wantedPos;
    public Vector3 wantedRot;
    void Start()
    {
        cardObject.GetComponent<UnityEngine.UI.Image>().sprite = cardType.image;
    }
    void Update()
    {
        if (moved)
        {
            double timeAlongPathNormalized = 5*(Time.realtimeSinceStartupAsDouble-timeMoved);
            timeAlongPathNormalized = System.Math.Pow(timeAlongPathNormalized, 1.0 / 3.0) * (2.0 / 3.0) + timeAlongPathNormalized * (1.0 / 3.0);
            if (timeAlongPathNormalized > 1) {
                this.gameObject.transform.position = wantedPos;
                this.gameObject.transform.rotation = Quaternion.Euler(wantedRot);
                moved = false;
            } else {
                this.gameObject.transform.position = new Vector3(
                    (float)(CurrentPos.x + (timeAlongPathNormalized*(wantedPos.x - CurrentPos.x))),
                    (float)(CurrentPos.y + (timeAlongPathNormalized*(wantedPos.y - CurrentPos.y))),
                    (float)(CurrentPos.z + (timeAlongPathNormalized*(wantedPos.z - CurrentPos.z)))
                );
                float tempXrot = (float)(CurrentRot.x + (timeAlongPathNormalized*(wantedRot.x - CurrentRot.x)));
                float tempYrot = (float)(CurrentRot.y + (timeAlongPathNormalized*(wantedRot.y - CurrentRot.y)));
                float wantedZ = wantedRot.z;
                float currentZ = CurrentRot.z;
                if (wantedZ > 180)
                {
                    wantedZ-=360;
                }
                if (currentZ > 180)
                {
                    currentZ-=360;
                }
                float tempZrot = (float)(currentZ + (timeAlongPathNormalized*(wantedZ - currentZ)));

                this.gameObject.transform.rotation = Quaternion.Euler(
                    (float)(CurrentRot.x + (timeAlongPathNormalized*(wantedRot.x - CurrentRot.x))),
                    (float)(CurrentRot.y + (timeAlongPathNormalized*(wantedRot.y - CurrentRot.y))),
                    tempZrot
                );

            }
            
        }
    }
    public void MoveCard(Vector3 wantedPostion , Vector3 wantedrotation)
    {
        CurrentPos = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y,this.gameObject.transform.position.z);
        CurrentRot = this.gameObject.transform.rotation.eulerAngles;
        
        wantedPos = wantedPostion;
        wantedRot = wantedrotation;
        moved = true;
        timeMoved = Time.realtimeSinceStartupAsDouble;
    }
    public void SetSelection(bool selection)
    {
        selected = selection;
        cardAnimator.SetBool("selected", selection);
    }
    public void click()
    {
        playerHandScript.ClickCard(index);
    }

    public void OnPointerEnter()
    {
        cardAnimator.SetBool("hovering", true);
        this.gameObject.transform.SetAsLastSibling();
    }

    public void OnPointerExit()
    {
        cardAnimator.SetBool("hovering", false);
    }
}
