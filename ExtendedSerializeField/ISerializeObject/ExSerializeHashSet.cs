#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeHashSet : IExSerializeObject
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
            IEnumerator list = source.GetType().GetMethod("GetEnumerator").Invoke(source, null) as IEnumerator;

            object[] clone = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];
            
            MethodInfo remove = source.GetType().GetMethod("Remove");
            MethodInfo add = source.GetType().GetMethod("Add");

            for (int index = 0; list.MoveNext(); ++index)
            {
                clone[index] = list.Current;
            }

            object[] result = new object[clone.Length];

            for (int index = 0; index < result.Length; ++index)
            {
                result[index] = DrawDataField(clone[index]);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                remove.Invoke(source, new object[]{ clone[index] });
            }
            for (int index = 0; index < result.Length; ++index)
            {
                add.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif