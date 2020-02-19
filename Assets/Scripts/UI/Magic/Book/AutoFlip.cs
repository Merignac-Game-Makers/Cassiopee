using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour
{
	public FlipMode Mode;
	public float PageFlipTime = .1f;
	public Book ControledBook;
	public int AnimationFramesCount = 40;
	bool isFlipping = false;

	void Start() {
		if (!ControledBook)
			ControledBook = GetComponent<Book>();
		ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
	}
	void PageFlipped() {
		isFlipping = false;
	}

	public void FlipRightPage() {
		if (ControledBook.baseBookContent.GetNextAvailablePage(ControledBook.currentPage)) {
			if (isFlipping) return;																// désactivé si on est déjà en train de tourner la page
			if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;				// désactivé si on est sur la dernière page
			isFlipping = true;																	// mouvement de page en cours
			float frameTime = PageFlipTime / AnimationFramesCount;								// durée d'une image
			float xc = (ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x);		// largeur du mouvement
			float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.95f;						// bas du bmouvement
			float dx = xc / AnimationFramesCount;												// incrément horizontal
			StartCoroutine(FlipRTL(xc, h, frameTime, dx));										// coroutine pour le mouvement
		}
	} 
	public void FlipLeftPage() {
		if (ControledBook.baseBookContent.GetPreviousAvailablePage(ControledBook.currentPage)) {
			if (isFlipping) return;																// désactivé si on est déjà en train de tourner la page
			if (ControledBook.currentPage <= 0) return;											// désactivé si on est sur la première page
			isFlipping = true;                                                                  // mouvement de page en cours
			float frameTime = PageFlipTime / AnimationFramesCount;                              // durée d'une image
			float xc = (ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x);        // largeur du mouvement
			float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.95f;                        // bas du bmouvement
			float dx = xc / AnimationFramesCount;                                               // incrément horizontal
			StartCoroutine(FlipLTR(xc, h, frameTime, dx));                                      // coroutine pour le mouvement
		}
	}

	/// <summary>
	/// Tourner la page de droite vers la gauche
	/// </summary>
	/// <param name="xc">largeur entre les coins du bas</param>
	/// <param name="h">bas du mouvement</param>
	/// <param name="frameTime">durée d'une image</param>
	/// <param name="dx">incrément horizontal</param>
	/// le mouvement du coin est une parabole
	/// la hauteur totale du mouvement est : h/5
	/// <returns></returns>
	IEnumerator FlipRTL(float xc, float h, float frameTime, float dx) {
		float ix = xc - dx;                                             // 1ère position horizontale mesurée à partir du coin gauche
		float x = ix - xc / 2;                                          // abscisse mesurée à partir du centre du livre
		float y = -h + (1 - Mathf.Pow(x / xc * 2, 2)) * h / 5;          // ordonnée
		ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));		// 1ère position du coin
		for (int i = 0; i < AnimationFramesCount; i++) {				// tant qu'on n'est pas au bout
			x = ix - xc / 2;											// calculer l'abscisse
			y = -h + (1 - Mathf.Pow(x / xc * 2, 2)) * h / 5;			// calculer l'ordonnée
			ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));	// déplacer le coin
			yield return new WaitForSeconds(frameTime);					// attendre la durée d'une image
			ix -= dx;													// décrémenter la position horizontale pour le mouvement suivant
		}
		ControledBook.f = new Vector3(x, y, 0);
		ControledBook.c = new Vector3(x, y, 0);
		ControledBook.ReleasePage();
		isFlipping = false;

	}
	IEnumerator FlipLTR(float xc, float h, float frameTime, float dx) {
		float ix = dx;													// 1ère position horizontale mesurée à partir du coin gauche
		float x = ix - xc / 2;                                          // abscisse mesurée à partir du centre du livre
		float y = -h + (1 - Mathf.Pow(x / xc * 2, 2)) * h / 5;          // ordonnée
		ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));        // 1ère position du coin
		for (int i = 0; i < AnimationFramesCount; i++) {                // tant qu'on n'est pas au bout
			x = ix - xc / 2;                                            // calculer l'abscisse
			y = -h + (1 - Mathf.Pow(x / xc * 2, 2)) * h / 5;            // calculer l'ordonnée
			ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));   // déplacer le coin
			yield return new WaitForSeconds(frameTime);                 // attendre la durée d'une image
			ix += dx;                                                   // incrémenter la position horizontale pour le mouvement suivant
		}
		ControledBook.f = new Vector3(x, y, 0);
		ControledBook.c = new Vector3(x, y, 0);
		ControledBook.ReleasePage();
		isFlipping = false;
	}
}
