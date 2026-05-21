using UnityEngine;
using UnityEngine.UI;

public class InfoTileScript : MonoBehaviour
{
    public static InfoTileScript infoTileScript;
    public Animator animator;
    public bool isOut;
    public CardData nullData;
    public CardData info;
    public CardData quedInfo;
    public Text unitName;
    public Text unitDescription;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nullData = new CardData();
        info = nullData;
        quedInfo = nullData;
        infoTileScript = this;
    }
    public void MoveOut(CardData infoToDisplay)
    {

        if (info == nullData)
        {
            isOut = true;
            info = infoToDisplay;
            unitName.text = info.name;
            unitDescription.text = "";
            if (info.type == "card")
            {
                unitDescription.text += "damage:" + info.damage + "\n";
                if (info.defense > 0)
                    unitDescription.text += "defense:" + info.defense + "\n";
                unitDescription.text += "range:" + info.range + "\n";
                unitDescription.text += "Description: \n" + info.description;
            }
            animator.SetBool("InfoIn", true);

        }
        else
        {
            quedInfo = infoToDisplay;
        }

    }
    public void MoveIn(CardData infoToDisplay)
    {
        if (infoToDisplay == info)
        {
            isOut = false;
            animator.SetBool("InfoIn", false);
            info = nullData;
            if (quedInfo != nullData)
            {
                MoveOut(quedInfo);
            }
        }
        else if (infoToDisplay == quedInfo)
        {
            quedInfo = nullData;
            isOut = false;
        }
    }
}
