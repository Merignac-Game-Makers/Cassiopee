using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpZoom : ZoomBase
{

    public override bool AllDone() {
        return loots.Count == 0;
    }
}
