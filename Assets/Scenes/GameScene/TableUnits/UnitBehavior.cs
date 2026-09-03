using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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
    public CardData unitData;

    /// <summary>
    /// GameObject containing the unit's image.
    /// </summary>
    public GameObject imageObj;
    public GameObject sheildObj;

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

    public float health;
    public float defense;
    public GameObject defenseBar;
    public GameObject HealthBar;
    public bool hasActed = false;
    public float quedDamage = 0;
    public (int x, int y)[] selectedPositions = new (int x, int y)[0];
    public Text HealthNumber;
    public Animator unitAnimator;
    public Image UnitImage;

    /// <summary>
    /// Initializes the unit's image and position.
    /// </summary>
    void Start()
    {
        imageObj.GetComponent<UnityEngine.UI.Image>().sprite = unitData.image;
        sheildObj.GetComponent<UnityEngine.UI.Image>().sprite = unitData.image;
        updatePosition();
        health = unitData.maxHealth;
        DisplayDamage();
    }
    void Update()
    {
        if (moved)
        {
            double timeAlongPathNormalized = 2.3 * (Time.realtimeSinceStartupAsDouble - timeMoved);
            timeAlongPathNormalized = System.Math.Pow(timeAlongPathNormalized, 1.0 / 3.0) * (2.0 / 3.0) + timeAlongPathNormalized * (1.0 / 3.0);
            if (timeAlongPathNormalized > 1)
            {
                currentPosition = wantedPosition;
                this.gameObject.transform.position = wantedPosition;
                moved = false;
                unitAnimator.SetBool("walking", false);
            }
            else
            {
                unitAnimator.SetBool("walking", true);
                this.gameObject.transform.position = new Vector3(
                    (float)(currentPosition.x + timeAlongPathNormalized * (wantedPosition.x - currentPosition.x)),
                    (float)(currentPosition.y + timeAlongPathNormalized * (wantedPosition.y - currentPosition.y)),
                    (float)(currentPosition.z + timeAlongPathNormalized * (wantedPosition.z - currentPosition.z))
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

    public bool damageThisUnit(double damage)
    {

        defense -= (float)(damage);
        if (defense < 0)
        {
            health += defense;
            defense = 0;
        }
        if (health <= 0)
        {
            Board.boardScript.removeUnit(this.gameObject);
            Board.boardScript.CheckIfUnitsAllDead();
            return true;
        }
        DisplayDamage();
        return false;
    }
    public void defendThisUnit(double defense)
    {

        this.defense += (float)defense;
        DisplayDamage();
    }
    public void SetHasActed(bool hasActed)
    {
        this.hasActed = hasActed;
        if (this.hasActed)
        {
            imageObj.GetComponent<UnityEngine.UI.Image>().sprite = unitData.greyImage;
        } else {
            imageObj.GetComponent<UnityEngine.UI.Image>().sprite = unitData.image;
        }
        unitAnimator.SetBool("active", !this.hasActed);
    }
    public void DisplayDamage()
    {
        if (defense > 0)
        {
            unitAnimator.SetBool("Sheild", true);
            HealthNumber.text = "<color=#549eb6>" + defense + "+</color>" + health + "/" + unitData.maxHealth;
        }
        else
        {
            unitAnimator.SetBool("Sheild", false);
            HealthNumber.text = health + "/" + unitData.maxHealth;
        }

        defenseBar.transform.localScale = new Vector3(defense / unitData.maxHealth, 1f, 1f);
        defenseBar.transform.position = new Vector3(this.gameObject.transform.position.x - 0.1667f * 0.48f * (1f - (defense / unitData.maxHealth)), defenseBar.transform.position.y, defenseBar.transform.position.z);
        HealthBar.transform.localScale = new Vector3(health / unitData.maxHealth, 1f, 1f);
        HealthBar.transform.position = new Vector3(this.gameObject.transform.position.x - 0.1667f * 0.48f * (1f - (health / unitData.maxHealth)), HealthBar.transform.position.y, HealthBar.transform.position.z);
    }

}
