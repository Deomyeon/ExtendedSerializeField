#if UNITY_EDITOR

using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class IExSerializeObject
{
    public static void DrawOnInspector(FieldInfo info, Object monoBehaviour)
    {
        
    }
    public delegate void ExSerializeMethod(FieldInfo info, Object monoBehaviour);

    public static object DrawDataField(object data)
    {
        if (data == null)
        {
            EditorGUILayout.LabelField("Invalid Data");
            return data;
        }
        if (data.GetType() == typeof(int))
        {
            return EditorGUILayout.IntField("Int", (int)data);
        }
        else if (data.GetType() == typeof(long))
        {
            return EditorGUILayout.LongField("Long", (long)data);
        }
        else if (data.GetType() == typeof(string))
        {
            return EditorGUILayout.TextField("String", (string)data);
        }
        else if (data.GetType() == typeof(float))
        {
            return EditorGUILayout.FloatField("Float", (float)data);
        }
        else if (data.GetType() == typeof(double))
        {
            return EditorGUILayout.DoubleField("Double", (double)data);
        }
        else if (data.GetType() == typeof(Vector2))
        {
            return EditorGUILayout.Vector2Field("", (Vector2)data);
        }
        else if (data.GetType() == typeof(Vector3))
        {
            return EditorGUILayout.Vector3Field("", (Vector3)data);
        }
        else if (data.GetType() == typeof(Vector4))
        {
            return EditorGUILayout.Vector4Field("", (Vector4)data);
        }
        else if (data.GetType() == typeof(Color))
        {
            return EditorGUILayout.ColorField((Color)data);
        }
        else if (data.GetType() == typeof(Rect))
        {
            return EditorGUILayout.RectField((Rect)data);
        }
        else if (data.GetType() == typeof(RectInt))
        {
            return EditorGUILayout.RectIntField((RectInt)data);
        }
        else if (data.GetType().BaseType == typeof(Object))
        {
            return EditorGUILayout.ObjectField((Object)data, data.GetType(), true);
        }
        else
        {
            EditorGUILayout.LabelField("Invalid Data");
            return data;
        }
    }
}

#endif
