#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeGenericQueue : IExSerializeObject
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
                object[] temp = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];
                
                if (temp.Length > 0)
                {
                    MethodInfo dequeue = source.GetType().GetMethod("Dequeue");
                    MethodInfo enqueue = source.GetType().GetMethod("Enqueue");
                    for (int index = 0; index < temp.Length; ++index)
                    {
                        temp[index] = dequeue.Invoke(source, null);
                    }
                    for (int index = 0; index < temp.Length; ++index)
                    {
                        enqueue.Invoke(source, new object[]{ temp[index] });
                    }
                    enqueue.Invoke(source, new object[]{ temp[temp.Length - 1] });
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        if (foldOuts[info] && source != null)
        {

            object[] result = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];
            bool[] removeList = new bool[result.Length];

            MethodInfo dequeue = source.GetType().GetMethod("Dequeue");
            MethodInfo enqueue = source.GetType().GetMethod("Enqueue");
            
            for (int index = 0; index < result.Length; ++index)
            {
                result[index] = dequeue.Invoke(source, null);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                enqueue.Invoke(source, new object[]{ result[index] });
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
                dequeue.Invoke(source, null);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                if (removeList[index]) continue;
                enqueue.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif