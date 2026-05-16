using UnityEngine;

/// <summary>
/// Handles the behavior and movement of units on the game board.
/// </summary>
public class UnitBehavior : MonoBehaviour
{
    /// <summary>
    /// Current grid position of the unit.
    /// </summary>
    public (int x, int y) position = (0, 0);

    /// <summary>
    /// Data associated with this unit.
    /// </summary>
    public CardData unitdata;

    /// <summary>
    /// GameObject containing the unit's image.
    /// </summary>
    public GameObject imageObj;

    /// <summary>
    /// Time when the unit started moving.
    /// </summary>
    public double timeMoved;

    /// <summary>
    /// Flag indicating if the unit is currently moving.
    /// </summary>
    public bool moved;

    /// <summary>
    /// Current position of the unit in world space.
    /// </summary>
    public Vector3 currentPosition;

    /// <summary>
    /// Target position for movement animation.
    /// </summary>
    public Vector3 wantedPosition;

    /// <summary>
    /// Initializes the unit's image and position.
    /// </summary>
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
