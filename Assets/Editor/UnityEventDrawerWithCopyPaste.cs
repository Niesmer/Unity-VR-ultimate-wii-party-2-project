// using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using UnityEditor;

[CustomPropertyDrawer(typeof(UnityEventBase), true)]
public class UnityEventDrawerWithCopyPaste : PropertyDrawer
{
    private UnityEditorInternal.UnityEventDrawer defaultDrawer = new();
    private readonly MethodInfo onGUI_Method;
    private readonly MethodInfo getPropertyHeight_Method;

    public UnityEventDrawerWithCopyPaste()
    {
        // Get the internal OnGUI and GetPropertyHeight methods from UnityEventDrawer
        onGUI_Method = typeof(UnityEditorInternal.UnityEventDrawer).GetMethod(
            "OnGUI",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            new[] { typeof(Rect), typeof(SerializedProperty), typeof(GUIContent) },
            null
        );

        if (onGUI_Method == null)
        {
            Debug.LogError("Failed to get OnGUI method from UnityEventDrawer. Method might have been changed or removed in this Unity version.");
        }

        getPropertyHeight_Method = typeof(UnityEditorInternal.UnityEventDrawer).GetMethod(
            "GetPropertyHeight",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            new[] { typeof(SerializedProperty), typeof(GUIContent) },
            null
        );

        if (getPropertyHeight_Method == null)
        {
            Debug.LogError("Failed to get GetPropertyHeight method from UnityEventDrawer. Method might have been changed or removed in this Unity version.");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (getPropertyHeight_Method != null)
        {
            // Ask Unity how much height the UnityEvent should take
            return (float)getPropertyHeight_Method.Invoke(defaultDrawer, new object[] { property, label });
        }
        Debug.LogError("GetPropertyHeight method is null. Using base implementation.");
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (onGUI_Method == null)
        {
            Debug.LogError("Failed to get OnGUI method from UnityEventDrawer.");
            return;
        }

        // Compute button sizes
        float buttonWidth = 50f;
        float spacing = 5f;
        float totalButtonWidth = (buttonWidth * 2) + spacing;

        // Get the correct height for the UnityEvent field
        float eventHeight = GetPropertyHeight(property, label);

        // Shrink UnityEvent field to fit buttons
        Rect eventRect = new(position.x, position.y, position.width - totalButtonWidth, eventHeight);
        Rect copyRect = new(position.x + position.width - totalButtonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);
        Rect pasteRect = new(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

        // Draw the default UnityEvent UI
        try
        {
            onGUI_Method.Invoke(defaultDrawer, new object[] { eventRect, property, label });
        }
        catch (TargetInvocationException ex)
        {
            Debug.LogError($"Error invoking OnGUI method: {ex.InnerException?.Message}");
            return;
        }

        // Get the actual UnityEvent reference
        UnityEventBase unityEvent = GetUnityEvent(property);
        if (unityEvent == null) return;

        // Draw Copy & Paste buttons
        if (GUI.Button(copyRect, "Copy"))
        {
            UnityEventClipboard.Copy(unityEvent);
        }

        if (GUI.Button(pasteRect, "Paste"))
        {
            UnityEventClipboard.Paste(unityEvent);
        }
    }

    private UnityEventBase GetUnityEvent(SerializedProperty property)
    {
        return fieldInfo?.GetValue(property.serializedObject.targetObject) as UnityEventBase;
    }
}
