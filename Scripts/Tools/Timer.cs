public class Timer
{
    float value;

    public void StartTimer(float maxtime)
    {
        value = maxtime;
    }

    public void Tick(float delta)
    {
        if (value > 0)
            value -= delta;
        else
            value = 0;
    }

    public bool IsGreaterThatZero()
    {
        return value > 0;
    }
}
