﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChapterManager : MonoBehaviour
{
	public Chapter chapter ;

	public virtual void Act(Item item) { }
}
