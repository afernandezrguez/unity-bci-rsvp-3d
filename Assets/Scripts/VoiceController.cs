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
    [SerializeField] private Button setConfigButton, startButton, stopButton;
    KeywordRecognizer keywordRecognizer;

    Dictionary<string, Action> wordToAction;

    // Start is called before the first frame update
    void Start()
    {
        wordToAction = new Dictionary<string, Action>
        {
            { "configurar", ReadyBCI2000 },
            { "comenzar", StartBCI2000 },
            { "parar", StopBCI2000 }
        };

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
        //Debug.Log("Has selecionado el comando: PREPARAR PRUEBA");
        setConfigButton.onClick.Invoke();
        FindObjectOfType<BringToFront>()?.BringUnityToFront();
    }

    private void StartBCI2000()
    {
        //Debug.Log("Has selecionado el comando: COMENZAR PRUEBA");
        startButton.onClick.Invoke();
    }

    private void StopBCI2000()
    {
        //Debug.Log("Has selecionado el comando: DETENER PRUEBA");
        stopButton.onClick.Invoke();
    }
}