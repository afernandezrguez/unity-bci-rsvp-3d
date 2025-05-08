using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Debug = UnityEngine.Debug;

public class VoiceController : MonoBehaviour
{
    [SerializeField] private Button startButton, setConfigButton;
    KeywordRecognizer keywordRecognizer;

    Dictionary<string, Action> wordToAction;

    // Start is called before the first frame update
    void Start()
    {
        wordToAction = new Dictionary<string, Action>();
        wordToAction.Add("preparar prueba", ReadyBCI2000);
        wordToAction.Add("comenzar prueba", StartBCI2000);

        keywordRecognizer = new KeywordRecognizer(wordToAction.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += WordRecognized;
        keywordRecognizer.Start();
    }

    private void WordRecognized(PhraseRecognizedEventArgs word)
    {
        Debug.Log(word.text);
        wordToAction[word.text].Invoke();
    }

    private void ReadyBCI2000()
    {
        Debug.Log("Has selecionado el comando: PREPARAR");
        setConfigButton.onClick.Invoke();
    }

    private void StartBCI2000()
    {
        Debug.Log("Has selecionado el comando: COMENZAR");
        startButton.onClick.Invoke();
    }

}