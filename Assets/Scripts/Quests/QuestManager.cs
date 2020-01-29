using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static QuestBase;
using static QuestBase.QuestStatus;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif


public class QuestManager : MonoBehaviour
{

	public static QuestManager Instance;

	public Affectation[] affectations;

	[HideInInspector]
	public List<QuestBase> quests;

	[Serializable]
	public struct Affectation : IEquatable<Affectation>
	{
		public PNJ pnj;
		public QuestBase quest;

		public Affectation(PNJ pnj, QuestBase quest) {
			this.pnj = pnj;
			this.quest = quest;
		}

		public bool Equals(Affectation other) {
			return pnj == other.pnj && quest == other.quest;
		}
	}

	private void Awake() {
		Instance = this;
	}

	public void Init() {
	}

	private void Start() {
		quests = new List<QuestBase>(GetComponentsInChildren<QuestBase>());
	}

	public void Add(QuestBase quest) {
		quests.Add(quest);
	}

	public List<PNJ> getOwners(QuestBase quest) {
		List<PNJ> result = new List<PNJ>();
		foreach (Affectation affectation in affectations) {
			if (affectation.quest == quest)
				result.Add(affectation.pnj);
		}
		return result;
	}
	public List<QuestBase> GetPending() {
		var list = new List<QuestBase>();
		foreach (QuestBase quest in quests) {
			if (quest.status == Accepted || quest.status == Failed)
				list.Add(quest);
		}
		return list;
	}
	public List<QuestBase> GetDone() {
		var list = new List<QuestBase>();
		foreach (QuestBase quest in quests) {
			if (quest.status == Done || quest.status == Passed)
				list.Add(quest);
		}
		return list;
	}

	public void SetStatus(QuestBase quest, QuestStatus status) {
		int idx = quests.IndexOf(quest);
		if (idx != -1) {                                // si la quête est dans la liste
			quests[idx].status = status;                // actualiser son statut
			QuestBookContent.Instance.UpdateQuestList();// mettre l'affichage à jour dans le grimoire
		}
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(QuestManager.Affectation))]
public class AffectationDrawerUIE : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property) {
		// Create property container element.
		var container = new VisualElement();

		// Create property fields.
		var pnj = new PropertyField(property.FindPropertyRelative("pnj"));
		var quest = new PropertyField(property.FindPropertyRelative("quest"));

		// Add fields to the container.
		container.Add(pnj);
		container.Add(quest);

		return container;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		EditorGUI.BeginProperty(position, label, property);

		//label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		var fieldWidth = position.width / 2;

		EditorGUI.PropertyField(
		new Rect(position.x, position.y, fieldWidth, position.height),
			property.FindPropertyRelative("pnj"), GUIContent.none);
		EditorGUI.PropertyField(
		new Rect(position.x + fieldWidth, position.y, fieldWidth, position.height),
			property.FindPropertyRelative("quest"), GUIContent.none);

		EditorGUI.EndProperty();
	}
}
#endif