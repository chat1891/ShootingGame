using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    protected float health;
    protected bool dead;
    public float startingHealth;

    public event System.Action OnDeath;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeHit (float damage, RaycastHit hit)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }
}
