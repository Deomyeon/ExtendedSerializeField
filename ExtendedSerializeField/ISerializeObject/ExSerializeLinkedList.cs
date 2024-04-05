#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class ExSerializeLinkedList : IExSerializeObject
{
    public new static void DrawOnInspector(FieldInfo info, Object monoBehaviour, ExSerializeFieldFlag flags)
    {
        object source = info.GetValue(monoBehaviour);

        if (!foldOuts.ContainsKey(info))
        {
            foldOuts[info] = false;
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
                IEnumerator iterator = source.GetType().GetMethod("GetEnumerator").Invoke(source, null) as IEnumerator;
                int count = (int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null);
                
                if (count > 0)
                {
                    for (int counter = 0; counter < count; ++counter)
                    {
                        iterator.MoveNext();
                    }

                    source.GetType().GetMethod("AddLast", source.GetType().GenericTypeArguments).Invoke(source, new object[] { iterator.Current });
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        if (foldOuts[info] && source != null)
        {
            IEnumerator list = source.GetType().GetMethod("GetEnumerator").Invoke(source, null) as IEnumerator;

            object[] result = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];
            bool[] removeList = new bool[result.Length];

            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int index = 0; list.MoveNext(); ++index)
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
                result[index] = DrawDataField(list.Current);
            }
            EditorGUILayout.EndVertical();

            source.GetType().GetMethod("Clear").Invoke(source, null);

            MethodInfo method = source.GetType().GetMethod("AddLast", source.GetType().GenericTypeArguments);
            
            for (int index = 0; index < result.Length; ++index)
            {
                if (removeList[index]) continue;
                method.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif