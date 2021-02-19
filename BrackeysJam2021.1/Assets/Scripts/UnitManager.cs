using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<Transform> attackableAnimalsList = new List<Transform>();
    private List<Transform> attackableHumansList = new List<Transform>();

    public static UnitManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one UnitManager in scene!");
            return;
        }
        instance = this;
    }
    

    public List<Transform> GetAttackableAnimalsList()
    {
        return attackableAnimalsList;
    }
    public void AddAnimalToAttackableList(Transform t)
    {
        attackableAnimalsList.Add(t);
    }
    public void RemoveAnimalFromAttackableList(Transform t)
    {
        attackableAnimalsList.Remove(t);
    }

    public List<Transform> GetAttackableHumansList()
    {
        return attackableHumansList;
    }
    public void AddHumanToAttackableList(Transform t)
    {
        attackableHumansList.Add(t);
    }
    public void RemoveHumanFromAttackableList(Transform t)
    {
        attackableHumansList.Remove(t);
    }
}
