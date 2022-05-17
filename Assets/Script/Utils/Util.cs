using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Util {


    public static int Rand(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    public static float Rand(float min, float max)
    {
        return Random.Range(min, max);
    }
    public static int GetPriority(int[] priorities)
    {
        int sum = 0;
        for (int i = 0; i < priorities.Length; ++i)
        {
            sum += priorities[i];
        }

        if (sum <= 0)
            return 0;

        int num = Rand(1, sum);

        sum = 0;
        for (int i = 0; i < priorities.Length; ++i)
        {
            int start = sum;
            sum += priorities[i];
            if (start < num && num <= sum)
            {
                return i;
            }
        }

        return 0;
    }
}
