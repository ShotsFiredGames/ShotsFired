using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSetting : MonoBehaviour
{
    public static Controls controls;

    void Awake()
    {
        controls = Controls.CreateWithDefaultBindings();
    }

    public static Controls GetControls()
    {
        if (controls == null)
            controls = Controls.CreateWithDefaultBindings();

        return controls;
    }

    public void RebindAControl(string actionName)
    {
        foreach (var action in controls.Actions)
        {
            if (action.Name.Equals(actionName))
            {
                print(action.Name);
                action.ListenForBinding();
            }
        }
    }
}
