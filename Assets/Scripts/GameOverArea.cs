using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            FindAnyObjectByType<HudUI>().ShowNextScreen().OnComplete(() =>
            {
                SceneManager.LoadScene("Ending");
            });
        }
    }
}
