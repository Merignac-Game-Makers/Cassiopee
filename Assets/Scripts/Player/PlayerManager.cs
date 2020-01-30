using System.Collections;
using System.Collections.Generic;
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

	float sqrInteractionDistance = 2.25f;                           // carré de la distance en deça de laquelle on déclenche les intéractions (1.5² = 2.25)
																	// (on utilise le carré pour gagner du temps de calcul en évitant une racine carrée pour la distance)

	InventoryUI m_InventoryUI;                                      // gestionnaire d'inventaire
	MagicManager m_MagicController;                                 // gestionnaire de magie

	public static PlayerManager Instance { get; protected set; }    // instance statique de cette classe

	// camera
	Camera mainCamera;                                              // caméra active

	// navigation
	[HideInInspector]
	public NavMeshAgent m_Agent;                                    // agent de navigation
	bool MoveAcrossNavMeshesStarted = false;                        // flag : est-on sur un nav mesh link ? (pour gérer la vitesse)
	[HideInInspector]
	public bool inTransit;                                          // l'agent est-il dans un transit (chagement de zone assisté)
	public bool canInterruptTransit;                                // un transit peut-il être interrompu ?

	// Interactions
	InteractableObject m_TargetInteractable = null;                 // objet avec lequel le joueur intéragit
	Activable m_TargetActivable = null;                             // objet magique avec lequel le joueur intéragit
	Collider m_TargetCollider;                                      // collider de l'objet en cours d'intéraction
	HighlightableObject m_Highlighted;                              // objet en surbrillance  sous le pointeur de la souris
	CharacterData m_CurrentTargetCharacterData = null;              // caractéristiques du PNJ en intéraction
	[HideInInspector]
	public InventoryUI.DragData m_InvItemDragging = null;           // objet d'inventaire en cours de drag & drop
	[HideInInspector]
	public MagicOrb m_MagicOrb = null;                              // orbe magique

	// CharacterData
	[HideInInspector]
	public CharacterData m_CharacterData;                           // caractéristiques du joueur (santé, force...)

	// Raycast
	RaycastHit[] m_RaycastHitCache = new RaycastHit[16];            // cache des résultats de lancer de rayon
	RaycastHit m_HitInfo = new RaycastHit();                        // résultat unitaire du lancer de rayon
	int m_InteractableLayer;                                        // layer des objets intéractibles
	int m_PlayerLayer;                                              // layer du personnage
	int raycastableLayers;                                          // tous les layers à tester pour le Raycast
	bool isClicOnUI;                                                // le clic en cours a-t-il débuté sur un élément d'interface ?

	// Visuel
	public Renderer body;                                           // Mesh renderer du corps
	public Texture2D standardTex;                                   // texture en mode 'standard'
	public Texture2D magicTex;                                      // texture en mode 'magie active'


	#region Initialisation
	void Awake() {
		Instance = this;                                            // création de l'instance statique
		mainCamera = Camera.main;                                   // initialisation de la caméra
	}

	// Start is called before the first frame update
	void Start() {
		m_InventoryUI = InventoryUI.Instance;                       // gestionnaire d'inventaire
		m_MagicController = MagicManager.Instance;              // gestionnaire de magie

		m_CharacterData = GetComponent<CharacterData>();            // caractéristiques du joueur
		m_CharacterData.Init();                                     // ... initialisation

		m_Agent = GetComponent<NavMeshAgent>();                     // préparation de la navigation

		m_InteractableLayer = 1 << LayerMask.NameToLayer("Interactable");       // layer des objets intéractibles
		m_PlayerLayer = 1 << LayerMask.NameToLayer("Player");                   // layer des objets intéractibles
																				//layersExceptPostProcessing = ~(1 << LayerMask.NameToLayer("PostProcess"));
																				//layersExceptIgnoreRaycast = ~(1 << LayerMask.NameToLayer("IgnoreRaycast"));

		var postProcessingMask = 1 << LayerMask.NameToLayer("PostProcess");
		var ignoreRaycastMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
		raycastableLayers = ~(postProcessingMask | ignoreRaycastMask);
	}
	#endregion


	// Update is called once per frame
	void Update() {
		// quitter le jeu par la touche escape
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}


		// Préparation du lancer de rayon de la caméra vers le pointeur de souris
		Ray screenRay = CameraController.Instance.GameplayCamera.ScreenPointToRay(Input.mousePosition);

		/**
		Si une intéraction a été demandée, sommes nous arrivés 'AU CONTACT' ?
		Rem : une intéraction peut être demandée soit :
			- par un clic sur un objet intéractible
			- par la collision avec un intéractible (<see cref="OnTriggerEnter">)
		*/
		if (m_TargetInteractable != null) {
			CheckInteractableRange();
		}

		// récupération des objets en cours de drag & drop 
		m_InvItemDragging = m_InventoryUI.currentlyDragged;         // objet d'inventaire
		m_MagicOrb = m_MagicController.dragging;                    // orbe magique

		// zoom
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		if (!Mathf.Approximately(mouseWheel, 0.0f)) {
			Vector3 view = mainCamera.ScreenToViewportPoint(Input.mousePosition);
			if (view.x > 0f && view.x < 1f && view.y > 0f && view.y < 1f)
				CameraController.Instance.Zoom(-mouseWheel * Time.deltaTime * 40.0f);
		}

		// au début d'un clic, on commence par effacer tous les objets en cours d'intéraction
		if (Input.GetMouseButtonDown(0)) {
			m_CurrentTargetCharacterData = null;
			m_TargetInteractable = null;
			m_TargetActivable = null;
			isClicOnUI = EventSystem.current.IsPointerOverGameObject();
		}

		// Gestion de la souris (mouseHover et clic)
		if (!isClicOnUI) {                                           // éviter de déplacer le personnage si on pointe sur un objet d'interface

			ObjectsRaycasts(screenRay);                             // Mettre en surbrillance les objets intéractibles lorsqu'ils sont sous le pointeur de souris

			if (m_InvItemDragging == null && m_MagicController?.dragging == null) {		// éviter de déplacer le personnage si on est en cours de drag & drop
				if (Input.GetMouseButton(0)) {											// si le bouton de la souris est appuyé
					if (m_InventoryUI.selectedEntry == null) {                          // si aucun objet d'inventaire n'est sélectionné
						if (m_TargetInteractable == null && m_TargetActivable == null && m_CurrentTargetCharacterData == null) {     // s'il n'y a pas d'intéraction en cours
							InteractableObject obj = m_Highlighted as InteractableObject;
							if (obj) {                                                                  // si on a cliqué sur un objet intéractible
								obj.Clicked = true;                                                     //	- l'objet a été cliqué
								RequestInteraction(obj);                                                //	- demander l'intéraction
							} else {
								CharacterData data = m_Highlighted as CharacterData;
								if (data) {                                                             // si on a cliqué sur le personnage
									m_CurrentTargetCharacterData = data;                                // pour l'instant on ne fait rien mais l'événement est détecté
																										// il pourrait être utilisé pour afficher un panneau de statistiques ou tout autre chose
								} else {                                                                // sinon => navigation
									if (Physics.Raycast(screenRay, out m_HitInfo, 5000, raycastableLayers) && (!inTransit)) {
										m_Agent.SetDestination(m_HitInfo.point);						// aller vers le point sélectionné
									}
								}
							}
						}
					} else {
						m_InventoryUI.DropOn3D(m_InventoryUI.selectedEntry.entry);
					}
				}
			}
		}

		if (inTransit && !m_Agent.hasPath)          // à la fin d'un déplacement 'en transit'
			EndTransit();                           // on n'est plus en transit

		// controler la vitesse sur les NavMesh Links (par défaut elle est trop rapide)
		if (m_Agent.isOnOffMeshLink && !MoveAcrossNavMeshesStarted) {
			MoveAcrossNavMeshesStarted = true;
			StartCoroutine(MoveAcrossNavMeshLink(m_Agent.destination));
		}
	}

	#region Visuel
	public void VisualMagicMode(bool on) {
		if (on) {
			body.material.EnableKeyword("_EMISSION");                                       // activer la texture émissive
			body.material.SetTexture("_MainTex", magicTex);
			;
		} else {
			body.material.DisableKeyword("_EMISSION");                                      // désactiver la texture émissive
			body.material.SetTexture("_MainTex", standardTex);
		}
	}
	#endregion

	#region Intéractions
	/// <summary>
	/// MouseHover :
	/// Recherche des objets interactibles sous le pointeur de souris
	/// </summary>
	/// <param name="screenRay">lancer de rayon</param>
	void ObjectsRaycasts(Ray screenRay) {
		bool somethingFound = false;

		// check for interactable Object
		int count = Physics.SphereCastNonAlloc(screenRay, .2f, m_RaycastHitCache, 1000.0f, m_InteractableLayer);                // objets du calque 'Interactable' sous la souris
		if (count > 0) {
			for (int i = 0; i < count; ++i) {                                                                                   // pour chacun d'entre eux
				InteractableObject obj = m_RaycastHitCache[i].collider.gameObject.GetComponentInParent<InteractableObject>();   // si c'est bien un intéractible => dans obj
				if (obj != null && obj.IsInteractable()) {                                                                      // et s'il est au statut 'actif'
					SwitchHighlightedObject(obj);                                                                               // mettre en surbrillance
					somethingFound = true;                                                                                      // flag : on a trouvé quelque chose
					break;                                                                                                      // on s'arrête au 1er objet trouvé
				}
			}
		}

		// check for player : même logique avec le layer 'player'
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

		if (!somethingFound && m_Highlighted != null) {         // si un objet est en surbrillance mais que la souris n'est plus dessus
			SwitchHighlightedObject(null);                      // éteindre l'objet
		}
	}

	/// <summary>
	/// bascule de surbrillance
	/// le material de l'objet doit utiliser un shader comportant une 'rim color'
	/// </summary>
	/// <param name="obj"></param>
	void SwitchHighlightedObject(HighlightableObject obj) {
		if (m_Highlighted != null && m_Highlighted != obj) {    // si un autre objet est en surbrillance
			var a = m_Highlighted as Activable;                 // si c'est un 'activable' (magie) => dans a
			if (!(a && a.IsActive))                            // si ce n'est pas un objet magique activé
				m_Highlighted.Highlight(false);                 //		=> éteindre l'objet précédent
		}
		m_Highlighted = obj;
		if (m_Highlighted && !m_Highlighted.isOn) {             // si le paramètre transmis est non null => il faut allumer l'objet
			var a = m_Highlighted as Activable;                 // si c'est un 'activable' (magie) => dans a
			if ((!a || !a.IsActive))                            // si ce n'est pas un activable OU si l'activable est NON activé
				m_Highlighted.Highlight(true);                  //		=> allumer

		}
	}

	/// <summary>
	/// détection de collision avec les objets intéractibles
	/// Si l'objet est un intéractible au statut 'actif'
	///		=> m_TargetInteractable contient l'objet
	///		=> m_TargetCollider contient son collider
	/// </summary>
	/// <param name="other">objet rencontré</param>
	public void OnTriggerEnter(Collider other) {
		m_TargetInteractable = other.gameObject.GetComponent<InteractableObject>();
		if (m_TargetInteractable != null) {                 // si l'objet rencontré est un 'intéractible'
			if (!m_TargetInteractable.IsInteractable()) {   //		si son statut est 'inactif'
				m_TargetInteractable = null;                //			on annule la détection
			} else {                                        //		si son statut est 'actif'
				m_TargetCollider = m_TargetInteractable.GetComponentInChildren<Collider>(); // on mémorise son collider
			}
		}
	}

	/// <summary>
	/// Comparaison de la distance d'un objet intéractible avec la distance de déclenchement des intéractions
	/// 
	/// on déclenche l'intéraction 'AU CONTACT', c'est à dire si
	///		la distance est inférieure à la distance de déclenchement
	///	ET
	///		l'objet est en mode 'onClick' ET il a été cliqué
	///		OU
	///		l'objet est en mode 'onTheFly'
	/// </summary>
	void CheckInteractableRange() {
		Vector3 distance = m_TargetCollider.ClosestPointOnBounds(transform.position) - transform.position;  // calcul de la distance

		if ((m_TargetInteractable.mode != InteractableObject.Mode.onClick || m_TargetInteractable.Clicked)
			&& distance.sqrMagnitude < sqrInteractionDistance) {
			m_TargetInteractable.InteractWith(m_CharacterData);         // déclencher l'intéraction
			m_TargetInteractable = null;                                // supprimer la détection pour éviter de redoubler l'intéraction
		}
	}

	/// <summary>
	/// Demander une intéraction :
	/// On dirige le joueur vers l'objet, l'intéraction sera déclenchée 'AU CONTACT'
	/// </summary>
	/// <param name="obj">l'objet avec lequel intéragir</param>
	public void RequestInteraction(InteractableObject obj) {
		if (obj.IsInteractable()) {                                         // si l'objet est au statut 'actif'
			if (obj.GetComponentInChildren<Activable>()) {                  // si l'objet est un objet magique activable
				(obj as Activable).Toggle();                                //	- basculer l'état de l'objet (activé/désactivé)
				m_TargetActivable = obj as Activable;                       //	- mémoriser l'objet magique (pour éviter les intéractions multiples)
			} else {                                                        // sinon 
				m_TargetInteractable = obj;                                 //	- mémoriser l'intéractible (il sera testé dans le prochain update pour déclencher l'intéraction 'AU CONTACT')
				m_TargetCollider = obj.GetComponentInChildren<Collider>();  //	- mémoriser le collider
				m_Agent.SetDestination(obj.transform.position);             //	- diriger le joueur vers l'objet
			}
		}
	}

	#endregion

	#region Navigation
	public void StartTransitTo(Vector3 pos) {
		m_Agent.SetDestination(pos);                    // diriger le joueur vers la destination
		inTransit = true;                               // on est en transit
		MagicManager.Instance.ResetConstellation();     // désactiver les objets magiques en quittant le lieu actuel
	}
	public void EndTransit() {
		inTransit = false;                              // on n'est plus en transit
	}

	/// <summary>
	/// interrompre la navigation
	/// </summary>
	public void StopAgent() {
		m_Agent.ResetPath();                    // annulation de la navigation en cours
		m_Agent.velocity = Vector3.zero;        // vitesse nulle
	}

	/// <summary>
	/// contrôler la vitesse sur les NavMesh Links
	/// (par défaut, dans UNITY,  les déplacements sont plus rapides sur les NavLinks... BUG ?)
	/// cette coroutine corrige le phénomène
	/// </summary>
	/// <param name="destination">destination</param>
	/// <returns></returns>
	IEnumerator MoveAcrossNavMeshLink(Vector3 destination) {
		OffMeshLinkData data = m_Agent.currentOffMeshLinkData;
		m_Agent.updateRotation = false;

		Vector3 startPos = m_Agent.transform.position;                      // départ
		Vector3 endPos = data.endPos + Vector3.up * m_Agent.baseOffset;     // arrivée
		float duration = (endPos - startPos).magnitude / m_Agent.speed;     // durée du déplacement
		float t = 0.0f;
		float tStep = 1.0f / duration;                                      // incrément

		while (t < 1.0f) {                                                  // tant qu'on est pas arrivé
			transform.position = Vector3.Lerp(startPos, endPos, t);         // calculer le point de passage
			m_Agent.SetDestination(transform.position);                     // aller au point de passage
			t += tStep * Time.deltaTime;                                    // incrémenter le timer
			yield return null;
		}
		// en fin de déplacement
		transform.position = endPos;                                        // annuler les arrondis de calcul sur la position finale
		m_Agent.updateRotation = true;                                      // orienter le personnage
		m_Agent.CompleteOffMeshLink();                                      // quitter le mode 'NavMesh Link'
		MoveAcrossNavMeshesStarted = false;
		m_Agent.SetDestination(destination);                                // continuer vers la destination initiale
	}
	#endregion

}
