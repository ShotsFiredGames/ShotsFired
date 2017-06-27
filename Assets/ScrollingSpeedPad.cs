using UnityEngine;
using System.Collections;

public class ScrollingSpeedPad : MonoBehaviour
{
	public Vector2 offset;
	Renderer rend;

	void Start() 
	{
		rend = GetComponent<Renderer>();
	}

	void Update() 
	{
		rend.material.SetTextureOffset("_MainTex", offset * Time.time);
	}
}