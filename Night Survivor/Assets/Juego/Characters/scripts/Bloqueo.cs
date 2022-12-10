using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloqueo : MonoBehaviour
{
    public static int contadorRecursos;

    void Start()
    {
        contadorRecursos = 0;
    }

    void Update()
    {
        if (contadorRecursos == 4)
            gameObject.SetActive(false);
    }
}
