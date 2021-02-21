using System.Collections;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] protected float startingHealth = 25;
    [SerializeField] private float healthRegenRatePerSecond = 0.5f; // 0.5 per second
    [SerializeField] private float healthRegenRate = 1f; // 1 second
    [SerializeField] private HealthBar healthBar = null;
    [SerializeField] protected float attack = 5;
    [SerializeField] protected float attackRange = 1;
    [SerializeField] protected float attacksPerSecond = 1;
    [SerializeField] private bool preventDestroy = false;
    [SerializeField] protected float timeUntilDestroyWhenDeath = 1f;
    [SerializeField] private AudioSource deathAudio = null;
    [SerializeField] protected Animator animator = null;
    
    private float currentHealth;
    private bool isDead = false;

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
        if(isDead) return; // only die once
        isDead = true;

        // Death animation
        animator.SetTrigger("Die");

        // death sound
        PlayDeathSound();

        // Destoy object
        if(gameObject != null && !preventDestroy)
        {
            print(preventDestroy);
            Destroy(this.gameObject, timeUntilDestroy);
        }
    }

    protected void PlayDeathSound()
    {
        // play audio if exists
        if(deathAudio == null)
        {
            Debug.Log("BaseUnit: " + gameObject.name + " has no death audio source");
        }
        else
        {
            deathAudio.Play();
        }
    }

    private IEnumerator RegenHealth()
    {
        while(true)
        {
            yield return new WaitForSeconds(healthRegenRate);

            // don't overheal
            float amountToHeal = healthRegenRatePerSecond;
            float potentialHealth = currentHealth + amountToHeal;
            if(potentialHealth > startingHealth)
            {
                amountToHeal = startingHealth - currentHealth;
            }
            AddCurrentHealth(amountToHeal);
        }
    }
}
