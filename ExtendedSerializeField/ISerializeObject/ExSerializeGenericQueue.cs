#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeGenericQueue : IExSerializeObject
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

            object[] result = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];

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
            for (int index = 0; index < result.Length; ++index)
            {
                result[index] = DrawDataField(result[index]);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                dequeue.Invoke(source, null);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                enqueue.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif