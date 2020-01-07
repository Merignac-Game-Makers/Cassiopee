using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/// <summary>
/// Gestion du personnage joueur
///		- Déplacements
///		- Intéractions
/// </summary>
public class PlayerManager : MonoBehaviour
{

	float defaultInteractionDistance = 1.5f;						// distance en deça de laquelle on déclenche les intéractions

	InventoryUI m_InventoryUI;										// gestionnaire d'inventaire
	MagicController m_MagicController;								// gestionnaire de magie

	public static PlayerManager Instance { get; protected set; }	// instance statique de cette classe

	// camera
	Camera mainCamera;												// caméra active

	// navigation
	[HideInInspector]
	public NavMeshAgent m_Agent;									// agent de navigation
	bool MoveAcrossNavMeshesStarted = false;						// flag : est-on sur un nav mesh link ? (pour gérer la vitesse)

	// Interactions
	InteractableObject m_TargetInteractable = null;					// objet avec lequel le joueur intéragit
	HighlightableObject m_Highlighted;								// objet en surbrillance  sous le pointeur de la souris
	Collider m_TargetCollider;										// collider de l'objet en cours d'intéraction
	CharacterData m_CurrentTargetCharacterData = null;				// caractéristiques du PNJ en intéraction
	[HideInInspector]	
	public InventoryUI.DragData m_InvItemDragging = null;			// objet d'inventaire en cours de drag & drop
	[HideInInspector]
	public MagicOrb m_MagicOrb = null;								// orbe magique

	// CharacterData
	[HideInInspector]
	public CharacterData m_CharacterData;							// caractéristiques du joueur (santé, force...)

	// Raycast
	RaycastHit[] m_RaycastHitCache = new RaycastHit[16];            // cache des résultats de lancer de rayon
	RaycastHit m_HitInfo = new RaycastHit();                        // résultat unitaire du lancer de rayon
	int m_InteractableLayer;                                        // layer des objets intéractibles
	int m_PlayerLayer;												// layer du personnage


	#region Initialisation
	void Awake() {
		Instance = this;											// création de l'instance statique
		mainCamera = Camera.main;									// initialisation de la caméra
	}

	// Start is called before the first frame update
	void Start() {
		m_InventoryUI = InventoryUI.Instance;						// gestionnaire d'inventaire
		m_MagicController = MagicController.Instance;				// gestionnaire de magie

		m_CharacterData = GetComponent<CharacterData>();			// caractéristiques du joueur
		m_CharacterData.Init();										// ... initialisation

		m_Agent = GetComponent<NavMeshAgent>();                     // préparation de la navigation

		m_InteractableLayer = 1 << LayerMask.NameToLayer("Interactable");       // layer des objets intéractibles
		m_PlayerLayer = 1 << LayerMask.NameToLayer("Player");					// layer des objets intéractibles
	}
	#endregion


	// Update is called once per frame
	void Update() {
		// quitter le jeu par la touche escape
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}


