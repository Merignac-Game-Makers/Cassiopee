using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBookContent : BaseBookContent
{
    public static QuestBookContent Instance;
    public QuestPageMaker[] content;

    private void Awake() {
        Instance = this;
    }

    public override PageMaker GetNextAvailablePage(int after) {
        if (after < content.Length - 1)
            return content[after + 1];
        return null;
    }

    public override PageMaker GetPreviousAvailablePage(int before) {
        if (before > 0)
            return content[before - 1];
        return null;
    }
}
