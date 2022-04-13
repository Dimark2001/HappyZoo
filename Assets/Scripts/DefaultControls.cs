// GENERATED AUTOMATICALLY FROM 'Assets/Configs/DefaultControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DefaultControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultControls"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""c308960a-f761-4dd5-92bb-59367c2cf26d"",
            ""actions"": [
                {
                    ""name"": ""Stick"",
                    ""type"": ""Value"",
                    ""id"": ""8b2e0165-13c4-483f-8cee-9e0e2c8ad906"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Touch"",
                    ""type"": ""Button"",
                    ""id"": ""bc3c1244-4c61-49cb-ae51-a5e1e25fadbb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchPosition"",
                    ""type"": ""Value"",
                    ""id"": ""a4ea746b-8c4e-47a9-a4bd-e6b65588ce7d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e703cb16-6c22-491e-9155-60e1aa0785a7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51fa8a6e-a536-4a37-93e0-ec269e9cc1f5"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29d1a597-4e11-447e-b9f1-f1ac28295dbc"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed1219af-ff65-4107-bea6-7219ca57825a"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ad38b3b-4337-4146-b1c6-3a601ebe4392"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Stick = m_Default.FindAction("Stick", throwIfNotFound: true);
        m_Default_Touch = m_Default.FindAction("Touch", throwIfNotFound: true);
        m_Default_TouchPosition = m_Default.FindAction("TouchPosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_Stick;
    private readonly InputAction m_Default_Touch;
    private readonly InputAction m_Default_TouchPosition;
    public struct DefaultActions
    {
        private @DefaultControls m_Wrapper;
        public DefaultActions(@DefaultControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Stick => m_Wrapper.m_Default_Stick;
        public InputAction @Touch => m_Wrapper.m_Default_Touch;
        public InputAction @TouchPosition => m_Wrapper.m_Default_TouchPosition;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @Stick.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnStick;
                @Stick.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnStick;
                @Stick.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnStick;
                @Touch.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTouch;
                @Touch.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTouch;
                @Touch.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTouch;
                @TouchPosition.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTouchPosition;
                @TouchPosition.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTouchPosition;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Stick.started += instance.OnStick;
                @Stick.performed += instance.OnStick;
                @Stick.canceled += instance.OnStick;
                @Touch.started += instance.OnTouch;
                @Touch.performed += instance.OnTouch;
                @Touch.canceled += instance.OnTouch;
                @TouchPosition.started += instance.OnTouchPosition;
                @TouchPosition.performed += instance.OnTouchPosition;
                @TouchPosition.canceled += instance.OnTouchPosition;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnStick(InputAction.CallbackContext context);
        void OnTouch(InputAction.CallbackContext context);
        void OnTouchPosition(InputAction.CallbackContext context);
    }
}
