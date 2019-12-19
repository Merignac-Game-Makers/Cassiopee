using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;
using TMPro;

public class DManager : MonoBehaviour
{

	public static DManager Instance;

	public GameObject container_NPC;
	public Image NPC_Sprite;
	public Text NPC_label;
	public TMP_Text text_NPC;

	public GameObject container_PLAYER;
	public Image PLAYER_Sprite;
	public Text PLAYER_Label;
	public TMP_Text text_PLAYER;
	public GameObject Buttons;
	public TMP_Text[] choices;

	private void Awake() {
		Instance = this;

	}

	// Start is called before the first frame update
	void Start() {
		container_NPC.SetActive(false);
		container_PLAYER.SetActive(false);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			if (VD.isActive)
				VD.Next();
		}
	}

	public void Begin(VIDE_Assign dialog) {
		PlayerControl.Instance.StopAgent();
		VD.OnNodeChange += UpdateUI;
		VD.OnEnd += End;
		VD.BeginDialogue(dialog);
	}

	void UpdateUI(VD.NodeData data) {
		container_NPC.SetActive(false);
		container_PLAYER.SetActive(false);

		if (data.isPlayer) {
			container_PLAYER.SetActive(true);
			// set sprite
			if (data.sprite != null)
				PLAYER_Sprite.sprite = data.sprite;
			else if (VD.assigned.defaultPlayerSprite != null)
				PLAYER_Sprite.sprite = VD.assigned.defaultPlayerSprite;
			//If it has a tag, show it, otherwise let's use the alias we set in the VIDE Assign
			if (data.tag.Length > 0)
				PLAYER_Label.text = data.tag;
			else
				PLAYER_Label.text = "Player";

			for (int i = 0; i < choices.Length; i++) {
				if (i < data.comments.Length) {
					choices[i].transform.parent.gameObject.SetActive(true);
					choices[i].text = data.comments[i];
				} else {
					choices[i].transform.parent.gameObject.SetActive(false);
				}
			}
		} else {
			container_NPC.SetActive(true);

			//Set node sprite if there's any, otherwise try to use default sprite
			if (data.sprite != null) {
				//For NPC sprite, we'll first check if there's any "sprite" key
				//Such key is being used to apply the sprite only when at a certain comment index
				//Check CrazyCap dialogue for reference
				if (data.extraVars.ContainsKey("sprite")) {
					if (data.commentIndex == (int)data.extraVars["sprite"])
						NPC_Sprite.sprite = data.sprite;
					else
						NPC_Sprite.sprite = VD.assigned.defaultNPCSprite; //If not there yet, set default dialogue sprite
				} else { //Otherwise use the node sprites
					NPC_Sprite.sprite = data.sprite;
				}
			} //or use the default sprite if there isnt a node sprite at all
			else if (VD.assigned.defaultNPCSprite != null)
				NPC_Sprite.sprite = VD.assigned.defaultNPCSprite;

			//If it has a tag, show it, otherwise let's use the alias we set in the VIDE Assign
			if (data.tag.Length > 0)
				NPC_label.text = data.tag;
			else
				NPC_label.text = VD.assigned.alias;

			text_NPC.text = data.comments[data.commentIndex];
		}
	}
	public void End(VD.NodeData data) {
		container_NPC.SetActive(false);
		container_PLAYER.SetActive(false);
		VD.OnNodeChange -= UpdateUI;
		VD.OnEnd -= End;
		VD.EndDialogue();
	}
	private void OnDisable() {
		if (container_NPC != null)
			End(null);
	}

	public void GetChoice(int choice) {
		VD.nodeData.commentIndex = choice;
		if (Input.GetMouseButtonUp(0))
			VD.Next();
	}

	public void Next() {
		VD.Next();
	}

	public bool PreConditions(VIDE_Assign dialogue) {
		var data = VD.nodeData;
		if (VD.isActive) {  //Stuff we check while the dialogue is active

		} else {            //Stuff we do right before the dialogue begins
			if (dialogue.alias == "Walter") { 
				if (questStatus == QuestStatus.Refused) {
					dialogue.overrideStartNode = 4;
				} else if (questStatus == QuestStatus.Done) {
					dialogue.overrideStartNode = 8;
				}
			}
		}
		return true;
	}

	public enum QuestStatus { None, Accepted, Refused, Done}
	[HideInInspector]
	public QuestStatus questStatus = QuestStatus.None;
	public void RefuseQuest() {
		questStatus = QuestStatus.Refused;
	}
	public void AcceptQuest() {
		questStatus = QuestStatus.Accepted;
	}
	public void QuestDone() {
		questStatus = QuestStatus.Done;
	}
}
