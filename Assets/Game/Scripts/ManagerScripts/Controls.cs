using UnityEngine;
using System.Collections;
using InControl;

public class Controls : PlayerActionSet
{
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


    public Controls()
    {
        Fire = CreatePlayerAction("Fire");
        Aim = CreatePlayerAction("Aim");
        Jump = CreatePlayerAction("Jump");
        Sprint = CreatePlayerAction("Sprint");
        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");
        Crouch = CreatePlayerAction("Crouch");

        LookLeft = CreatePlayerAction("Look Left");
        LookRight = CreatePlayerAction("Look Right");
        LookUp = CreatePlayerAction("Look Up");
        LookDown = CreatePlayerAction("Look Down");

        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
        Look = CreateTwoAxisPlayerAction(LookLeft, LookRight, LookDown, LookUp);
    }


    public static Controls CreateWithDefaultBindings()
    {
        var controls = new Controls();

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

        controls.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        controls.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
        controls.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
        controls.LookDown.AddDefaultBinding(InputControlType.RightStickDown);

        controls.Left.AddDefaultBinding(InputControlType.DPadLeft);
        controls.Right.AddDefaultBinding(InputControlType.DPadRight);
        controls.Up.AddDefaultBinding(InputControlType.DPadUp);
        controls.Down.AddDefaultBinding(InputControlType.DPadDown);

        controls.LookUp.AddDefaultBinding(Mouse.PositiveY);
        controls.LookDown.AddDefaultBinding(Mouse.NegativeY);
        controls.LookLeft.AddDefaultBinding(Mouse.NegativeX);
        controls.LookRight.AddDefaultBinding(Mouse.PositiveX);

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
