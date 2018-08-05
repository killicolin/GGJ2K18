using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    bool openGate = false;
    bool closeGate = false;
    bool changeScene = false;

    IEnumerator NextLevel()
    {

        GameObject player = GameController.Instance.player;
        if (!openGate)
        {
            openGate = true;
            GameController.Instance.canBeKilled = false;
            GetComponent<Animator>().SetBool("OnElevator", true);
            GameObject.Find("DyingHUDPanel").SetActive(false);
            player.GetComponent<Animator>().SetBool("isMoving", false);
            player.GetComponent<Collider2D>().enabled = !enabled;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<PlayerController>().enabled = false;
            GameController.Instance.soundOpenElevator();
            yield return new WaitForSeconds(1.5f);

        }
        if (!closeGate)
        {
            closeGate = true;
            player.GetComponent<SpriteRenderer>().enabled = !enabled;
            GetComponent<Animator>().SetBool("OnElevator", false);
            GameController.Instance.soundCloseElevator();
            yield return new WaitForSeconds(GameController.Instance.ElevatorClose.length);
        }
        if (!changeScene)
        {
            changeScene = true;
            GameController.Instance.soundGoUpElevator();
            yield return new WaitForSeconds(GameController.Instance.ElevatorUp.length);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    //Start Coroutine on TriggerEnter
    void OnTriggerEnter2D(Collider2D player)
    {
        PNJBehaviour pnj = player.gameObject.GetComponent<PNJBehaviour>();
        if (pnj != null && player.gameObject == GameController.Instance.player)
        {
            StartCoroutine("NextLevel");
        }
    }
}
