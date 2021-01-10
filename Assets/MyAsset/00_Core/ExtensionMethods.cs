using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods
{
    public static Transform FindChildren(this Transform transform, string name)
    {
        int count = transform.childCount;

        Transform temp;

        for (int i = 0; i < count; ++i)
        {
            temp = transform.GetChild(i);

            if (temp.name == name)
            {
                return temp;
            }
            else if (temp.childCount > 0)
            {
                temp = FindChildren(temp, name);

                if (temp != null)
                {
                    return temp;
                }
            }
        }

        return null;
    }
    public static void ChangeAlpha(this Image image, float a)
    {
        if (a < 0)
            a = 0;
        if (a > 1)
            a = 1;

        Color temp = image.color;
        temp.a = a;
        image.color = temp;
    }
}
