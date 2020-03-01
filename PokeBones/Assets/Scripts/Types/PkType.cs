using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PkType : MonoBehaviour
{
    public string typeName;
    public List<PkType> weakAgainst = new List<PkType>();
    public List<PkType> strongAgainst = new List<PkType>();
}

public class Fire : PkType
{
    public Fire()
    {
        typeName = "Fire";
        strongAgainst.Add(new Grass());
        weakAgainst.Add(new Water());
    }
}

public class Grass : PkType
{
    public Grass()
    {
        typeName = "Grass";
        weakAgainst.Add(new Fire());
        strongAgainst.Add(new Water());
    }
}

public class Water : PkType
{
    public Water()
    {
        typeName = "Water";
        weakAgainst.Add(new Fire());
        strongAgainst.Add(new Grass());
    }
}

public class Normal : PkType
{
    public Normal()
    {
        typeName = "Normal";
    }
}
