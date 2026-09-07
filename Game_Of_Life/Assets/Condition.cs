using UnityEngine;

public class Condition : MonoBehaviour
{
    public Condition() { }
    public virtual bool Test(Agent agent) = 0;
}
