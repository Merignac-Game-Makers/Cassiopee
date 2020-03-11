using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacesUI : MonoBehaviour
{
	public static PlacesUI Instance;

	public float swipeDistanceThreshold = 25;
	public bool isSwiping { get; private set; }
	public PlaceCamera currentPlace { get; set; }

	private Vector2 startTouch;
	private bool swipeLeft;
	private bool swipeRight;


	private void Awake() {
				Instance = this;
	}

	public void Init() {
		gameObject.SetActive(false);
	}


	void Update() {

		DetectSwipe();

		if (currentPlace) {
			if (swipeLeft) {              // swipe à gauche ?
				currentPlace.TurnLeft();
			} else if (swipeRight) {      // swipe à droite ?
				currentPlace.TurnRight();
			}
		}
	}

	public void SetCurrentPlace(PlaceCamera place) {
			gameObject.SetActive(place != null);
			currentPlace = place;
	}

	/// <summary>
	/// Détecter un geste Swipe
	/// </summary>
	private void DetectSwipe() {
		#region Standalone Inputs
		if (Input.GetMouseButtonDown(0)) {
			startTouch = Input.mousePosition;
		} else if (Input.GetMouseButtonUp(0)) {
			AnalyzeGesture((Vector2)Input.mousePosition - startTouch);
		}
		if (Input.GetMouseButton(0)) {
			isSwiping = (Vector2)Input.mousePosition != startTouch;
		}
		#endregion

		#region Mobile Input
		if (Input.touches.Length > 0) {
			if (Input.touches[0].phase == TouchPhase.Began) {
				startTouch = Input.touches[0].position;
			} else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
				AnalyzeGesture(Input.touches[0].position - startTouch);
			}
			isSwiping = Input.touches[0].position != startTouch;
		}
		#endregion

	}

	/// <summary>
	/// vérifier que le mouvement est suffisamment ample
	/// </summary>
	/// <param name="swipeDelta">mouvement sur l'écran</param>
	private void AnalyzeGesture(Vector2 swipeDelta) {
		// Distance
		if (Mathf.Abs(swipeDelta.x) > swipeDistanceThreshold) {
			swipeLeft = swipeDelta.x < 0;
			swipeRight = swipeDelta.x > 0;
		} else {
			swipeLeft = swipeRight = false;
		}
		startTouch = Vector2.zero;
	}

	public void Reset() {
		swipeLeft = swipeRight = isSwiping = false;
	}


	public void TurnLeft() {
		if (currentPlace) {
			currentPlace.TurnLeft();
		}
	}

	public void TurnRight() {
		if (currentPlace) {
			currentPlace.TurnRight();
		}
	}
}
