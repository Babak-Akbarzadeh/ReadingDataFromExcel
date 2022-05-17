using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingDataFromExcel
{
	public static class ArrayInitializer

	{

		/// <summary>
		/// This function creates array and initial it
		/// </summary>
		/// <param name="ArrayName">Refrence the name of array (call by refrence)</param>
		/// <param name="FirstDime">It is the size of the first dimantion </param>
		/// <param name="InitVal">It is the initialing value </param>
		public static void CreateArray<T>(ref T[] ArrayName, int FirstDime, T InitVal)
		{
			ArrayName = new T[FirstDime];
			for (int i = 0; i < FirstDime; i++)
			{

				ArrayName[i] = InitVal;
			}
		}

		/// <summary>
		/// This function creates array and initial it
		/// </summary>
		/// <param name="ArrayName">Refrence the name of array (call by refrence)</param>
		/// <param name="FirstDime">It is the size of the first dimantion </param>
		/// <param name="SecondDime">It is the size of the second dimantion </param>
		/// <param name="InitVal">It is the initialing value </param>
		public static void CreateArray<T>(ref T[][] ArrayName, int FirstDime, int SecondDime, T InitVal)
		{
			ArrayName = new T[FirstDime][];
			for (int i = 0; i < FirstDime; i++)
			{
				ArrayName[i] = new T[SecondDime];
				for (int j = 0; j < SecondDime; j++)
				{
					ArrayName[i][j] = InitVal;
				}
			}
		}

		/// <summary>
		/// This function creates array and initial it
		/// </summary>
		/// <param name="ArrayName">Refrence the name of array (call by refrence)</param>
		/// <param name="FirstDime">It is the size of the first dimantion </param>
		/// <param name="SecondDime">It is the size of the second dimantion </param>
		/// <param name="ThirdDime">It is the size of the third dimantion </param>
		/// <param name="InitVal">It is the initialing value </param>
		public static void CreateArray<T>(ref T[][][] ArrayName, int FirstDime, int SecondDime, int ThirdDime, T InitVal)
		{
			ArrayName = new T[FirstDime][][];
			for (int i = 0; i < FirstDime; i++)
			{
				ArrayName[i] = new T[SecondDime][];
				for (int j = 0; j < SecondDime; j++)
				{
					ArrayName[i][j] = new T[ThirdDime];
					for (int k = 0; k < ThirdDime; k++)
					{
						ArrayName[i][j][k] = InitVal;
					}

				}
			}
		}

		/// <summary>
		/// This function creates array and initial it
		/// </summary>
		/// <param name="ArrayName">Refrence the name of array (call by refrence)</param>
		/// <param name="FirstDime">It is the size of the first dimantion </param>
		/// <param name="SecondDime">It is the size of the second dimantion </param>
		/// <param name="ThirdDime">It is the size of the third dimantion </param>
		/// <param name="ForthDime">It is the size of the forth dimantion </param>
		/// <param name="InitVal">It is the initialing value </param>
		public static void CreateArray<T>(ref T[][][][] ArrayName, int FirstDime, int SecondDime, int ThirdDime, int ForthDime, T InitVal)
		{
			ArrayName = new T[FirstDime][][][];
			for (int i = 0; i < FirstDime; i++)
			{
				ArrayName[i] = new T[SecondDime][][];
				for (int j = 0; j < SecondDime; j++)
				{
					ArrayName[i][j] = new T[ThirdDime][];
					for (int k = 0; k < ThirdDime; k++)
					{
						ArrayName[i][j][k] = new T[ForthDime];
						for (int l = 0; l < ForthDime; l++)
						{

							ArrayName[i][j][k][l] = InitVal;

						}

					}

				}
			}
		}



		public static int[] sortOrder(int[] theArray, bool ascending)
		{
			int[] theOrder = new int[theArray.Count()];
			for (int i = 0; i < theArray.Count(); i++)
			{
				theOrder[i] = i;
			}
			for (int x = 0; x < theArray.Count(); x++)
			{
				for (int y = x + 1; y < theArray.Count(); y++)
				{
					if (ascending)
					{
						if (theArray[theOrder[x]] > theArray[theOrder[y]])
						{
							int tmp = theOrder[x];
							theOrder[x] = theOrder[y];
							theOrder[y] = tmp;
						}
					}
					else
					{
						if (theArray[theOrder[x]] > theArray[theOrder[y]])
						{
							int tmp = theOrder[x];
							theOrder[x] = theOrder[y];
							theOrder[y] = tmp;
						}
					}
				}
			}

			return theOrder;
		}

		public static int[] sortOrder(long[] theArray, bool ascending)
		{
			int[] theOrder = new int[theArray.Count()];
			for (int i = 0; i < theArray.Count(); i++)
			{
				theOrder[i] = i;
			}
			for (int x = 0; x < theArray.Count(); x++)
			{
				for (int y = x + 1; y < theArray.Count(); y++)
				{
					if (ascending)
					{
						if (theArray[theOrder[x]] > theArray[theOrder[y]])
						{
							int tmp = theOrder[x];
							theOrder[x] = theOrder[y];
							theOrder[y] = tmp;
						}
					}
					else
					{
						if (theArray[theOrder[x]] > theArray[theOrder[y]])
						{
							int tmp = theOrder[x];
							theOrder[x] = theOrder[y];
							theOrder[y] = tmp;
						}
					}
				}
			}

			return theOrder;
		}
	}
}
