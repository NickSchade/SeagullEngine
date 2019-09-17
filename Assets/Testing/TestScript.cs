using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClass
{
    string GetString();
}
public class Class1 : IClass
{
    public virtual string GetString()
    {
        return "from class1";
    }
}
public class Class2 : Class1, IClass
{
    public override string GetString()
    {
        return "from class2";
    }
}
public class Class3 : Class2, IClass
{
    public override string GetString()
    {
        return base.GetString() +"from class3";
    }
}
