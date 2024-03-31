#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeStack : IExSerializeObject
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
            Stack data = new Stack(source as Stack);
            object[] list = new object[data.Count];

            for (int index = 0; index < list.Length; ++index)
            {
                list[index] = DrawDataField(data.Pop());
            }

            info.SetValue(monoBehaviour, (object)(new Stack(list)));
        
        }
    }
}

#endif