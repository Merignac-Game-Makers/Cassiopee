using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Page", menuName = "Custom/Spell book page", order = -999)]
public class Page : ScriptableObject
{
	public string Title;
	public Sprite Picture;
	public string Text;
}


#if UNITY_EDITOR
[CustomEditor(typeof(Page))]
public class PageEditor : Editor
{
	SerializedProperty m_TitleProperty;
	SerializedProperty m_PictureProperty;
	SerializedProperty m_TextProperty;

	Page m_Page;

	public void OnEnable() {
		m_TitleProperty = serializedObject.FindProperty("Title");
		m_PictureProperty = serializedObject.FindProperty("Picture");
		m_TextProperty = serializedObject.FindProperty("Text");

		//serializedObject.ApplyModifiedProperties();
		m_Page = (Page)target;
	}

	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;
		serializedObject.Update();

		EditorGUILayout.PropertyField(m_TitleProperty);
		EditorGUILayout.PropertyField(m_PictureProperty);

		//EditorGUILayout.PropertyField(m_TextProperty, GUILayout.MinHeight(128));
		var options = new GUILayoutOption[] { GUILayout.MinHeight(128), GUILayout.ExpandHeight(true)};
		m_TextProperty.stringValue = GUILayout.TextArea(m_TextProperty.stringValue, options);
		//m_TextProperty.stringValue = CreateEditor(m_Page).target.ToString();
		//m_TextProperty.stringValue.Replace("\\n", "\n");
		serializedObject.ApplyModifiedProperties();
	}
}
#endif
