using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent ( typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State { Idle, Chasing, Attacking};
    State currentState;

    LivingEntity targetEntity;

    NavMeshAgent pathFinder;
    Transform target;
    Renderer EnemyRenderer;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;

    float myCollisionRadius;
    float TargetCollisionRadius;

    float nextAttackTime;

    Material SkinMaterial;
    Color originalColor;

    bool HasTarget;
    float damage = 1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //this calls start method in LivingEntity
        pathFinder = GetComponent<NavMeshAgent>();
        SkinMaterial = GetComponent<Renderer>().material;
        originalColor = SkinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            HasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;

            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            TargetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            //"RGBA(0.110, 0.953, 0.000, 1.000)"
            //this.GetComponent<MeshRenderer>().material.color = new Color(0.052f, 0.283f, 0.169f, 0.200f);
            EnemyRenderer = transform.GetComponent<Renderer>();

            //A coroutine is like a function that has the ability to pause execution and return control to Unity but then to continue where it left off on the following frame.
            StartCoroutine(UpdatePath());

            StartCoroutine(Fade());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //this is expensive to calculate path every frame
        //pathFinder.SetDestination(target.position);
        if (HasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + TargetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }

    }

    void OnTargetDeath()
    {
        HasTarget = false;
        currentState = State.Idle;
    }

    IEnumerator Attack()
    {
        //store starting position, target position
        //start position => target position => back to start position
        SkinMaterial.color = Color.red;

        currentState = State.Attacking;
        //path finder is interfere with this
        //do not disable path finder, it causes errors in pathFinder.SetDestination

        Vector3 originalPosition = transform.position;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - directionToTarget * (myCollisionRadius);

        float percent = 0;
        float attackSpeed = 3;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if(percent >= 0.5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            // y = 4(-x^2 + x)
            float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;

        }

        SkinMaterial.color = originalColor;
        currentState = State.Chasing;
    }

    IEnumerator UpdatePath()
    {
        //update the path every second instead of every frame

        float refreshRate = 0.25f;
        if (currentState == State.Chasing)
        {
            while (HasTarget)
            {
                //Vector3 TargetPosition = new Vector3(target.position.x, 0, target.position.z); // make sure it is on the ground
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                Vector3 TargetPosition = target.position - directionToTarget * (myCollisionRadius + TargetCollisionRadius + attackDistanceThreshold / 2); // make sure it is on the ground
                if (!dead)
                {
                    pathFinder.SetDestination(TargetPosition);
                }
                yield return new WaitForSeconds(refreshRate);
                //Debug.Log("yield return in enemy works");
            }
        }

    }

    IEnumerator Fade()
    {
        for (float ft = 0.1f; ft <= 1; ft += 0.1f)
        {
            Color c = EnemyRenderer.material.color;
            c.g = ft;
            EnemyRenderer.material.color = c;
            //EnemyRenderer.material.SetColor("_Color", Color.red);
            yield return new WaitForSeconds(.5f);
        }
    }

}
