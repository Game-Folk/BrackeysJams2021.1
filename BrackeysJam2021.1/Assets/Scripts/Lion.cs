using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : BaseUnit
{
    [SerializeField] private float stompAttackDamage = 5f;
    [SerializeField] private float stompAttackRange = 1f;

    private UnitManager unitManager = null;
    private List<Transform> enemiesList = null;

    public override void Start()
    {
        base.Start();

        unitManager = UnitManager.instance;

        enemiesList = unitManager.GetAttackableHumansList();
        unitManager.AddAnimalToAttackableList(transform); // allow lion to be attacked
    }   

    public void StompAttack()
    {
        // Play animation


        // collect enemies within range
        List<BaseUnit> enemiesInRange = new List<BaseUnit>();
        foreach(Transform enemy in enemiesList)
        {
            float distance = Vector2.Distance(enemy.position, transform.position);
            if(distance < stompAttackRange)
            {
                enemiesInRange.Add(enemy.GetComponent<BaseUnit>());
            }
        }

        // deal damage to collected enemies
        foreach(BaseUnit enemy in enemiesInRange)
        {
            enemy.AddCurrentHealth(-stompAttackDamage);
        }
    }
}
