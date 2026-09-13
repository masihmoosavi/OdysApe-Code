using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Odysape
{
    public struct PlacementManager : IComponentData
    {
        public int2 landDimensions;
    }
}
