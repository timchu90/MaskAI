using UnityEngine;
using System.Collections;
using SimpleJSON;
public class jsonParse : MonoBehaviour {
	string text = "";
	string s = 
		"{\"ptype\":\"snippet\"," +
			"\"text\":\"I trusted her\", "   +
			"\"type\":\"statement\"," +
			"\"time\":1200," +
			"\"emotions\":{\"anger\":0.5 , \"disgust\":0.6, \"happy\":0.0, \"sad\":0.3, \"scared\":0.0}}";
	string s1 = 
		"{\"ptype\":\"snippet\"," +
			"\"text\":\"I reeeally reeeally trusted her\", "   +
			"\"type\":\"statement\"," +
			"\"time\":1200," +
			"\"emotions\":{\"anger\":0.5 , \"disgust\":0.6, \"happy\":0.0, \"sad\":0.3, \"scared\":0.0}}";

	// Use this for initialization
	IEnumerator Start () {
		yield return StartCoroutine (handler (s));
		yield return StartCoroutine (handler (s1));
	}

	IEnumerator handler(string new_str){
		Debug.Log ("JSON PARSER");
		var packet = JSON.Parse(new_str);
		var packetType = packet["ptype"].Value;
		text = packet["text"].Value;
		var type = packet["type"].Value;
		var time = packet["time"].AsInt;
		var anger = packet["emotions"]["anger"].AsFloat;
		var disgust = packet["emotions"]["disgust"].AsFloat;
		var happy = packet["emotions"]["happy"].AsFloat;
		var sad = packet["emotions"]["sad"].AsFloat;
		var scared = packet["emotions"]["scared"].AsFloat;
		Debug.Log ("text:" + text + 
		           " time:" + time + 
		           " anger:" + anger + 
		           " disgust:" + disgust + 
		           " happy:" + happy + 
		           " sad:" + sad + 
		           " scared:" + scared + "\n");
		Audio audio = new Audio ();
		AudioSource source = gameObject.GetComponent<AudioSource> ();
		yield return StartCoroutine(audio.Play( text, false , source));
	}

	GUIStyle text_style = new GUIStyle ();
	void OnGUI() {
		GUI.contentColor = Color.black;
		
		//GUI.Button (new Rect ((Screen.width)/2, 15, 100, 50), "Play");
		text_style.fontSize = 20;
		GUI.Label(new Rect ((Screen.width)/2-50 ,(Screen.height)-40,(Screen.width)/4,(Screen.height)/4), text , text_style);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
