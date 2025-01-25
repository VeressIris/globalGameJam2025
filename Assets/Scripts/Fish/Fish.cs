using DG.Tweening;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Fish properties")]
    public FishSpawner fishSpawner;
    public bool isDead;

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
    void Flip()
    {
        Vector2 direction = target - (Vector2)transform.position;

        if (direction.x > 0 && flipped == false)
        {
            transform.DOKill();
            transform.DOScaleX(scaleX, 0.3f);
            flipped = true;
        }
        else if (direction.x < 0 && flipped == true)
        {
            transform.DOKill();
            transform.DOScaleX(-scaleX, 0.3f);
            flipped = false;
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
        if (moving)
        {
            if (Vector2.Distance(transform.position, target) > 0.1f)
            {
                var scared = fishSpawner.isPlayerInRadius;
                var moveSpeedActual = scared ? moveSpeed : moveSpeed * 2; 
                transform.position = Vector2.MoveTowards(transform.position, target,(moveSpeedActual * Time.deltaTime));
                Flip();
            }
            else
            {
                moving = false;
            }
        }
        else
        {
            target = fishSpawner.RandomPointInCircle();
            moving = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet)
        {
            if(bullet.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 1)
            {
                impaledEffect.SetActive(true);
                enabled = false;
                isDead = true; 
            }
            
            var clone = Instantiate(bloodEffect, transform.position, Quaternion.identity);
            bullet.PlayDestroyAnimation();
        }
    }

    #endregion
}
