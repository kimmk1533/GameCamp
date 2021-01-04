using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자 정의 수학 함수 스크립트.

public static class MyMathf
{
    public static int Factorial(int num)    //팩토리얼(n!).
    {
        int sum = 1;

        for (int i = 1; i <= num; ++i)
        {
            sum *= i;
        }

        return sum;
    }
    public static int Combination(int n, int r) //조합.
    {
        int numerator = Factorial(n);
        int denominator = Factorial(r) * Factorial(n - r);

        return numerator / denominator;
    }
}
