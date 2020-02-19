using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using static PageMaker.Side;
using static Book.Section;

public enum FlipMode
{
	RightToLeft,
	LeftToRight
}
//[ExecuteInEditMode]
public class Book : MonoBehaviour
{
	public Canvas canvas;
	public RectTransform bookPanel;

	// section
	public enum Section { magic, quests, diary }
	public Section section { get; private set; }

	// Pages
	public int TotalPageCount => pages.Count;
	public BaseBookContent baseBookContent { get; private set; }    // gestionnaire du contenu commun
	public MagicBookContent magicBookContent;                       // gestionnaire du contenu de la section magie
	public QuestBookContent questBookContent;                       // gestionnaire du contenu de la section quêtes
	public DiaryBookContent diaryBookContent;                       // gestionnaire du contenu de la section journal

	List<PageMaker> pages;
	List<PageMaker> magicPages;                                     // les pages de la section magie
	List<PageMaker> questPages;                                     // les pages de la section quêtes
	public List<PageMaker> diaryPages { get; set; }                 // les pages de la section journal

	//public int lastAvailablePage;

	public bool interactable = true;

	public int currentPage = 0;
	public int currentMagicPage { get; set; } = 0;
	int currentQuestPage = 0;
	int currentDiaryPage = 0;

	public Transform pagesStack;
	GameObject prevLeft;
	GameObject prevRight;
	GameObject currLeft;
	public GameObject currRight { get; private set; }
	GameObject nextLeft;
	GameObject nextRight;

	public Vector3 EndBottomLeft {
		get { return ebl; }
	}
	public Vector3 EndBottomRight {
		get { return ebr; }
	}
	public float Height {
		get {
			return bookPanel.rect.height;
		}
	}
	public Image ClippingPlane;
	public Image NextPageClip;
	public RectTransform Left;
	public RectTransform LeftNext;
	public RectTransform Right;
	public RectTransform RightNext;
	public UnityEvent OnFlip;
	float radius1, radius2;

	//Spine Bottom
	Vector3 sb;
	//Spine Top
	Vector3 st;
	//corner of the page
	Vector3 c;
	//Edge Bottom Right
	Vector3 ebr;
	//Edge Bottom Left
	Vector3 ebl;
	//follow point 
	[HideInInspector]
	public Vector3 f;
	bool pageDragging = false;
	//current flip mode
	FlipMode mode;

	public static Book Instance { get; private set; }

	private void Awake() {
		Instance = this;
	}

	void Start() {
		section = magic;
		UpdateCurrentPages();
		magicBookContent = MagicBookContent.Instance;
		questBookContent = QuestBookContent.Instance;
	}

	public void Init() {
		float scaleFactor = 1;
		if (canvas) scaleFactor = canvas.scaleFactor;
		float pageWidth = (bookPanel.rect.width * scaleFactor - 1) / 2;
		float pageHeight = bookPanel.rect.height * scaleFactor;
		Left.gameObject.SetActive(false);
		Right.gameObject.SetActive(false);
		//UpdateSprites();
		Vector3 globalsb = bookPanel.transform.position + new Vector3(0, -pageHeight / 2);
		sb = transformPoint(globalsb);
		Vector3 globalebr = bookPanel.transform.position + new Vector3(pageWidth, -pageHeight / 2);
		ebr = transformPoint(globalebr);
		Vector3 globalebl = bookPanel.transform.position + new Vector3(-pageWidth, -pageHeight / 2);
		ebl = transformPoint(globalebl);
		Vector3 globalst = bookPanel.transform.position + new Vector3(0, pageHeight / 2);
		st = transformPoint(globalst);
		radius1 = Vector2.Distance(sb, ebr);
		float scaledPageWidth = pageWidth / scaleFactor;
		float scaledPageHeight = pageHeight / scaleFactor;
		radius2 = Mathf.Sqrt(scaledPageWidth * scaledPageWidth + scaledPageHeight * scaledPageHeight);
		ClippingPlane.rectTransform.sizeDelta = new Vector2(scaledPageWidth * 2, scaledPageHeight + scaledPageWidth * 2);
		NextPageClip.rectTransform.sizeDelta = new Vector2(scaledPageWidth, scaledPageHeight + scaledPageWidth * 0.6f);

		// initialisation du contenu des sections
		magicPages = new List<PageMaker>(magicBookContent.content);
		questPages = new List<PageMaker>(questBookContent.content);
		diaryPages = new List<PageMaker>(diaryBookContent.content);

		// on commence sur la section magie
		SetSection(magic);
	}

