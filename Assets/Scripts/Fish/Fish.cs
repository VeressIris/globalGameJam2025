using DG.Tweening;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Fish properties")]
    public FishSpawner fishSpawner;
    public bool isDead;
    public bool scared;

    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] public float value;

    [Header("Spawning/moving properties")]
    
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject impaledEffect;
    bool flipped = false;
    float scaleX;

    bool moving;
    Vector2 target;
    // Flip horizontally to face target
    void UpdateFlip()
    {
        Vector2 direction = target - (Vector2)transform.position;

        if (direction.x > 0 && flipped == true)
        {
            transform.DOKill();
            transform.DOScaleX(scaleX, 0.3f);
            flipped = false;
        }
        else if (direction.x < 0 && flipped == false)
        {
            transform.DOKill();
            transform.DOScaleX(-scaleX, 0.3f);
            flipped = true;
        }
    }

    #region Unity Messages

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    float initialScale;

    void Start()
    {
        transform.position = fishSpawner.RandomPointInCircle();
        scaleX = transform.localScale.x;
        impaledEffect = transform.Find("Spear").gameObject;
        impaledEffect.SetActive(false);
        initialScale = transform.localScale.x;
        //transform.localScale = Vector2.zero;
        //transform.DOScale(initialScale, 0.3f);
    }

    void Update()
    {
        UpdateFlip();

        var targetDistToPlayer = ((Vector2)fishSpawner.player.transform.position-target).sqrMagnitude;
        var distToPlayer = (fishSpawner.player.transform.position - transform.position).sqrMagnitude;
        var scaredDist = 7;
        if (targetDistToPlayer < scaredDist*scaredDist)
        {
            moving = false;
        }

        if (moving)
        {
            if (Vector2.Distance(transform.position, target) > 0.1f)
            {  
                scared = distToPlayer < scaredDist * scaredDist; 
                var moveSpeedActual = scared ? moveSpeed * 2 : moveSpeed; 
                transform.position = Vector2.MoveTowards(transform.position, target,(moveSpeedActual * Time.deltaTime));
            }
            else
            {
                moving = false;
            }
        }
        else
        {

            var maxPos = Vector2.zero;
            float currentDistance = 0;
            for(int i = 0; i < 5; i++)
            {
                var pos = fishSpawner.RandomPointInCircle();
                var dist = (fishSpawner.player.transform.position - transform.position).sqrMagnitude;
                if (currentDistance < dist)
                {
                    maxPos = pos;
                    currentDistance = dist; 
                }
            }
            target = maxPos;
            moving = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet)
        {
            var hitSpeed = bullet.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
            var clone = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            if (hitSpeed > 4)
            {
                impaledEffect.SetActive(true);
                enabled = false;
                isDead = true;
                bullet.PlayDestroyAnimation();
                AudioManager.instance.Play("FishDied");
            }
            else
            {
                DOVirtual.DelayedCall(2, () =>
                {
                    if (bullet)
                    {
                        bullet.PlayDestroyAnimation();
                    }
                });
                clone.transform.localScale *= .5f;
            }
            
        }
    }

    #endregion
}
