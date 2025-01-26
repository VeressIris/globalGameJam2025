using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public List<ConversationLineClass> conversation = new List<ConversationLineClass>();
    [HideInInspector] public GameObject textBox;
    [SerializeField] private bool hasCharacter = false;
    [SerializeField] private Sprite characterSprite;
    private Image characterImage;
    private TMP_Text characterNameTMP;
    private TMP_Text textTMP;
    [HideInInspector] public int currentLine = 0;

    [SerializeField] private float delay = 0.15f;
    private Coroutine typewriterCoroutine; // Reference to the running coroutine
    private bool typing = false;
    [SerializeField] private bool usedElsewhere = false;
    private CanvasGroup canvasGroup;

    // Event to notify when the dialogue ends
    public event System.Action OnDialogueEnd;

    void Awake()
    {
        if (usedElsewhere)
        {
            SetUpTextBox();
        }
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
                    // reset UI
                    canvasGroup.alpha = 0;
                    currentLine = 0;
                    textTMP.text = "";
                    characterImage.sprite = null;
                    
                    // Trigger the event when dialogue ends
                    OnDialogueEnd?.Invoke();
                }
                else
                {
                    if (conversation[currentLine].hasAudio)
                    {
                        PlaySFX();
                    }

                    typewriterCoroutine = StartCoroutine(UpdateTextBox());
                }
            }
        }
    }

    private void SetUpTextBox()
    {
        if (!hasCharacter)
        {
            textBox = GameObject.Find("TextBox");
        }
        else
        {
            textBox = GameObject.Find("CharacterTextBox");
            characterImage = GameObject.Find("CharacterImage").GetComponent<Image>();
            characterImage.sprite = characterSprite;
        }
        characterNameTMP = GameObject.Find("Name").GetComponent<TMP_Text>();
        textTMP = GameObject.Find("Lines").GetComponent<TMP_Text>();
        textTMP.text = "";
        canvasGroup = textBox.GetComponent<CanvasGroup>();
    }
            
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            SetUpTextBox();

            currentLine = 0;
            
            canvasGroup.alpha = 1;

            typewriterCoroutine = StartCoroutine(UpdateTextBox());
        }
    }

    public IEnumerator UpdateTextBox()
    {
        if (conversation[currentLine].hasAudio)
        {
            PlaySFX();
        }

        typing = true;

        textTMP.text = ""; // Clear text box
        characterNameTMP.text = conversation[currentLine].characterName;

        foreach (char chr in conversation[currentLine].line)
        {
            textTMP.text += chr;
            yield return new WaitForSeconds(delay);

            // Break out early if skipping
            if (!typing) yield break;
        }

        typing = false;
    }

    private void PlaySFX()
    {
        AudioSource audioSrc = this.gameObject.GetComponent<AudioSource>();
        audioSrc.clip = conversation[currentLine].clip;
        audioSrc.Play();
    }
}
