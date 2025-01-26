using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    [SerializeField] Image fillImage;
    bool passedHalf = false;

    PlayerController player;

    float targetAmount;
    float currentAmount;

    float speedRef;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        int slices = 24;

        targetAmount = Mathf.Round(player.oxygen * slices) / slices;

        fillImage.fillAmount = Mathf.SmoothDamp(fillImage.fillAmount, targetAmount, ref speedRef, .1f);
        if (player.oxygen > .4f)
        {
            passedHalf = false;
        }
        else if(passedHalf == false)
        {
            passedHalf = true;
            AudioManager.instance.Play("Warning");
            var animDuration = .4f;
            fillImage.DOColor(Color.red, animDuration);
            fillImage.transform.parent.DOScale(1.2f, animDuration).OnComplete(() =>
            {
                fillImage.DOColor(Color.white, animDuration);
                fillImage.transform.parent.DOScale(1, animDuration);
            });
        }
    }
}
