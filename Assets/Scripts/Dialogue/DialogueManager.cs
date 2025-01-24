using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<ConversationLineClass> conversation = new List<ConversationLineClass>();
    private GameObject textBox;
    private TMP_Text characterNameTMP;
    private TMP_Text textTMP;
    private int currentLine = 0;

    [SerializeField] private float delay = 0.15f;
    private bool typing = false;

    void Awake()
    {
        textBox = GameObject.Find("TextBox");
        characterNameTMP = GameObject.Find("Name").GetComponent<TMP_Text>();
        textTMP = GameObject.Find("Lines").GetComponent<TMP_Text>();
    }

    void Start()
    {
        // make sure text box is clear
        textTMP.text = "";
        textBox.SetActive(false);
    }

    void Update()
    {
        // progress through conversation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!typing)
            {
                currentLine++;
            }
            
            if (currentLine >= conversation.Count)
            {
                textBox.SetActive(false);
            }
            else
            {
                if (!typing)
                {
                    StartCoroutine(UpdateTextBox());
                }
                else
                {
                    typing = false;
                    StopAllCoroutines();
                    textTMP.text = conversation[currentLine].line;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            currentLine = 0;

            textBox.SetActive(true);

            StartCoroutine(UpdateTextBox());
        }
    }

    private IEnumerator UpdateTextBox()
    {
        typing = true;

        textTMP.text = ""; // clear text box
        characterNameTMP.text = conversation[currentLine].characterName;
        
        // display text character by char
        foreach (char chr in conversation[currentLine].line)
        {
            textTMP.text += chr;
            yield return new WaitForSeconds(delay); // wait before displaying next char
        }
        
        typing = false;
    }
}
