using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelectableObject))]
public class SelectableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SelectableObject obj = (SelectableObject)target;

        obj.m_Type = (SelectableObject.E_Type)EditorGUILayout.EnumPopup("클릭됐을 때 할 행동", obj.m_Type);

        switch (obj.m_Type)
        {
            case SelectableObject.E_Type.None:
                break;
            case SelectableObject.E_Type.SetActive:
                GUILayout.Space(10f);
                obj.m_Active = EditorGUILayout.Toggle("변경 할 상태", obj.m_Active);
                GUILayout.Space(10f);
                GUILayout.Label("[ 아무 값도 안넣을 경우 자기 자신 ]");
                obj.m_Object = (GameObject)EditorGUILayout.ObjectField("변경 할 오브젝트", obj.m_Object, typeof(GameObject) , true);
                break;
            case SelectableObject.E_Type.MoveCamera:
                GUILayout.Space(10f);
                obj.m_Position = EditorGUILayout.Vector3Field("이동할 위치", obj.m_Position);
                break;
        }
    }
}