	public Vector3 transformPoint(Vector3 global) {
		Vector2 localPos = bookPanel.InverseTransformPoint(global);
		//RectTransformUtility.ScreenPointToLocalPointInRectangle(BookPanel, global, null, out localPos);
		return localPos;
	}
	void Update() {
		if (pageDragging && interactable) {
			UpdateBook();
		}
		//Debug.Log("mouse local pos:" + transformPoint(Input.mousePosition));
		//Debug.Log("mouse  pos:" + Input.mousePosition);
	}
	public void UpdateBook() {
		f = Vector3.Lerp(f, transformPoint(Input.mousePosition), Time.deltaTime * 10);
		if (mode == FlipMode.RightToLeft)
			UpdateBookRTLToPoint(f);
		else
			UpdateBookLTRToPoint(f);
	}
	public void UpdateBookLTRToPoint(Vector3 followLocation) {
		mode = FlipMode.LeftToRight;
		f = followLocation;
		Left.transform.SetParent(ClippingPlane.transform, true);

		Right.transform.SetParent(bookPanel.transform, true);
		LeftNext.transform.SetParent(bookPanel.transform, true);

		c = Calc_C_Position(followLocation);
		Vector3 t1;
		float T0_T1_Angle = Calc_T0_T1_Angle(c, ebl, out t1);
		if (T0_T1_Angle < 0) T0_T1_Angle += 180;

		ClippingPlane.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle - 90);
		ClippingPlane.transform.position = bookPanel.TransformPoint(t1);

		//page position and angle
		Left.transform.position = bookPanel.TransformPoint(c);
		float C_T1_dy = t1.y - c.y;
		float C_T1_dx = t1.x - c.x;
		float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
		Left.transform.eulerAngles = new Vector3(0, 0, C_T1_Angle - 180);

