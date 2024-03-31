#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using static IExSerializeObject;

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class ExSerializeEditor : Editor
{
    static Dictionary<Type, ExSerializeMethod> extendedTypes = new Dictionary<Type, ExSerializeMethod>{ 
        {typeof(Stack<>), ExSerializeGenericStack.DrawOnInspector},
        {typeof(Queue<>), ExSerializeGenericQueue.DrawOnInspector},
        {typeof(Stack), ExSerializeStack.DrawOnInspector},
        {typeof(Queue), ExSerializeQueue.DrawOnInspector},
        {typeof(HashSet<>), ExSerializeHashSet.DrawOnInspector},
        {typeof(Hashtable), ExSerializeHashtable.DrawOnInspector},
        {typeof(Dictionary<,>), ExSerializeDictionary.DrawOnInspector},
        {typeof(ArrayList), ExSerializeArrayList.DrawOnInspector},
        {typeof(LinkedList<>), ExSerializeLinkedList.DrawOnInspector}
        };

    public override void OnInspectorGUI()
    {
        if (ExSerializeField.targets.Contains(serializedObject.targetObject as MonoBehaviour))
        {
            serializedObject.Update();

            SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");
            using (new EditorGUI.DisabledScope(scriptProperty != null))
            {
                EditorGUILayout.PropertyField(scriptProperty, true);
            }
        
            if (ExSerializeField.targetFields.ContainsKey(serializedObject.targetObject as MonoBehaviour))
            {
                (FieldInfo[], bool[]) fields = ExSerializeField.targetFields[serializedObject.targetObject as MonoBehaviour];

                for (int index = 0; index < fields.Item1.Length; ++index)
                {
                    if (fields.Item2[index])
                    {
                        FieldInfo field = fields.Item1[index];
                        Type type = null;

                        if (field.FieldType.IsGenericType)
                        {
                            type = field.FieldType.GetGenericTypeDefinition();
                        }
                        else
                        {
                            type = field.FieldType;
                        }

                        if (extendedTypes.ContainsKey(type))
                        {
                            if (serializedObject.targetObject)
                            EditorGUILayout.Space(5);
                            extendedTypes[type](field, serializedObject.targetObject);
                            EditorGUILayout.Space(5);
                        }
                    }
                    else
                    {
                        SerializedProperty property = serializedObject.FindProperty(fields.Item1[index].Name);
                        if (property != null)
                        {
                            EditorGUILayout.PropertyField(property);
                        }
                    }
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            base.OnInspectorGUI();
        }
    }
}

#endif