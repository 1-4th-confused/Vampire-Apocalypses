using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    public (int x, int y) position = (0, 0);
    public CardData unitdata;
    public GameObject imageObj;
    public double timeMoved;
    public bool moved;
    public Vector3 currentPosition;
    public Vector3 wantedPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imageObj.GetComponent<UnityEngine.UI.Image>().sprite = unitdata.image;
        updatePosition();

    }
    void Update()
    {
        if (moved)
        {
            double timeAlongPathNormalized = 6*(Time.realtimeSinceStartupAsDouble-timeMoved);
            timeAlongPathNormalized = System.Math.Pow(timeAlongPathNormalized, 1.0 / 3.0) * (2.0 / 3.0) + timeAlongPathNormalized * (1.0 / 3.0);
            if (timeAlongPathNormalized > 1) {
                currentPosition = wantedPosition;
                this.gameObject.transform.position = wantedPosition;
                moved = false;
            } else {
                this.gameObject.transform.position = new Vector3(
                    (float) (currentPosition.x + timeAlongPathNormalized*(wantedPosition.x -currentPosition.x)),
                    (float) (currentPosition.y + timeAlongPathNormalized*(wantedPosition.y -currentPosition.y)),
                    (float) (currentPosition.z + timeAlongPathNormalized*(wantedPosition.z -currentPosition.z))
                );
            }
        }
        
            
    }

    public void updatePosition((int x, int y) pos)
    {
        position = pos;
        this.transform.position = new Vector3(0.32f * (position.x - 3), 0.24f, 0.32f * (position.y - 2));
    }

    public void updatePosition()
    {
        this.transform.position = new Vector3(0.32f * (position.x - 3), 0.24f, 0.32f * (position.y - 2));
    }

    public void movePosition((int x, int y) pos)
    {
        position = pos;
        moved = true;
        currentPosition = this.transform.position;
        wantedPosition = new Vector3(0.32f * (position.x - 3), 0.24f, 0.32f * (position.y - 2));
        timeMoved = Time.realtimeSinceStartupAsDouble;
    }
    public void Step(int x, int y)
    {
        position = (position.x + x, position.y + y);

    }

}
