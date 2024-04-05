#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeHashtable : IExSerializeObject
{
    
    
    public new static void DrawOnInspector(FieldInfo info, Object monoBehaviour, ExSerializeFieldFlag flags)
    {
        Hashtable source = info.GetValue(monoBehaviour) as Hashtable;

        if (!foldOuts.ContainsKey(info))
        {
            foldOuts[info] = false;
            scrollPositions[info] = Vector2.zero;
        }

        EditorGUILayout.BeginHorizontal();
        foldOuts[info] = EditorGUILayout.Foldout(foldOuts[info], info.Name, true);
        if (flags.HasFlag(ExSerializeFieldFlag.Clear))
        {
            if (EditorGUILayout.Foldout(false, "Clear", true, GUI.skin.button))
            {
                info.SetValue(monoBehaviour, (object)(source = new Hashtable()));
            }
        }
        EditorGUILayout.EndHorizontal();
        

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
            scrollPositions[info] = EditorGUILayout.BeginScrollView(scrollPositions[info], true, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            if (flags.HasFlag(ExSerializeFieldFlag.RemoveElement)) EditorGUILayout.Space(30);
            EditorGUILayout.LabelField("Key", style);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Value", style);
            EditorGUILayout.EndHorizontal();
            for (int index = 0; index < keys.Count; ++index)
            {
                EditorGUILayout.BeginHorizontal();
                if (flags.HasFlag(ExSerializeFieldFlag.RemoveElement))
                {
                    EditorGUILayout.Space(10);
                    if (EditorGUILayout.Foldout(false, "Remove", true, GUI.skin.label))
                    {
                        keys.RemoveAt(index);
                        values.RemoveAt(index);
                        if (index > 0) --index;
                    }
                    EditorGUILayout.Space(10);
                }
                if (keys.Count > 0) keys[index] = DrawDataField(keys[index]);
                EditorGUILayout.Space(10);
                if (values.Count > 0) values[index] = DrawDataField(values[index]);
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