using UnityEngine;
using System.Collections;

public class anim : MonoBehaviour {
	int x;
	SkinnedMeshRenderer skinnedMeshRenderer;
	// Use this for initialization
	void Start () {
		x = 0;
		skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

	}
	
	// Update is called once per frame
	void Update () { 
		x = x + 1;
		skinnedMeshRenderer.SetBlendShapeWeight(1, x);
		skinnedMeshRenderer.SetBlendShapeWeight(2, x);
	
	}
}
