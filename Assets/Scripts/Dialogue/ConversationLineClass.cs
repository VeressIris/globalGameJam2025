using UnityEngine;

[System.Serializable]
public class ConversationLineClass
{
    public string characterName;
    public string line; // what the character says
    public bool hasAudio;
    public AudioClip? clip;
}
