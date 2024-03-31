#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeQueue : IExSerializeObject
{
    static Dictionary<FieldInfo, bool> foldOuts = new Dictionary<FieldInfo, bool>();

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

            for (int index = 0; index < list.Length; ++index)
            {
                list[index] = DrawDataField(list[index]);
            }

            info.SetValue(monoBehaviour, (object)(new Queue(list)));
            
        }
    }
}

#endif