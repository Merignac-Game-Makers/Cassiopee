using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Paragraphe de journal", menuName = "Custom/Paragraphe", order = -999)]
public class Paragraph : ScriptableObject
{
	public string id;
	public string text;
}
