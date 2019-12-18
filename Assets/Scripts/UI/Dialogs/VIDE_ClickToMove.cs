using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class VIDE_ClickToMove : MonoBehaviour
{
	public static VIDE_ClickToMove Instance;

	NavMeshAgent m_Agent;
	RaycastHit m_HitInfo = new RaycastHit();

	private void Awake() {
		Instance = this;
	}

	void Start() {
		m_Agent = GetComponent<NavMeshAgent>();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift)) {
			if (!EventSystem.current.IsPointerOverGameObject()) {
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
					m_Agent.destination = m_HitInfo.point;
			}
		}

	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Others"))) {
			Debug.Log(other);
			other.gameObject.GetComponentInChildren<VD_Trigger>().Run();
			//FluentManager.Instance.ExecuteClosestAction(gameObject);
		}

	}

	//public void OnDialogStart() {
	//	gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
	//}

	//public void OnDialogFinish() {
	//	gameObject.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
	//}
}
