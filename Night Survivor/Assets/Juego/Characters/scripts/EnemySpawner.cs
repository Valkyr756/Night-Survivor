using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float timer;             //Tiempo definidio de spawn
    public GameObject ghost;        //Objeto que se clonar�

    float m_Countdown;

    void Start()
    {
        m_Countdown = timer;
    }

    void Update()
    {
        m_Countdown -= Time.deltaTime;

        if (m_Countdown <= 0f)
        {
            Instantiate(ghost, new Vector3(0.9f, 0f, -3.5f), Quaternion.identity);
            m_Countdown = timer;
        }
    }
}
