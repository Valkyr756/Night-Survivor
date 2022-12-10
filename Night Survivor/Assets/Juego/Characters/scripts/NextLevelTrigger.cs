using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject youWin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            youWin.SetActive(true);
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}
