using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBookContent : MonoBehaviour
{
	public abstract PageMaker GetNextAvailablePage(int after);
	public abstract PageMaker GetPreviousAvailablePage(int before);

	}
