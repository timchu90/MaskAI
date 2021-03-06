using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

public class Audio : MonoBehaviour {

	public string lang = "en";
	//public bool man = false;
	public string final_word = "I ";
	public string word1 = "really really";
	public string word2 = "hate you";
	public float vol = 0.7f;
	public bool word_bold = false;
	public bool word1_bold = true;
	public bool word2_bold = false;
	//tl = lang , q = word
	// Use this for initialization




	public IEnumerator Play(string word , bool bold, AudioSource source){
		final_word = word;
		Debug.Log ("I am here 2!"); 
		// Remove the "spaces" in excess

		Regex rgx = new Regex ("\\s+");
		// Replace the "spaces" with "% 20" for the link Can be interpreted
		string result = rgx.Replace (word, "%20");
//		Debug.Log (word + result); 
//		string url = "http://translate.google.com/translate_tts?tl=" + lang + "&q=" + result;
		string url = "http://tts-api.com/tts.mp3?q=" + result;
		Debug.Log (url);
		WWW www = new WWW (url);
		while (!www.isDone) {
//			Debug.Log ("DONE?");
			yield return www;
		}



		//AudioClip webclip = www.audioClip;
		AudioClip webclip = www.GetAudioClip (false, true, AudioType.MPEG);
		while (!webclip.isReadyToPlay)
		{
			Debug.Log( "Not Ready !");
			yield return null;
		}

		source.clip = webclip;


		source.volume = vol;
		
		if (bold) {
			source.pitch = 0.95f;
			source.volume = vol + 0.3f;
		} else {
			source.pitch = 1f;
			source.volume = vol;
		}
		//source.PlayDelayed (10); 
		source.Play (); 
		while (source.isPlaying) {
			// do nothing and keep returning while audio is still playing
			yield return null;
		}

		source.Stop();

	}



	// Update is called once per frame
	void Update () {
		
	}
}
