using UnityEngine;

public class MagicTarget : InteractableObject
{
	public override bool IsInteractable => true;

	public bool isFree => IsFree();

	Vector3 m_TargetPoint;

	void Awake() {
		m_TargetPoint = transform.position;
	}

	protected override void Start() {
		base.Start();
	}


	void Update() {
		Debug.DrawLine(m_TargetPoint, m_TargetPoint + new Vector3(0, 2, 0), Color.magenta);
	}

	public override void InteractWith(HighlightableObject target) {

	}

	public bool IsFree() {
		return !GetComponentInChildren<MagicOrb>();
	}

	public void MakeMagicalStuff(MagicOrb orb) {
		Debug.Log("DO MAGIC !!!");
		if (orb.GetComponentInChildren<MagicOrb>().orbType == MagicOrb.OrbType.Moon) {
			gameObject.transform.localScale /= 1.2f;
		} else {
			gameObject.transform.localScale *= 1.2f;
		}
		Destroy(orb.gameObject);
	}
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(Target))]
//public class TargetObjectEditor : Editor
//{
//	SerializedProperty m_IsQuest;
//	SerializedProperty m_Quest;
//	SerializedProperty m_Mode;

//	void OnEnable() {
//		m_IsQuest = serializedObject.FindProperty("IsQuest");
//		m_Quest = serializedObject.FindProperty("Quest");
//		m_Mode = serializedObject.FindProperty("m_Mode");

//		serializedObject.ApplyModifiedProperties();
//	}

//	public override void OnInspectorGUI() {
//		serializedObject.Update();

//		EditorGUILayout.PropertyField(m_Mode);
//		EditorGUILayout.PropertyField(m_IsQuest);

//		if (m_IsQuest.boolValue) {
//			EditorGUILayout.PropertyField(m_Quest);
//		}

//		serializedObject.ApplyModifiedProperties();

//	}

//}
//#endif
