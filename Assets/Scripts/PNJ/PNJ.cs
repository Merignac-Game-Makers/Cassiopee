using UnityEngine;
/// <summary>
/// Classe générique pour les PNJ
/// => Intéraction par défaut = interrompre le déplacement + lancer le dialogue
/// </summary>
public class PNJ : InteractableObject
{
    public string Name;
    public Sprite image;

    public override bool IsInteractable => true;

    public override void InteractWith(HighlightableObject target) {
        base.InteractWith(target);
        PlayerManager.Instance.StopAgent();
        GetComponentInChildren<DialogueTrigger>()?.Run();
    }

}
