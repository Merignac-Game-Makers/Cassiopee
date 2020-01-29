using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBookContent : BaseBookContent
{
    public static MagicBookContent Instance;
    public MagicPageMaker[] content;

    private void Awake() {
        Instance = this;
    }

    public override PageMaker GetNextAvailablePage(int after) {
        for (int i = after + 1; i < content.Length; i++) {
            if (content[i].page.isAvailable)
                return content[i];
        }
        return null;
    }

    public override PageMaker GetPreviousAvailablePage(int before) {
        for (int i = before - 1; i >= 0; i--) {
            if (content[i].page.isAvailable)
                return content[i];
        }
        return null;
    }
}
