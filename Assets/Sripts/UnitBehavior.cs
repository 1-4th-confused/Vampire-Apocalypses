using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    public (int x, int y) position = (0,0);
    public CardData unitdata;


// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = unitdata.image;
        updatePosition();

    }

    void updatePosition(int x,int y)
    {
        position = (x,y);
        this.transform.position = new Vector3(0.32f * (position.x-3), 0.24f, 0.32f * (position.y-2));
    }

    void updatePosition()
    {
        this.transform.position = new Vector3(0.32f * (position.x-3), 0.24f, 0.32f * (position.y-2));
    }

    void movePosition(int x,int y)
    {
        position = (position.x + x,position.y + y);
        this.transform.position = new Vector3(0.32f * (position.x-3), 0.24f, 0.32f * (position.y-2));
    }

    // Update is called once per frame
    void Update()
    {
    }
}
