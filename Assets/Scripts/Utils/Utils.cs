using System;

public class Utils
{
	public float Remap(float val, float minIn, float maxIn, float minOut, float maxOut)
    {
        return minOut + (val - minIn) * (maxOut - minOut) / (maxIn - minIn);
    }
}