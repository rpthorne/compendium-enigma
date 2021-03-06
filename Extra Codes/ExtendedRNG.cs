//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using Random = UnityEngine.Random;
namespace general
{
	public static class ExtendedRNG
	{
		public static bool nextBool(this UnityEngine.Random r)
		{
			return UnityEngine.Random.Range(0, 2) < 1 ? false : true;

		}
		public static int nextInt(this UnityEngine.Random r, int max)
		{
			return UnityEngine.Random.Range(0, max);
		}

        public static float nextFloat(this UnityEngine.Random r, int max)
        {
            return (float)UnityEngine.Random.Range(0, max);
        }
		/**
		 * this method will return the index of a series of options in an array
		 * the array holds the weighted chance of each occurance happening
		 */
		public static int weightedInt(this UnityEngine.Random r, int[] max)
		{
			int tempPos = 0;
			for(int i = 0; i < max.Length; i++)
				tempPos += max[i];
			if (tempPos == 0)
				return -1;
			//throw error
			tempPos = UnityEngine.Random.Range(0, tempPos);
			int iterator;
			for (iterator = 0; tempPos >= 0; iterator++)
				tempPos = tempPos - max[iterator];
			return iterator - 1;
		}

	}
}

