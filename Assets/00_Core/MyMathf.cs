using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMathf
{
    public static int Factorial(int num)
    {
        int sum = 1;

        for (int i = 1; i <= num; ++i)
        {
            sum *= i;
        }

        return sum;
    }
    public static int Combination(int n, int r)
    {
        int numerator = Factorial(n);
        int denominator = Factorial(r) * Factorial(n - r);

        return numerator / denominator;
    }
}
