#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeLinkedList : IExSerializeObject
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
            IEnumerator list = source.GetType().GetMethod("GetEnumerator").Invoke(source, null) as IEnumerator;

            object[] result = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];

            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int index = 0; list.MoveNext(); ++index)
            {
                result[index] = DrawDataField(list.Current);
            }
            EditorGUILayout.EndVertical();

            source.GetType().GetMethod("Clear").Invoke(source, null);

            MethodInfo method = source.GetType().GetMethod("AddLast", source.GetType().GenericTypeArguments);
            
            for (int index = 0; index < result.Length; ++index)
            {
                method.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif