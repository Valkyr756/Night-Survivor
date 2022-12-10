using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public int maxHealth = 50;
    int currentHealth;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //Animacion

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("rip");
        //Animacion
        animator.SetBool("isDead", true);

        //Desactiva enemigo
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
