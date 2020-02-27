using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class NullReferensFinder : EditorWindow
{
    struct NullObjectInfo
    {
        public UnityEngine.Object Target;
        public FieldInfo TargetField;
        public WarningLevel Level;
    }

    private List<NullObjectInfo> _findingCaсhe;

    [MenuItem("Window/Analysis/Null Referens Cheacker")]
    public static void ShowWindow()
    {
        GetWindow<NullReferensFinder>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find"))
            _findingCaсhe = FindNuleables();

        DrawNulleables();
    }

    List<NullObjectInfo> FindNuleables()
    {
        var value = new List<NullObjectInfo>();
        var monobevahers = FindObjectsOfType<MonoBehaviour>();

        foreach (var item in monobevahers)
        {
            List<NullObjectInfo> fields;

            if (CanTakeObject(item, out fields) == false)
                continue;

            value.AddRange(fields);
        }

        bool CanTakeObject(MonoBehaviour exemplar, out List<NullObjectInfo> fields)
        {
            fields = new List<NullObjectInfo>();
            bool result = false;

            foreach (var item in exemplar.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var atribute = item.GetCustomAttribute<NullError>(true);
                var selfValue = item.GetValue(exemplar);

                if ((selfValue == null ? true : selfValue.Equals(null)) == false || atribute == null)
                    continue;

                fields.Add(new NullObjectInfo { Target = exemplar, Level = atribute.Level, TargetField = item });
                result = true;
            }

            return result;
        }

        return value;
    }

    void DrawNulleables()
    {
        if (_findingCaсhe == null)
            return;

        foreach (var item in _findingCaсhe)
            DrawNulleable(item);

        void DrawNulleable(NullObjectInfo nulleableObject)
        {
            GUILayout.BeginHorizontal();

            if (nulleableObject.Level == WarningLevel.low)
                GUILayout.Label("low warning level");
            else
                GUILayout.Label("high warning level", EditorStyles.boldLabel);

            GUILayout.Label($"field: {nulleableObject.TargetField.Name}");
            GUILayout.Label($"object: {nulleableObject.Target.name}");

            if (GUILayout.Button("Go To Object"))
                Selection.activeObject = nulleableObject.Target;

            GUILayout.EndHorizontal();
        }
    }

}
#endif