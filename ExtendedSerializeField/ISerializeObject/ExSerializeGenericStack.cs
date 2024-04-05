#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class ExSerializeGenericStack : IExSerializeObject
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

            object[] result = new object[(int)source.GetType().GetProperty("Count").GetMethod.Invoke(source, null)];

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
            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int index = 0; index < result.Length; ++index)
            {
                result[index] = DrawDataField(result[index]);
            }
            EditorGUILayout.EndVertical();
            for (int index = 0; index < result.Length; ++index)
            {
                pop.Invoke(source, null);
            }
            for (int index = 0; index < result.Length; ++index)
            {
                push.Invoke(source, new object[]{ result[index] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif