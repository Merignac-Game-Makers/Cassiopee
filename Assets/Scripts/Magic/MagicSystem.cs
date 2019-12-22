﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour
{

	public enum SelectedArtefact { None, Moon, Sun }
	public SelectedArtefact m_SelectedArtefact;

	public GameObject SunButton;
	public GameObject MoonButton;

	static public MagicSystem Instance;

	private void Awake() {
		Instance = this;
	}

	// Start is called before the first frame update
	void Start() {
		m_SelectedArtefact = SelectedArtefact.Sun;
	}

	// Update is called once per frame
	void Update() {

	}

	public void selectSun() {
		m_SelectedArtefact = SelectedArtefact.Sun;
		MagicController.Instance.UpdateArtefact(m_SelectedArtefact);
		//SunButton.GetComponent<Toggle>().Select();
	}

	public void selectMoon() {
		m_SelectedArtefact = SelectedArtefact.Moon;
		MagicController.Instance.UpdateArtefact(m_SelectedArtefact);
		//MoonButton.GetComponent<Toggle>().Select();
	}

	public void selectNone() {
		m_SelectedArtefact = SelectedArtefact.None;
	}
}
