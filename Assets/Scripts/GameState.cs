
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

public class GameState
{
    private static bool _isBurst;
    public static bool isBurst { 
        get => _isBurst;
        set 
        {
            _isBurst = value;
            Notify(nameof(isBurst));
        } 
    }

    #region Change Notifier

    private static Dictionary<string, List<Action<string>>> listeners = new();

    public static void AddListener(string fieldName, Action<string> listener)
    {
        if (listeners.ContainsKey(fieldName))
        {
            listeners[fieldName].Add(listener);
        }
        else
        {
            listeners[fieldName] = new() { listener };

        }
    }

    public static void RemoveListener(string fieldName, Action<string> listener)
    {
        if (listeners.ContainsKey(fieldName))
        {
            listeners[fieldName].Remove(listener);
        }
   
    }

    private static void Notify(string fieldName)
    {
        if (listeners.ContainsKey(fieldName))
        { 
            foreach (var listener in listeners[fieldName])
            {
                listener(fieldName);
            }
        }
    }
    #endregion

}
