using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 보기 전용 (수정 가능)
public class ShowOnlyAttribute : PropertyAttribute
{

}

// 읽기 전용 (수정 불가능)
public class ReadOnlyAttribute : PropertyAttribute
{
    public readonly bool runtimeOnly;

    public ReadOnlyAttribute(bool runtimeOnly = false)
    {
        this.runtimeOnly = runtimeOnly;
    }
}