using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
	/// <summary>
	/// Classe générique pour comparer 2 listes
	/// </summary>
	/// <returns>true si les 2 listes contiennent les même éléments</returns>
	public static bool IsLike<T>(this List<T> list, List<T> other) {
		if (list.Count != other.Count)
			return false;
		else {
			for (int i = 0; i < list.Count; i++) {
				if (!list[i].Equals(other[i]))
					return false;
			}
			return true;
		}
	}

	/// <summary>
	/// classe générique pour renvoyer le dernier élément d'une liste
	/// (null si la liste est vide)
	/// </summary>
	public static T Last<T>(this List<T> list) where T : class {
		return list.Count > 0 ? list[list.Count - 1] : null;
	}


	public static List<T> Reverse<T>(this IEnumerable<T> list) where T : class {
		return new List<T>(new Stack<T>(list));
	}

	/// <summary>
	/// modifier la taille d'un liste
	/// </summary>
	public static void Resize<T>(this List<T> list, int sz, T c) {
		int cur = list.Count;
		if (sz < cur)
			list.RemoveRange(sz, cur - sz);
		else if (sz > cur) {
			if (sz > list.Capacity) //this bit is purely an optimisation, to avoid multiple automatic capacity changes.
				list.Capacity = sz;
			list.AddRange(Enumerable.Repeat(c, sz - cur));
		}
	}
	public static void Resize<T>(this List<T> list, int sz) where T : new() {
		Resize(list, sz, new T());
	}


	public static bool HasMethod(this object objectToCheck, string methodName) {
		var type = objectToCheck.GetType();
		return type.GetMethod(methodName) != null;
	}
}
