using UnityEngine.Events;
using System.Reflection;
using System;

public static class UnityEventClipboard
{
    private static object copiedPersistentCalls;
    private static Type unityEventBaseType = typeof(UnityEventBase);

    public static void Copy(UnityEventBase unityEvent)
    {
        copiedPersistentCalls = ClonePersistentCalls(GetPersistentCalls(unityEvent));
    }

    public static void Paste(UnityEventBase targetEvent)
    {
        if (targetEvent == null || copiedPersistentCalls == null) return;
        SetPersistentCalls(targetEvent, ClonePersistentCalls(copiedPersistentCalls));
    }

    public static void ClearPersistentCalls(UnityEventBase unityEvent)
    {
        SetPersistentCalls(unityEvent, Activator.CreateInstance(GetPersistentCalls(unityEvent).GetType()));
    }

    public static object GetPersistentCalls(UnityEventBase unityEvent)
    {
        return unityEventBaseType.GetField("m_PersistentCalls", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(unityEvent);
    }

    public static void SetPersistentCalls(UnityEventBase unityEvent, object newCalls)
    {
        unityEventBaseType.GetField("m_PersistentCalls", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(unityEvent, newCalls);
    }

    public static object ClonePersistentCalls(object originalPersistentCalls)
    {
        if (originalPersistentCalls == null) return null;

        Type persistentCallsType = originalPersistentCalls.GetType();
        object newPersistentCalls = Activator.CreateInstance(persistentCallsType);

        FieldInfo callsField = persistentCallsType.GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
        if (callsField != null)
        {
            object originalCalls = callsField.GetValue(originalPersistentCalls);
            object newCalls = DeepCloneList(originalCalls);
            callsField.SetValue(newPersistentCalls, newCalls);
        }

        return newPersistentCalls;
    }

    private static object DeepCloneList(object originalList)
    {
        if (originalList == null) return null;

        Type listType = originalList.GetType();
        object newList = Activator.CreateInstance(listType);

        MethodInfo addMethod = listType.GetMethod("Add");
        if (addMethod != null)
        {
            foreach (var item in (System.Collections.IEnumerable)originalList)
            {
                object clonedItem = ClonePersistentCall(item);
                addMethod.Invoke(newList, new object[] { clonedItem });
            }
        }

        return newList;
    }

    private static object ClonePersistentCall(object originalCall)
    {
        if (originalCall == null) return null;

        Type persistentCallType = originalCall.GetType();
        object newCall = Activator.CreateInstance(persistentCallType);

        foreach (FieldInfo field in persistentCallType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            field.SetValue(newCall, field.GetValue(originalCall));
        }

        return newCall;
    }
}
