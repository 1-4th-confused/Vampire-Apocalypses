using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{   
    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> vampireUnits = new List<GameObject>();
    public GameObject unitPrefab;
    void Start()
    {
        CardManager.ReadUnitsJSON();
        units.Add(Instantiate(unitPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, this.gameObject.transform));
        units[0].GetComponent<UnitBehavior>().unitdata = ((CardData) CardManager.unitTypes[0]);
    }
}