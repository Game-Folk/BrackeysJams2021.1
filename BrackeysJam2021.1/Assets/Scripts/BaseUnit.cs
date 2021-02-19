using System.Collections;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] protected float startingHealth = 25;
    [SerializeField] private float healthRegenRatePerSecond = 0.5f;
    [SerializeField] private float healthRegenRate = 1f;
    [SerializeField] private HealthBar healthBar = null;
    [SerializeField] protected float attack = 5;
    [SerializeField] protected float attackRange = 1;
    [SerializeField] protected float attacksPerSecond = 1;
    [SerializeField] protected float timeUntilDestroyWhenDeath = 1f;
    
    private float currentHealth;

    public virtual void Awake()
    {
        
    }

    public virtual void Start()
    {
        currentHealth = startingHealth;
        healthBar.SetMaxHealth(startingHealth);
        StartCoroutine(RegenHealth());
    }    

    public virtual void Update()
    {
        // if(currentHealth <= 0) Die(timeUntilDestroyWhenDeath);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void AddCurrentHealth(float h)
    {
        currentHealth += h;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0) Die(timeUntilDestroyWhenDeath);
    }
    
    public virtual void Die(float timeUntilDestroy)
    {
        // Death animation

        // Destoy object
        if(gameObject != null)
        {
            Destroy(this.gameObject, timeUntilDestroy);
        }
    }

    private IEnumerator RegenHealth()
    {
        while(true)
        {
            yield return new WaitForSeconds(healthRegenRate);
            AddCurrentHealth(healthRegenRatePerSecond);
        }
    }
}
