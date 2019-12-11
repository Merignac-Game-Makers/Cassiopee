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

	void OnEnable() {
		m_TitleProperty = serializedObject.FindProperty(nameof(Page.Title));
		m_PictureProperty = serializedObject.FindProperty(nameof(Page.Picture));
		m_TextProperty = serializedObject.FindProperty(nameof(Page.Text));

		m_Page = target as Page;
	}
	public override void OnInspectorGUI() {
		EditorStyles.textField.wordWrap = true;

		serializedObject.Update();
		EditorGUILayout.PropertyField(m_TitleProperty);
		EditorGUILayout.PropertyField(m_PictureProperty);
		EditorGUILayout.PropertyField(m_TextProperty, GUILayout.MinHeight(128));
		serializedObject.ApplyModifiedProperties();
	}
}

#endif
