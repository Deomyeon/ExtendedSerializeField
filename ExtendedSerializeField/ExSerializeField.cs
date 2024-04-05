using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
[InitializeOnLoad]
public class ExSerializeField : Attribute
{
    #if UNITY_EDITOR
    public static HashSet<MonoBehaviour> targets = new HashSet<MonoBehaviour>();
    public static Dictionary<MonoBehaviour, (FieldInfo[], bool[])> targetFields = new Dictionary<MonoBehaviour, (FieldInfo[], bool[])>();

    static ExSerializeField()
    {
        EditorApplication.update += Update;
    }

    ExSerializeFieldFlag flags;
    public ExSerializeField()
    {
        this.flags = ExSerializeFieldFlag.Default;
    }

    public ExSerializeField(ExSerializeFieldFlag flags)
    {
        this.flags = flags;
    }

    static List<bool> fieldCheckList = new List<bool>();

    static void Update()
    {
        if (Selection.activeGameObject == null)
        {
            targets.Clear();
            targetFields.Clear();
            return;
        }
        MonoBehaviour[] monoBehaviours = Selection.activeGameObject.GetComponents<MonoBehaviour>();
        
        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            FieldInfo[] objectFields = monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            fieldCheckList.Clear();
            for (int index = 0; index < objectFields.Length; ++index)
            {
                ExSerializeField attribute = Attribute.GetCustomAttribute(objectFields[index], typeof(ExSerializeField)) as ExSerializeField;
                
                if (attribute != null)
                {
                    targets.Add(monoBehaviour);
                    fieldCheckList.Add(true);
                }
                else
                {
                    fieldCheckList.Add(false);
                }
            }
            targetFields[monoBehaviour] = (objectFields, fieldCheckList.ToArray());
        }
    }
    #endif
}

public enum ExSerializeFieldFlag
{
    Default = 0,
    Clear = 1,
    RemoveElement = 2,
    AddElement = 3,

}