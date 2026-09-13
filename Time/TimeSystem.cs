using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Odysape
{
    public partial struct TimeSystem : ISystem
    {
        private const float Tick_Time = 0.2f;
        private const int Tick_Per_Hour = 25;
        private const int Tick_Per_Day = 600;
        private const int Max_Tick_Per_Frame = 5;
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var timeManager = SystemAPI.GetSingletonRW<TimeData>();
            if (!timeManager.ValueRO.isStop)
            {
                timeManager.ValueRW.accmulator += SystemAPI.Time.DeltaTime;
                for (int i = 0; i < Max_Tick_Per_Frame; i++)
                {
                    if (timeManager.ValueRO.accmulator >= Tick_Time)
                    {
                        timeManager.ValueRW.accmulator -= Tick_Time;
                        for (int j = 0; j < timeManager.ValueRO.gameSpeed; j++)
                        {
                            timeManager.ValueRW.currentTick++;
                            //OnTick?.Invoke();
                            if (timeManager.ValueRO.currentTick % Tick_Per_Hour == 0 && timeManager.ValueRO.currentTick > 0)
                            {
                                timeManager.ValueRW.currentHour++;
                                //OnNewHour?.Invoke();
                            }
                            if (timeManager.ValueRO.currentTick % Tick_Per_Day == 0 && timeManager.ValueRO.currentTick > 0)
                            {
                                timeManager.ValueRW.currentDay++;
                                //OnNewDay?.Invoke();
                            }
                        }
                    }
                    else break;
                }
            }
            else return;
        }

        public static void GoForwardOneTick(ref TimeData timeManager)
        {
            //var timeManager = SystemAPI.GetSingletonRW<TimeData>();
            timeManager.currentTick++;
            //OnTick?.Invoke();
            if (timeManager.currentTick % Tick_Per_Hour == 0 && timeManager.currentTick > 0)
            {
                timeManager.currentHour++;
                //OnNewHour?.Invoke();
            }
            if (timeManager.currentTick % Tick_Per_Day == 0 && timeManager.currentTick > 0)
            {
                timeManager.currentDay++;
                //OnNewDay?.Invoke();
            }
        }

        public static void GoForwardDays(ref TimeData timeManager , int day)
        {
            int ticks = day * Tick_Per_Day;
            for (int i = 0; i < ticks; i++)
            {
                GoForwardOneTick (ref timeManager);
            }
        }
    }
}
