using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneClass
{
    public bool hasImage = true;
    public Sprite? sprite;
    public bool hasConversation = false;
    public List<ConversationLineClass>? conversation = new List<ConversationLineClass>();
}
