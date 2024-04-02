#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeHashtable : IExSerializeObject
{
    
    
    static Dictionary<FieldInfo, bool> foldOuts = new Dictionary<FieldInfo, bool>();
    static Dictionary<FieldInfo, Vector2> scrollPositions = new Dictionary<FieldInfo, Vector2>();
    
    public new static void DrawOnInspector(FieldInfo info, Object monoBehaviour)
    {
        Hashtable source = info.GetValue(monoBehaviour) as Hashtable;

        if (!foldOuts.ContainsKey(info))
        {
            foldOuts[info] = false;
            scrollPositions[info] = Vector2.zero;
        }

        foldOuts[info] = EditorGUILayout.Foldout(foldOuts[info], info.Name, true);

        if (foldOuts[info] && source != null)
        {
            List<object> keys = new List<object>();
            List<object> values = new List<object>();

            for (IEnumerator iterator = source.Keys.GetEnumerator(); iterator.MoveNext();)
            {
                keys.Add(iterator.Current);
            }
            for (IEnumerator iterator = source.Values.GetEnumerator(); iterator.MoveNext();)
            {
                values.Add(iterator.Current);
            }

            
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.green;
            scrollPositions[info] = EditorGUILayout.BeginScrollView(scrollPositions[info], true, false);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Key", style);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Value", style);
            EditorGUILayout.EndHorizontal();
            for (int index = 0; index < keys.Count; ++index)
            {
                EditorGUILayout.BeginHorizontal();
                keys[index] = DrawDataField(keys[index]);
                EditorGUILayout.Space(10);
                values[index] = DrawDataField(values[index]);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(10);
            EditorGUILayout.EndScrollView();

            Hashtable result = new Hashtable();
            for (int index = 0; index < keys.Count; ++index)
            {
                int rIndex = keys.Count - index - 1;
                result[keys[rIndex]] = values[rIndex];
            }

            info.SetValue(monoBehaviour, (object)(result));
        }
        
    }
}

#endif