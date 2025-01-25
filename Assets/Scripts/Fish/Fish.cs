using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Fish properties")]
    [SerializeField] private string name;
    [SerializeField] private int rarity; // 0 indexed
    [SerializeField] private float moveSpeed = 1.5f;
    private SpriteRenderer sr;
    private bool moving = false;
    private bool flipped = false;

    [Header("Spawning/moving properties")]
    private Vector2 topLeftCornerLimit;
    private Vector2 bottomRightCornerLimit;
    private Vector2 target;
    private float scaleX;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetLimits();
        transform.position = NewPosition();
        scaleX = transform.localScale.x;
    }

    void Update()
    {
        if (moving)
        {
            if (Vector2.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                Flip();
            }
            else
            {
                moving = false;
            }
        }
        else
        {
            target = NewPosition();
            moving = true;
        }
    }

    // Get new position within bounds
    public Vector2 NewPosition()
    {
        float randomX = Random.Range(topLeftCornerLimit.x, bottomRightCornerLimit.x);
        float randomY = Random.Range(topLeftCornerLimit.y, bottomRightCornerLimit.y);

        return new Vector2(randomX, randomY);
    }

    // Set limits based on rarity
    private void SetLimits()
    {
        topLeftCornerLimit = new Vector2(-15f, 0f - 10 * rarity - 3);
        bottomRightCornerLimit = new Vector2(15f, -10 * (rarity + 1));
        Debug.Log(name + '\n' + topLeftCornerLimit + '\n' + bottomRightCornerLimit);
    }

    // Flip horizontally to face target
    private void Flip()
    {
        Vector2 direction = target - (Vector2)transform.position;

        if (direction.x > 0)
        {
            transform.DOKill();
            transform.DOScaleX(scaleX, 0.3f);
            flipped = true;
        }
        else if (direction.x < 0)
        {
            transform.DOKill();
            transform.DOScaleX(-scaleX, 0.3f);
            flipped = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.GetComponent<Bullet>())
        {
            Destroy(this.gameObject);
        }
    }
}
