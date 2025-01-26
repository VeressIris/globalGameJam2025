using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigibody;
    Animator animator;

    public bool hasDynamite;
    public int money;
    public bool shouldShowInventory => surfaceHeight < transform.position.y;

    [Range(0, 1)]
    public float oxygen;
    public float oxygenCapacitySeconds = 30;

    [SerializeField] SpearGun spearGun;
    [SerializeField] float verticalVelocity = 2;
    [SerializeField] float horizontalVelocity = 2;
    [SerializeField] float maxTilt = 30;

    [Header("Animation")]
    [SerializeField] float animationSpeedCoef = 1;
    [SerializeField] float minAnimSwimmingSpeed = .1f;

    [SerializeField] float surfaceHeight;
    [SerializeField] float maxDepth;

    Volume volume;
    Vignette vignette;
    FishingNet fishNet;

    public bool flipped;
    float scaleX;

    Vector2 velocityDrv;
    Vector2 currentVelocity;

    float targetSpearGunRotation;

    float cameraSizeVelocity;
    float initialOrthoSize;
    void UpdateCamera()
    {
        var isSprinting = Input.GetKey(KeyCode.LeftShift);
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, isSprinting ? 8 : 6, ref cameraSizeVelocity, .5f);
    }

    void UpdateOxygen()
    {
        if (transform.position.y > surfaceHeight)
        {
            oxygen = 1;
            fishNet.SellFish(this);
        }
        else
        {
            oxygen -= Time.deltaTime / oxygenCapacitySeconds;
            oxygen = Mathf.Clamp01(oxygen);
        }

        if (oxygen == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateVignette()
    {
        vignette.intensity.value = Utils.Remap(transform.position.y, maxDepth, surfaceHeight, .2f, 0.0f);
    }

    void UpdateAnimations()
    {
        animator.SetFloat("SwimmingSpeed", Mathf.Max(Mathf.Abs(currentVelocity.magnitude * animationSpeedCoef), minAnimSwimmingSpeed));
    }

    float rotationSpeed = 0;
    void UpdateSpearGunRotation()
    {
        var dif = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - spearGun.transform.position);
        targetSpearGunRotation = (flipped ? (Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg) : (180 + Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg));
         
        var eu = spearGun.transform.eulerAngles;
        eu.z = Mathf.SmoothDampAngle(eu.z, targetSpearGunRotation, ref rotationSpeed,.1f);
        spearGun.transform.eulerAngles = eu;
    }

    void UpdateMovement()
    {
        bool sprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            spearGun.Fire();
        }

        var input = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            input += new Vector2(0, 1);
        if (Input.GetKey(KeyCode.S))
            input += new Vector2(0, -1);
        if (Input.GetKey(KeyCode.A))
        {
            transform.DOKill();
            transform.DOScaleX(-scaleX, .6f);
            input += new Vector2(-1, 0);
            flipped = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.DOKill();
            transform.DOScaleX(scaleX, .6f);
            input += new Vector2(1, 0);
            flipped = true;
        }
        var targetVelocity = Vector2.Scale(input, new Vector2(sprinting ? horizontalVelocity * 2 : horizontalVelocity, sprinting ? verticalVelocity * 1.5f : verticalVelocity));
        currentVelocity = Vector2.SmoothDamp(currentVelocity, targetVelocity, ref velocityDrv, .2f);
        rigibody.linearVelocity = currentVelocity;

        var eu = transform.localEulerAngles;
        eu.z = currentVelocity.y * maxTilt * (flipped ? 1 : -1);
        transform.localEulerAngles = eu;

        if (input == Vector2.zero)
            rigibody.Sleep();

    }

    #region Unity Messages
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flipped = true;
        scaleX = transform.localScale.x;
        volume = FindAnyObjectByType<Volume>();
        fishNet = GetComponent<FishingNet>();
        if (!volume.sharedProfile.TryGet(out vignette))
        {
            vignette = volume.sharedProfile.Add<Vignette>(true);
        }
        fishNet = GetComponentInChildren<FishingNet>();
        oxygen = 1;
    }

    void Update()
    {
        UpdateMovement();
        UpdateSpearGunRotation();
        UpdateAnimations();
        UpdateVignette();
        UpdateOxygen();
        UpdateCamera();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var fish = collision.gameObject.GetComponent<Fish>();
        if (fish && fish.isDead)
        {
            fishNet.AddFish(fish.gameObject);
        }
    }
    #endregion
}
