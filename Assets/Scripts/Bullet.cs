using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float firingForce = 10;
    [SerializeField] GameObject cavitationBubbles;
    [SerializeField] float lifeTime = 10;
    [SerializeField] float shutOffVelocity = .2f;

    float timeToDie;
    bool isDead;

    public void PlayDestroyAnimation()
    {
        if (isDead)
            return;

        isDead = true;

        GetComponent<Collider2D>().enabled = false;

        transform.DOScale(0, .4f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(firingForce * transform.right, mode: ForceMode2D.Impulse);
        timeToDie = Time.time + lifeTime;
    }

    void OnCollisionEnter(Collision collision)
    {

    }

    void Update()
    {
        if (shutOffVelocity > rb.linearVelocity.magnitude)
        {
            Utils.DisableParticleSystemEmission(cavitationBubbles);
        }

        if (timeToDie < Time.time && isDead == false)
        {
            PlayDestroyAnimation();
        }
    }
}
