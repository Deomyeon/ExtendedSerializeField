#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeStack : IExSerializeObject
{
    
    public new static void DrawOnInspector(FieldInfo info, Object monoBehaviour, ExSerializeFieldFlag flags)
    {
        object source = info.GetValue(monoBehaviour);

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
                info.SetValue(monoBehaviour, (object)(source = new Stack()));
            }
        }
        EditorGUILayout.EndHorizontal();
        
        
        if (flags.HasFlag(ExSerializeFieldFlag.CloneElement))
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(55);
            if (EditorGUILayout.Foldout(false, "Clone", true, GUI.skin.button))
            {
                var list = source as Stack;
                if (list.Count > 0)
                {
                    list.Push(list.Peek());
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        if (foldOuts[info] && source != null)
        {
            Stack data = new Stack(source as Stack);
            List<object> list = new List<object>(data.ToArray());

            scrollPositions[info] = EditorGUILayout.BeginScrollView(scrollPositions[info], true, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.box);
            for (int index = 0; index < list.Count; ++index)
            {
                EditorGUILayout.BeginHorizontal();
                if (flags.HasFlag(ExSerializeFieldFlag.RemoveElement))
                {
                    EditorGUILayout.Space(10);
                    if (EditorGUILayout.Foldout(false, "Remove", true, GUI.skin.label))
                    {
                        list.RemoveAt(index);
                        if (index > 0) --index;
                    }
                    EditorGUILayout.Space(10);
                }
                if (list.Count > 0) list[index] = DrawDataField(list[index]);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            info.SetValue(monoBehaviour, (object)(new Stack(list)));
        
        }
    }
}

#endif