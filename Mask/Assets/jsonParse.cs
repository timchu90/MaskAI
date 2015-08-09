using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Text;
using System.IO;

public class jsonParse : MonoBehaviour {
	int x;
	SkinnedMeshRenderer skinnedMeshRenderer;
	string output;
	string text = "";
	/*
	string s = 
		"{\"ptype\":\"snippet\"," +
			"\"text\":\"I trusted her\", "   +
			"\"type\":\"statement\"," +
			"\"time\":0," +
			"\"emotions\":{\"anger\":0.5 , \"disgust\":0.6, \"happy\":0.0, \"sad\":0.3, \"scared\":0.0}}";
	string s1 = 
		"{\"ptype\":\"snippet\"," +
			"\"text\":\"I reeeally reeeally trusted her\", "   +
			"\"type\":\"statement\"," +
			"\"time\":0," +
			"\"emotions\":{\"anger\":0.5 , \"disgust\":0.6, \"happy\":0.0, \"sad\":0.3, \"scared\":0.0}}";
	*/
	private string Load(string fileName)
	{
		// Handle any problems that might arise when reading the text
		string line;
		// Create a new StreamReader, tell it which file to read and what encoding the file
		// was saved as
		StreamReader theReader = new StreamReader(fileName, Encoding.Default);
		
		// Immediately clean up the reader after this block of code is done.
		// You generally use the "using" statement for potentially memory-intensive objects
		// instead of relying on garbage collection.
		// (Do not confuse this with the using directive for namespace at the 
		// beginning of a class!)
		using (theReader)
		{
			// While there's lines left in the text file, do this:
			do
			{
				line = theReader.ReadLine();
				
				if (line != null)
				{
					// Do whatever you need to do with the text line, it's a string now
					// In this example, I split it into arguments based on comma
					// deliniators, then send that array to DoStuff()
					output = string.Concat (output,line);
				}
			}
			while (line != null);
			
			// Done reading, close the reader and return true to broadcast success    
			theReader.Close();
		}
		return output;
	}

	// Use this for initialization
	IEnumerator Start () {
		x = 0;
		skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		string metadata = Load ("Assets/SampleMetadata.JSON");
		string s = Load ("Assets/SamplePayload.JSON");
		//Debug.Log (metadata);
		var metapacket = JSON.Parse (metadata);
		yield return StartCoroutine (handler (s));
		//yield return new WaitForSeconds(5);
		//yield return StartCoroutine (handler (s1));
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
//		yield return StartCoroutine(audio.Play( text, false));
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
		x = x + 1;
		skinnedMeshRenderer.SetBlendShapeWeight(1, x);
		skinnedMeshRenderer.SetBlendShapeWeight(2, x);
	}
}