		// Lancer de rayon
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);

		// le collider a-t-il détecté un objet intéractible ?
		if (m_TargetInteractable != null) {
			CheckInteractableRange();
		}

		// récupération des objets en cours de drag & drop 
		m_InvItemDragging = m_InventoryUI.CurrentlyDragged;			// objet d'inventaire
		m_MagicOrb = m_MagicController.dragging;                    // orbe magique

		// zoom
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if (!Mathf.Approximately(mouseWheel, 0.0f)) {
			Vector3 view = mainCamera.ScreenToViewportPoint(Input.mousePosition);
			if (view.x > 0f && view.x < 1f && view.y > 0f && view.y < 1f)
				CameraController.Instance.Zoom(-mouseWheel * Time.deltaTime * 40.0f);
		}

		// au début d'u clic, on commence par effacer tous les objets en cours d'intéraction
		if (Input.GetMouseButtonDown(0)) {
			m_CurrentTargetCharacterData = null;
			m_TargetInteractable = null;
		}

		// Gestion de la souris
		if (!EventSystem.current.IsPointerOverGameObject()) {		// éviter de déplacer le personnage si on pointe sur un objet d'interface
			
			ObjectsRaycasts(screenRay);								// Trouver les objets sous la souris & mettre en surbrillance les objets intéractibles
			
			if (m_InvItemDragging == null && m_MagicController.dragging == null) { // éviter de déplacer le personnage si on est en cours de drag & drop

				// si le bouton de la souris est appuyé
				if (Input.GetMouseButton(0)) {
					if (m_TargetInteractable == null && m_CurrentTargetCharacterData == null) {		// s'il n'y a pas d'intéraction en cours
								if (m_InventoryUI.isOn)												// refermer automatiquement l'inventaire
									UIManager.Instance.inventoryButton.Toggle();
						
						InteractableObject obj = m_Highlighted as InteractableObject;				
						if (obj) {                                                                  // si on a cliqué sur un objet intéractible
							if (obj.GetComponentInChildren<Activable>()) {
								Activate((Activable) obj);                                          //		si c'est un 'activable' => activer
							} else {
								obj.Clicked = true;													//		sinon, c'est un 'intéractible' => intéragir
								InteractWith(obj);
							}

							
						} else {
							CharacterData data = m_Highlighted as CharacterData;
							if (data) {																// si on a cliqué sur le personnage
								m_CurrentTargetCharacterData = data;								// pour l'instant on ne fait rien mais l'événement est détecté
																									// il pourrait être utilisé pour afficher un panneau de statistiques ou tout autre chose
							} else {								// sinon => navigation
								if (Physics.Raycast(screenRay.origin, screenRay.direction, out m_HitInfo))
									m_Agent.destination = m_HitInfo.point;
							}
						}
					}
				}
			}
		}

		// controler la vitesse sur les NavMesh Links (par défaut elle est trop rapide)
		if (m_Agent.isOnOffMeshLink && !MoveAcrossNavMeshesStarted) {
			MoveAcrossNavMeshesStarted = true;
			StartCoroutine(MoveAcrossNavMeshLink(m_Agent.destination));
		}

	}

	/// <summary>
	/// Recherche des objets interactibles sous le pointeur de souris
	/// </summary>
	/// <param name="screenRay">lancer de rayon</param>
	void ObjectsRaycasts(Ray screenRay) {
		bool somethingFound = false;

		// check for interactable Object
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

		// check for player
		count = Physics.SphereCastNonAlloc(screenRay, .2f, m_RaycastHitCache, 1000.0f, m_PlayerLayer);
		if (count > 0) {
			for (int i = 0; i < count; ++i) {
				CharacterData obj = m_RaycastHitCache[i].collider.gameObject.GetComponentInParent<CharacterData>();
				if (obj != null) {
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

	/// <summary>
	/// bascule de surbrillance
	/// le material de l'objet doit utiliser un shader comportant une 'rim color'
	/// </summary>
	/// <param name="obj"></param>
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

	#region Intéractions
	/// <summary>
	/// détection de collision avec les objets intéractibles
	/// Si l'objet est un intéractible au statut 'actif'
	///		=> m_TargetInteractable contient l'objet
	///		=> m_TargetCollider contient son collider
	/// </summary>
	/// <param name="other">objet rencontré</param>
	private void OnTriggerEnter(Collider other) {
		m_TargetInteractable = other.gameObject.GetComponent<InteractableObject>();
		if (m_TargetInteractable!=null) {					// si l'objet rencontré est un 'intéractible'
			if (!m_TargetInteractable.IsInteractable) {		//		si son statut est 'inactif'
				m_TargetInteractable = null;				//			on annule la détection
			} else {										//		si son statut est 'actif'
				m_TargetCollider = m_TargetInteractable.GetComponentInChildren<Collider>();	// on mémorise son collider
			}
		}
	}

	/// <summary>
	/// Comparaison de la distance d'un objet intéractible avec la distance de déclenchement des intéractions
	/// </summary>
	void CheckInteractableRange() {
		Vector3 distance = m_TargetCollider.ClosestPointOnBounds(transform.position) - transform.position;	// calcul de la distance

		// on déclenche l'intéraction si
		//		la distance est inférieure à la distance de déclenchement
		//	ET
		//		l'objet est en mode 'onClick' ET il a été cliqué
		//		OU
		//		l'objet est en mode 'onTheFly'
		if ((m_TargetInteractable.mode!=InteractableObject.Mode.onClick || m_TargetInteractable.Clicked)	 
			&& distance.sqrMagnitude < defaultInteractionDistance * defaultInteractionDistance) {
			m_TargetInteractable.InteractWith(m_CharacterData);			// déclencher l'intéraction
			m_TargetInteractable = null;                                // supprimer la détection pour éviter de redoubler l'intéraction
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
			// Debug.Log("Item activated");
			m_TargetCollider = obj.GetComponentInChildren<Collider>();
			m_TargetInteractable = obj;
			if (obj.IsInteractable)
				obj.Toggle();
		}
	}
	#endregion

	/// <summary>
	/// interrompre la navigation
	/// </summary>
	public void StopAgent() {
		m_Agent.ResetPath();					// znnulztion de la navigation en cours
		m_Agent.velocity = Vector3.zero;		// vitesse nulle
	}

	/// <summary>
	/// contrôler la vitesse sur les NavMesh Links
	/// </summary>
	/// <param name="destination">destination</param>
	/// <returns></returns>
	IEnumerator MoveAcrossNavMeshLink(Vector3 destination) {
		OffMeshLinkData data = m_Agent.currentOffMeshLinkData;
		m_Agent.updateRotation = false;

		Vector3 startPos = m_Agent.transform.position;						// départ
		Vector3 endPos = data.endPos + Vector3.up * m_Agent.baseOffset;		// arrivée
		float duration = (endPos - startPos).magnitude / m_Agent.speed;		// durée du déplacement
		float t = 0.0f;
		float tStep = 1.0f / duration;										// incrément

		while (t < 1.0f) {													// tant qu'on est pas arrivé
			transform.position = Vector3.Lerp(startPos, endPos, t);			// calculer le point de passage
			m_Agent.destination = transform.position;						// aller au point de passage
			t += tStep * Time.deltaTime;									// incrémenter le timer
			yield return null;
		}
		// en fin de déplacement
		transform.position = endPos;										// annuler les arrondis de calcul sur la position finale
		m_Agent.updateRotation = true;										// orienter le personnage
		m_Agent.CompleteOffMeshLink();										// quitter le mode 'NavMesh Link'
		MoveAcrossNavMeshesStarted = false;
		m_Agent.destination = destination;									// continuer vers la destination initiale
	}
}
