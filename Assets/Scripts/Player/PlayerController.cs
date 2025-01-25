using DG.Tweening;
using Mono.Cecil;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigibody;
    Animator animator;

    [SerializeField] SpearGun spearGun;
    [SerializeField] float verticalVelocity = 2;
    [SerializeField] float horizontalVelocity = 2;
    [SerializeField] float maxTilt = 30;

    [SerializeField] float middleTiltAngle = -30;
    [SerializeField] float maxTiltAngle = 30;
    [Header("Animation")]
    [SerializeField] float animationSpeedCoef = 1;
    [SerializeField] float minAnimSwimmingSpeed = .1f;

    bool flipped;
    float scaleX;

    Vector2 velocityDrv;
    Vector2 currentVelocity;

    float targetSpearGunRotation;
    float targetRotation;

    void UpdateAnimations()
    {
        animator.SetFloat("SwimmingSpeed", Mathf.Max(Mathf.Abs(currentVelocity.x * animationSpeedCoef), minAnimSwimmingSpeed));
    }


    void UpdateSpearGunRotation()
    {
        var dif = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - spearGun.transform.position);
        targetSpearGunRotation = (flipped ? (Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg ) : (180-Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg )); 
        var eu = spearGun.transform.localEulerAngles;
        eu.z = targetSpearGunRotation;
        spearGun.transform.localEulerAngles = eu;   
    }

    void UpdateMovement()
    {
        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            spearGun.Fire();
        }

        var input = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            input += new Vector2(0, 1);
        if(Input.GetKey(KeyCode.S))
            input += new Vector2(0, -1);
        if (Input.GetKey(KeyCode.A))
        {
            transform.DOKill();
            transform.DOScaleX(-scaleX,.3f);
            input += new Vector2(-1, 0);
            flipped = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.DOKill();
            transform.DOScaleX(scaleX, .3f);
            input += new Vector2(1, 0);
            flipped = true;
        }
        var targetVelocity = Vector2.Scale(input, new Vector2(sprinting ? horizontalVelocity * 2 : horizontalVelocity, verticalVelocity));
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
        animator = GetComponent<Animator>();
        flipped = true;
        scaleX = transform.localScale.x;
    }

    void Update()
    {
        UpdateMovement();
        UpdateSpearGunRotation();
        UpdateAnimations();
    }
    #endregion
}
