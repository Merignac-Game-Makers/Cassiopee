using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ZoomUI : MonoBehaviour
{
	public bool active = false;
	public Image cache, frame;

	SwapCamera swapCamera { get; set; }
	private void Start() {
		cache.alphaHitTestMinimumThreshold = 0.5f;
		frame.alphaHitTestMinimumThreshold = 0.5f;
		
	}

	public void Enter(SwapCamera swapCamera) {
		this.swapCamera = swapCamera;
		if (active)
			gameObject.SetActive(true);
	}

	public void Exit() {
		swapCamera.Exit();
	}
}
