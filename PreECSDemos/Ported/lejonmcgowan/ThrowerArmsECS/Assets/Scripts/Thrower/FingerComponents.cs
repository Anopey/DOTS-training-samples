﻿
using Unity.Entities;
using Unity.Mathematics;

public struct FingerRenderComponentData: IComponentData
{
    public Entity fingerEntity;
    public int jointIndex;
}

public struct FingerThickness : IComponentData
{
    public float value;
    
    public static implicit operator FingerThickness(float thick) =>  new FingerThickness
    {
        value = thick
    };

    public static implicit operator float(FingerThickness c) => c.value;
}

public struct FingerLength : IComponentData
{
    public float value;
    
    public static implicit operator FingerLength(float len) =>  new FingerLength
    {
        value = len
    };

    public static implicit operator float(FingerLength c) => c.value;
}

public struct FingerGrabbedTag : IComponentData
{
    
}

public struct FingerIndex: IComponentData
{
    public int value;
    
    public static implicit operator FingerIndex(int index) => new FingerIndex
    {
        value = index
    };
    
    public static implicit operator int(FingerIndex c) => c.value;
}

public struct FingerGrabTimer: IComponentData
{
    public float value;
    
    public static implicit operator FingerGrabTimer(float index) => new FingerGrabTimer
    {
        value = index
    };
    
    public static implicit operator float(FingerGrabTimer c) => c.value;
}

public struct FingerParent: IComponentData
{
    public  Entity armParentEntity; //(read-only access to arm’s hand target and GrabT, shared by all fingers) 
    
    public static implicit operator FingerParent(Entity pos) => new FingerParent
    {
        armParentEntity = pos
    };
    
    public static implicit operator Entity(FingerParent c) => c.armParentEntity;
}


public struct FingerJointElementData: IBufferElementData
{
    public float3 value;
    
    public static implicit operator FingerJointElementData(float3 pos) => new FingerJointElementData
    {
        value = pos
    };
    
    public static implicit operator float3(FingerJointElementData c) => c.value;
}


