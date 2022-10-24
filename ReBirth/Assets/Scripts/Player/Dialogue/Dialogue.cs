using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {
    
    [SerializeField] private string _npcName; // NPC we're talking to
    
    [TextArea(3, 10)]
    [SerializeField] private string[] _sentences;

    public string GetNPCName() { return _npcName; }
    public string[] GetSentences() { return _sentences; }

}
