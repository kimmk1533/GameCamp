using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인스펙터 읽기 전용 설정 스크립트.
/*[사용법].
 * 변수 앞에 [ReadOnly]를 붙여 사용.
 * 어트리뷰트 옆에 괄호붙이고 bool값 삽입 가능(기본값 = false, 인게임 일때만 readonly = true, 항상 readonly = false).
*/

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