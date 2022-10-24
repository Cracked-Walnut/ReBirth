using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    private Queue<string> _sentences;

    // Start is called before the first frame update
    void Start() {
        _sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue _dialogue) {
        Debug.Log("Starting conversation with " + _dialogue.GetNPCName());

        _sentences.Clear();

        foreach(string _sentence in _dialogue.GetSentences()) {
            _sentences.Enqueue(_sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (_sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string _sentence = _sentences.Dequeue();
        Debug.Log(_sentence);
    }

    void EndDialogue() {
        Debug.Log("End of conversation");
    }
}
