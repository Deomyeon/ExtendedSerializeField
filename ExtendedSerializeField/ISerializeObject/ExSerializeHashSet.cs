#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeHashSet : IExSerializeObject
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

        if (foldOuts[info] && source != null)
        {
            IEnumerator list = source.GetType().GetMethod("GetEnumerator").Invoke(source, null) as IEnumerator;

            object[] clone = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];
            bool[] removeList = new bool[clone.Length];

            MethodInfo remove = source.GetType().GetMethod("Remove");
            MethodInfo add = source.GetType().GetMethod("Add");

            for (int index = 0; list.MoveNext(); ++index)
            {
                clone[index] = list.Current;
            }

            object[] result = new object[clone.Length];

            EditorGUILayout.BeginVertical(GUI.skin.box);
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
                result[index] = DrawDataField(clone[index]);
            }
            EditorGUILayout.EndVertical();
            for (int index = 0; index < result.Length; ++index)
            {
                remove.Invoke(source, new object[]{ clone[index] });
            }
            for (int index = 0; index < result.Length; ++index)
            {
                if (removeList[index]) continue;
                add.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif