using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class BubbleMemoriesManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI dialogueText;

    CanvasGroup canvasGroup;
    bool isVisible = false;
    int convoIdx = -1;
    List<ConversationLineClass> lines;
    Tween typrwriterAnim;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    public void StartConvo(List<ConversationLineClass> list)
    {
        lines = list;
        convoIdx = -1;
        NextLine();
    }

    void NextLine()
    {
        if (lines == null)
            return;
        convoIdx++;
        if (lines.Count < convoIdx)
        {
            convoIdx = -1;
            lines = null;
            Hide();
        }
        else
        {
            if (typrwriterAnim != null)
            {
                typrwriterAnim.Kill();
                convoIdx--;
                convoIdx = Mathf.Max(0, convoIdx);
                dialogueText.text = lines[convoIdx].line;
                typrwriterAnim = null;
                return;
            }

            if (convoIdx >= lines.Count)
            {
                lines = null;
                convoIdx = -1;
                Hide();
                return;
            }

            var currentLine = lines[convoIdx];
            typrwriterAnim = DOVirtual.Int(0, currentLine.line.Length, currentLine.line.Length * 0.05f, x =>
            {
                dialogueText.text = currentLine.line.Substring(0, x);
            }).OnComplete(() =>
            {
                typrwriterAnim = null;
            });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void Show()
    {
        if (isVisible)
            return;
        isVisible = true;
        gameObject.SetActive(true);
        canvasGroup.DOKill();
        canvasGroup.DOFade(1, 1);
    }
    public void Hide()
    {
        if (isVisible == false)
            return;
        isVisible = false;
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
