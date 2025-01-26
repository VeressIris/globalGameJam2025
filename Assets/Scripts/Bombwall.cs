using DG.Tweening;
using UnityEngine;

public class Bombwall : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    bool played = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player && player.hasDynamite && played == false)
        {
            played = true;
            DOVirtual.DelayedCall(2, () =>
            {
                explosion.SetActive(true);
                AudioManager.instance.Play("Explosion");
                DOVirtual.DelayedCall(.2f, () =>
                {
                    gameObject.transform.DOScale(0, .2f).OnComplete(() =>
                    {
                        Destroy(gameObject);
                    });
                });
            });
        }
    }
}
