using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<ConversationLineClass> conversation = new List<ConversationLineClass>();
    private GameObject textBox;
    private TMP_Text characterNameTMP;
    private TMP_Text textTMP;
    private int currentLine = 0;

    void Start()
    {
        textBox = GameObject.Find("TextBox");
        characterNameTMP = GameObject.Find("Name").GetComponent<TMP_Text>();
        textTMP = GameObject.Find("Lines").GetComponent<TMP_Text>();
        textBox.SetActive(false);
    }

    void Update()
    {
        // go to next line
        if (Input.GetKeyDown(KeyCode.Space) && currentLine < conversation.Count)
        {
            currentLine++;
            UpdateTextBox();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            textBox.SetActive(true);
            Debug.Log("Dialgoue in progress");

            UpdateTextBox();
        }
    }

    private void UpdateTextBox()
    {
        textTMP.text = conversation[currentLine].line;
        characterNameTMP.text = conversation[currentLine].characterName;
    }
}
