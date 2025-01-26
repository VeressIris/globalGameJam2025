using System.Collections.Generic;
using UnityEngine;

public class MemoriesBubble : MonoBehaviour
{
    public List<ConversationLineClass> conversation = new List<ConversationLineClass>();

    BubbleMemoriesManager bubbleMemoriesManager;
    Vector3 initialSize;
    private void Start()
    {
        bubbleMemoriesManager = FindAnyObjectByType<BubbleMemoriesManager>();
        initialSize = transform.localScale;
    }

    void Update()
    {
        transform.localScale = initialSize * Utils.Remap(Mathf.Sin(Time.time), -1, 1, .8f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            bubbleMemoriesManager.Show();
            bubbleMemoriesManager.StartConvo(conversation);
        }
    }

}
