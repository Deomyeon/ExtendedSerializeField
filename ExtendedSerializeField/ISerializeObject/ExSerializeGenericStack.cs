#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class ExSerializeGenericStack : IExSerializeObject
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
                source.GetType().GetMethod("Clear").Invoke(source, null);
                info.SetValue(monoBehaviour, (object)(source));
            }
        }
        EditorGUILayout.EndHorizontal();
        
        
        if (flags.HasFlag(ExSerializeFieldFlag.CloneElement))
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(55);
            if (EditorGUILayout.Foldout(false, "Clone", true, GUI.skin.button))
            {
                if ((int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null) > 0)
                {
                    MethodInfo pop = source.GetType().GetMethod("Pop");
                    MethodInfo push = source.GetType().GetMethod("Push");
                    object temp = pop.Invoke(source, null);
                    push.Invoke(source, new object[] { temp });
                    push.Invoke(source, new object[] { temp });
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        if (foldOuts[info] && source != null)
        {

            object[] result = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];
            bool[] removeList = new bool[result.Length];

            MethodInfo pop = source.GetType().GetMethod("Pop");
            MethodInfo push = source.GetType().GetMethod("Push");
            
            for (int index = 0; index < result.Length; ++index)
            {
                result[result.Length - index - 1] = pop.Invoke(source, null);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                push.Invoke(source, new object[]{ result[index] });
            }
            scrollPositions[info] = EditorGUILayout.BeginScrollView(scrollPositions[info], true, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.box);
            for (int index = 0; index < result.Length; ++index)
            {
                if (flags.HasFlag(ExSerializeFieldFlag.RemoveElement))
                {
                    EditorGUILayout.Space(10);
                    if (EditorGUILayout.Foldout(false, "Remove", true, GUI.skin.label))
                    {
                        removeList[index] = true;
                    }
                    EditorGUILayout.Space(10);
                }
                result[index] = DrawDataField(result[index]);
            }
            EditorGUILayout.EndScrollView();
            for (int index = 0; index < result.Length; ++index)
            {
                pop.Invoke(source, null);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                if (removeList[index]) continue;
                push.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif