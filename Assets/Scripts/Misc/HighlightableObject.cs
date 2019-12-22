﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// All object that can be highlighted (enemies, interactable object etc.) derive from this class, which takes care
/// of setting the material parameters for it when it gets highlighted.
/// If the object use another material, it will just ignore all the changes.
/// </summary>
public class HighlightableObject : MonoBehaviour
{
	protected Renderer[] m_Renderers;

	int m_RimColorID;
	int m_RimPowID;
	int m_RimIntensityID;

	float m_on, m_off;


	Color[] m_OriginalRimColor;
	float[] m_SavedRimIntensity;

	MaterialPropertyBlock m_PropertyBlock;

	Color transparent = new Color(0, 0, 0, 0);

	protected virtual void Start() {
		m_Renderers = GetComponents<Renderer>();

		m_RimColorID = Shader.PropertyToID("_RimColor");
		m_RimPowID = Shader.PropertyToID("_RimPower");
		m_RimIntensityID = Shader.PropertyToID("_RimIntensity");

		m_PropertyBlock = new MaterialPropertyBlock();

		m_OriginalRimColor = new Color[m_Renderers.Length];
		m_SavedRimIntensity = new float[m_Renderers.Length];

		for (int i = 0; i < m_Renderers.Length; ++i) {
			var rend = m_Renderers[i];
			var mat = rend.sharedMaterial;

			//it's not a rim colored material, just set that entry to null to skip it on setting
			if (!mat.HasProperty(m_RimColorID)) {
				m_Renderers[i] = null;
				continue;
			}

			m_OriginalRimColor[i] = mat.GetColor(m_RimColorID);

			//"init" the property block with the property we want to retrieve
			m_PropertyBlock.SetColor(m_RimColorID, transparent);
			m_PropertyBlock.SetFloat(m_RimPowID, mat.GetFloat(m_RimPowID));
			m_PropertyBlock.SetFloat(m_RimIntensityID, mat.GetFloat(m_RimIntensityID));

			rend.SetPropertyBlock(m_PropertyBlock);

			m_on = m_PropertyBlock.GetFloat(m_RimPowID) * .25f;
			m_off = m_PropertyBlock.GetFloat(m_RimPowID) * 4.0f;

		}
	}

	/// <summary>
	/// true  : this will switch all the material parameters to make it glow
	/// false : this will switch all the material parameters to make it back to normal
	/// </summary>
	public void Highlight(bool on) {
		for (int i = 0; i < m_Renderers?.Length; ++i) {
			var rend = m_Renderers[i];

			if (rend == null)
				continue;

			rend.GetPropertyBlock(m_PropertyBlock);

			if (on) {
				m_PropertyBlock.SetColor(m_RimColorID, m_OriginalRimColor[i]);
				m_PropertyBlock.SetFloat(m_RimPowID, m_on);

				m_SavedRimIntensity[i] = m_PropertyBlock.GetFloat(m_RimIntensityID);
				m_PropertyBlock.SetFloat(m_RimIntensityID, 1.0f);
			} else {
				m_PropertyBlock.SetColor(m_RimColorID, transparent);
				m_PropertyBlock.SetFloat(m_RimPowID, m_off);
				m_PropertyBlock.SetFloat(m_RimIntensityID, m_SavedRimIntensity[i]);
			}

			rend.SetPropertyBlock(m_PropertyBlock);
		}

	}
}
