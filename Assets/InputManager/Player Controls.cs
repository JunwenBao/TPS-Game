//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/InputManager/Player Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controls"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""40cbb5af-f985-4683-bce4-b64b651174d8"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""d2dc5048-84ec-41ef-80df-af775d9db186"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""d3704bb2-a243-4b88-936e-8b05ae994a10"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""a7911546-d17b-4237-8e50-63684127da61"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchShoulder"",
                    ""type"": ""Button"",
                    ""id"": ""c8330161-0b55-4bb4-bead-62e2af4aff71"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""d032249f-ffda-4ccd-aec3-581837327ab4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 1"",
                    ""type"": ""Button"",
                    ""id"": ""4a4191b8-a94b-4df8-8899-6ab0326e34b3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 2"",
                    ""type"": ""Button"",
                    ""id"": ""c0037b6b-2a95-45b3-ae4f-823c61c258e2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drop Current Weapon"",
                    ""type"": ""Button"",
                    ""id"": ""c73a1413-de39-4bbb-ac12-04550b5dc072"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""eacf6fac-eeca-41b0-8020-a2164d2fd1d2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 3"",
                    ""type"": ""Button"",
                    ""id"": ""12aad7fd-27a1-4e2d-822e-1aa296ecf7c7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 4"",
                    ""type"": ""Button"",
                    ""id"": ""bbf6f013-9a4f-4ad0-ad1f-5cff4352ec92"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 5"",
                    ""type"": ""Button"",
                    ""id"": ""4ef7bb7b-f4c9-4b5e-9603-9b25f3182d2b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Toggle Weapon Mode"",
                    ""type"": ""Button"",
                    ""id"": ""46319d31-47e1-4580-9de0-af290f3cd5e7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""20cefed9-d59b-42f4-9307-d1f384f5be98"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f8b512f7-cd19-422e-8cd7-5ccd95c2f8d2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""4b426d88-336f-43d7-aba9-f2ac3f9bd6d4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b16327fa-7b20-4019-81e9-2656c8694923"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""876917c9-df6b-4887-937e-20c38eaee76c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""38109d14-a5b2-43fb-9291-98b68c20ae50"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""764fa34f-64c7-4087-80ae-fe83937f4059"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""15447b9a-0a9e-43ee-bd70-a5d5c068b334"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b34ce574-40fb-491d-b5b4-9774ee213ac9"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e660ced8-516a-407a-bc8c-0ff8c8c59008"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a73a89a-0092-497a-ab28-1d4e37908e35"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop Current Weapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2884b76-e2b1-432f-b884-a9b0a0b92497"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c265e2e6-c88c-426b-b8f7-2cc7bc680122"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86398e71-910b-4dc1-bfd1-20cd09940f5a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6cf7850-44d7-4317-be57-ab44f1c6a88c"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d11d0fdc-f831-4616-95f2-96688a2d95ef"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle Weapon Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34e6a189-07b9-4d2a-ab10-739049988610"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fe33cde-7cb9-47d1-97bf-235b5c5cbf43"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""042c6a7d-fa6d-4d56-8498-27f11e9a2aca"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchShoulder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CameraControls"",
            ""id"": ""81a1a4eb-b56c-4413-993a-e5b9783d3c8f"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""b78bb9ac-69bc-4cca-b70c-d8ab827a5d90"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""7f4a8773-1d89-402c-adab-2b3aa2815497"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3ae6b63d-af91-4214-96aa-522086934f38"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""affa5e9a-eca9-4ea1-9110-0f3d85a21c04"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f2b0211e-29af-4cca-bd96-ed781ea47886"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""fdf44988-aa9e-49f7-b388-1a136b081315"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_Fire = m_Character.FindAction("Fire", throwIfNotFound: true);
        m_Character_Aim = m_Character.FindAction("Aim", throwIfNotFound: true);
        m_Character_SwitchShoulder = m_Character.FindAction("SwitchShoulder", throwIfNotFound: true);
        m_Character_Run = m_Character.FindAction("Run", throwIfNotFound: true);
        m_Character_EquipSlot1 = m_Character.FindAction("Equip Slot - 1", throwIfNotFound: true);
        m_Character_EquipSlot2 = m_Character.FindAction("Equip Slot - 2", throwIfNotFound: true);
        m_Character_DropCurrentWeapon = m_Character.FindAction("Drop Current Weapon", throwIfNotFound: true);
        m_Character_Reload = m_Character.FindAction("Reload", throwIfNotFound: true);
        m_Character_EquipSlot3 = m_Character.FindAction("Equip Slot - 3", throwIfNotFound: true);
        m_Character_EquipSlot4 = m_Character.FindAction("Equip Slot - 4", throwIfNotFound: true);
        m_Character_EquipSlot5 = m_Character.FindAction("Equip Slot - 5", throwIfNotFound: true);
        m_Character_ToggleWeaponMode = m_Character.FindAction("Toggle Weapon Mode", throwIfNotFound: true);
        m_Character_Interaction = m_Character.FindAction("Interaction", throwIfNotFound: true);
        // CameraControls
        m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
        m_CameraControls_Look = m_CameraControls.FindAction("Look", throwIfNotFound: true);
        m_CameraControls_Zoom = m_CameraControls.FindAction("Zoom", throwIfNotFound: true);
    }

    ~@PlayerControls()
    {
        UnityEngine.Debug.Assert(!m_Character.enabled, "This will cause a leak and performance issues, PlayerControls.Character.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_CameraControls.enabled, "This will cause a leak and performance issues, PlayerControls.CameraControls.Disable() has not been called.");
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Character
    private readonly InputActionMap m_Character;
    private List<ICharacterActions> m_CharacterActionsCallbackInterfaces = new List<ICharacterActions>();
    private readonly InputAction m_Character_Movement;
    private readonly InputAction m_Character_Fire;
    private readonly InputAction m_Character_Aim;
    private readonly InputAction m_Character_SwitchShoulder;
    private readonly InputAction m_Character_Run;
    private readonly InputAction m_Character_EquipSlot1;
    private readonly InputAction m_Character_EquipSlot2;
    private readonly InputAction m_Character_DropCurrentWeapon;
    private readonly InputAction m_Character_Reload;
    private readonly InputAction m_Character_EquipSlot3;
    private readonly InputAction m_Character_EquipSlot4;
    private readonly InputAction m_Character_EquipSlot5;
    private readonly InputAction m_Character_ToggleWeaponMode;
    private readonly InputAction m_Character_Interaction;
    public struct CharacterActions
    {
        private @PlayerControls m_Wrapper;
        public CharacterActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @Fire => m_Wrapper.m_Character_Fire;
        public InputAction @Aim => m_Wrapper.m_Character_Aim;
        public InputAction @SwitchShoulder => m_Wrapper.m_Character_SwitchShoulder;
        public InputAction @Run => m_Wrapper.m_Character_Run;
        public InputAction @EquipSlot1 => m_Wrapper.m_Character_EquipSlot1;
        public InputAction @EquipSlot2 => m_Wrapper.m_Character_EquipSlot2;
        public InputAction @DropCurrentWeapon => m_Wrapper.m_Character_DropCurrentWeapon;
        public InputAction @Reload => m_Wrapper.m_Character_Reload;
        public InputAction @EquipSlot3 => m_Wrapper.m_Character_EquipSlot3;
        public InputAction @EquipSlot4 => m_Wrapper.m_Character_EquipSlot4;
        public InputAction @EquipSlot5 => m_Wrapper.m_Character_EquipSlot5;
        public InputAction @ToggleWeaponMode => m_Wrapper.m_Character_ToggleWeaponMode;
        public InputAction @Interaction => m_Wrapper.m_Character_Interaction;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void AddCallbacks(ICharacterActions instance)
        {
            if (instance == null || m_Wrapper.m_CharacterActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @SwitchShoulder.started += instance.OnSwitchShoulder;
            @SwitchShoulder.performed += instance.OnSwitchShoulder;
            @SwitchShoulder.canceled += instance.OnSwitchShoulder;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @EquipSlot1.started += instance.OnEquipSlot1;
            @EquipSlot1.performed += instance.OnEquipSlot1;
            @EquipSlot1.canceled += instance.OnEquipSlot1;
            @EquipSlot2.started += instance.OnEquipSlot2;
            @EquipSlot2.performed += instance.OnEquipSlot2;
            @EquipSlot2.canceled += instance.OnEquipSlot2;
            @DropCurrentWeapon.started += instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.performed += instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.canceled += instance.OnDropCurrentWeapon;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @EquipSlot3.started += instance.OnEquipSlot3;
            @EquipSlot3.performed += instance.OnEquipSlot3;
            @EquipSlot3.canceled += instance.OnEquipSlot3;
            @EquipSlot4.started += instance.OnEquipSlot4;
            @EquipSlot4.performed += instance.OnEquipSlot4;
            @EquipSlot4.canceled += instance.OnEquipSlot4;
            @EquipSlot5.started += instance.OnEquipSlot5;
            @EquipSlot5.performed += instance.OnEquipSlot5;
            @EquipSlot5.canceled += instance.OnEquipSlot5;
            @ToggleWeaponMode.started += instance.OnToggleWeaponMode;
            @ToggleWeaponMode.performed += instance.OnToggleWeaponMode;
            @ToggleWeaponMode.canceled += instance.OnToggleWeaponMode;
            @Interaction.started += instance.OnInteraction;
            @Interaction.performed += instance.OnInteraction;
            @Interaction.canceled += instance.OnInteraction;
        }

        private void UnregisterCallbacks(ICharacterActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @SwitchShoulder.started -= instance.OnSwitchShoulder;
            @SwitchShoulder.performed -= instance.OnSwitchShoulder;
            @SwitchShoulder.canceled -= instance.OnSwitchShoulder;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @EquipSlot1.started -= instance.OnEquipSlot1;
            @EquipSlot1.performed -= instance.OnEquipSlot1;
            @EquipSlot1.canceled -= instance.OnEquipSlot1;
            @EquipSlot2.started -= instance.OnEquipSlot2;
            @EquipSlot2.performed -= instance.OnEquipSlot2;
            @EquipSlot2.canceled -= instance.OnEquipSlot2;
            @DropCurrentWeapon.started -= instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.performed -= instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.canceled -= instance.OnDropCurrentWeapon;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @EquipSlot3.started -= instance.OnEquipSlot3;
            @EquipSlot3.performed -= instance.OnEquipSlot3;
            @EquipSlot3.canceled -= instance.OnEquipSlot3;
            @EquipSlot4.started -= instance.OnEquipSlot4;
            @EquipSlot4.performed -= instance.OnEquipSlot4;
            @EquipSlot4.canceled -= instance.OnEquipSlot4;
            @EquipSlot5.started -= instance.OnEquipSlot5;
            @EquipSlot5.performed -= instance.OnEquipSlot5;
            @EquipSlot5.canceled -= instance.OnEquipSlot5;
            @ToggleWeaponMode.started -= instance.OnToggleWeaponMode;
            @ToggleWeaponMode.performed -= instance.OnToggleWeaponMode;
            @ToggleWeaponMode.canceled -= instance.OnToggleWeaponMode;
            @Interaction.started -= instance.OnInteraction;
            @Interaction.performed -= instance.OnInteraction;
            @Interaction.canceled -= instance.OnInteraction;
        }

        public void RemoveCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICharacterActions instance)
        {
            foreach (var item in m_Wrapper.m_CharacterActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CharacterActions @Character => new CharacterActions(this);

    // CameraControls
    private readonly InputActionMap m_CameraControls;
    private List<ICameraControlsActions> m_CameraControlsActionsCallbackInterfaces = new List<ICameraControlsActions>();
    private readonly InputAction m_CameraControls_Look;
    private readonly InputAction m_CameraControls_Zoom;
    public struct CameraControlsActions
    {
        private @PlayerControls m_Wrapper;
        public CameraControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_CameraControls_Look;
        public InputAction @Zoom => m_Wrapper.m_CameraControls_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void AddCallbacks(ICameraControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Add(instance);
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
        }

        private void UnregisterCallbacks(ICameraControlsActions instance)
        {
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
        }

        public void RemoveCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);
    public interface ICharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnSwitchShoulder(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnEquipSlot1(InputAction.CallbackContext context);
        void OnEquipSlot2(InputAction.CallbackContext context);
        void OnDropCurrentWeapon(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnEquipSlot3(InputAction.CallbackContext context);
        void OnEquipSlot4(InputAction.CallbackContext context);
        void OnEquipSlot5(InputAction.CallbackContext context);
        void OnToggleWeaponMode(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
    }
    public interface ICameraControlsActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}
