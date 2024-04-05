#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeQueue : IExSerializeObject
{
    public new static void DrawOnInspector(FieldInfo info, Object monoBehaviour)
    {
        object source = info.GetValue(monoBehaviour);

        if (!foldOuts.ContainsKey(info))
        {
            foldOuts[info] = false;
        }

        foldOuts[info] = EditorGUILayout.Foldout(foldOuts[info], info.Name, true);

        if (foldOuts[info] && source != null)
        {
            object[] list = (source as Queue).ToArray();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int index = 0; index < list.Length; ++index)
            {
                list[index] = DrawDataField(list[index]);
            }
            EditorGUILayout.EndVertical();

            info.SetValue(monoBehaviour, (object)(new Queue(list)));
            
        }
    }
}

#endif