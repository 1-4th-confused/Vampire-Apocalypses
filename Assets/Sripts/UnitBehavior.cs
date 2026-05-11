using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    public (int x, int y) position = (0,0);
    public CardData unitdata;


// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = unitdata.image;
        updatePosition(0,0);

    }

    void updatePosition(int x,int y)
    {
        position = (x,y);
        this.transform.position = new Vector3(0.32f * position.x, 0.24f, 0.32f * position.y);
    }

    void movePosition(int x,int y)
    {
        position = (position.x + x,position.y + y);
        this.transform.position = new Vector3(0.32f * position.x, 0.24f, 0.32f * position.y);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
