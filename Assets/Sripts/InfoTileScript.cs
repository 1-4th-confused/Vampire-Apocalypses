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

    // Update is called once per frame
    void Update()
    {
        Debug.Log(info == nullData);
    }
    public void MoveOut(CardData infoToDisplay)
    {
        Debug.Log(info);
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
                unitDescription.text += "range:" + info.range +"\n";
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
            isOut = true;
            Debug.Log("SetInfo to:" + infoToDisplay);
            info = nullData;
            animator.SetBool("InfoIn", false);
            if (quedInfo != nullData)
            {
                Debug.Log(quedInfo);
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
