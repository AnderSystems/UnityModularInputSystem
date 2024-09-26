using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DebugVar : MonoBehaviour
{
    static DebugVar instance;

    public static DebugVar main
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<DebugVar>();
            }

            if (instance == null)
            {
                instance = new GameObject("DebugVar").AddComponent<DebugVar>();
            }
            return instance;
        }
    }

    [System.Serializable]
    public class debugObject
    {
        public Object obj;
        [System.Serializable]
        public class debugVar
        {
            public string varName;
            public object value;
        }
        [SerializeField]
        public List<debugVar> vars = new List<debugVar>();

        public debugVar GetVarByName(string varName)
        {
            foreach (var v in vars)
            {
                if (v.varName == varName)
                {
                    return v;
                }
            }
            return null;
        }

        public void DebugVar(string varName, object value)
        {
            if (vars == null)
            {
                vars = new List<debugVar>();
            }
            else
            {
                debugVar v = GetVarByName(varName);
                if (v == null)
                {
                    v = new debugVar() { varName = varName, value = value };
                    vars.Add(v);
                }
                else
                {
                    v.value = value;
                }
            }
        }
    }
    [SerializeField]
    public List<debugObject> objects = new List<debugObject>();

    [System.Serializable]
    public class alert
    {
        public string title; public object value; public Object target;
    }
    [SerializeField]
    public List<alert> alerts = new List<alert>();

    public debugObject GetDebugObject(Object target)
    {
        foreach (var o in objects)
        {
            if (o.obj == target)
            {
                return o;
            }
        }
        return null;
    }
    public static void Log(string varName, object value, Object target)
    {
        debugObject o = main.GetDebugObject(target);
        if (o == null)
        {
            o = new debugObject();
            o.obj = target;
            o.DebugVar(varName, value);
            main.objects.Add(o);
        }
        else
        {
            o.DebugVar(varName, value);
        }
    }
    public static void Alert(string title, object value, Object target, float time = 3f)
    {
        Alert(title, value, target, time, alertType.Info);
    }
    public enum alertType
    {
        Info, Warning, Error
    }
    public static async void Alert(string title, object value, Object target, float time = 3, alertType AlertType = alertType.Info)
    {
        string prefix = "";
        string suffix = "</color>";
        switch (AlertType)
        {
            case alertType.Info:
                Debug.Log($"<b>{title}</b>\n{value}", target);
                prefix = "<color=white>";
                break;
            case alertType.Warning:
                Debug.LogWarning($"<b>{title}</b>\n{value}", target);
                prefix = "<color=orange> (!)";
                break;
            case alertType.Error:
                Debug.LogError($"<b>{title}</b>\n{value}", target);
                prefix = "<color=red> [!]";
                break;
            default:
                break;
        }

        //Remove older alertes if alert count is greater than 5
        if (main.alerts.Count > 5)
        {
            main.alerts.RemoveRange(0, main.alerts.Count - 5);
        }

        //Remove other alert with same title and target
        foreach (var a in main.alerts)
        {
            if (a.title == title && a.target == target)
            {
                main.alerts.Remove(a);
            }
        }

        alert alert = new alert() { title = prefix + title + suffix, value = prefix + value + suffix, target = target };
        main.alerts.Add(alert);
        //Remove alert
        await Task.Delay((int)(time * 1000));
        main.alerts.Remove(alert);
    }

    private void OnGUI()
    {
        string debugText = "";

        //Generate debug text
        foreach (var o in objects)
        {
            debugText += $"<b>[{o.obj}]</b>\n";

            foreach (var v in o.vars)
            {
                debugText += $"{v.varName}: {v.value}\n";
            }
            debugText += "\n\n";
        }

        //Calculate text size
        Vector2 debugTextSize = GUI.skin.label.CalcSize(new GUIContent(debugText));

        //Draw Shadow
        GUI.color = new Color(0, 0, 0, 1);
        GUI.Label(new Rect(11, 11, debugTextSize.x, debugTextSize.y), debugText);

        //Draw text
        GUI.color = new Color(1, 1, 1, 1f);
        GUI.Label(new Rect(10, 10, debugTextSize.x, debugTextSize.y), debugText);


        //Generate Alert
        string alertText = "";
        foreach (var a in alerts)
        {
            alertText += $"<b>{a.title}</b>\n{a.value}\n<size=8>{a.target}</size>\n\n";
        }

        //Calculate text size
        Vector2 alertTextSize = GUI.skin.label.CalcSize(new GUIContent(alertText));

        GUIStyle alertStyle = new GUIStyle();
        alertStyle.alignment = TextAnchor.UpperRight;
        alertStyle.normal.textColor = new Color(1, 1, 1, 1f);

        //Remove custom colors from text
        string shadowText = alertText.Replace("<color=white>", "").Replace("</color>", "").Replace("<color=orange>", "").Replace("</color>", "").Replace("<color=red>", "").Replace("</color>", "");

        //Draw Shadow
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.Label(new Rect(Screen.width - 10 - alertTextSize.x, 11, alertTextSize.x, alertTextSize.y), shadowText, alertStyle);

        //Draw text
        GUI.color = new Color(1, 1, 1, 1f);
        GUI.Label(new Rect(Screen.width - 10 - alertTextSize.x, 10, alertTextSize.x, alertTextSize.y), alertText, alertStyle);
    }

    private void Awake()
    {
        objects.Clear();
        alerts.Clear();
    }
}
