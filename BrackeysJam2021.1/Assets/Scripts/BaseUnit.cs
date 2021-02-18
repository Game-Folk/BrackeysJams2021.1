using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] protected float startingHealth = 25;
    [SerializeField] protected float attack = 5;
    [SerializeField] protected float attackRange = 1;
    [SerializeField] protected float attacksPerSecond = 1;
    [SerializeField] protected float timeUntilDestroyWhenDeath = 0f;
    
    private float currentHealth;

    public virtual void Awake()
    {
        currentHealth = startingHealth;
    }

    public virtual void Start()
    {

    }    

    public virtual void Update()
    {
        if(currentHealth <= 0) Die(timeUntilDestroyWhenDeath);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void AddCurrentHealth(float h)
    {
        currentHealth += h;
    }
    
    public virtual void Die(float timeUntilDestroy)
    {
        // Death animation

        // Destoy object
        Destroy(this.gameObject, timeUntilDestroy);
    }
}
