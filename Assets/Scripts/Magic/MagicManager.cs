using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe destinée au contrôle de la magie :
///		- 
/// </summary>
public class MagicManager : MonoBehaviour
{
	public static MagicManager Instance;		// instance statique

	public MagicBookContent magicBookContent;	// les pages de magie
	public LineRenderer lineRendered;			// pour tracer les éclairs
	public MagicOrb moonOrb;					// orbe magique LUNE
	public MagicOrb sunOrb;						// orbe magique SOLEIL
	public Material moonRay;					// éclairs couleur LUNE
	public Material sunRay;						// éclairs couleur SOLEIL

	public bool isOn => MagicUI.Instance.artefactButton.gameObject.activeInHierarchy;   // la magie n'est active que si le médaillon est visible
	public MagicOrb currentOrb { get; private set; }        // l'orbe existante

	List<Activable> magicActivatedItems;    // liste des items magiques activés
	string orbConstellation;                // la constellation qui a généré l'orbe
	public bool isClosed { get; private set; } = false;    // flag : la constellation est elle fermée (faux par défaut)

	MagicUI.SelectedArtefact activeArtfact => MagicUI.Instance.selectedArtefact;    // artefact sélectionné (LUNE ou SOLEIL)
	/// <summary>
	/// Ensemble des pages du grimoire => constellations traçables
	/// Une page peut définir une constellation et une seule
	/// Une constellation est définie par la liste des objets activables qui la composent
	/// Une même constellation peut être présente à plusieurs endroits différents => plusieurs listes
	/// </summary>
	List<PageTemplate> pages;

	[HideInInspector]
	public MagicOrb dragging;               // orbe magique en cours de 'drag and drop'

	// création de l'instance statique
	private void Awake() {
		Instance = this;
	}

	private void Start() {
		magicActivatedItems = new List<Activable>();
		// récupération des pages portés par 'MagicBookContent'
		pages = new List<PageTemplate>();
		foreach (PageTemplate page in magicBookContent.gameObject.GetComponentsInChildren<PageTemplate>()) {
			pages.Add(page);
		}
	}

	/// <summary>
	/// Ajouter ou retirer un item activable à la constellation en cours de traçage
	/// Après l'ajout, on vérifie si une constellation connue a été traçée
	/// Si c'est le cas, on génère l'orbe et on efface la constellation
	/// </summary>
	/// <param name="item">l'item à ajouter ou à retirer</param>
	public void AddOrRemove(Activable item) {
		if (!magicActivatedItems.Contains(item)) {      // on ne peut pas ajouter une objet déjà dans la liste
			Add(item);
		} else {
			Remove(item);
		}
	}

	/// <summary>
	/// ajouter un objet à la liste 
	/// </summary>
	/// <param name="item"></param>
	public void Add(Activable item) {
		if (!magicActivatedItems.Contains(item)) {                                      // on ne peut pas ajouter un objet déjà dans la liste
			if (magicActivatedItems.Count > 0) {
				LineRenderer lr = Instantiate(lineRendered, item.transform);            // ajouter un 'lineRenderer' pour pouvoir tracer les éclairs
				lr.GetComponent<LineRenderer>().material =
					activeArtfact == MagicUI.SelectedArtefact.Moon ? moonRay : sunRay;  // définir la couleur des éclairs

				Electric electicScript = item.gameObject.AddComponent<Electric>();      // tracer l'éclair
				electicScript.transformPointA = magicActivatedItems.Last().transform;   // entre les 
				electicScript.transformPointB = item.transform;                         // 2 derniers items
			}
			magicActivatedItems.Add(item);
			if (TestConstellation()) {                                                  // si la constellation est complète et correcte
				CreateOrb();                                                            // créer un orbe
				StartResetConstellation(2);                                             // effacer la constellation (timer 2 secondes)
			}
		}
	}

	/// <summary>
	///  annuler l'activation d'un objet...
	///  ... et gérer la 'troncature' de la constellation en cours
	/// </summary>
	/// <param name="item"></param>
	public void Remove(Activable item) {
		int idx = magicActivatedItems.IndexOf(item);                                    // soit idx l'index de l'item à retirer
		if (idx != -1) {                                                                // si l'item est dans la liste

			if (idx != 0 && idx == magicActivatedItems.Count - 1) {                     // si c'est le dernier
				Inactivate(idx);                                                        //	- désactiver celui-là seulement
			} else if (idx != 0) {                                                      // s'il est 'dans le milieu'
				for (int i = magicActivatedItems.Count - 1; i > idx; i--)               //	- désactiver tous les suivants
					Inactivate(i);
			} else {                                                                    // si c'est le 1er... 
				if (magicActivatedItems.Count > 2) {                                    // et qu'il y a au moins 3 points
					item.Activate();                                                    //	- ne pas revenir au visuel 'désactivé'
					CloseConstellation();                                               //	- fermer la constellation
				} else {																// sinon
					ResetConstellation();												//	- effacer tout
				}
			}
		}
	}

