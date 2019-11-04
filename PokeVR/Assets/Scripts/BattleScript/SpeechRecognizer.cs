using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;

public class SpeechRecognizer : MonoBehaviour {
    public TextAsset moveNames;
    List<string> list = new List<string>();
    private KeywordRecognizer keywordRecognizer;
    public GameObject yourPoke;

	// Use this for initialization
	void Start () {
        moveNames = Resources.Load<TextAsset>("PokeMoveDescript");
        string[] rows = moveNames.text.Split(new char[] { '\n' });
        for (int i = 0; i < rows.Length; i++)
        {
            list.Add(rows[i].Substring(0, rows[i].IndexOf(',')));
        }
        keywordRecognizer = new KeywordRecognizer(list.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        //manager = GameObject.FindGameObjectWithTag("GameController");
	}
	
    void RecognizedSpeech(PhraseRecognizedEventArgs eventArgs)
    {
        yourPoke.GetComponent<MoveClassifier>().DataExtracter(eventArgs.text);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
