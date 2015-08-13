
using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Text;
using System.IO;

public class jsonParse : MonoBehaviour {
	int snippetCount = 0;
	int num_action = 0;
	string[] snippetArray = new string[10];
	string metadata;
	SkinnedMeshRenderer skinnedMeshRenderer;
	string text = "";
	float timer;
	float time;
	float[] blendshapes = new float[20]; //array holds goal values for all 
	private string Load(string fileName)
	{
		// Handle any problems that might arise when reading the text
		string line;
		string output = "";
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
					//concatenante each line to the output string
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
		skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		metadata = Load ("Assets/SampleMetadata.JSON");
		string s_try = Load("Assets/SamplePayload.JSON");
		//Debug.Log (s_try);
		var jsonpacket = JSON.Parse(s_try);
		num_action = jsonpacket ["content"].Count;
		Debug.Log ("How many actions ? " + num_action);
		for (int i = 0; i<num_action; i++) {
			snippetArray [i] = jsonpacket ["content"] [i].ToString();
		}
		//snippetArray[0] = Load("Assets/SamplePayload1.JSON");
		//snippetArray[1] = Load("Assets/SamplePayload2.JSON");
		//snippetArray[2] = Load("Assets/SamplePayload3.JSON");

		string s = snippetArray [snippetCount];
		Debug.Log (s);
		snippetCount++;
		//Debug.Log (metadata);
		yield return StartCoroutine (handler (s,metadata));
		//yield return new WaitForSeconds(5);
		//yield return StartCoroutine (handler (s1));
	}

	IEnumerator handler(string new_str, string meta){
		Debug.Log ("JSON PARSER");
		var packet = JSON.Parse(new_str);
		var metapacket = JSON.Parse(meta);
		timer = 0;

		//JSON PACKET PARSING
		var packetType = packet["ptype"].Value;
		text = packet["text"].Value;
		var type = packet["type"].Value;
		time = packet["time"].AsInt;

		var angry = packet["emotions"]["anger"].AsFloat;
		var disgust = packet["emotions"]["disgust"].AsFloat;
		var happy = packet["emotions"]["happy"].AsFloat;
		var sad = packet["emotions"]["sad"].AsFloat;
		var scared = packet["emotions"]["scared"].AsFloat;
		float[] emotions = new float[5];
		emotions [0] = happy;
		emotions [1] = sad;
		emotions [2] = angry;
		emotions [3] = scared;
		emotions [4] = disgust;

		Debug.Log ("text:" + text + 
		           " time:" + time + 
		           " angry:" + angry + 
		           " disgust:" + disgust + 
		           " happy:" + happy + 
		           " sad:" + sad + 
		           " scared:" + scared + "\n");

		//JSON METADATA PARSING
		//get all weight values of every key and average the non-zero values

		for (int j = 8; j < 28; j++) {
			blendshapes[j-8] = 0; //metadata file starts at 8 to skip initial name,id,age etc. metadata
			var count = 0;
			for (int i = 0; i < 5; i++) {
				if(metapacket ["profiles"] [0] [j] [i].AsFloat * emotions[i] > 0){
					blendshapes[j-8] = blendshapes[j-8] + (metapacket ["profiles"] [0] [j] [i].AsFloat * emotions[i]);
					count++;
				}
			}
			if(count >0){
				blendshapes[j-8] = blendshapes[j-8] / count;
			}
		}

		//debug log code
		/*for (int i = 0; i < 20; i++) {
			Debug.Log (blendshapes [i]);
		}
		*/
		if (packetType == "snippet") {
			Audio audio = new Audio ();
			AudioSource source = gameObject.GetComponent<AudioSource> ();
			if (type == "enhance") {
				yield return StartCoroutine (audio.Play (text, true, source));
			} else
				yield return StartCoroutine (audio.Play (text, false, source));
		}
	}

	GUIStyle text_style = new GUIStyle ();
	void OnGUI() {
		GUI.contentColor = Color.black;
		
		//GUI.Button (new Rect ((Screen.width)/2, 15, 100, 50), "Play");
		text_style.fontSize = 20;
		GUI.Label(new Rect ((Screen.width)/2-55 ,(Screen.height)-40,(Screen.width)/4,(Screen.height)/4), text , text_style);
	}

	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 20; i++) {
			if(skinnedMeshRenderer.GetBlendShapeWeight(i) < blendshapes[i]){
				skinnedMeshRenderer.SetBlendShapeWeight(i, skinnedMeshRenderer.GetBlendShapeWeight(i)+1);
			}
			if(skinnedMeshRenderer.GetBlendShapeWeight(i) > blendshapes[i]){
				skinnedMeshRenderer.SetBlendShapeWeight(i, skinnedMeshRenderer.GetBlendShapeWeight(i)-1);
			}
		}
		if (timer < time) {
			timer = timer + 33;
			//Debug.Log (timer);
		} 
		else {
			//Debug.Log ("fetching next snippet");
			//Debug.Log (snippetCount);
			if(snippetCount<num_action){
				timer = 0;
				string s = snippetArray [snippetCount];
				snippetCount++;
				StartCoroutine (handler (s,metadata));
			}
		}
	}
}
