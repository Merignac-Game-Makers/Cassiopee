using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour
{
	public FlipMode Mode;
	public float PageFlipTime = .2f;
	public float TimeBetweenPages = 1;
	public float DelayBeforeStarting = 0;
	public bool AutoStartFlip = true;
	public Book ControledBook;
	public int AnimationFramesCount = 40;
	bool isFlipping = false;
	// Use this for initialization
	void Start() {
		//PageFlipTime /= 10;
		if (!ControledBook)
			ControledBook = GetComponent<Book>();
		if (AutoStartFlip)
			StartFlipping();
		ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
	}
	void PageFlipped() {
		isFlipping = false;
	}
	public void StartFlipping() {
		StartCoroutine(FlipToEnd());
	}
	public void FlipRightPage() {

		if (ControledBook.baseBookContent.GetNextAvailablePage(ControledBook.currentPage)) {
			if (isFlipping) return;
			if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;
			isFlipping = true;
			float frameTime = PageFlipTime / AnimationFramesCount;
			float xc = (ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x);
			float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.95f;
			float dx = xc / AnimationFramesCount;
			StartCoroutine(FlipRTL(xc, h, frameTime, dx));
		}
	}
	public void FlipLeftPage() {
		if (ControledBook.baseBookContent.GetPreviousAvailablePage(ControledBook.currentPage)) {
			if (isFlipping) return;
			if (ControledBook.currentPage <= 0) return;
			isFlipping = true;
			float frameTime = PageFlipTime / AnimationFramesCount;
			float xc = (ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x);
			float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.95f;
			float dx = xc / AnimationFramesCount;
			StartCoroutine(FlipLTR(xc, h, frameTime, dx));
		}
	}
	IEnumerator FlipToEnd() {
		yield return new WaitForSeconds(DelayBeforeStarting);
		float frameTime = PageFlipTime / AnimationFramesCount;
		float xc = (ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x);
		float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.95f;
		float dx = xc / AnimationFramesCount;
		// y = -(h/(xl)^2)*(x-xc)^2         
		//               y         
		//               |          
		//               |          
		//               |          
		//_______________|_________________x         
		//              o|o             |
		//           o   |   o          |
		//         o     |     o        | h
		//        o      |      o       |
		//       o------xc-------o      -
		//               |
		//               |
		switch (Mode) {
			case FlipMode.RightToLeft:
				while (ControledBook.currentPage < ControledBook.TotalPageCount) {
					StartCoroutine(FlipRTL(xc, h, frameTime, dx));
					yield return new WaitForSeconds(TimeBetweenPages);
				}
				break;
			case FlipMode.LeftToRight:
				while (ControledBook.currentPage > 0) {
					StartCoroutine(FlipLTR(xc, h, frameTime, dx));
					yield return new WaitForSeconds(TimeBetweenPages);
				}
				break;
		}
	}
	IEnumerator FlipRTL(float xc, float h, float frameTime, float dx) {
		float x = xc - dx;
		float kx = x - xc / 2;
		float y = -h + (1 - (kx / xc * 2 * kx / xc * 2)) * h / 5;
		ControledBook.DragRightPageToPoint(new Vector3(kx, y, 0));
		for (int i = 0; i < AnimationFramesCount; i++) {
			kx = x - xc / 2;
			y = -h + (1 - (kx / xc * 2 * kx / xc * 2)) * h / 5;
			ControledBook.UpdateBookRTLToPoint(new Vector3(kx, y, 0));
			yield return new WaitForSeconds(frameTime);
			x -= dx;
		}
		ControledBook.f = new Vector3(kx, y, 0);
		ControledBook.ReleasePage();
		isFlipping = false;

	}
	IEnumerator FlipLTR(float xc, float h, float frameTime, float dx) {
		float x = dx;
		float kx = x - xc / 2;
		float y = -h + (1 - (kx / xc * 2 * kx / xc * 2)) * h / 5;
		ControledBook.DragLeftPageToPoint(new Vector3(kx, y, 0));
		for (int i = 0; i < AnimationFramesCount; i++) {
			kx = x - xc / 2;
			y = -h + (1 - (kx / xc * 2 * kx / xc * 2)) * h / 5;
			ControledBook.UpdateBookLTRToPoint(new Vector3(kx, y, 0));
			yield return new WaitForSeconds(frameTime);
			x += dx;
		}
		ControledBook.f = new Vector3(kx, y, 0);
		ControledBook.ReleasePage();
		isFlipping = false;
	}
}
