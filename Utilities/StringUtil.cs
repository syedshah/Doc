// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringUtil.cs" company="DST Nexdox">
//   Copyright (c) DST Nexdox. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Utilities
{
  using System;

  public static class StringUtil
  {
    public static Double FuzzyMatch(String stringA, String stringB)
    {
      if (stringA == stringB)
      {
        return 100; // Exact match
      }

      Int32[,] matchArray = new Int32[stringA.Length + 1, stringB.Length + 1]; // matrix
      Int32 weight;
      Double percentage;

      // Initialise matching matrix
      for (Int32 loopa = 0; loopa <= stringA.Length; matchArray[loopa, 0] = loopa++)
      {
      }

      for (Int32 loopa = 0; loopa <= stringB.Length; matchArray[0, loopa] = loopa++)
      {
      }

      if (stringA.Length > 0 && stringB.Length > 0)
      {
        // Interate around matrix....
        for (Int32 loopa = 1; loopa <= stringA.Length; loopa++)
        {
          for (Int32 loopb = 1; loopb <= stringB.Length; loopb++)
          {
            // Identity matches and assign Weight..
            if (stringB.Substring(loopb - 1, 1) == stringA.Substring(loopa - 1, 1))
            {
              weight = 0;
            }
            else
            {
              weight = 1;
            }

            // This is the magical Levenshtein bit....wahooo!!!
            matchArray[loopa, loopb] =
             System.Math.Min(
             System.Math.Min(matchArray[loopa - 1, loopb] + 1, matchArray[loopa, loopb - 1] + 1),
             matchArray[loopa - 1, loopb - 1] + weight);
          }
        }

        percentage = 100 / Convert.ToDouble(System.Math.Max(stringA.Length, stringB.Length));
        percentage = 100 - (percentage * matchArray[stringA.Length, stringB.Length]);
      }
      else
      {
        if (stringA.Length == stringB.Length)
        {
          percentage = 100; // Both empty strings. skip out 100% match
        }
        else
        {
          percentage = 0; // One empty, no match.
        }
      }

      percentage = Math.Round(percentage, 3);

      // Debug.WriteLine(stringA  + "|" + stringB + "|" + dPercentage.ToString());
      return percentage;
    }
  }
}
