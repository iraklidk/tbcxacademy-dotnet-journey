using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    public static class CollectionExtensions
    {
        public static int[] MergeSequence(this int[] array, int[] anotherArray)
        {
            int sizeArray = array.Length;
            int sizeAnotherArray = anotherArray.Length;
            int[] newArray = new int[sizeArray + sizeAnotherArray];
            for (int i = 0; i < sizeArray; ++i)
            {
                newArray[i] = array[i];
            }
            for (int j = 0; j < sizeAnotherArray; ++j)
            {
                newArray[sizeArray + j] = anotherArray[j];
            }

            return newArray;
        }
    }
}