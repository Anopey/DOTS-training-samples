﻿using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

unsafe public class RailGeneration : SystemBase
{
    unsafe protected override void OnUpdate()
    {
        var railPrefab = GetSingleton<MetroData>().RailPrefab;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities.ForEach((in PathRef pathdata) =>
        { 
            ref var positionsB = ref pathdata.Data.Value.Positions;
            ref var handlesInB = ref pathdata.Data.Value.HandlesIn;
            ref var handlesOutB = ref pathdata.Data.Value.HandlesOut;
            ref var distancesB = ref pathdata.Data.Value.Distances;
            float totalAbsoluteDistance = pathdata.Data.Value.TotalDistance;
            float absoluteDistance = 0.0f;
            float3 color = pathdata.Data.Value.Colour;

            NativeArray<float3> nativePositions = positionsB.ToNativeArray();
            NativeArray<float3> nativeHandlesIn = handlesInB.ToNativeArray();
            NativeArray<float3> nativeHandlesOut = handlesOutB.ToNativeArray();
            NativeArray<float> nativeDistances = distancesB.ToNativeArray();

            int count = nativePositions.Length;

            while (absoluteDistance < totalAbsoluteDistance)
            {
                float coef = absoluteDistance / totalAbsoluteDistance;

                float3 railPos = BezierHelpers.GetPosition(nativePositions, nativeHandlesIn, nativeHandlesOut, nativeDistances, totalAbsoluteDistance, coef);
                float3 railRot = BezierHelpers.GetNormalAtPosition(nativePositions, nativeHandlesIn, nativeHandlesOut, nativeDistances, totalAbsoluteDistance, coef);

                var railEntity = ecb.Instantiate(railPrefab);
                var railTranslation = new Translation { Value = railPos };
                var railRotation = new Rotation { Value = quaternion.LookRotation(railRot, new float3(0, 1, 0)) };

                var col = new URPMaterialPropertyBaseColor { Value = new float4(color.x, color.y, color.z, 1f) };

                ecb.SetComponent(railEntity, railTranslation);
                ecb.SetComponent(railEntity, railRotation);
                ecb.AddComponent(railPrefab, col);

                absoluteDistance += Globals.RAIL_SPACING;
            }
        }).Run();

        ecb.Playback(EntityManager);

        Enabled = false;
    }
}

namespace MetroECS
{
}