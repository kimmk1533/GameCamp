using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public static class EditorList
{
    public static void Show(SerializedProperty list)
    {
        ++EditorGUI.indentLevel;
        if (list.isExpanded)
        {
            for (int i = 0; i < list.arraySize; ++i)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
        }
        --EditorGUI.indentLevel;
    }
    public static void Show(int Count, SerializedProperty list1, SerializedProperty list2)
    {
        EditorGUILayout.PropertyField(list1);
        ++EditorGUI.indentLevel;
        for (int i = 0; i < Count; ++i)
        {
            EditorGUILayout.PropertyField(list1.GetArrayElementAtIndex(i));
            EditorGUILayout.PropertyField(list2.GetArrayElementAtIndex(i));
        }
        --EditorGUI.indentLevel;
    }
}

[CustomEditor(typeof(SelectableObject))]
public class SelectableObjectEditor : Editor
{
    //public int m_ListCount;

    public override void OnInspectorGUI()
    {
        SelectableObject obj = target as SelectableObject;

        obj.m_Enable = EditorGUILayout.Toggle("활성화", obj.m_Enable);

        if (!obj.m_Enable)
            return;

        GUILayout.Space(5f);
        obj.m_FadeType = (E_FadeType)EditorGUILayout.EnumPopup("페이드 여부", obj.m_FadeType);
        GUILayout.Space(5f);
        obj.m_Type = (E_SelectableObjectActionType)EditorGUILayout.EnumFlagsField("클릭됐을 때 할 행동", obj.m_Type);

        GUILayout.Space(5f);
        GUILayout.Label("======================================");
        GUILayout.Space(5f);

        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_StartEvent"), true);
        GUILayout.Space(5f);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_EndEvent"), true);

        if (obj.m_Type == 0)
            return;

        GUILayout.Space(5f);
        GUILayout.Label("======================================");
        GUILayout.Space(5f);

        GUILayout.Label("[ 화살표 표시 여부 ]");

        obj.m_LeftActive = EditorGUILayout.Toggle("왼쪽 방향", obj.m_LeftActive);
        obj.m_RightActive = EditorGUILayout.Toggle("오른쪽 방향", obj.m_RightActive);
        obj.m_UpActive = EditorGUILayout.Toggle("위쪽 방향", obj.m_UpActive);
        obj.m_DownActive = EditorGUILayout.Toggle("아래쪽 방향", obj.m_DownActive);

        if (obj.m_Type.HasFlag(E_SelectableObjectActionType.SetActive))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            //m_ListCount = EditorGUILayout.IntField("오브젝트 갯수", m_ListCount);

            SerializedProperty m_Actives = this.serializedObject.FindProperty("m_Actives");
            SerializedProperty m_Objects = this.serializedObject.FindProperty("m_Objects");

            ++EditorGUI.indentLevel;
            GUILayout.Label("[ m_Actives랑 m_Objects 크기 일치해야함 ]");
            GUILayout.Label("[ 첫 번째 오브젝트는 null값일 시 자기 자신이 됨 ]");
            --EditorGUI.indentLevel;

            EditorGUILayout.PropertyField(m_Actives, true);
            EditorGUILayout.PropertyField(m_Objects, true);

            //m_Actives.arraySize = m_ListCount;
            //m_Objects.arraySize = m_ListCount;

            //EditorList.Show(m_ListCount, m_Actives, m_Objects);
        }
        if (obj.m_Type.HasFlag(E_SelectableObjectActionType.MoveCamera))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            if (!obj.m_LastActive)
            {
                obj.m_DirectionMove = EditorGUILayout.Toggle("현재 방향으로 이동?", obj.m_DirectionMove);
            }
            if (!obj.m_DirectionMove)
            {
                obj.m_LastActive = EditorGUILayout.Toggle("이전 위치로 이동?", obj.m_LastActive);
            }

            if (!obj.m_DirectionMove && !obj.m_LastActive)
            {
                GUILayout.Space(5f);
                GUILayout.Label("======================================");
                GUILayout.Space(5f);

                obj.m_Position = EditorGUILayout.Vector3Field("이동할 위치", obj.m_Position);
            }
        }
        if (obj.m_Type.HasFlag(E_SelectableObjectActionType.ChangeImage))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            obj.m_IsOnce = EditorGUILayout.Toggle("한 번만 실행?", obj.m_IsOnce);

            GUILayout.Space(5f);
            obj.m_Image = (Sprite)EditorGUILayout.ObjectField("변경 될 이미지", obj.m_Image, typeof(Sprite), true);

            GUILayout.Space(5f);
            GUILayout.Label("[ 아무 값도 안넣을 경우 자기 자신 ]");
            obj.m_Renderer = (SpriteRenderer)EditorGUILayout.ObjectField("변경 할 렌더러", obj.m_Renderer, typeof(SpriteRenderer), true);
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}
