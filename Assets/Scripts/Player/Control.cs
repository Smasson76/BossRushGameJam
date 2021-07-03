// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Player/Control.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Control : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Control()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Control"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""f9c6c382-2e93-465a-bdab-759ceb4fab1c"",
            ""actions"": [
                {
                    ""name"": ""Camera Control"",
                    ""type"": ""Value"",
                    ""id"": ""581a2498-6894-4dcb-aaac-90f2aa745b1d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerMove"",
                    ""type"": ""Value"",
                    ""id"": ""a9f4318c-01ed-4fc8-9cfd-bf218ac41d69"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GrappleStart"",
                    ""type"": ""Button"",
                    ""id"": ""104df7e0-9e95-4ea3-ac2b-fbcac7f7b222"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""GrappleEnd"",
                    ""type"": ""Button"",
                    ""id"": ""8946be8e-be71-424a-9bfe-d013c9b9efea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Value"",
                    ""id"": ""11175b10-4b64-42c4-a41c-e28ba95b5fcb"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slow"",
                    ""type"": ""Value"",
                    ""id"": ""fce496d8-a50d-4d4d-b189-21a48be4d29e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pop"",
                    ""type"": ""Value"",
                    ""id"": ""e5d3f04d-76c9-4ab9-b065-566c1d35a32d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1eeaa0b0-2cba-4db4-a9a4-410adcc8d73d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Camera Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""cbaedf55-d1be-4d5c-90c4-ad3a08cc576e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3eac39f5-4fd5-42e9-ae43-3e488b01b97f"",
                    ""path"": ""<Keyboard>/W"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e34f73a8-0ce0-417e-aaa2-d6d684bf5118"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""27ff8398-af0e-41bc-9c22-54de56962c37"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""704478cf-5c90-406b-a6b5-54d0a8c46638"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""PlayerMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0365e732-fc94-40dc-8fb4-43a7a77b4788"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""GrappleStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b1ee1cf-5f32-435d-95ba-10ee57661312"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""GrappleEnd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0b97066-87c1-48e0-bf00-22856ff38d47"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90dff0de-9601-4b56-b5b4-86b9d233a5c2"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Slow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e14570c4-92e5-48de-bef3-c8bdcce9cf86"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Pop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and mouse"",
            ""bindingGroup"": ""Keyboard and mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_CameraControl = m_PlayerMovement.FindAction("Camera Control", throwIfNotFound: true);
        m_PlayerMovement_PlayerMove = m_PlayerMovement.FindAction("PlayerMove", throwIfNotFound: true);
        m_PlayerMovement_GrappleStart = m_PlayerMovement.FindAction("GrappleStart", throwIfNotFound: true);
        m_PlayerMovement_GrappleEnd = m_PlayerMovement.FindAction("GrappleEnd", throwIfNotFound: true);
        m_PlayerMovement_Boost = m_PlayerMovement.FindAction("Boost", throwIfNotFound: true);
        m_PlayerMovement_Slow = m_PlayerMovement.FindAction("Slow", throwIfNotFound: true);
        m_PlayerMovement_Pop = m_PlayerMovement.FindAction("Pop", throwIfNotFound: true);
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

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_CameraControl;
    private readonly InputAction m_PlayerMovement_PlayerMove;
    private readonly InputAction m_PlayerMovement_GrappleStart;
    private readonly InputAction m_PlayerMovement_GrappleEnd;
    private readonly InputAction m_PlayerMovement_Boost;
    private readonly InputAction m_PlayerMovement_Slow;
    private readonly InputAction m_PlayerMovement_Pop;
    public struct PlayerMovementActions
    {
        private @Control m_Wrapper;
        public PlayerMovementActions(@Control wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraControl => m_Wrapper.m_PlayerMovement_CameraControl;
        public InputAction @PlayerMove => m_Wrapper.m_PlayerMovement_PlayerMove;
        public InputAction @GrappleStart => m_Wrapper.m_PlayerMovement_GrappleStart;
        public InputAction @GrappleEnd => m_Wrapper.m_PlayerMovement_GrappleEnd;
        public InputAction @Boost => m_Wrapper.m_PlayerMovement_Boost;
        public InputAction @Slow => m_Wrapper.m_PlayerMovement_Slow;
        public InputAction @Pop => m_Wrapper.m_PlayerMovement_Pop;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @CameraControl.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraControl;
                @CameraControl.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraControl;
                @CameraControl.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraControl;
                @PlayerMove.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayerMove;
                @PlayerMove.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayerMove;
                @PlayerMove.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPlayerMove;
                @GrappleStart.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnGrappleStart;
                @GrappleStart.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnGrappleStart;
                @GrappleStart.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnGrappleStart;
                @GrappleEnd.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnGrappleEnd;
                @GrappleEnd.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnGrappleEnd;
                @GrappleEnd.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnGrappleEnd;
                @Boost.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnBoost;
                @Slow.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSlow;
                @Slow.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSlow;
                @Slow.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnSlow;
                @Pop.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPop;
                @Pop.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPop;
                @Pop.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnPop;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CameraControl.started += instance.OnCameraControl;
                @CameraControl.performed += instance.OnCameraControl;
                @CameraControl.canceled += instance.OnCameraControl;
                @PlayerMove.started += instance.OnPlayerMove;
                @PlayerMove.performed += instance.OnPlayerMove;
                @PlayerMove.canceled += instance.OnPlayerMove;
                @GrappleStart.started += instance.OnGrappleStart;
                @GrappleStart.performed += instance.OnGrappleStart;
                @GrappleStart.canceled += instance.OnGrappleStart;
                @GrappleEnd.started += instance.OnGrappleEnd;
                @GrappleEnd.performed += instance.OnGrappleEnd;
                @GrappleEnd.canceled += instance.OnGrappleEnd;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @Slow.started += instance.OnSlow;
                @Slow.performed += instance.OnSlow;
                @Slow.canceled += instance.OnSlow;
                @Pop.started += instance.OnPop;
                @Pop.performed += instance.OnPop;
                @Pop.canceled += instance.OnPop;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);
    private int m_KeyboardandmouseSchemeIndex = -1;
    public InputControlScheme KeyboardandmouseScheme
    {
        get
        {
            if (m_KeyboardandmouseSchemeIndex == -1) m_KeyboardandmouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and mouse");
            return asset.controlSchemes[m_KeyboardandmouseSchemeIndex];
        }
    }
    public interface IPlayerMovementActions
    {
        void OnCameraControl(InputAction.CallbackContext context);
        void OnPlayerMove(InputAction.CallbackContext context);
        void OnGrappleStart(InputAction.CallbackContext context);
        void OnGrappleEnd(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnSlow(InputAction.CallbackContext context);
        void OnPop(InputAction.CallbackContext context);
    }
}
