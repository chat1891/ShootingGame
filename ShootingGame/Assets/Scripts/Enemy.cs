using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent ( typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    NavMeshAgent pathFinder;
    Transform target;
    Renderer EnemyRenderer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //this calls start method in LivingEntity
        pathFinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //"RGBA(0.110, 0.953, 0.000, 1.000)"
        //this.GetComponent<MeshRenderer>().material.color = new Color(0.052f, 0.283f, 0.169f, 0.200f);
        EnemyRenderer = transform.GetComponent<Renderer>();
        
        //A coroutine is like a function that has the ability to pause execution and return control to Unity but then to continue where it left off on the following frame.
        StartCoroutine(UpdatePath());

        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        //this is expensive to calculate path every frame
        //pathFinder.SetDestination(target.position);
    }

    IEnumerator UpdatePath()
    {
        //update the path every second instead of every frame
        float refreshRate = 0.25f;
        while (target != null)
        {
            Vector3 TargetPosition = new Vector3(target.position.x, 0, target.position.z); // make sure it is on the ground
            if (!dead)
            {
                pathFinder.SetDestination(TargetPosition);
            }
            yield return new WaitForSeconds(refreshRate);
            //Debug.Log("yield return in enemy works");
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
