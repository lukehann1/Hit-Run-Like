using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public InputContainer inputContainer = new InputContainer();
    PlayerControls keys;

    public void InputHandlerInit()
    {
        keys = new PlayerControls();
        keys.Enable();

        keys.Player.LeftStick.performed += i => inputContainer.leftStick = i.ReadValue<Vector2>();
        keys.Player.RightStick.performed += i => inputContainer.rightStick = i.ReadValue<Vector2>();
        keys.Player.A.started += i => inputContainer.a_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.B.started += i => inputContainer.b_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.X.started += i => inputContainer.x_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.Y.started += i => inputContainer.y_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.LB.started += i => inputContainer.lb_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.RB.started += i => inputContainer.rb_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.LT.started += i => inputContainer.lt_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.RT.started += i => inputContainer.rt_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.LA.started += i => inputContainer.la_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.RA.started += i => inputContainer.ra_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.Start.started += i => inputContainer.start_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.Select.started += i => inputContainer.select_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.DPadUp.started += i => inputContainer.dPadUp_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.DPadDown.started += i => inputContainer.dPadDown_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.DPadLeft.started += i => inputContainer.dPadLeft_input.pressed = i.ReadValue<float>() > 0 ? true : false;
        keys.Player.DPadRight.started += i => inputContainer.dPadRight_input.pressed = i.ReadValue<float>() > 0 ? true : false;
    }

    public void InputHandlerUpdate()
    {
        inputContainer.a_input.held = GetButtonHeldStatus(keys.Player.A.phase);
        inputContainer.b_input.held = GetButtonHeldStatus(keys.Player.B.phase);
        inputContainer.x_input.held = GetButtonHeldStatus(keys.Player.X.phase);
        inputContainer.y_input.held = GetButtonHeldStatus(keys.Player.Y.phase);
        inputContainer.lb_input.held = GetButtonHeldStatus(keys.Player.LB.phase);
        inputContainer.rb_input.held = GetButtonHeldStatus(keys.Player.RB.phase);
        inputContainer.lt_input.held = GetButtonHeldStatus(keys.Player.LT.phase);
        inputContainer.rt_input.held = GetButtonHeldStatus(keys.Player.RT.phase);

        // Uncomment to test input
        //TestAnalogSticks();
        //TestButtonPressed();
        //TestButtonHeld();
    }

    public void InputHandlerLateUpdate()
    {
        inputContainer.ResetInputs();
    }

    bool GetButtonHeldStatus(InputActionPhase phase)
    {
        return phase == InputActionPhase.Performed;
    }

    void TestAnalogSticks()
    {
        Debug.Log("Left Stick x: " + inputContainer.leftStick.x + " y: " + inputContainer.leftStick.y);
        Debug.Log("Right Stick x: " + inputContainer.rightStick.x + " y: " + inputContainer.rightStick.y);
    }

    void TestButtonPressed()
    {
        if(inputContainer.a_input.pressed)
            Debug.Log("A Pressed");
        if (inputContainer.b_input.pressed)
            Debug.Log("B Pressed");
        if (inputContainer.x_input.pressed)
            Debug.Log("X Pressed");
        if (inputContainer.y_input.pressed)
            Debug.Log("Y Pressed");
        if (inputContainer.lb_input.pressed)
            Debug.Log("LB Pressed");
        if (inputContainer.rb_input.pressed)
            Debug.Log("RB Pressed");
        if (inputContainer.lt_input.pressed)
            Debug.Log("LT Pressed");
        if (inputContainer.rt_input.pressed)
            Debug.Log("RT Pressed");
        if (inputContainer.la_input.pressed)
            Debug.Log("LA Pressed");
        if (inputContainer.ra_input.pressed)
            Debug.Log("RA Pressed");
        if (inputContainer.start_input.pressed)
            Debug.Log("Start Pressed");
        if (inputContainer.select_input.pressed)
            Debug.Log("Select Pressed");
        if (inputContainer.dPadUp_input.pressed)
            Debug.Log("Up Pressed");
        if (inputContainer.dPadDown_input.pressed)
            Debug.Log("Down Pressed");
        if (inputContainer.dPadLeft_input.pressed)
            Debug.Log("Left Pressed");
        if (inputContainer.dPadRight_input.pressed)
            Debug.Log("Right Pressed");
    }

    void TestButtonHeld()
    {
        if (inputContainer.a_input.held)
            Debug.Log("A Held");
        if (inputContainer.b_input.held)
            Debug.Log("B Held");
        if (inputContainer.x_input.held)
            Debug.Log("X Held");
        if (inputContainer.y_input.held)
            Debug.Log("Y Held");
        if (inputContainer.lb_input.held)
            Debug.Log("LB Held");
        if (inputContainer.rb_input.held)
            Debug.Log("RB Held");
        if (inputContainer.lt_input.held)
            Debug.Log("LT Held");
        if (inputContainer.rt_input.held)
            Debug.Log("RT Held");
    }
}
