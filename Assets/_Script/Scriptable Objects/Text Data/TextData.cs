using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new level", menuName = "Text/ScrollingText Asset")]
public class ScrollTextData : ScriptableObject
{
    [Serializable] public class Text
    {
        [TextArea] public string text;
        public float scrollSpeed = 0.05f;
    }
    public List<Text> text;
}
