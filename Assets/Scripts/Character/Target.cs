using UnityEngine;

public enum TargetTypeEnum
{
    Head,
    Trunk,
    Arm,
    Leg
}

public class Target : MonoBehaviour
{
    public TargetTypeEnum targetType;
}