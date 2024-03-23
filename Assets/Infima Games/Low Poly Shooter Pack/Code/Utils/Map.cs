public static class Map
{
    public static float Translate(float value, float fromMin,
        float fromMax, float toMin, float toMax)
    {
        var leftSpan = fromMax - fromMin;
        var rightSpan = toMax - toMin;
        var valueScaled = value - fromMin / leftSpan;
        return toMin + (valueScaled * rightSpan);
    }
}