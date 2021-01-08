using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(SelectableObject))]
public class SelectableObjectEditor : Editor
{
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

        if (obj.m_Type == 0)
            return;

        if (obj.m_Type.HasFlag(E_SelectableObjectActionType.SetActive))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            obj.m_Active = EditorGUILayout.Toggle("변경 할 상태", obj.m_Active);

            GUILayout.Space(5f);
            GUILayout.Label("[ 아무 값도 안넣을 경우 자기 자신 ]");
            obj.m_Object = (GameObject)EditorGUILayout.ObjectField("변경 할 오브젝트", obj.m_Object, typeof(GameObject), true);
        }
        if (obj.m_Type.HasFlag(E_SelectableObjectActionType.MoveCamera))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            GUILayout.Label("[ 화살표 표시 여부 ]");

            obj.m_LeftActive = EditorGUILayout.Toggle("왼쪽 방향", obj.m_LeftActive);
            obj.m_RightActive = EditorGUILayout.Toggle("오른쪽 방향", obj.m_RightActive);
            obj.m_UpActive = EditorGUILayout.Toggle("위쪽 방향", obj.m_UpActive);
            obj.m_DownActive = EditorGUILayout.Toggle("아래쪽 방향", obj.m_DownActive);

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
    }
}
