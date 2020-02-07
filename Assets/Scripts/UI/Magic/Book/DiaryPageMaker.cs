using TMPro;
using UnityEngine.UI;
using static PageMaker.Side;


public class DiaryPageMaker : PageMaker
{
	public TMP_Text title;

	VerticalLayoutGroup currentContent => side == left ? leftContent : rightContent;

	public TMP_Text paragraphTemplate;

	Side side = left;

	public ChapterManager chapterManager;
	public Chapter chapter { get; private set; }

	private void Start() {
		chapter = chapterManager.chapter;
	}
	public override void Make() {
		if (chapter == null) return;
			
		title.text = chapter.title;
		Clear(leftContent);
		Clear(rightContent);

		for (int i = 0; i < chapter.state.Count; i++) {
			if (chapter.paragraphs[i].enabled) {
				AddParagraph(chapter.paragraphs[i].Get(chapter.state[i]));
				//chapter.state[i] = chapter.paragraphs[i].Next();
			}
		}
	}

	public void SetChapterManager(ChapterManager cm) {
		chapterManager = cm;
		chapter = cm.chapter;
	}

	public void AddParagraph(string paragraph) {
		TMP_Text newParagraph = Instantiate(paragraphTemplate, currentContent.transform, false);
		newParagraph.text = paragraph;
	}

	public void EnableParagraph(int p) {
		chapter.EnableParagraph(p);
	}

	public void SetParagraphVersion(int paragraph, int version) {
		chapter.SetParagraphVersion(paragraph, version);
	}

	public void SetParagraphNext(int paragraph) {
		chapter.SetParagraphNext(paragraph);
	}

}
