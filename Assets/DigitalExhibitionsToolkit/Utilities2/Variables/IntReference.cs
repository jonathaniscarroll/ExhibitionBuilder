﻿using System;
[Serializable]
public class IntReference
{
	public bool UseConstant = true;
	public int ConstantValue;
	public IntVar Variable;

	public IntReference()
	{ }

	public IntReference(int value)
	{
		UseConstant = true;
		ConstantValue = value;
	}

	public int Value
	{
		get { return UseConstant ? ConstantValue : Variable.Value; }
		set{if(UseConstant)ConstantValue = value; if(!UseConstant)Variable.Value = value;}
	}

	public static implicit operator int(IntReference reference)
	{
		return reference.Value;
	}
}

