using UnityEditor;

public class WaterInWell : QuestBase
{

	public MagicPageMaker magicPage;

	/// <summary>
	/// bien qu'apparement inutiles les méthodes 'Quest...'
	/// sont nécessaires ici pour que le VIDE_Dialogue Editor puisse les utiliser
	/// (il ne gère pas les classes héritées)
	/// </summary>
	public override void RefuseQuest() {
		base.RefuseQuest();
	}
	public override void AcceptQuest() {
		base.AcceptQuest();
		if (magicPage) {
			magicPage.page.isAvailable = true;
			Book book = Book.Instance;
			if (book) {
				book.SetMagicSection(magicPage);
			}
		}
	}
	public override void QuestDone() {
		base.QuestDone();
	}
	public override void QuestFailed() {
		base.QuestFailed();
	}
	public override void QuestPassed() {
		base.QuestPassed();
	}


	/// <summary>
	/// Conditions de succès de cette quête
	/// </summary>
	/// <returns></returns>
	public override void UpdateStatus() {
		// QuestStatus.Done est mis à jour par MagicWater.cs
	}

	public override void Reset() {
		// no reset
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(WaterInWell)), CanEditMultipleObjects]
public class WaterInWellEditor : QuestBaseEditor
{
	WaterInWell thisQuest;
	SerializedProperty pMagicPage;

	public override void OnEnable() {
		base.OnEnable();
		thisQuest = (WaterInWell)target;
		pMagicPage = serializedObject.FindProperty(nameof(thisQuest.magicPage));

	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
		base.OnInspectorGUI();
		EditorGUILayout.PropertyField(pMagicPage);
		serializedObject.ApplyModifiedProperties();
	}

}
#endif
