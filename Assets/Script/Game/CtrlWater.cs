using UnityEngine;
using System.Collections;

public class CtrlWater : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Renderer renderer = GetComponent<Renderer>();
		Vector2 offset = renderer.sharedMaterial.mainTextureOffset;
		offset.x = Mathf.Sin(Time.time * 2) * 0.01f;
		offset.y = Mathf.Repeat(offset.y - 0.001f, 1.0f);
		renderer.sharedMaterial.mainTextureOffset = offset;
		renderer.sharedMaterial.SetTextureOffset("_BumpMap", offset);
	}
}
