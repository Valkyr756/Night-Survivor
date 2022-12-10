using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private int currentHealth;
    private float dist;     //distancia de enemigo al protagonista
    private float countdown = 0f;
    private float cooldown = 0f;


    public int maxHealth = 30;  
    public float attackRange = 0.5f;    //rango de la esfera que indica el ataque 
    public int attackDamage = 10;

    Animator m_Animator;
    Rigidbody m_Rigidbody;

    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public Transform[] distancias;
    public Transform attackPoint;   //punto donde se coloca la esfera 
    public LayerMask playerLayer;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        currentHealth = maxHealth;
        navMeshAgent.SetDestination(waypoints[0].position);
        dist = Vector3.Distance(distancias[0].position, distancias[1].position);
    }

    void Update()
    {
        dist = Vector3.Distance(distancias[0].position, distancias[1].position);

        if (dist < 20)
        {
            navMeshAgent.SetDestination(waypoints[1].position);
            if (countdown > 0)
            {
                countdown -= 1 * Time.deltaTime;
                m_Animator.SetFloat("Blend", 0f, 0.1f, Time.deltaTime);
                navMeshAgent.speed = 0f;
            }
            else
            {
                m_Animator.SetFloat("Blend", 0.5f, 0.1f, Time.deltaTime);
                navMeshAgent.speed = 0.2f;

                //Ataque
                if (cooldown > 0)
                    cooldown -= 1 * Time.deltaTime;
                else if (dist < 1)
                    StartCoroutine(Attack());
            }
        }
        else
        {
            m_Animator.SetFloat("Blend", 0f, 0.1f, Time.deltaTime);
            navMeshAgent.speed = 0f;
        }

        
    }

    private IEnumerator Attack()
    {
        cooldown = 2f;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
        foreach (Collider player in hitEnemies)
        {
            player.GetComponent<PlayerMuve>().PlayerTakeDamage(attackDamage);
        }

        //Animaci�n
        m_Animator.SetLayerWeight(m_Animator.GetLayerIndex("Attack Layer"), 1);
        m_Animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        m_Animator.SetLayerWeight(m_Animator.GetLayerIndex("Attack Layer"), 0);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        countdown = 2f;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Animacion
        m_Animator.SetBool("isDead", true);

        //Desactiva enemigo
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
