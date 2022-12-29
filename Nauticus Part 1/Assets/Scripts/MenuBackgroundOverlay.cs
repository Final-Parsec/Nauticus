﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuBackgroundOverlay : MonoBehaviour {
	private Image image;

	// Use this for initialization
	void Start () {
		this.image = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Fade(double start, double end, double length)
	{
		Fade ((float)start, (float)end, (float)length);
	}
	
	public void Fade(float start, float end, float length)
	{
		StartCoroutine(FadeCoroutine(start, end, length));
	}
	
	private IEnumerator FadeCoroutine(float start, float end, float length)
	{
		if (this.image.color.a == start)
		{		
			for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/length)) { //for the length of time
				Color colorT = image.color;
				
				colorT.a = Mathf.Lerp(start, end, i); //lerp the value of the transparency from the start value to the end value in equal increments				
				yield return new WaitForSeconds(.001f);
				
				
				image.color = colorT;		
			}	
			Color colorEnd = image.color;
			colorEnd.a = end;  // ensure the fade is completely finished (because lerp doesn't always end on an exact value)\			
			image.color = colorEnd;
		}
	}
}
