using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DialogueEntryNode.QuestStatus;

public abstract class QuestBase : MonoBehaviour
{

	//public enum QuestStatus { NonAvailable, Available, Accepted, Refused, Done, Failed, Passed }
	[HideInInspector]
	public DialogueEntryNode.QuestStatus status = Available;

	public static QuestsUI questsUI;
	public static QuestManager questManager;

	public Sprite picture;
	public string title;
	public string shortText;

	private void Start() {
		questsUI = QuestsUI.Instance;
		questManager = QuestManager.Instance;
	}



	public virtual void QuestAvailable() {
		status = Available;
		questManager.SetStatus(this, Available);
	}
	public virtual void RefuseQuest() {
		status = Refused;
		questManager.SetStatus(this, Refused);
	}
	public virtual void AcceptQuest() {
		status = Accepted;
		questManager.SetStatus(this, Accepted);
	}
	public virtual void QuestDone() {
		status = Done;
		questManager.SetStatus(this, Done);
	}
	public virtual void QuestFailed() {
		status = Failed;
		questManager.SetStatus(this, Failed);
	}
	public virtual void QuestPassed() {
		status = Passed;
		questManager.SetStatus(this, Passed);
	}
	public abstract bool TestSuccess();
	public abstract void Reset();



}

#if UNITY_EDITOR
[CustomEditor(typeof(QuestBase))]
[CanEditMultipleObjects]
public class QuestBaseEditor : Editor
{
	SerializedProperty p_picture;
	SerializedProperty p_title;
	SerializedProperty p_text;

	QuestBase quest;

	public virtual void OnEnable() {
		quest = (QuestBase) target;
		p_picture = serializedObject.FindProperty(nameof(quest.picture));
		p_title = serializedObject.FindProperty(nameof(quest.title));
		p_text = serializedObject.FindProperty(nameof(quest.shortText));
	}


	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		EditorGUILayout.PropertyField(p_picture);
		p_picture.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Vignette", "Vignette pour cette quête"), quest.picture, typeof(Sprite), false);

		EditorGUILayout.PropertyField(p_title);
		EditorGUILayout.PropertyField(p_text, GUILayout.MinHeight(128));

		serializedObject.ApplyModifiedProperties();
	}
	void OnInspectorUpdate() {
		Repaint();
	}
}
#endif
