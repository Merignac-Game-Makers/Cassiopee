using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{

	float defaultInteractionDistance = 1.5f;

	InventoryUI m_InventoryUI;

	public static PlayerControl Instance { get; protected set; }

	// camera
	Camera m_MainCamera;

	// navigation
	[HideInInspector]
	public NavMeshAgent m_Agent;
	//NavMeshPath m_CalculatedPath;
	bool MoveAcrossNavMeshesStarted = false;

	// Interactions
	InteractableObject m_TargetInteractable = null;
	HighlightableObject m_Highlighted;
	Collider m_TargetCollider;
	CharacterData m_CurrentTargetCharacterData = null;
	[HideInInspector]
	public InventoryUI.DragData m_CurrentlyDragged = null;

	// CharacterData
	[HideInInspector]
	public CharacterData m_CharacterData;

	// Raycast
	RaycastHit[] m_RaycastHitCache = new RaycastHit[16];
	int m_TargetLayer;
	int m_InteractableLayer;
	Vector3 m_LastRaycastResult;


	void Awake() {
		Instance = this;
		m_MainCamera = Camera.main;
	}

	// Start is called before the first frame update
	void Start() {
		m_InventoryUI = InventoryUI.Instance;

		m_CharacterData = GetComponent<CharacterData>();
		m_CharacterData.Init();

		m_Agent = GetComponent<NavMeshAgent>();

		//m_CalculatedPath = new NavMeshPath();

		m_InteractableLayer = 1 << LayerMask.NameToLayer("Interactable");
		m_TargetLayer = 1 << LayerMask.NameToLayer("Target");

		m_LastRaycastResult = transform.position;

	}

	// Update is called once per frame
	void Update() {
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit m_HitInfo = new RaycastHit();

		// test if we are approaching an interractable object
		if (m_TargetInteractable != null) {
			CheckInteractableRange();
		}

		// item that we are currently dragging
		m_CurrentlyDragged = m_InventoryUI.CurrentlyDragged;

		// zoom
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if (!Mathf.Approximately(mouseWheel, 0.0f)) {
			Vector3 view = m_MainCamera.ScreenToViewportPoint(Input.mousePosition);
			if (view.x > 0f && view.x < 1f && view.y > 0f && view.y < 1f)
				CameraController.Instance.Zoom(-mouseWheel * Time.deltaTime * 40.0f);
		}

		//if we click the mouse button, we clear any previous targets
		if (Input.GetMouseButtonDown(0)) {
			m_CurrentTargetCharacterData = null;
			m_TargetInteractable = null;
		}

		// if pointer is NOT over UI
		if (!EventSystem.current.IsPointerOverGameObject()) {
			//Raycast to find object currently under the mouse cursor
			ObjectsRaycasts(screenRay);

			// if we are NOT currently dragging an item
			if (m_CurrentlyDragged == null) {
				// on mouse click
				if (Input.GetMouseButton(0)) {
					if (m_TargetInteractable == null && m_CurrentTargetCharacterData == null) {
						// click on an interractable item ?
						InteractableObject obj = m_Highlighted as InteractableObject;
						if (obj) {
							if (obj.GetComponentInChildren<Loot>() || obj.GetComponentInChildren<Target>() || obj.GetComponentInChildren<PNJ>()) {
								obj.Clicked = true;
								InteractWith(obj);
							}  else if (obj.GetComponentInChildren<Activable>())
								Activate(obj as Activable);

							// click on player ?
						} else {
							CharacterData data = m_Highlighted as CharacterData;
							if (data) {
								m_CurrentTargetCharacterData = data;
								// or just move ?
							} else {
								//MoveCheck(screenRay);
								if (Physics.Raycast(screenRay.origin, screenRay.direction, out m_HitInfo))
									m_Agent.destination = m_HitInfo.point;
							}
						}
					}
				}
				// if we are dragging an item
			}
		}

		// control speed on NavMesh Links
		if (m_Agent.isOnOffMeshLink && !MoveAcrossNavMeshesStarted) {
			MoveAcrossNavMeshesStarted = true;
			StartCoroutine(MoveAcrossNavMeshLink(m_Agent.destination));
		}

	}

	void ObjectsRaycasts(Ray screenRay) {
		bool somethingFound = false;

		//first check for interactable Object
		int count = Physics.SphereCastNonAlloc(screenRay, .2f, m_RaycastHitCache, 1000.0f, m_InteractableLayer);
		if (count > 0) {
			for (int i = 0; i < count; ++i) {
				InteractableObject obj = m_RaycastHitCache[i].collider.gameObject.GetComponentInParent<InteractableObject>();
				if (obj != null && obj.IsInteractable) {
					SwitchHighlightedObject(obj);
					somethingFound = true;
					break;
				}
			}
		}
		//else {
		//    count = Physics.SphereCastNonAlloc(screenRay, 1.0f, m_RaycastHitCache, 1000.0f, m_TargetLayer);
		//    if (count > 0) {
		//        CharacterData data = m_RaycastHitCache[0].collider.GetComponentInParent<CharacterData>();
		//        if (data != null) {
		//            SwitchHighlightedObject(data);
		//            somethingFound = true;
		//        }
		//    }
		//}

		//second check for target (where to drop item)
		count = Physics.SphereCastNonAlloc(screenRay, 1.0f, m_RaycastHitCache, 1000.0f, m_TargetLayer);
		if (count > 0) {
			for (int i = 0; i < count; ++i) {
				InteractableObject obj = m_RaycastHitCache[0].collider.GetComponentInParent<Target>();
				if (obj != null && obj.IsInteractable) {
					SwitchHighlightedObject(obj);
					somethingFound = true;
					break;
				}
			}
		}

		if (!somethingFound && m_Highlighted != null) {
			SwitchHighlightedObject(null);
		}
	}

	private void OnTriggerEnter(Collider other) {
		m_TargetInteractable = other.gameObject.GetComponent<InteractableObject>();
		if (m_TargetInteractable!=null && !m_TargetInteractable.IsInteractable) m_TargetInteractable = null;
		if (m_TargetInteractable != null)
			m_TargetCollider = m_TargetInteractable.GetComponentInChildren<Collider>();
	}

	/*
        void MoveCheck(Ray screenRay) {
            if (m_CalculatedPath.status == NavMeshPathStatus.PathComplete) {
                m_Agent.SetPath(m_CalculatedPath);
                m_CalculatedPath.ClearCorners();
            }

            if (Physics.RaycastNonAlloc(screenRay, m_RaycastHitCache, 1000.0f, m_LevelLayer) > 0) {
                Vector3 point = m_RaycastHitCache[0].point;
                //avoid recomputing path for close enough click
                if (Vector3.SqrMagnitude(point - m_LastRaycastResult) > 1.0f) {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(point, out hit, 0.5f, NavMesh.AllAreas)) {//sample just around where we hit, avoid setting destination outside of navmesh (ie. on building)
                        m_LastRaycastResult = point;
                        m_Agent.SetDestination(hit.position);
                        m_Agent.CalculatePath(hit.position, m_CalculatedPath);
                    }
                }
            }
        }
    */
	void SwitchHighlightedObject(HighlightableObject obj) {
		if (m_Highlighted != null) {
			var a = m_Highlighted as Activable;
			if (!a || !a.IsActive)
				m_Highlighted.Highlight(false);
		}
		m_Highlighted = obj;
		if (m_Highlighted) {
			var a = m_Highlighted as Activable;
			if (!a || !a.IsActive)
				m_Highlighted.Highlight(true);
		}
	}


	void CheckInteractableRange() {

		Vector3 distance = m_TargetCollider.ClosestPointOnBounds(transform.position) - transform.position;

		if ((m_TargetInteractable.m_Mode!=InteractableObject.mode.onClick || m_TargetInteractable.Clicked) && distance.sqrMagnitude < defaultInteractionDistance * defaultInteractionDistance) {
			//StopAgent();
			m_TargetInteractable.InteractWith(m_CharacterData);
			m_TargetInteractable = null;
		}
	}

	public void InteractWith(InteractableObject obj) {
		if (obj.IsInteractable) {
			m_TargetCollider = obj.GetComponentInChildren<Collider>();
			m_TargetInteractable = obj;
			m_Agent.SetDestination(obj.transform.position);
		}
	}
	public void Activate(Activable obj) {
		if (obj.IsInteractable) {
			Debug.Log("Item activated");
			m_TargetCollider = obj.GetComponentInChildren<Collider>();
			m_TargetInteractable = obj;
			if (obj.IsInteractable)
				obj.Toggle();
		}
	}

	public void StopAgent() {
		m_Agent.ResetPath();
		m_Agent.velocity = Vector3.zero;
	}

	// control speed on NavMesh Links
	IEnumerator MoveAcrossNavMeshLink(Vector3 destination) {
		OffMeshLinkData data = m_Agent.currentOffMeshLinkData;
		m_Agent.updateRotation = false;

		Vector3 startPos = m_Agent.transform.position;
		Vector3 endPos = data.endPos + Vector3.up * m_Agent.baseOffset;
		float duration = (endPos - startPos).magnitude / m_Agent.speed;
		float t = 0.0f;
		float tStep = 1.0f / duration;
		while (t < 1.0f) {
			transform.position = Vector3.Lerp(startPos, endPos, t);
			m_Agent.destination = transform.position;
			t += tStep * Time.deltaTime;
			yield return null;
		}
		transform.position = endPos;
		m_Agent.updateRotation = true;
		m_Agent.CompleteOffMeshLink();
		MoveAcrossNavMeshesStarted = false;
		m_Agent.destination = destination;
	}
}
