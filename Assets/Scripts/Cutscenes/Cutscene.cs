using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    [Tooltip("What scene do you want to load (index)")]
    [SerializeField] private int destinationSceneIndex;
    [SerializeField] private List<SceneClass> scenes = new List<SceneClass>();
    private Image imageCanvas;
    private int currentSceneIndex = 0;
    private SceneClass currentScene;
    private DialogueManager dialogueManager;
    private bool talking = false;

    void Awake()
    {
        imageCanvas = GameObject.Find("ImageCanvas").GetComponent<Image>();
        dialogueManager = this.gameObject.GetComponent<DialogueManager>();
    }

    void Start()
    {
        dialogueManager.textBox.SetActive(false);

        // Subscribe to the DialogueManager's OnDialogueEnd event
        dialogueManager.OnDialogueEnd += EndDialogue;

        GoToScene(0); // trigger first scene
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!talking)
            {
                currentSceneIndex++;
                Debug.Log(currentSceneIndex);
                if (currentSceneIndex < scenes.Count)
                {
                    GoToScene(currentSceneIndex);
                }
            }
        }
    }

    private void GoToScene(int scene)
    {
        if (currentSceneIndex >= scenes.Count)
        {
            // load next unity scene
            SceneManager.LoadScene(destinationSceneIndex);
            return;
        }

        currentScene = scenes[scene];

        if (currentScene.hasImage)
        {
            imageCanvas.color = Color.white;
            imageCanvas.sprite = currentScene.sprite;
        }
        else
        {
            imageCanvas.color = Color.black;
        }

        if (currentScene.hasConversation)
        {
            talking = true;
            dialogueManager.textBox.SetActive(true);
            dialogueManager.conversation = currentScene.conversation;
            dialogueManager.currentLine = 0;

            StartCoroutine(dialogueManager.UpdateTextBox());
        }
    }

    private void EndDialogue()
    {
        talking = false;
        currentSceneIndex++;
        GoToScene(currentSceneIndex);
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid errors
        dialogueManager.OnDialogueEnd -= EndDialogue;
    }
}