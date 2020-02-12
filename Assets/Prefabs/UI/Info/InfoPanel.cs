using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
	public TMP_Text title, text;
	public Image checkMark;
	Animator animator;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	public void Toggle(bool on) {
		if (on)
			animator.SetTrigger("open");
		else
			animator.SetTrigger("close");
	}
	public void Set(string title, string text, bool check) {
		this.title.text = title;
		this.text.text = text;
		checkMark.enabled = check;
	}

	public void Show(float s) {
		StartCoroutine(Ishow(s));
	}
	IEnumerator Ishow(float s) {
		Toggle(true);
		yield return new WaitForSeconds(s);
		Toggle(false);
	}
}
