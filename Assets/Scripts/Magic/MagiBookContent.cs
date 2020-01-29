using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiBookContent : MonoBehaviour
{
    public static MagiBookContent Instance;
    public PageTemplate[] content;

    private void Awake() {
        Instance = this;
        content = GetComponents<PageTemplate>();
    }

    public PageTemplate GetNextAvailablePage(int after) {
        for (int i = after+1; i<content.Length; i++) {
            if (content[i].page.isAvailable)
                return content[i];
        }
        return null;
    }

    public PageTemplate GetPreviousAvailablePage(int before) {
        for (int i = before - 1; i >= 0; i--) {
            if (content[i].page.isAvailable)
                return content[i];
        }
        return null;
    }
}
