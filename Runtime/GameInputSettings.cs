using UnityEngine;


[CreateAssetMenu(fileName = "InputSettings", menuName = "InputSystem/InputSettings")]
public class GameInputSettings : ScriptableObject
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
        public GameInput.button[] Buttons;
        /// <summary>
        /// An array of input axis
        /// </summary>
        [SerializeField]
        public GameInput.axis[] Axis;
    }
    [Tooltip("The input layout list")]
    [SerializeField]
    public Inputs m_Inputs;
}
