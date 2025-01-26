using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{

    [SerializeField] List<Image> digiImages;
    [SerializeField] List<Sprite> digits;
    
    [SerializeField] Color equipmentColor1;
    [SerializeField] Color equipmentColor2;
    [SerializeField] Color equipmentColor3;

    List<string> unlocked = new();

    CanvasGroup canvasGroup;
    PlayerController player;
    bool isMenuShown;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();

        foreach (Button but in transform.GetComponentsInChildren<Button>())
        {
            but.onClick.AddListener(() =>
            {
                var tank = player.transform.Find("Tank").GetComponent<SpriteRenderer>();
                var spearGunImage = player.transform.Find("Mana Gun").Find("Gun").GetComponent<SpriteRenderer>();
                var spearGun = player.GetComponentInChildren<SpearGun>();

                int price = 100000;
                
                if (but.name == "Gun1")
                {
                    price = 5;
                }
                else if (but.name == "Gun2" && unlocked.Contains("Gun1"))
                {
                    price = 25;
                }
                else if (but.name == "Gun3" && unlocked.Contains("Gun2"))
                {
                    price = 50;
                }
                else if (but.name == "Oxigen1")
                {
                    price = 15;
                }
                else if (but.name == "Oxigen2" && unlocked.Contains("Oxigen1"))
                {
                    price = 30;
                }
                else if (but.name == "Oxigen3" && unlocked.Contains("Oxigen2"))
                {
                    price = 60;
                }
                else if (but.name == "Dynamite")
                {
                    price = 100;
                }

                if (price > player.money)
                {
                    return;
                }
                unlocked.Add(but.name);
                player.money -= price;
                but.interactable = false;

                if (but.name == "Gun1")
                {
                    spearGunImage.color = equipmentColor1;
                    spearGun.firingSpeed = 15;
                }
                else if (but.name == "Gun2")
                {
                    spearGunImage.color = equipmentColor2;
                    spearGun.firingSpeed = 20;
                }
                else if (but.name == "Gun3")
                {
                    spearGunImage.color = equipmentColor3;
                    spearGun.firingSpeed = 25;
                }
                else if (but.name == "Oxigen1")
                {
                    tank.color = equipmentColor1;
                    player.oxygenCapacitySeconds = 45;
                }
                else if (but.name == "Oxigen2")
                {
                    tank.color = equipmentColor2;
                    player.oxygenCapacitySeconds = 60;
                }
                else if (but.name == "Oxigen3")
                {
                    tank.color = equipmentColor3;
                    player.oxygenCapacitySeconds = 80;
                }
                else if (but.name == "Dynamite")
                {
                    player.hasDynamite = true;
                }
            });
        }
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        isMenuShown = false;
        gameObject.SetActive(false);
    }

    void DisplayNumber(int number)
    {
        digiImages[0].sprite = digits[number % 10];
        number /= 10;
        digiImages[1].sprite = digits[number % 10];
        number /= 10;
        digiImages[2].sprite = digits[number % 10];
    }


    public void Show()
    {
        if (isMenuShown)
            return;
        gameObject.SetActive(true);
        isMenuShown = true;
        canvasGroup.DOFade(1, .5f);
    }
    public void Hide()
    {
        if (isMenuShown == false)
            return;
        isMenuShown = false;
        canvasGroup.DOFade(0, .5f).OnComplete(() =>
        {

        });
    }

    void Update()
    {
        DisplayNumber(player.money);
    }
}
