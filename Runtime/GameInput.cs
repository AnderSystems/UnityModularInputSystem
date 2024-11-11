using UnityEngine;
using UnityEngine.InputSystem;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GameInput))]
public class CustomGameInputEditor : Editor
{
    [MenuItem("GameObject/New Input System")]
    public static void CreateNewGameInputObject()
    {
        GameInput inp = GameInput.CreateNewGameInputObject();
    }
}
#endif

//namespace AnderSystems
//{
public class GameInput : MonoBehaviour
{
    public static InputSettings selectedSettings;

    //Set Singleton
    static GameInput instance;
    public static GameInput main
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameInput>();
            }

            if (instance == null)
            {
                instance = CreateNewGameInputObject();
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

    public static GameInput CreateNewGameInputObject()
    {
        GameInput GameInput = new GameObject("GameInput").AddComponent<GameInput>();
        return GameInput;
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
            if (main.settings.m_Inputs.Buttons[i].m_name == name)
            {
                r = main.settings.m_Inputs.Buttons[i];
                break;
            }
        }
        if (r == null)
        {
            Debug.LogError("[GameInput] Button not found: " + name);
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
            Debug.LogError("[GameInput] Axis not found: " + name);
        }
        return r;
    }

    public static bool GetButton(string name)
    {
        return SearchButton(name).isPressed;
    }
    public static bool GetButtonDown(string name)
    {
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

    //Mono
    private void Update()
    {
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
    }
}
//}