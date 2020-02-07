using UnityEngine;
using UnityEngine.UI;

public class ZoomUI : MonoBehaviour
{
	public bool active = false;

	SwapCamera swapCamera { get; set; }

	public void Enter(SwapCamera swapCamera) {
		this.swapCamera = swapCamera;
		if (active)
			gameObject.SetActive(true);
	}

	public void Exit() {
		swapCamera.Exit();
	}
}
