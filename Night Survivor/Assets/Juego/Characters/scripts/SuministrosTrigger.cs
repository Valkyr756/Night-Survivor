using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuministrosTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject indicador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Bloqueo.contadorRecursos += 1;
            indicador.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