		NextPageClip.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle - 90);
		NextPageClip.transform.position = bookPanel.TransformPoint(t1);
		LeftNext.transform.SetParent(NextPageClip.transform, true);
		Right.transform.SetParent(ClippingPlane.transform, true);
		Right.transform.SetAsFirstSibling();

	}
	public void UpdateBookRTLToPoint(Vector3 followLocation) {
		mode = FlipMode.RightToLeft;
		f = followLocation;
		Right.transform.SetParent(ClippingPlane.transform, true);

		Left.transform.SetParent(bookPanel.transform, true);
		RightNext.transform.SetParent(bookPanel.transform, true);
		c = Calc_C_Position(followLocation);
		Vector3 t1;
		float T0_T1_Angle = Calc_T0_T1_Angle(c, ebr, out t1);
		if (T0_T1_Angle >= -90) T0_T1_Angle -= 180;

		ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);
		ClippingPlane.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle + 90);
		ClippingPlane.transform.position = bookPanel.TransformPoint(t1);

		//page position and angle
		Right.transform.position = bookPanel.TransformPoint(c);
		float C_T1_dy = t1.y - c.y;
		float C_T1_dx = t1.x - c.x;
		float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
		Right.transform.eulerAngles = new Vector3(0, 0, C_T1_Angle);

		NextPageClip.transform.eulerAngles = new Vector3(0, 0, T0_T1_Angle + 90);
		NextPageClip.transform.position = bookPanel.TransformPoint(t1);
		RightNext.transform.SetParent(NextPageClip.transform, true);
		Left.transform.SetParent(ClippingPlane.transform, true);
		Left.transform.SetAsFirstSibling();

	}
	private float Calc_T0_T1_Angle(Vector3 c, Vector3 bookCorner, out Vector3 t1) {
		Vector3 t0 = (c + bookCorner) / 2;
		float T0_CORNER_dy = bookCorner.y - t0.y;
		float T0_CORNER_dx = bookCorner.x - t0.x;
		float T0_CORNER_Angle = Mathf.Atan2(T0_CORNER_dy, T0_CORNER_dx);
		// float T0_T1_Angle = 90 - T0_CORNER_Angle;

		float T1_X = t0.x - T0_CORNER_dy * Mathf.Tan(T0_CORNER_Angle);
		T1_X = normalizeT1X(T1_X, bookCorner, sb);
		t1 = new Vector3(T1_X, sb.y, 0);
		////////////////////////////////////////////////
		//clipping plane angle=T0_T1_Angle
		float T0_T1_dy = t1.y - t0.y;
		float T0_T1_dx = t1.x - t0.x;
		float T0_T1_Angle = Mathf.Atan2(T0_T1_dy, T0_T1_dx) * Mathf.Rad2Deg;
		return T0_T1_Angle;
	}
	private float normalizeT1X(float t1, Vector3 corner, Vector3 sb) {
		if (t1 > sb.x && sb.x > corner.x)
			return sb.x;
		if (t1 < sb.x && sb.x < corner.x)
			return sb.x;
		return t1;
	}
	private Vector3 Calc_C_Position(Vector3 followLocation) {
		Vector3 c;
		f = followLocation;
		float F_SB_dy = f.y - sb.y;
		float F_SB_dx = f.x - sb.x;
		float F_SB_Angle = Mathf.Atan2(F_SB_dy, F_SB_dx);
		Vector3 r1 = new Vector3(radius1 * Mathf.Cos(F_SB_Angle), radius1 * Mathf.Sin(F_SB_Angle), 0) + sb;

		float F_SB_distance = Vector2.Distance(f, sb);
		if (F_SB_distance < radius1)
			c = f;
		else
			c = r1;
		float F_ST_dy = c.y - st.y;
		float F_ST_dx = c.x - st.x;
		float F_ST_Angle = Mathf.Atan2(F_ST_dy, F_ST_dx);
		Vector3 r2 = new Vector3(radius2 * Mathf.Cos(F_ST_Angle),
		   radius2 * Mathf.Sin(F_ST_Angle), 0) + st;
		float C_ST_distance = Vector2.Distance(c, st);
		if (C_ST_distance > radius2)
			c = r2;
		return c;
	}
	public void DragRightPageToPoint(Vector3 point) {
		if (nextLeft == null) return;
		pageDragging = true;
		mode = FlipMode.RightToLeft;
		f = point;

		NextPageClip.rectTransform.pivot = new Vector2(0, 0.12f);
		ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);

		Left.gameObject.SetActive(true);
		Left.pivot = new Vector2(0, 0);
		Left.transform.position = RightNext.transform.position;
		Left.transform.eulerAngles = new Vector3(0, 0, 0);
		//------------------------------- feuille qui tourne : page de gauche (c'est l'actuelle page de droite)
		SetPage(Left, currRight);
		Left.transform.SetAsFirstSibling();

		Right.gameObject.SetActive(true);
		Right.transform.position = RightNext.transform.position;
		Right.transform.eulerAngles = new Vector3(0, 0, 0);
		//------------------------------- feuille qui tourne : page de droite (ce sera la page de gauche après rotation)
		SetPage(Right, nextLeft);

		//------------------------------- page suivante :  page de droite
		SetPage(RightNext, nextRight);

		LeftNext.transform.SetAsFirstSibling();
		UpdateBookRTLToPoint(f);
	}

	public void OnMouseDragRightPage() {
		if (interactable) {
			DragRightPageToPoint(transformPoint(Input.mousePosition));
		}

	}
	public void DragLeftPageToPoint(Vector3 point) {
		if (currentPage <= 0) return;
		pageDragging = true;
		mode = FlipMode.LeftToRight;
		f = point;

		NextPageClip.rectTransform.pivot = new Vector2(1, 0.12f);
		ClippingPlane.rectTransform.pivot = new Vector2(0, 0.35f);

		Right.gameObject.SetActive(true);
		Right.transform.position = LeftNext.transform.position;
		//------------------------------- feuille qui tourne : page de droite (c'est l'actuelle page de gauche)
		SetPage(Right, currLeft);
		Right.transform.eulerAngles = new Vector3(0, 0, 0);
		Right.transform.SetAsFirstSibling();

		Left.gameObject.SetActive(true);
		Left.pivot = new Vector2(1, 0);
		Left.transform.position = LeftNext.transform.position;
		Left.transform.eulerAngles = new Vector3(0, 0, 0);
		//------------------------------- feuille qui tourne : page de gauche (ce sera la page de droite après rotation)
		SetPage(Left, prevRight);

		//------------------------------- page précédente :  page de gauche
		SetPage(LeftNext, prevLeft);

		RightNext.transform.SetAsFirstSibling();
		UpdateBookLTRToPoint(f);
	}
	public void OnMouseDragLeftPage() {
		if (interactable) {
			DragLeftPageToPoint(transformPoint(Input.mousePosition));
		}

	}
	public void OnMouseRelease() {
		if (interactable)
			ReleasePage();
	}
	public void ReleasePage() {
		if (pageDragging) {
			pageDragging = false;
			float distanceToLeft = Vector2.Distance(c, ebl);
			float distanceToRight = Vector2.Distance(c, ebr);
			if (distanceToRight < distanceToLeft && mode == FlipMode.RightToLeft)
				TweenBack();
			else if (distanceToRight > distanceToLeft && mode == FlipMode.LeftToRight)
				TweenBack();
			else
				TweenForward();
		}
	}


	public void SetSection(Book.Section section) {
		currLeft = null;
		currRight = null;
		prevLeft = null;
		prevRight = null;
		nextLeft = null;
		nextRight = null;
		switch (this.section) {
			case magic:
				currentMagicPage = currentPage;
				break;
			case quests:
				currentQuestPage = currentPage;
				break;
			case diary:
				currentDiaryPage = currentPage;
				break;
		}

		this.section = section;
		switch (section) {
			case magic:
				pages = magicPages;
				currentPage = currentMagicPage;
				baseBookContent = magicBookContent;
				break;
			case quests:
				questPages = new List<PageMaker>(questBookContent.content);
				pages = questPages;
				currentPage = currentQuestPage;
				baseBookContent = questBookContent;
				break;
			case diary:
				pages = diaryPages;
				currentPage = currentDiaryPage;
				baseBookContent = diaryBookContent;
				break;
		}
		MakePages();
		UpdateCurrentPages();
	}
	public void SetMagicSection(PageMaker pm = null) {
		if (pm) {
			if (section == magic)
				currentPage = magicBookContent.GetPageIndex(pm);
			else
				currentMagicPage = magicBookContent.GetPageIndex(pm);
		}
		SetSection(magic);
	}
	public void SetMagicSection() {
		SetMagicSection(null);
	}
	public void SetQuestSection() {
		SetSection(quests);
	}
	public void SetDiarySection() {
		SetSection(diary);
	}

	Coroutine currentCoroutine;
	public void UpdateCurrentPages() {
		SetPage(LeftNext, currLeft);
		SetPage(RightNext, currRight);
	}

	public void TweenForward() {
		if (mode == FlipMode.RightToLeft)
			currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f, () => { Flip(); }));
		else
			currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f, () => { Flip(); }));
	}
	void Flip() {
		if (mode == FlipMode.RightToLeft) {
			var pt = baseBookContent.GetNextAvailablePage(currentPage);     // trouver la page suivante
			if (pt) {
				currentPage = pages.IndexOf(pt);
				SetPage(LeftNext, nextLeft);
				SetPage(RightNext, nextRight);
			}
		} else {
			var pt = baseBookContent.GetPreviousAvailablePage(currentPage); // trouver la page précédente
			if (pt) {
				currentPage = pages.IndexOf(pt);
				SetPage(LeftNext, prevLeft);
				SetPage(RightNext, prevRight);
			}
		}

		MakePages();

		LeftNext.transform.SetParent(bookPanel.transform, true);
		Left.transform.SetParent(bookPanel.transform, true);
		LeftNext.transform.SetParent(bookPanel.transform, true);
		Left.gameObject.SetActive(false);
		Right.gameObject.SetActive(false);
		Right.transform.SetParent(bookPanel.transform, true);
		RightNext.transform.SetParent(bookPanel.transform, true);

		//StartUpdateSprites2();

		if (OnFlip != null)
			OnFlip.Invoke();
	}
	public void TweenBack() {
		if (mode == FlipMode.RightToLeft) {
			currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f,
				() => {
					UpdateCurrentPages();
					RightNext.transform.SetParent(bookPanel.transform);
					Right.transform.SetParent(bookPanel.transform);

					Left.gameObject.SetActive(false);
					Right.gameObject.SetActive(false);
					pageDragging = false;
				}
				));
		} else {
			currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f,
				() => {
					UpdateCurrentPages();

					LeftNext.transform.SetParent(bookPanel.transform);
					Left.transform.SetParent(bookPanel.transform);

					Left.gameObject.SetActive(false);
					Right.gameObject.SetActive(false);
					pageDragging = false;
				}
				));
		}
	}
	public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish) {
		int steps = (int)(duration / 0.025f);
		Vector3 displacement = (to - f) / steps;
		for (int i = 0; i < steps - 1; i++) {
			if (mode == FlipMode.RightToLeft)
				UpdateBookRTLToPoint(f + displacement);
			else
				UpdateBookLTRToPoint(f + displacement);

			yield return new WaitForSeconds(0.025f);
		}
		if (onFinish != null)
			onFinish();
	}

	void SetPage(Transform owner, GameObject page) {
		page.transform.SetParent(owner, false);
	}

	void MakePages() {
		foreach (PageMaker pm in pagesStack.GetComponentsInChildren<PageMaker>()) {
			Destroy(pm.gameObject);
		}
		MakePreviousPages();
		MakeCurrentPages();
		MakeNextPages();
	}

	public void MakeCurrentPages() {
		if (currentPage >= 0 && currentPage < TotalPageCount) {
			PageMaker pm = pages[currentPage];
			pm.Make();
			pm.SetSide(left);
			currLeft = Instantiate(pm.gameObject, pagesStack, false).gameObject;
			pm.SetSide(right);
			currRight = Instantiate(pm.gameObject, pagesStack, false).gameObject;
		}
	}
	void MakePreviousPages() {
		PageMaker pm = baseBookContent.GetPreviousAvailablePage(currentPage);
		if (pm) {
			pm.Make();
			pm.SetSide(left);
			prevLeft = Instantiate(pm.gameObject, pagesStack, false).gameObject;
			pm.SetSide(right);
			prevRight = Instantiate(pm.gameObject, pagesStack, false).gameObject;
		}
	}
	void MakeNextPages() {
		PageMaker pm = baseBookContent.GetNextAvailablePage(currentPage);
		if (pm) {
			pm.Make();
			pm.SetSide(left);
			nextLeft = Instantiate(pm.gameObject, pagesStack, false).gameObject;
			pm.SetSide(right);
			nextRight = Instantiate(pm.gameObject, pagesStack, false).gameObject;
		}
	}
}
