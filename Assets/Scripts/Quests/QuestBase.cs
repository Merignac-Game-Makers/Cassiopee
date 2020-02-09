using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static QuestBase.QuestStatus;

public abstract class QuestBase : MonoBehaviour, IQuest
{
	[SerializeField]
	public enum QuestStatus { Unavailable, Available, Refused, Accepted, Done, Failed, Passed }

	[HideInInspector]
	public QuestStatus status = Unavailable;

	//public static QuestsUI questsUI;
	public static QuestManager questManager;

	public Sprite picture;
	public string title;
	public string shortText;

	public QuestBase[] nextQuests;     // quêtes suivantes qui ne seront accessible qu'après celle-ci
	[HideInInspector]
	public QuestBase[] previousQuests; // quêtes précédentes 

	public InfoPanel infopanel { get; private set; }

	private void Start() {
		//questsUI = QuestsUI.Instance;
		questManager = QuestManager.Instance;
		infopanel = UIManager.Instance.GetComponentInChildren<InfoPanel>();
	}



	public virtual void QuestUnavailable() {
		//status = Available;
		questManager.SetStatus(this, Unavailable);
	}
	public virtual void QuestAvailable() {
		//status = Available;
		questManager.SetStatus(this, Available);
	}
	public virtual void RefuseQuest() {
		//status = Refused;
		questManager.SetStatus(this, Refused);
	}
	public virtual void AcceptQuest() {
		//status = Accepted;
		//UIManager.Instance.inventoryButton?.gameObject.GetComponentInParent<Animator>()?.SetTrigger("startColor");
		infopanel.Set("Nouvel objectif", shortText, false);
		infopanel.Show(3);
		QuestBookContent.Instance.AddQuest(this);
		questManager.SetStatus(this, Accepted);
	}
	public virtual void QuestDone() {
		//status = Done;
		//UIManager.Instance.inventoryButton?.gameObject.GetComponentInParent<Animator>()?.SetTrigger("startColor");
		infopanel.Set("Objectif atteint", shortText, true);
		infopanel.Show(3);
		QuestBookContent.Instance.UpdateQuest(this);
		questManager.SetStatus(this, Done);
	}
	public virtual void QuestFailed() {
		//status = Failed;
		questManager.SetStatus(this, Failed);
	}
	public virtual void QuestPassed() {
		//status = Passed;
		questManager.SetStatus(this, Passed);
	}
	public abstract void UpdateStatus();
	public abstract void Reset();

	public void QuestStep(QuestBase quest, QuestStatus status) {
		questManager.SetStatus(quest, status);
	}

	public bool IsPending() {
		return status == Accepted || status == Failed;
	}
	public bool IsDone() {
		return status == Done || status == Passed;
	}
}



#if UNITY_EDITOR
[CustomEditor(typeof(QuestBase))]
[CanEditMultipleObjects]
public class QuestBaseEditor : Editor
{
	SerializedProperty p_status;
	SerializedProperty p_picture;
	SerializedProperty p_title;
	SerializedProperty p_text;

	QuestBase quest;

	public virtual void OnEnable() {
		quest = (QuestBase)target;

		p_status = serializedObject.FindProperty(nameof(quest.status));
		p_picture = serializedObject.FindProperty(nameof(quest.picture));
		p_title = serializedObject.FindProperty(nameof(quest.title));
		p_text = serializedObject.FindProperty(nameof(quest.shortText));
	}


	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();
		//var status = p_status.objectReferenceValue as QuestBase.QuestStatus;
		EditorGUILayout.PropertyField(p_status);

		EditorGUILayout.PropertyField(p_picture);
		p_picture.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Vignette", "Vignette pour cette quête"), quest.picture, typeof(Sprite), false);

		EditorGUILayout.PropertyField(p_title);
		EditorGUILayout.PropertyField(p_text, GUILayout.MinHeight(128));

		serializedObject.ApplyModifiedProperties();
	}
}
#endif
