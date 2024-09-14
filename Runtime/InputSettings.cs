using UnityEngine;


[CreateAssetMenu(fileName = "InputSettings", menuName = "InputSystem/InputSettings")]
public class InputSettings : ScriptableObject
{
    /// <summary>
    /// A collection input class
    /// </summary>
    [System.Serializable]
    public class Inputs
    {
        /// <summary>
        /// An array of input button
        /// </summary>
        [SerializeField]
        public InputSystem.button[] Buttons;
        /// <summary>
        /// An array of input axis
        /// </summary>
        [SerializeField]
        public InputSystem.axis[] Axis;
    }
    [Tooltip("The input layout list")]
    [SerializeField]
    public Inputs m_Inputs;
}
