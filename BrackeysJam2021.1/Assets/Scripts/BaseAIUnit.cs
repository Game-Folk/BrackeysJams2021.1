using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BaseAIUnit : BaseUnit
{
    [SerializeField] protected float updateAITimesPerSeconds = 10;
    [SerializeField] protected float massToPushThrough = 10f;
    [SerializeField] protected float timeToBeHeavy = 5f;
    [SerializeField] protected float attackSearchRange = 5f;
    [SerializeField] protected float timeToDestroyStandByTarget = 3f;
    [SerializeField] private Transform spriteTransform = null;
    [SerializeField] private AudioSource affirmativeActionAudio = null;

    private AIPath aIPath;
    private AIDestinationSetter aIDestinationSetter;
    private Rigidbody2D rgbd;
    private float mass_OG;
    protected Transform targetTransform = null;
    protected BaseUnit attackTarget = null;
    protected Transform standByLocation = null;

    protected Node topNode;

    public override void Awake()
    {
        base.Awake();

        aIPath = GetComponent<AIPath>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        rgbd = GetComponent<Rigidbody2D>();
        mass_OG = rgbd.mass;
    }

    public override void Start()
    {
        base.Start();

        ConstructBehaviorTree();
        StartCoroutine(EvaluateBehaviorTree());
        StartCoroutine(Attack());
    }

    public override void Update()
    {
        base.Update();

        UpdateAnimator();
        CorrectDirectionFacing();
    }

    public override void Die(float timeUntilDestroy)
    {
        base.Die(timeUntilDestroy);

        aIPath.maxSpeed = 0;
    }

    
    public virtual void ConstructBehaviorTree()
    {

    }

    IEnumerator EvaluateBehaviorTree()
    {
        while(true)
        {
            // just wait again, if no topNode
            if(topNode == null) 
            {
                yield return new WaitForSeconds(1/updateAITimesPerSeconds);
                continue;
            }

            topNode.Evaluate();
            yield return new WaitForSeconds(1/updateAITimesPerSeconds);
        }
    }

    public virtual IEnumerator Attack()
    {
        while(true)
        {
            // just wait again, if no target
            if(attackTarget == null) 
            {
                yield return new WaitForSeconds(1/attacksPerSecond);
                continue;
            }

            if(Vector2.Distance(attackTarget.transform.position, transform.position) < attackRange)
            {
                BaseUnit attackTargetTemp = attackTarget;

                // check if you'll kill the target
                if(attackTargetTemp.GetCurrentHealth() - attack <= 0)
                {
                    targetTransform = standByLocation;
                    // reset attack target
                    attackTarget = null;
                }
                attackTargetTemp.AddCurrentHealth(-attack);
            }
            yield return new WaitForSeconds(1/attacksPerSecond);
        }
        
    }

    // the idea here, is that we reset the actions from the behavior tree
    // except for the action we're choosing
    public virtual void ResetActionFlags(string nameOfAction)
    {
        if(!nameOfAction.Equals("Attacking"))
        {
            attackTarget = null;
        }
        if(!nameOfAction.Equals("Standing By"))
        {
            // standByLocation = null;
        }
    }
    
    public virtual void UpdateAnimator()
    {
        // Moving & idling
        float x = Mathf.Pow( Mathf.Abs(aIPath.velocity[0]), 2 );
        float y = Mathf.Pow( Mathf.Abs(aIPath.velocity[1]), 2 );
        float speed = Mathf.Sqrt( x + y );
        animator.SetFloat("Speed", speed);
        
        // Attacking
        // TODO: move this to attack, and change to a trigger instead of bool
        if(attackTarget == null)
        {
            animator.SetBool("Attacking", false);
        }
        else
        {
            animator.SetBool("Attacking", true);
        }
    }
    
    public bool SetDestination(Transform dest)
    {
        // Set its mass so it can push through
        // StartCoroutine(ResetMass(mass_OG, timeToBeHeavy));
        // rgbd.mass = massToPushThrough;
        // NOTE: this^ appears to cause more problems than solutions

        if(dest == null)
        {
            aIDestinationSetter.target = targetTransform;
            return true;
        }

        aIDestinationSetter.target = dest;
        return true;
    }
    private IEnumerator ResetMass(float mass_OG, float timeToBeHeavy)
    {
        yield return new WaitForSeconds(timeToBeHeavy);
        rgbd.mass = mass_OG; // reset the mass
    }

    public BaseUnit GetAttackTarget()
    {
        return attackTarget;
    }
    public void SetAttackTarget(BaseUnit newAttackTarget)
    {
        attackTarget = newAttackTarget;
        targetTransform = newAttackTarget.transform;
        ResetActionFlags("Attacking");
    }

    public Transform GetStandByLocation()
    {
        return standByLocation;
    }
    public void SetStandByLocation(Transform newStandByLocation)
    {
        // if already has a stand by location
        if(standByLocation != null)
        {
            Destroy(standByLocation.gameObject, timeToDestroyStandByTarget);
        }

        PlayAffirmativeActionSound();

        standByLocation = newStandByLocation;
        targetTransform = newStandByLocation;
        ResetActionFlags("Standing By");
    }

    protected void PlayAffirmativeActionSound()
    {
        // play audio if exists
        if(affirmativeActionAudio == null)
        {
            Debug.Log("BaseAIUnit: " + gameObject.name + " has no affirmative audio source");
        }
        else
        {
            affirmativeActionAudio.Play();
        }
    }

    private void CorrectDirectionFacing()
    {
        // negative, y rot = 0
        if(aIPath.velocity[0] < 0)
        {
            spriteTransform.eulerAngles = new Vector3(spriteTransform.eulerAngles.x, 0, spriteTransform.eulerAngles.z);
        }
        else if(aIPath.velocity[0] > 0) // positive, y rot = 180
        {
            spriteTransform.eulerAngles = new Vector3(spriteTransform.eulerAngles.x, 180, spriteTransform.eulerAngles.z);
        }
    }

}
