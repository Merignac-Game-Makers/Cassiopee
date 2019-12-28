using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe destinée au contrôle de la magie :
///		- 
/// </summary>
public class MagicController : MonoBehaviour
{
	public static MagicController Instance;         // instance statique

	public LineRenderer lineRendered;       // pour tracer les éclairs
	public MagicOrb moonOrb;                // orbe magique LUNE
	public MagicOrb sunOrb;                 // orbe magique SOLEIL
	public Material moonRay;                // éclairs couleur LUNE
	public Material sunRay;                 // éclairs couleur SOLEIL

	List<Activable> magicActivatedItems;    // liste des items magiques activés
	string orbConstellation;                // la constellation qui a généré l'orbe

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
		foreach (PageTemplate page in gameObject.GetComponentsInChildren<PageTemplate>()) {
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
		if (!magicActivatedItems.Contains(item)) {
			//Debug.Log("add " + item.name);
			if (magicActivatedItems.Count > 0) {
				LineRenderer lr = Instantiate(lineRendered, item.transform);
				lr.GetComponent<LineRenderer>().material = activeArtfact == MagicUI.SelectedArtefact.Moon ? moonRay : sunRay;

				Electric electicScript = item.gameObject.AddComponent<Electric>();      // tracer l'éclair
				electicScript.transformPointA = magicActivatedItems.Last().transform;   // entre les 
				electicScript.transformPointB = item.transform;                         // 2 derniers items
			}
			magicActivatedItems.Add(item);
			if (TestConstellation()) {
				//Debug.Log("DONE !!!");
				var orb = activeArtfact == MagicUI.SelectedArtefact.Moon ? moonOrb : sunOrb;    // quel orbe ?
				orb.constellation = orbConstellation;
				Instantiate(orb, PlayerManager.Instance.gameObject.transform);                  // générer l'orbe
				StartCoroutine(ResetConstellation(2));                                          // effacer la constellation
			}
		} else {
			//Debug.Log("remove " + item.name);
			int idx = magicActivatedItems.IndexOf(item);                    // pour l'item à retirer
			for (int i = magicActivatedItems.Count - 1; i >= idx; i--) {    // et les suivants
				Activable obj = magicActivatedItems[i];
				Destroy(obj.GetComponent<Electric>());                                  // effacer
				Destroy(obj.GetComponentInChildren<LineRenderer>()?.gameObject);        // les éclairs
				magicActivatedItems.RemoveAt(i);                                        // retirer l'item de la liste
				obj.m_IsActive = false;                         // retour au visuel
				obj.Highlight(false);                           // 'désélectionné'
			}
		}
	}

	/// <summary>
	/// Vérifier si une constellation est dans la liste des constellations valides
	/// </summary>
	bool TestConstellation() {
		foreach (PageTemplate page in pages) {
			foreach (Constellation constellation in page.constellations) {
				if (magicActivatedItems.IsLike(constellation.objects)) {
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
	/// <returns></returns>
	IEnumerator ResetConstellation(float s) {
		yield return new WaitForSeconds(s);
		AddOrRemove(magicActivatedItems[0]);
		yield return null;
	}
}

/// <summary>
/// extensions de 'List<Activable>' pour commodité
/// </summary>
public static partial class Extensions
{
	public static Activable Last(this List<Activable> list) {
		return list[list.Count - 1];
	}
	public static void AddItem(this List<Activable> list, GameObject item) {
		if (item.GetComponentInChildren<Activable>() != null)
			list.Add(item.GetComponentInChildren<Activable>());
	}
	public static bool IsLike(this List<Activable> list, List<Activable> other) {
		if (list.Count != other.Count)
			return false;
		else {
			for (int i = 0; i < list.Count; i++) {
				if (list[i] != other[i])
					return false;
			}
			return true;
		}

	}
}
