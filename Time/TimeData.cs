using Unity.Entities;

namespace Odysape
{
    public struct TimeData : IComponentData
    {
        public float accmulator; // sum of Time.deltaTime
        public int currentTick; // number of current tick
        public int currentHour; // number of current hour
        public int currentDay; // number of current day
        public bool isStop; 
        public int gameSpeed; // x1 , x2 , x4
    }
}
