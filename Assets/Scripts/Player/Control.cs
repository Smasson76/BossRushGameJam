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
                    ""name"": ""Megaboost"",
                    ""type"": ""Button"",
                    ""id"": ""d17a6fe2-5932-4cf3-9efa-7e636598104c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""DirectCameraControl"",
                    ""type"": ""Value"",
                    ""id"": ""55db1fbf-5fbf-42ec-8d73-b5bf416c0d24"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DirectCameraControlDelta"",
                    ""type"": ""Value"",
                    ""id"": ""31b97224-5c9d-4f03-b1ae-9da3bfee9677"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
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
                    ""id"": ""0c39e333-a1d9-4074-91cd-b0fb99477ece"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Megaboost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5388da43-af2b-4085-889a-bd2cbd1a9803"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DirectCameraControl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b71787db-5e01-48b0-8d2d-4b86fe7093e5"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DirectCameraControlDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CameraControl"",
            ""id"": ""76619169-7bc9-4fe5-952a-1d777f3425fb"",
            ""actions"": [
                {
                    ""name"": ""Camera Control"",
                    ""type"": ""Value"",
                    ""id"": ""635c3dce-72b3-4610-9c77-8c55d7498d6e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f424e7ba-4e5b-4cb7-9241-79584801de94"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Camera Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MainMenuControls"",
            ""id"": ""3b2200e4-a1fc-4f32-877c-822205561258"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""529501cd-a6cd-4781-81bf-f03af33261de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d17296a7-b715-48d9-8c30-eef2ddc0a680"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerSpawnerControls"",
            ""id"": ""9c1044f3-deff-4700-b124-82f4f7ec0c01"",
            ""actions"": [
                {
                    ""name"": ""Spawning"",
                    ""type"": ""Button"",
                    ""id"": ""3849fc66-c7da-42d6-a983-07eebba220f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""561d5d1e-d668-471e-94a6-770bfd11daa4"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Spawning"",
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
        m_PlayerMovement_PlayerMove = m_PlayerMovement.FindAction("PlayerMove", throwIfNotFound: true);
        m_PlayerMovement_GrappleStart = m_PlayerMovement.FindAction("GrappleStart", throwIfNotFound: true);
        m_PlayerMovement_GrappleEnd = m_PlayerMovement.FindAction("GrappleEnd", throwIfNotFound: true);
        m_PlayerMovement_Boost = m_PlayerMovement.FindAction("Boost", throwIfNotFound: true);
        m_PlayerMovement_Slow = m_PlayerMovement.FindAction("Slow", throwIfNotFound: true);
        m_PlayerMovement_Megaboost = m_PlayerMovement.FindAction("Megaboost", throwIfNotFound: true);
        m_PlayerMovement_DirectCameraControl = m_PlayerMovement.FindAction("DirectCameraControl", throwIfNotFound: true);
        m_PlayerMovement_DirectCameraControlDelta = m_PlayerMovement.FindAction("DirectCameraControlDelta", throwIfNotFound: true);
        // CameraControl
        m_CameraControl = asset.FindActionMap("CameraControl", throwIfNotFound: true);
        m_CameraControl_CameraControl = m_CameraControl.FindAction("Camera Control", throwIfNotFound: true);
        // MainMenuControls
        m_MainMenuControls = asset.FindActionMap("MainMenuControls", throwIfNotFound: true);
        m_MainMenuControls_Click = m_MainMenuControls.FindAction("Click", throwIfNotFound: true);
        // PlayerSpawnerControls
        m_PlayerSpawnerControls = asset.FindActionMap("PlayerSpawnerControls", throwIfNotFound: true);
        m_PlayerSpawnerControls_Spawning = m_PlayerSpawnerControls.FindAction("Spawning", throwIfNotFound: true);
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
    private readonly InputAction m_PlayerMovement_PlayerMove;
    private readonly InputAction m_PlayerMovement_GrappleStart;
    private readonly InputAction m_PlayerMovement_GrappleEnd;
    private readonly InputAction m_PlayerMovement_Boost;
    private readonly InputAction m_PlayerMovement_Slow;
    private readonly InputAction m_PlayerMovement_Megaboost;
    private readonly InputAction m_PlayerMovement_DirectCameraControl;
    private readonly InputAction m_PlayerMovement_DirectCameraControlDelta;
    public struct PlayerMovementActions
    {
        private @Control m_Wrapper;
        public PlayerMovementActions(@Control wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlayerMove => m_Wrapper.m_PlayerMovement_PlayerMove;
        public InputAction @GrappleStart => m_Wrapper.m_PlayerMovement_GrappleStart;
        public InputAction @GrappleEnd => m_Wrapper.m_PlayerMovement_GrappleEnd;
        public InputAction @Boost => m_Wrapper.m_PlayerMovement_Boost;
        public InputAction @Slow => m_Wrapper.m_PlayerMovement_Slow;
        public InputAction @Megaboost => m_Wrapper.m_PlayerMovement_Megaboost;
        public InputAction @DirectCameraControl => m_Wrapper.m_PlayerMovement_DirectCameraControl;
        public InputAction @DirectCameraControlDelta => m_Wrapper.m_PlayerMovement_DirectCameraControlDelta;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
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
                @Megaboost.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMegaboost;
                @Megaboost.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMegaboost;
                @Megaboost.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMegaboost;
                @DirectCameraControl.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDirectCameraControl;
                @DirectCameraControl.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDirectCameraControl;
                @DirectCameraControl.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDirectCameraControl;
                @DirectCameraControlDelta.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDirectCameraControlDelta;
                @DirectCameraControlDelta.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDirectCameraControlDelta;
                @DirectCameraControlDelta.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnDirectCameraControlDelta;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
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
                @Megaboost.started += instance.OnMegaboost;
                @Megaboost.performed += instance.OnMegaboost;
                @Megaboost.canceled += instance.OnMegaboost;
                @DirectCameraControl.started += instance.OnDirectCameraControl;
                @DirectCameraControl.performed += instance.OnDirectCameraControl;
                @DirectCameraControl.canceled += instance.OnDirectCameraControl;
                @DirectCameraControlDelta.started += instance.OnDirectCameraControlDelta;
                @DirectCameraControlDelta.performed += instance.OnDirectCameraControlDelta;
                @DirectCameraControlDelta.canceled += instance.OnDirectCameraControlDelta;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // CameraControl
    private readonly InputActionMap m_CameraControl;
    private ICameraControlActions m_CameraControlActionsCallbackInterface;
    private readonly InputAction m_CameraControl_CameraControl;
    public struct CameraControlActions
    {
        private @Control m_Wrapper;
        public CameraControlActions(@Control wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraControl => m_Wrapper.m_CameraControl_CameraControl;
        public InputActionMap Get() { return m_Wrapper.m_CameraControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlActions instance)
        {
            if (m_Wrapper.m_CameraControlActionsCallbackInterface != null)
            {
                @CameraControl.started -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraControl;
                @CameraControl.performed -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraControl;
                @CameraControl.canceled -= m_Wrapper.m_CameraControlActionsCallbackInterface.OnCameraControl;
            }
            m_Wrapper.m_CameraControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CameraControl.started += instance.OnCameraControl;
                @CameraControl.performed += instance.OnCameraControl;
                @CameraControl.canceled += instance.OnCameraControl;
            }
        }
    }
    public CameraControlActions @CameraControl => new CameraControlActions(this);

    // MainMenuControls
    private readonly InputActionMap m_MainMenuControls;
    private IMainMenuControlsActions m_MainMenuControlsActionsCallbackInterface;
    private readonly InputAction m_MainMenuControls_Click;
    public struct MainMenuControlsActions
    {
        private @Control m_Wrapper;
        public MainMenuControlsActions(@Control wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_MainMenuControls_Click;
        public InputActionMap Get() { return m_Wrapper.m_MainMenuControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainMenuControlsActions set) { return set.Get(); }
        public void SetCallbacks(IMainMenuControlsActions instance)
        {
            if (m_Wrapper.m_MainMenuControlsActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_MainMenuControlsActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_MainMenuControlsActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_MainMenuControlsActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_MainMenuControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public MainMenuControlsActions @MainMenuControls => new MainMenuControlsActions(this);

    // PlayerSpawnerControls
    private readonly InputActionMap m_PlayerSpawnerControls;
    private IPlayerSpawnerControlsActions m_PlayerSpawnerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerSpawnerControls_Spawning;
    public struct PlayerSpawnerControlsActions
    {
        private @Control m_Wrapper;
        public PlayerSpawnerControlsActions(@Control wrapper) { m_Wrapper = wrapper; }
        public InputAction @Spawning => m_Wrapper.m_PlayerSpawnerControls_Spawning;
        public InputActionMap Get() { return m_Wrapper.m_PlayerSpawnerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerSpawnerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerSpawnerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerSpawnerControlsActionsCallbackInterface != null)
            {
                @Spawning.started -= m_Wrapper.m_PlayerSpawnerControlsActionsCallbackInterface.OnSpawning;
                @Spawning.performed -= m_Wrapper.m_PlayerSpawnerControlsActionsCallbackInterface.OnSpawning;
                @Spawning.canceled -= m_Wrapper.m_PlayerSpawnerControlsActionsCallbackInterface.OnSpawning;
            }
            m_Wrapper.m_PlayerSpawnerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Spawning.started += instance.OnSpawning;
                @Spawning.performed += instance.OnSpawning;
                @Spawning.canceled += instance.OnSpawning;
            }
        }
    }
    public PlayerSpawnerControlsActions @PlayerSpawnerControls => new PlayerSpawnerControlsActions(this);
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
        void OnPlayerMove(InputAction.CallbackContext context);
        void OnGrappleStart(InputAction.CallbackContext context);
        void OnGrappleEnd(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnSlow(InputAction.CallbackContext context);
        void OnMegaboost(InputAction.CallbackContext context);
        void OnDirectCameraControl(InputAction.CallbackContext context);
        void OnDirectCameraControlDelta(InputAction.CallbackContext context);
    }
    public interface ICameraControlActions
    {
        void OnCameraControl(InputAction.CallbackContext context);
    }
    public interface IMainMenuControlsActions
    {
        void OnClick(InputAction.CallbackContext context);
    }
    public interface IPlayerSpawnerControlsActions
    {
        void OnSpawning(InputAction.CallbackContext context);
    }
}