	private void Inactivate(int idx) {
		Destroy(magicActivatedItems[idx].GetComponent<Electric>());                                 // effacer
		Destroy(magicActivatedItems[idx].GetComponentInChildren<LineRenderer>()?.gameObject);       // les éclairs
		magicActivatedItems[idx].Inactivate();                                                      // retour au visuel 'désélectionné'
		OpenConstellation();                                                                        // la constellation n'est pas fermée							
		magicActivatedItems.RemoveAt(idx);                                                          // retirer l'item de la liste
	}

	private void CloseConstellation() {
		if (isClosed) return;                                                   // on ne peut pas cloturer une constellation déjà close

		var item = magicActivatedItems[0];                                      // on travaille sur le 1er objet
		LineRenderer lr = Instantiate(lineRendered, item.transform);            // ajouter un 'lineRenderer' pour pouvoir tracer les éclairs
		lr.GetComponent<LineRenderer>().material =
			activeArtfact == MagicUI.SelectedArtefact.Moon ? moonRay : sunRay;  // définir la couleur des éclairs
		Electric electicScript = item.gameObject.AddComponent<Electric>();      // tracer l'éclair
		electicScript.transformPointA = item.transform;                         // entre le 1er 
		electicScript.transformPointB = magicActivatedItems.Last().transform;   // et le dernier
		isClosed = true;


		foreach(Activable obj in magicActivatedItems) {
			var lRend = obj.GetComponentInChildren<LineRenderer>();
			var mats = lRend.materials;
			mats[0] = moonRay;
			mats[1] = sunRay;
			lRend.materials = mats;
		}

		StartResetConstellation(2);			// effacer la constellation après 2 secondes
		/**
		 * Ajouter ici
		 * l'effet magique de la fermeture
		 */
	}

	private void OpenConstellation() {
		Destroy(magicActivatedItems[0].GetComponent<Electric>());                                 // effacer
		Destroy(magicActivatedItems[0].GetComponentInChildren<LineRenderer>()?.gameObject);       // les éclairs
		isClosed = false;
	}

	/// <summary>
	/// Créer un orbe en fonction du médaillon et de la constellation
	/// </summary>
	private void CreateOrb() {
		var orb = activeArtfact == MagicUI.SelectedArtefact.Moon ? moonOrb : sunOrb;    // quel orbe (LUNE/SOLEIL)?
		orb.constellation = orbConstellation;                                           // quelle constellation ?
		currentOrb = Instantiate(orb, PlayerManager.Instance.gameObject.transform);     // générer l'orbe
		OrbEntryUI orbEntry = MagicUI.Instance.orbPanel.GetComponentInChildren<OrbEntryUI>();
		orbEntry.Init(new OrbEntryUI.OrbEntry(orb));
		MagicUI.Instance.orbPanel.SetActive(true);
	}

	/// <summary>
	/// détruire un orbe
	/// </summary>
	/// <param name="orb"></param>
	public void DestroyOrb() {
		if (currentOrb)
			Destroy(currentOrb.gameObject);
		currentOrb = null;
	}

	/// <summary>
	/// Vérifier si une constellation est dans la liste des constellations valides
	/// </summary>
	bool TestConstellation() {
		foreach (PageTemplate page in pages) {
			foreach (Constellation constellation in page.constellations) {
				if (magicActivatedItems.IsLike(constellation.objects) ||                            // tester dans un sens
					magicActivatedItems.Reverse<Activable>().IsLike(constellation.objects)) {       // et dans l'autre
					orbConstellation = page.page.constellation;
					return true;
				}
			}
		}
		orbConstellation = null;
		return false;
	}

	/// <summary>
	/// Changer la couleur des éclairs si on change l'artefact sélectionné
	/// </summary>
	/// <param name="artefact"></param>
	public void UpdateArtefact(MagicUI.SelectedArtefact artefact) {
		Material mat = activeArtfact == MagicUI.SelectedArtefact.Moon ? moonRay : sunRay;
		for (int i = 1; i < magicActivatedItems.Count; i++) {
			magicActivatedItems[i].GetComponentInChildren<LineRenderer>().material = mat;
		}
	}

	/// <summary>
	/// désélectionner toute une constellation
	/// </summary>
	/// <param name="s"></param>
	public void ResetConstellation() {
		StartResetConstellation(0);
	}
	void StartResetConstellation(float s) {
		StartCoroutine(ResetConstellation(s));      // effacer la constellation
	}
	IEnumerator ResetConstellation(float s) {
		yield return new WaitForSeconds(s);
		for (int i = magicActivatedItems.Count - 1; i >= 0; i--) {
			Inactivate(i);
		}
	}

	/// <summary>
	/// Actions à mener lorsqu'on désactive le grimoire
	/// </summary>
	public void SetMagicOff() {
		DestroyOrb();               // détruire l'orebe si on en possède un
		ResetConstellation();       // effacer la constellation en cours
	}
}
