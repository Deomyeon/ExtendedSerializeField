#if UNITY_EDITOR

using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class ExSerializeDictionary : IExSerializeObject
{
    static Dictionary<FieldInfo, bool> foldOuts = new Dictionary<FieldInfo, bool>();
    static Dictionary<FieldInfo, Vector2> scrollPositions = new Dictionary<FieldInfo, Vector2>();

    public new static void DrawOnInspector(FieldInfo info, Object monoBehaviour)
    {
        object source = info.GetValue(monoBehaviour);

        if (!foldOuts.ContainsKey(info))
        {
            foldOuts[info] = false;
            scrollPositions[info] = Vector2.zero;
        }

        foldOuts[info] = EditorGUILayout.Foldout(foldOuts[info], info.Name, true);

        if (foldOuts[info] && source != null)
        {
            List<object> keys = new List<object>();
            List<object> values = new List<object>();

            MethodInfo tryAdd = source.GetType().GetMethod("TryAdd");
            MethodInfo remove = source.GetType().GetMethod("Remove", new System.Type[] { source.GetType().GenericTypeArguments[0] });

            for (IEnumerator iterator = source.GetType().GetMethod("GetEnumerator").Invoke(source, null) as IEnumerator; iterator.MoveNext();)
            {
                keys.Add(iterator.Current.GetType().GetProperty("Key").GetMethod.Invoke(iterator.Current, null));
                values.Add(iterator.Current.GetType().GetProperty("Value").GetMethod.Invoke(iterator.Current, null));
            }
            
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.green;
            scrollPositions[info] = EditorGUILayout.BeginScrollView(scrollPositions[info], true, false);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Key", style);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Value", style);
            EditorGUILayout.EndHorizontal();
            for (int index = 0; index < keys.Count; ++index)
            {
                remove.Invoke(source, new object[]{ keys[index] });
                
                EditorGUILayout.BeginHorizontal();
                keys[index] = DrawDataField(keys[index]);
                EditorGUILayout.Space(10);
                values[index] = DrawDataField(values[index]);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space(10);
            EditorGUILayout.EndScrollView();

            for (int index = 0; index < keys.Count; ++index)
            {
                int rIndex = keys.Count - index - 1;
                tryAdd.Invoke(source, new object[]{ keys[rIndex], values[rIndex] });
            }

            info.SetValue(monoBehaviour, (object)(source));

        }
        
    }
}

#endif