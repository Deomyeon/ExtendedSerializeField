#if UNITY_EDITOR

using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class IExSerializeObject
{
    
    protected static Dictionary<FieldInfo, bool> foldOuts = new Dictionary<FieldInfo, bool>();
    protected static Dictionary<object, Vector2> scrollPositions = new Dictionary<object, Vector2>();

    public static void DrawOnInspector(FieldInfo info, Object monoBehaviour, ExSerializeFieldFlag flags)
    {
        
    }
    public delegate void ExSerializeMethod(FieldInfo info, Object monoBehaviour, ExSerializeFieldFlag flags);

    public static object DrawDataField(object data)
    {
        if (data == null)
        {
            EditorGUILayout.LabelField("None", GUI.skin.textField);
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
        else if (data.GetType().IsEnum)
        {
            return EditorGUILayout.EnumFlagsField(data.GetType().Name, (System.Enum)data);
        }
        else if (data.GetType().IsSerializable)
        {
            if (!scrollPositions.ContainsKey(data))
            {
                scrollPositions[data] = Vector2.zero;
            }
            scrollPositions[data] = EditorGUILayout.BeginScrollView(scrollPositions[data], true, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            foreach (var field in data.GetType().GetFields())
            {
                field.SetValue(data, DrawDataField(field.GetValue(data)));
                EditorGUILayout.Space(10);
            }
            foreach (var property in data.GetType().GetProperties())
            {
                object temp = DrawDataField(property.GetMethod.Invoke(data, null));
                property.SetMethod?.Invoke(data, new object[]{ temp });
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            return data;
        }
        else
        {
            if (data as Object != null)
            {
                return EditorGUILayout.ObjectField((Object)data, data.GetType(), true);
            }
            else
            {
                EditorGUILayout.LabelField("Invalid Type", GUI.skin.textField);
                return data;
            }
        }
    }
}

#endif
