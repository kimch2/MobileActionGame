using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	// Unityの非公開クラスをBitbucketから拝借
	// 一度作ったオブジェクトをstatic領域に確保したまま使いまわす為のクラス
	internal static class ListPool<T>
	{
		// Object pool to avoid allocations.
		private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, l => l.Clear());

		public static List<T> Get()
		{
			return s_ListPool.Get();
		}

		public static void Release(List<T> toRelease)
		{
			s_ListPool.Release(toRelease);
		}
	}
}
