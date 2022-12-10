using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMuve : MonoBehaviour
{
    private int currentHealth;
    private float turnSpeed = 40f;
    private float cooldown;         //debido a que el enfriamiento del ataque también se ve afectado 
    private float cooldownForUI;    //por el tiempo de espera de corrutina, se usa esta otra variable  
                                    //para que se le sume 1 y así equivalga al verdadero tiempo de espera
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public int maxHealth = 30;
    public LayerMask enemyLayers;

    //variables para UI
    public TextMeshProUGUI cooldownText;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject youWin;
    public GameObject gameOver;

    //variable para collider de la meta
    public GameObject endingCollider;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    RigidbodyConstraints originalConstraints;   //almacena las constraints del personaje

    void Start()
    {
        cooldown = 0;

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        
        originalConstraints = m_Rigidbody.constraints;
        currentHealth = maxHealth;
    }

    void Update()
    {
        cooldownForUI = cooldown + 1;   //aquí es donde se le suma el 1 para equivaler al cooldown "real"
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.x = horizontal;
        m_Movement.y = 0f;
        m_Movement.z = vertical;
        m_Movement.Normalize();     //Para que diagonal sea igual de rapido que pulsando una sola tecla  

        //Para comprobar cuando el personaje se mueve
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        bool runInput = Input.GetKey(KeyCode.LeftShift);

        //Animaciones de movimiento
        if (isWalking && runInput)
        {
            m_Animator.SetFloat("Blend", 1f, 0.1f, Time.deltaTime);
            m_Rigidbody.constraints = originalConstraints;
        }
        else if (isWalking)
        {
            m_Animator.SetFloat("Blend", 0.5f, 0.1f, Time.deltaTime);
            m_Rigidbody.constraints = originalConstraints;
        }
        else
        {
            m_Animator.SetFloat("Blend", 0f, 0.1f, Time.deltaTime);
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

            //Cooldown para ataque y animaci�n de este (respectivamente)
            if (cooldown > 0)
            cooldown -= 1 * Time.deltaTime;
        else if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(Attack());

        //Para controlar la velocidad con la que rota
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        //Elementos del canvas
            //Temporizador de ataque
        if (cooldown > 0)
            cooldownText.text = cooldownForUI.ToString("0");
        else
            cooldownText.text = cooldown.ToString("0");

        //Inputs para reiniciar o salir del juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator Attack()
    {
        //Cooldown
        cooldown = 2f;

        //Rango Ataque
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        //Animación
        m_Animator.SetLayerWeight(m_Animator.GetLayerIndex("Attack Layer"), 1);
        m_Animator.SetTrigger("Attack");
        
        yield return new WaitForSeconds(0.9f);
        m_Animator.SetLayerWeight(m_Animator.GetLayerIndex("Attack Layer"), 0);
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;

        //Corazones de vida
        if (currentHealth == 20)
            heart3.SetActive(false);
        else if (currentHealth == 10)
            heart2.SetActive(false);
        else if (currentHealth <= 0)
        {
            heart1.SetActive(false);
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

        //Activa el game over
        StartCoroutine(GameOver());
    }

    //Se coloca en un corrutina para que no salte inmediatamente trás morir
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        gameOver.SetActive(true);
    }

    //Simplemente comprueba que el jugador haya llegado al final
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == endingCollider)
        youWin.SetActive(true);
    }

    //Para calcular el movimiento de un  personaje animado(3D)
    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
