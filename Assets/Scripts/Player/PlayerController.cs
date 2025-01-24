using DG.Tweening;
using Mono.Cecil;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigibody;

    [SerializeField] float verticalVelocity = 2;
    [SerializeField] float horizontalVelocity = 2;
    [SerializeField] float maxTilt = 30;

    bool flipped;
    float scaleX;

    Vector2 velocityDrv;
    Vector2 currentVelocity;

    void UpdateMovement()
    {
        var input = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            input += new Vector2(0, 1);
        if(Input.GetKey(KeyCode.S))
            input += new Vector2(0, -1);
        if (Input.GetKey(KeyCode.A))
        {
            transform.DOKill();
            transform.DOScaleX(-scaleX,.4f);
            input += new Vector2(-1, 0);
            flipped = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.DOKill();
            transform.DOScaleX(scaleX, .4f);
            input += new Vector2(1, 0);
            flipped = true;
        }
        var targetVelocity = Vector2.Scale(input, new Vector2(horizontalVelocity, verticalVelocity));
        currentVelocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref velocityDrv, .2f);
        rigibody.linearVelocity = currentVelocity;

        var eu = transform.localEulerAngles;
        eu.z = currentVelocity.y * maxTilt * (flipped ? 1 : -1);
        transform.localEulerAngles = eu;
    }

    #region Unity Messages
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigibody = GetComponent<Rigidbody2D>();
        flipped = false;
        scaleX = transform.localScale.x;
    }

    void Update()
    {
        UpdateMovement();
    }
    #endregion
}
