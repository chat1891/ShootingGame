using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    public LayerMask colllisionMask; //on which layer the projectile will collide with
    float speed = 10;
    Renderer EnemyRenderer;
    Color EnemyOrigionalColor = new Color(0.110f, 0.953f, 0.000f, 1.000f);
    float damage = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetSpeed (float newSpeed)
    {
        speed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, moveDistance, colllisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(Hit);        
        }
        //Hit.collider.gameObject.GetComponent<Renderer>().material.color = new Color(0.052f, 0.283f, 0.169f, 0.200f);
    }

    void OnHitObject(RaycastHit hit)
    {
        //print(hit.collider.gameObject.name);   
        //EnemyRenderer = hit.collider.gameObject.GetComponent<Renderer>();
        //FlashRed();
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        //not all the object will have IDamageable attach to it
        if(damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject); //destroy the projectile when it hits enemy
    }

    /// <summary>
    /// enemy change color to red when got hit
    /// but StartCoroutine wont work if projectile is destroyed
    /// </summary>
    void FlashRed()
    {
        EnemyRenderer.material.color = Color.red;
        // StartCoroutine wont work if the object is destroyed 
        StartCoroutine(ResetColor());
    }
    IEnumerator ResetColor()
    //void ResetColor()
    {
        float flashTime = 0.1f;
        while (EnemyRenderer.material.color != EnemyOrigionalColor)
        {   
            yield return new WaitForSeconds(flashTime);
            EnemyRenderer.material.color = EnemyOrigionalColor;
        }
    }
}
