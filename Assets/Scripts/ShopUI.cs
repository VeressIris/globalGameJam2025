using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] List<Image> digiImages;
    [SerializeField] List<Sprite> digits;

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
                var tank = player.transform.Find("Tank").GetComponent<Image>();
                var spearGun = player.transform.Find("Mana Gun").Find("Gun").GetComponent<Image>();
                
                int price = 0;

                if (but.name == "Gun1")
                {
                    price = 5;
                }
                else if (but.name == "Gun2")
                {
                    price = 25;
                }
                else if (but.name == "Gun3")
                {
                    price = 50;
                }
                else if (but.name == "Oxigen1")
                {
                    price = 15;
                }
                else if (but.name == "Oxigen2")
                {
                    price = 30;
                }
                else if (but.name == "Oxigen3")
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
                player.money -= price;
                but.interactable = false;

                if (but.name == "Gun1")
                {

                }
                else if (but.name == "Gun2")
                {

                }
                else if (but.name == "Gun3")
                {

                }
                else if (but.name == "Oxigen1")
                {

                }
                else if (but.name == "Oxigen2")
                {

                }
                else if (but.name == "Oxigen3")
                {

                }
                else if (but.name == "Dynamite")
                {

                }
            });
        }
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        isMenuShown = false;
    }

    void DisplayNumber(int number)
    {
        digiImages[0].sprite = digits[number % 10];
        number /= 10;
        digiImages[1].sprite = digits[number % 10];
        number /= 10;
        digiImages[2].sprite = digits[number % 10];
    }


    void Show()
    {
        if (isMenuShown)
            return;
        isMenuShown = true;
        canvasGroup.DOFade(1, .5f);
    }
    void Hide()
    {
        if (isMenuShown == false)
            return;
        isMenuShown = false;
        canvasGroup.DOFade(0, .5f);
    }

    void Update()
    {
        DisplayNumber(player.money);
        if (player.shouldShowInventory)
            Show();
        else
            Hide();
    }
}
