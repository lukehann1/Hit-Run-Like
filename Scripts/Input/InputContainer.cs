using UnityEngine;

public class InputContainer
{
    public Vector2 leftStick, rightStick;
    public ButtonContainer a_input, b_input, x_input, y_input,
        lb_input, rb_input, lt_input, rt_input,
        la_input, ra_input, start_input, select_input,
        dPadUp_input, dPadDown_input, dPadLeft_input, dPadRight_input;

    public InputContainer()
    {
        a_input = new ButtonContainer();
        b_input = new ButtonContainer();
        x_input = new ButtonContainer();
        y_input = new ButtonContainer();
        lb_input = new ButtonContainer();
        rb_input = new ButtonContainer();
        lt_input = new ButtonContainer();
        rt_input = new ButtonContainer();
        la_input = new ButtonContainer();
        ra_input = new ButtonContainer();
        start_input = new ButtonContainer();
        select_input = new ButtonContainer();
        dPadUp_input = new ButtonContainer();
        dPadDown_input = new ButtonContainer();
        dPadLeft_input = new ButtonContainer();
        dPadRight_input = new ButtonContainer();
    }

    public void ResetInputs()
    {
        a_input.pressed = false;
        b_input.pressed = false;
        x_input.pressed = false;
        y_input.pressed = false;
        lb_input.pressed = false;
        rb_input.pressed = false;
        lt_input.pressed = false;
        rt_input.pressed = false;
        la_input.pressed = false;
        ra_input.pressed = false;
        start_input.pressed = false;
        select_input.pressed = false;
        dPadUp_input.pressed = false;
        dPadDown_input.pressed = false;
        dPadLeft_input.pressed = false;
        dPadRight_input.pressed = false;
    }
}
