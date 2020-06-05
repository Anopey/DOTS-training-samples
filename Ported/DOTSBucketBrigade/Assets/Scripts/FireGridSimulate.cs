﻿/*
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class FireGridSimulate : SystemBase
{

    // TODO: Rolling sum
    [BurstCompile]
    struct GridAccumulateAxisJob : IJob
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<float> InputCells;
        
        [NativeDisableParallelForRestriction] public NativeArray<float> OutputCells;

        public int2 Dimensions;
        public int SearchSize;
        public float Flashpoint;
        public bool CheckFlashPoint;

        public void Execute()
        {
            int accumBufferSize = (SearchSize * 2) + 1;
            float[] accumBuffer = new float[accumBufferSize];
            int accumBufferIdx = 0;
            float accumulatedTotal;
            
            for (int index = 0; index < InputCells.Length; ++index)
            {
                int cellRowIndex = index / Dimensions.x;
                int cellColumnIndex = index % Dimensions.x;

                int y = cellRowIndex;

                if (cellColumnIndex == 0)
                {
                    accumBufferIdx = 0;
                    for (int i = 0; i < accumBufferSize; ++i)
                    {
                        accumBuffer[i] = 0;
                    }

                    for (int fwdOffset = 0; fwdOffset < SearchSize; ++fwdOffset)
                    {
                        accumBuffer[accumBufferIdx] = InputCells[index + fwdOffset];
                    }
                    
                    
                    accumulatedTotal = 0;
                }
                
                

                
                int newIndex = cellColumnIndex * Dimensions.x + cellRowIndex;
                OutputCells[newIndex] = change;
            }
        }
    }

    [BurstCompile]
    struct GridFinaliseJob : IJobParallelFor
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<float> InputCells;
        
        [NativeDisableParallelForRestriction] public NativeArray<FireGridCell> OutputCells;

        public float TransferRate;

        public void Execute(int index)
        {
            FireGridCell cell = OutputCells[index]; 
            cell.Temperature = math.clamp(cell.Temperature + (InputCells[index] * TransferRate), -1.0f, 1.0f);
            OutputCells[index] = cell;
        }
    }

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<BucketBrigadeConfig>();
    }

    protected override void OnUpdate()
    {
        var config = GetSingleton<BucketBrigadeConfig>();
        int2 dimensions = config.GridDimensions;
        float transfer = config.TemperatureIncreaseRate;
        float flashpoint = config.Flashpoint;
        int searchSize = config.HeatRadius;
        int size = dimensions.x * dimensions.y;

        float delta = Time.DeltaTime;

        var fireGrid = GetSingletonEntity<FireGrid>();
        var cellBufferAsArray = EntityManager.GetBuffer<FireGridCell>(fireGrid).AsNativeArray();
        
        var gridCopy = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        gridCopy.CopyFrom(NativeArrayExtensions.Reinterpret<FireGridCell, float>(cellBufferAsArray));
        
        var accumColumns = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        var accumRows = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        
        // Accumulate columns
        Dependency = new GridAccumulateAxisJob
        {
            InputCells = gridCopy,
            OutputCells = accumColumns,

            Dimensions = dimensions,
            SearchSize = searchSize,
            Flashpoint = flashpoint,
            CheckFlashPoint = true,
        }.Schedule(Dependency);
        
        // Accumulate rows
        Dependency = new GridAccumulateAxisJob
        {
            InputCells = accumColumns,
            OutputCells = accumRows,

            Dimensions = dimensions,
            SearchSize = searchSize,
            Flashpoint = flashpoint,
            CheckFlashPoint = false,
        }.Schedule(Dependency);

        Dependency = new GridFinaliseJob
        {
            InputCells = accumRows,
            OutputCells = cellBufferAsArray,
            
            TransferRate = transfer * delta
        }.Schedule(size, 1000, Dependency);
    }
}
*/