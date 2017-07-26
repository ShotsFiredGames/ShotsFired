using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class Controls : PlayerActionSet
{
    
    //public Controls AssignKeyBindings()
    //{
        
    //    //Controls controls = new Controls();
    //    //controls.Load((PlayerPrefs.GetString("SaveInfoJump")));
    //    ////controls.Jump.AddBinding(controls.Load((PlayerPrefs.GetString("SaveInfoJump"));

    //    //controls.Load(PlayerPrefs.GetString("SaveInfoFire"));
    //    //PlayerPrefs.GetString("SaveInfoAim");
    //    //PlayerPrefs.GetString("SaveInfoSprint");
    //    //PlayerPrefs.GetString("SaveInfoLeft");
    //    //PlayerPrefs.GetString("SaveInfoRight");
    //    //PlayerPrefs.GetString("SaveInfoUp");
    //    //PlayerPrefs.GetString("SaveInfoDown");
    //    //PlayerPrefs.GetString("SaveInfoCrouch");
    //    //PlayerPrefs.GetString("SaveInfoLookLeft");
    //    //PlayerPrefs.GetString("SaveInfoLookRight");
    //    //PlayerPrefs.GetString("SaveInfoLookUp");
    //    //PlayerPrefs.GetString("SaveInfoLookDown");
    //    //Debug.Log("loading of assigned keys");
    //    return null;
    //}
    public PlayerAction Fire;
    public PlayerAction Aim;
    public PlayerAction Jump;
    public PlayerAction Sprint;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerAction Crouch;

    public PlayerAction LookLeft;
    public PlayerAction LookRight;
    public PlayerAction LookUp;
    public PlayerAction LookDown;

    public PlayerTwoAxisAction Move;
    public PlayerTwoAxisAction Look;

    public PlayerAction ScoreBoard;
    public static Dictionary<PlayerAction, byte> playerActionDictionary = new Dictionary<PlayerAction, byte>();
    public static Dictionary<BindingSource, byte> bindingSourceDictionary = new Dictionary<BindingSource, byte>();

    public Controls()
    {
        Fire = CreatePlayerAction("Fire");
        playerActionDictionary.Add(Fire, 0);
        Aim = CreatePlayerAction("Aim");
        playerActionDictionary.Add(Aim, 1);

        Jump = CreatePlayerAction("Jump");
        playerActionDictionary.Add(Jump, 2);

        Sprint = CreatePlayerAction("Sprint");
        playerActionDictionary.Add(Sprint, 3);

        Left = CreatePlayerAction("Move Left");
        playerActionDictionary.Add(Left, 4);

        Right = CreatePlayerAction("Move Right");
        playerActionDictionary.Add(Right, 5);

        Up = CreatePlayerAction("Move Up");
        playerActionDictionary.Add(Up, 6);

        Down = CreatePlayerAction("Move Down");
        playerActionDictionary.Add(Down, 7);

        Crouch = CreatePlayerAction("Crouch");
        playerActionDictionary.Add(Crouch, 8);

        LookLeft = CreatePlayerAction("Look Left");
        playerActionDictionary.Add(LookLeft, 9);
        LookRight = CreatePlayerAction("Look Right");
        playerActionDictionary.Add(LookRight, 10);
        LookUp = CreatePlayerAction("Look Up");
        playerActionDictionary.Add(LookUp, 11);
        LookDown = CreatePlayerAction("Look Down");
        playerActionDictionary.Add(LookDown, 12);

        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
        Look = CreateTwoAxisPlayerAction(LookLeft, LookRight, LookDown, LookUp);

        ScoreBoard = CreatePlayerAction("Score Board");
        playerActionDictionary.Add(ScoreBoard, 13);
    }

    public static Controls CreateNewBindings(PlayerAction _pa)
    {
        Controls controls = new Controls();
        try { 
            foreach(BindingSource _bs in _pa.Bindings)
            {
                Debug.Log(_bs.Name.ToString());
                for(byte i = 0; i < playerActionDictionary.Values.Count; i++)
                {
                    Debug.Log(_bs.Name.ToString() + " " + i);
                    if (playerActionDictionary.ContainsValue(i))
                        bindingSourceDictionary.Add(_bs, i);
                }
            }
        }
        catch
        {
            Debug.LogError("No bindings in " + _pa.Name.ToString());
        }
        return null;
    }
    public static Controls CreateWithDefaultBindings()
    {
        Controls controls = new Controls();

        controls.Fire.AddDefaultBinding(InputControlType.RightTrigger);
        controls.Fire.AddDefaultBinding(Mouse.LeftButton);

        controls.Aim.AddDefaultBinding(InputControlType.LeftTrigger);
        controls.Aim.AddDefaultBinding(Mouse.RightButton);

        controls.Jump.AddDefaultBinding(Key.Space);
        controls.Jump.AddDefaultBinding(InputControlType.Action1);

        controls.Sprint.AddDefaultBinding(InputControlType.LeftStickButton);
        controls.Sprint.AddDefaultBinding(Key.LeftShift);

        controls.Up.AddDefaultBinding(Key.UpArrow);
        controls.Down.AddDefaultBinding(Key.DownArrow);
        controls.Left.AddDefaultBinding(Key.LeftArrow);
        controls.Right.AddDefaultBinding(Key.RightArrow);

        controls.Up.AddDefaultBinding(Key.W);
        controls.Down.AddDefaultBinding(Key.S);
        controls.Left.AddDefaultBinding(Key.A);
        controls.Right.AddDefaultBinding(Key.D);

        controls.Crouch.AddDefaultBinding(InputControlType.RightStickButton);
        controls.Crouch.AddDefaultBinding(Key.C);

        controls.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        controls.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        controls.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        controls.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        controls.Left.AddDefaultBinding(InputControlType.DPadLeft);
        controls.Right.AddDefaultBinding(InputControlType.DPadRight);
        controls.Up.AddDefaultBinding(InputControlType.DPadUp);
        controls.Down.AddDefaultBinding(InputControlType.DPadDown);

        controls.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        controls.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
        controls.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
        controls.LookDown.AddDefaultBinding(InputControlType.RightStickDown);

        controls.LookLeft.AddDefaultBinding(Mouse.NegativeX);
        controls.LookRight.AddDefaultBinding(Mouse.PositiveX);
        controls.LookUp.AddDefaultBinding(Mouse.PositiveY);
        controls.LookDown.AddDefaultBinding(Mouse.NegativeY);

        controls.ScoreBoard.AddDefaultBinding(Key.Tab);
        controls.ScoreBoard.AddDefaultBinding(InputControlType.TouchPadButton);
        controls.ScoreBoard.AddDefaultBinding(InputControlType.Command);
        
        controls.ListenOptions.IncludeUnknownControllers = true;
        controls.ListenOptions.MaxAllowedBindings = 4;
        //			playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
        //			playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        //			playerActions.ListenOptions.IncludeMouseButtons = true;

        controls.ListenOptions.OnBindingFound = (action, binding) =>
        {
            if (binding == new KeyBindingSource(Key.Escape))
            {
                action.StopListeningForBinding();
                return false;
            }
            return true;
        };

        controls.ListenOptions.OnBindingAdded += (action, binding) =>
        {
            Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name);
        };

        controls.ListenOptions.OnBindingRejected += (action, binding, reason) =>
        {
            Debug.Log("Binding rejected... " + reason);
        };

        return controls;
    }
}
