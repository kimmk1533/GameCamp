using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

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
        obj.m_ConditionType = (E_SelectableObjectConditionType)EditorGUILayout.EnumFlagsField("행동의 조건", obj.m_ConditionType);

        this.serializedObject.Update();

        if (obj.m_ConditionType.HasFlag(E_SelectableObjectConditionType.HasItem))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_RequireItems"), new GUIContent("필요한 아이템들"), true);
        }
        if (obj.m_ConditionType.HasFlag(E_SelectableObjectConditionType.ActiveItem))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            obj.m_RequireItem = (E_ItemType)EditorGUILayout.EnumPopup("필요한 아이템", obj.m_RequireItem);
            //EditorGUILayout.IntField("필요한 아이템의 인덱스", obj.m_RequireItemIndex);
        }
        if (obj.m_ConditionType.HasFlag(E_SelectableObjectConditionType.SameImage))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            if (obj.m_CheckRenderer == null)
            {
                obj.m_CheckImage = (Image)EditorGUILayout.ObjectField("원본 이미지", obj.m_CheckImage, typeof(Image), true);
            }
            if (obj.m_CheckImage == null)
            {
                obj.m_CheckRenderer = (SpriteRenderer)EditorGUILayout.ObjectField("원본 렌더러", obj.m_CheckRenderer, typeof(SpriteRenderer), true);
            }
            obj.m_CheckSprite = (Sprite)EditorGUILayout.ObjectField("확인 할 이미지", obj.m_CheckSprite, typeof(Sprite), true);
        }
        if (obj.m_ConditionType.HasFlag(E_SelectableObjectConditionType.CheckActive))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            GUILayout.Label("현재 이 상태일 경우 조건 충족");
            obj.m_CheckActive = EditorGUILayout.Toggle("상태", obj.m_CheckActive);
            obj.m_ActiveObject = (GameObject)EditorGUILayout.ObjectField("확인 할 오브젝트", obj.m_ActiveObject, typeof(GameObject), true);
        }
        if (obj.m_ConditionType.HasFlag(E_SelectableObjectConditionType.CheckPosition))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            GUILayout.Label("현재 이 위치일 경우 조건 충족");
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_CheckPosition"), new GUIContent("위치"), true);
            obj.m_PositionObject = (GameObject)EditorGUILayout.ObjectField("확인 할 오브젝트", obj.m_PositionObject, typeof(GameObject), true);
        }

        if (obj.m_ConditionType != 0)
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
        }

        GUILayout.Space(5f);
        obj.m_ActionType = (E_SelectableObjectActionType)EditorGUILayout.EnumFlagsField("클릭됐을 때 할 행동", obj.m_ActionType);

        GUILayout.Space(5f);
        GUILayout.Label("======================================");
        GUILayout.Space(5f);

        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_StartEvent"), true);
        GUILayout.Space(5f);
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_EndEvent"), true);

        if (obj.m_ActionType == 0)
        {
            this.serializedObject.ApplyModifiedProperties();
            return;
        }

        GUILayout.Space(5f);
        GUILayout.Label("======================================");
        GUILayout.Space(5f);

        obj.m_DirActive = EditorGUILayout.Toggle("화살표 변경?", obj.m_DirActive);

        if (obj.m_DirActive)
        {
            obj.m_LeftActive = EditorGUILayout.Toggle("왼쪽 방향", obj.m_LeftActive);
            obj.m_RightActive = EditorGUILayout.Toggle("오른쪽 방향", obj.m_RightActive);
            obj.m_UpActive = EditorGUILayout.Toggle("위쪽 방향", obj.m_UpActive);
            obj.m_DownActive = EditorGUILayout.Toggle("아래쪽 방향", obj.m_DownActive);
        }

        if (obj.m_ActionType.HasFlag(E_SelectableObjectActionType.SetActive))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            //m_ListCount = EditorGUILayout.IntField("오브젝트 갯수", m_ListCount);

            SerializedProperty m_Actives = this.serializedObject.FindProperty("m_Actives");
            SerializedProperty m_Objects = this.serializedObject.FindProperty("m_Objects");

            GUILayout.Label("[ m_Actives랑 m_Objects 크기 일치해야함 ]");
            GUILayout.Label("[ 첫 번째 오브젝트는 null값일 시 자기 자신이 됨 ]");

            EditorGUILayout.PropertyField(m_Actives, true);
            EditorGUILayout.PropertyField(m_Objects, true);

            //m_Actives.arraySize = m_ListCount;
            //m_Objects.arraySize = m_ListCount;

            //EditorList.Show(m_ListCount, m_Actives, m_Objects);
        }
        if (obj.m_ActionType.HasFlag(E_SelectableObjectActionType.MoveCamera))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            if (!obj.m_MoveToPreviousPos)
            {
                obj.m_DirectionMove = EditorGUILayout.Toggle("현재 방향으로 이동?", obj.m_DirectionMove);
            }
            if (!obj.m_DirectionMove)
            {
                obj.m_MoveToPreviousPos = EditorGUILayout.Toggle("이전 위치로 이동?", obj.m_MoveToPreviousPos);
            }

            if (!obj.m_DirectionMove && !obj.m_MoveToPreviousPos)
            {
                GUILayout.Space(5f);
                GUILayout.Label("======================================");
                GUILayout.Space(5f);

                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Position"), new GUIContent("이동할 위치"));

                //obj.m_Position = EditorGUILayout.Vector3Field("이동할 위치", obj.m_Position);
            }
        }
        if (obj.m_ActionType.HasFlag(E_SelectableObjectActionType.ChangeImage))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            obj.m_IsOnce = EditorGUILayout.Toggle("한 번만 실행?", obj.m_IsOnce);

            GUILayout.Space(5f);
            obj.m_Sprite = (Sprite)EditorGUILayout.ObjectField("변경 될 이미지", obj.m_Sprite, typeof(Sprite), true);

            GUILayout.Space(5f);
            GUILayout.Label("[ 아무 값도 안넣을 경우 자기 자신 ]");

            if (obj.m_Renderer == null)
            {
                obj.m_Image = (Image)EditorGUILayout.ObjectField("변경 할 이미지", obj.m_Image, typeof(Image), true);
            }
            if (obj.m_Image == null)
            {
                obj.m_Renderer = (SpriteRenderer)EditorGUILayout.ObjectField("변경 할 렌더러", obj.m_Renderer, typeof(SpriteRenderer), true);
            }
        }
        if (obj.m_ActionType.HasFlag(E_SelectableObjectActionType.AddItem))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_AddItemTypes"));
            //obj.m_AddItemTypes = (E_ItemType)EditorGUILayout.EnumPopup("추가할 아이템들", obj.m_AddItemTypes);
        }
        if (obj.m_ActionType.HasFlag(E_SelectableObjectActionType.RemoveItem))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_RemoveItemTypes"));
            //obj.m_RemoveItemTypes = (E_ItemType)EditorGUILayout.EnumPopup("제거할 아이템들", obj.m_RemoveItemTypes);
        }
        if (obj.m_ActionType.HasFlag(E_SelectableObjectActionType.PlaySound))
        {
            GUILayout.Space(5f);
            GUILayout.Label("======================================");
            GUILayout.Space(5f);

            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_AudioStartSeconds"), new GUIContent("오디오 재생 대기 시간"), true);
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_Audios"), new GUIContent("재생할 오디오"), true);
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}
