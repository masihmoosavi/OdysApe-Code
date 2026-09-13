using Unity.Entities;
using UnityEngine;

namespace Odysape
{
    class TimeManagerMono : MonoBehaviour
    {
        public float accmulator; // sum of Time.deltaTime
        public int currentTick; // number of current tick
        public int currentHour; // number of current hour
        public int currentDay; // number of current day
        public bool isStop; 
        public int gameSpeed; // x1 , x2 , x4
    }

    class TimeManagerMonoBaker : Baker<TimeManagerMono>
    {
        public override void Bake(TimeManagerMono authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new TimeData
            {
                accmulator = authoring.accmulator,
                currentTick = authoring.currentTick,
                currentHour = authoring.currentHour,
                currentDay = authoring.currentDay,
                isStop = authoring.isStop,
                gameSpeed = authoring.gameSpeed,
            });
        }
    }
}
