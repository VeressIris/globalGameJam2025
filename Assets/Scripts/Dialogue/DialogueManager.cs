using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public List<ConversationLineClass> conversation = new List<ConversationLineClass>();
    [HideInInspector] public GameObject textBox;
    private TMP_Text characterNameTMP;
    private TMP_Text textTMP;
    [HideInInspector] public int currentLine = 0;

    [SerializeField] private float delay = 0.15f;
    private Coroutine typewriterCoroutine; // Reference to the running coroutine
    private bool typing = false;
    [SerializeField] private bool usedElsewhere = false;

    // Event to notify when the dialogue ends
    public event System.Action OnDialogueEnd;

    void Awake()
    {
        textBox = GameObject.Find("TextBox");
        characterNameTMP = GameObject.Find("Name").GetComponent<TMP_Text>();
        textTMP = GameObject.Find("Lines").GetComponent<TMP_Text>();
    }

    void Start()
    {
        // make sure text box is clear at the start
        textTMP.text = "";

        if (!usedElsewhere) textBox.SetActive(false);
    }

    void Update()
    {
        // Handle skipping typewriter effect or progressing the dialogue
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (typing)
            {
                typing = false;

                // Safeguard against null coroutine
                if (typewriterCoroutine != null)
                {
                    StopCoroutine(typewriterCoroutine);
                    typewriterCoroutine = null; // Clear the reference
                }

                textTMP.text = conversation[currentLine].line;
            }
            else
            {
                // Move to the next line
                currentLine++;

                if (currentLine >= conversation.Count)
                {
                    textBox.SetActive(false);

                    // Trigger the event when dialogue ends
                    OnDialogueEnd?.Invoke();
                }
                else
                {
                    typewriterCoroutine = StartCoroutine(UpdateTextBox());
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
    public IEnumerator UpdateTextBox()
    {
        typing = true;

        textTMP.text = ""; // Clear text box
        characterNameTMP.text = conversation[currentLine].characterName;

        // Type out the line character by character
        foreach (char chr in conversation[currentLine].line)
        {
            textTMP.text += chr;
            yield return new WaitForSeconds(delay); // Wait before displaying the next character

            // Break out early if skipping
            if (!typing) yield break;
        }

        typing = false;
    }
}
