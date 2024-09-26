using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(InputSystem))]
public class InputSystemEditor : Editor
{
    [MenuItem("GameObject/New Input System")]
    public static void CreateNewInputSystemObject()
    {
        InputSystem inp = InputSystem.CreateNewInputSystemObject();
    }
}
#endif

public class InputSystem : MonoBehaviour
{
    public static InputSettings selectedSettings;

    //Set Singleton
    static InputSystem instance;
    public static InputSystem main
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputSystem>();
            }

            if (instance == null)
            {
                instance = CreateNewInputSystemObject();
#if UNITY_EDITOR
                instance.settings = selectedSettings;
#endif
            }
            return instance;
        }
    }


    /// <summary>
    /// A Input Key data
    /// </summary>
    [System.Serializable]
    public class key
    {
        [Tooltip("Registered key")]
        public KeyCode keyCode;
    }

    /// <summary>
    /// A Input Button data
    /// </summary>
    [System.Serializable]
    public class button
    {
        /// <summary>
        /// The internal name of the button
        /// </summary>
        [HideInInspector]
        internal string m_name;
        /// <summary>
        /// The public name of the button
        /// </summary>
        public string Name;

        /// <summary>
        /// The registered keys
        /// </summary>
        public key[] keys;

        /// <summary>
        /// This button is current pressed?
        /// </summary>
        public bool isPressed { get; set; }
        /// <summary>
        /// This button is down?
        /// </summary>
        public bool isDown { get; set; }
        /// <summary>
        /// This button is up?
        /// </summary>
        public bool isUp { get; set; }
    }

    /// <summary>
    /// A Input Axis Data
    /// </summary>
    [System.Serializable]
    public class axis
    {
        /// <summary>
        /// The internal name of the button
        /// </summary>
        [HideInInspector]
        internal string m_name;
        /// <summary>
        /// The public name of the button
        /// </summary>
        public string Name;

        /// <summary>
        /// The type of axis
        /// </summary>
        [System.Serializable]
        public enum axisType
        {
            Mouse,
            ScrollWheel,
            CurrentJoystick,
            OnlyJoystick1,
            OnlyJoystick2,
            OnlyJoystick3,
            OnlyJoystick4,
        }
        [SerializeField]
        public axisType type;

        [Header("Keys")]
        public key[] Keys_Up;
        public key[] Keys_Down;
        [Space]
        public key[] Keys_Left;
        public key[] Keys_Right;

        public Vector2 value { get; set; }
        public Vector2 delta { get; set; }
    }

    public static InputSystem CreateNewInputSystemObject()
    {
        InputSystem inputSystem = new GameObject("InputSystem").AddComponent<InputSystem>();
        return inputSystem;
    }


    /// <summary>
    /// The current input settings
    /// </summary>
    public InputSettings settings;
    /// <summary>
    /// The current input list
    /// </summary>
    [SerializeField]
    public InputSettings.Inputs inputs;

    public void ControllAxis(axis axis)
    {
        //Apply Joystick or Mouse Axis
        Joystick joystick = Joystick.current;
        axis.delta = Vector2.zero;
        switch (axis.type)
        {
            case axis.axisType.Mouse:
                axis.value = new Vector2(Input.mousePositionDelta.x, Input.mousePositionDelta.y);
                axis.delta = axis.value;
                break;
            case axis.axisType.ScrollWheel:
                axis.value = new Vector2(Input.mouseScrollDelta.x, Input.mouseScrollDelta.y);
                axis.delta = axis.value;
                break;
            case axis.axisType.CurrentJoystick:
                axis.delta = Vector2.zero;
                axis.value = Vector2.zero;

                if (joystick != null)
                {
                    axis.value = joystick.stick.ReadValue();
                }
                break;
            case axis.axisType.OnlyJoystick1:
                axis.delta = Vector2.zero;
                axis.value = Vector2.zero;

                joystick = Joystick.all[0];
                if (joystick != null)
                {
                    axis.value = joystick.stick.ReadValue();
                }
                break;
            case axis.axisType.OnlyJoystick2:
                axis.delta = Vector2.zero;
                axis.value = Vector2.zero;

                joystick = Joystick.all[1];
                if (joystick != null)
                {
                    axis.value = joystick.stick.ReadValue();
                }
                break;
            case axis.axisType.OnlyJoystick3:
                axis.delta = Vector2.zero;
                axis.value = Vector2.zero;

                joystick = Joystick.all[2];
                if (joystick != null)
                {
                    axis.value = joystick.stick.ReadValue();
                }
                break;
            case axis.axisType.OnlyJoystick4:
                axis.delta = Vector2.zero;
                axis.value = Vector2.zero;

                joystick = Joystick.all[3];
                if (joystick != null)
                {
                    axis.value = joystick.stick.ReadValue();
                }
                break;
        }

        //Controll axis by key
        Vector2 KeyDelta = new Vector2(0, 0);
        for (int i = 0; i < axis.Keys_Up.Length; i++)
        {
            if (Input.GetKey(axis.Keys_Up[i].keyCode))
            {
                KeyDelta.y += 1;
            }
        }
        for (int i = 0; i < axis.Keys_Down.Length; i++)
        {
            if (Input.GetKey(axis.Keys_Down[i].keyCode))
            {
                KeyDelta.y -= 1;
            }
        }
        for (int i = 0; i < axis.Keys_Left.Length; i++)
        {
            if (Input.GetKey(axis.Keys_Left[i].keyCode))
            {
                KeyDelta.x -= 1;
            }
        }
        for (int i = 0; i < axis.Keys_Right.Length; i++)
        {
            if (Input.GetKey(axis.Keys_Right[i].keyCode))
            {
                KeyDelta.x += 1;
            }
        }
        axis.value += KeyDelta;
        axis.delta += KeyDelta;
    }
    public void ControllButton(button button)
    {
        for (int i = 0; i < button.keys.Length; i++)
        {
            button.isPressed = Input.GetKey(button.keys[i].keyCode);
            button.isDown = Input.GetKeyDown(button.keys[i].keyCode);
            button.isUp = Input.GetKeyUp(button.keys[i].keyCode);
        }
    }

    //Acess
    public static button SearchButton(string name)
    {
        button r = null;
        for (int i = 0; i < main.settings.m_Inputs.Buttons.Length; i++)
        {
            if (main.settings.m_Inputs.Buttons[i].m_name.ToLower() == name.ToLower())
            {
                r = main.settings.m_Inputs.Buttons[i];
                break;
            }
        }
        if (r == null)
        {
            Debug.LogError("[InputSystem] Button not found: " + name);
        }
        return r;
    }
    public static axis SearchAxis(string name)
    {
        axis r = null;
        for (int i = 0; i < main.settings.m_Inputs.Axis.Length; i++)
        {
            if (main.settings.m_Inputs.Axis[i].m_name == name)
            {
                r = main.settings.m_Inputs.Axis[i];
                break;
            }
        }
        if (r == null)
        {
            Debug.LogError("[InputSystem] Axis not found: " + name);
        }
        return r;
    }

    public static bool GetButton(string name)
    {
        return SearchButton(name).isPressed;
    }
    public static bool GetButtonDown(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }
        return SearchButton(name).isDown;
    }
    public static bool GetButtonUp(string name)
    {
        return SearchButton(name).isUp;
    }
    public static Vector2 GetAxis(string name)
    {
        return SearchAxis(name).value;
    }
    public static string GetButtonName(string name)
    {
        string r = "<h1>[" + GetButtonNameRaw(name) + "]</h1>";

        if (r == "<h1>[Mouse0]</h1>")
        {
            r = "<h1>[LMB]</h1>";
        }

        if (r == "<h1>[Mouse1]</h1>")
        {
            r = "<h1>[RMB]</h1>";
        }

        if (r == "<h1>[Mouse2]</h1>")
        {
            r = "<h1>[Scroll Button]</h1>";
        }

        return r;
    }
    public static string GetButtonNameRaw(string name)
    {
        return SearchButton(name).keys[0].keyCode.ToString();
    }


    public void SetButtonNames()
    {
        //Set Button Names
        for (int i = 0; i < settings.m_Inputs.Buttons.Length; i++)
        {
            if (string.IsNullOrEmpty(settings.m_Inputs.Buttons[i].Name))
            {
                settings.m_Inputs.Buttons[i].m_name = settings.m_Inputs.Buttons[i].keys[0].keyCode.ToString();
            }
            else
            {
                settings.m_Inputs.Buttons[i].m_name = settings.m_Inputs.Buttons[i].Name;
            }
        }
    }
    public void SetAxisNames()
    {
        //Set Axis Names
        for (int i = 0; i < settings.m_Inputs.Axis.Length; i++)
        {
            if (string.IsNullOrEmpty(settings.m_Inputs.Axis[i].Name))
            {
                settings.m_Inputs.Axis[i].m_name = settings.m_Inputs.Axis[i].type.ToString();
            }
            else
            {
                settings.m_Inputs.Axis[i].m_name = settings.m_Inputs.Axis[i].Name;
            }
        }
    }

    //Mono
    public void Awake()
    {
        Debug.Log("InputSettings: " + settings.name);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        SetButtonNames();
        SetAxisNames();
    }
    private void Update()
    {
        DebugVar.Log("Current InputSystem", settings.name, this);
        DebugVar.Log("Current InputSystem Axis", settings.m_Inputs.Axis.Length, this);

        string axisBtns = "";
        for (int i = 0; i < settings.m_Inputs.Axis.Length; i++)
        {
            axisBtns += settings.m_Inputs.Axis[i].m_name + "\n";
        }
        DebugVar.Log("Current InputSystem Axis", settings.m_Inputs.Axis.Length + "\n" + axisBtns, this);

        DebugVar.Log("Current InputSystem Btns", settings.m_Inputs.Buttons.Length, this);


        //Update Buttons
        for (int i = 0; i < settings.m_Inputs.Buttons.Length; i++)
        {
            ControllButton(settings.m_Inputs.Buttons[i]);
        }

        //Update Axis
        for (int i = 0; i < settings.m_Inputs.Axis.Length; i++)
        {
            ControllAxis(settings.m_Inputs.Axis[i]);
        }
    }


    public void OnValidate()
    {
        if (!settings)
        {
            inputs = null;
        }
        else
        {
            if (inputs != settings.m_Inputs)
            {
                inputs = settings.m_Inputs;
            }
            SetButtonNames();
            SetAxisNames();
        }
    }
}
