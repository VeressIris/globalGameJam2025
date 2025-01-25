using DG.Tweening;
using Unity.VisualScripting;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(firingForce * transform.right, mode:ForceMode2D.Impulse);
        timeToDie = Time.time + lifeTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        
    }

    void Update()
    {
        if(shutOffVelocity > rb.linearVelocity.magnitude)
        {
            Utils.DisableParticleSystemEmission(cavitationBubbles);
        }
    
        if (timeToDie < Time.time && isDead == false)
        {
            isDead = true;

            transform.DOScale(0, .4f).OnComplete(() =>
            {
                if (gameObject)
                    Destroy(gameObject);
            });
        }
    }
}
