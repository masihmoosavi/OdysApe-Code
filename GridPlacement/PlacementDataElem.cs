using System.Runtime.Serialization;
using Unity.Entities;
using Unity.Mathematics;

namespace Odysape
{
    public struct PlacementDataElem : IBufferElementData
    {
        public int3 position;
        public bool isEmpty;
        public bool isStackable; // Is it possible to add another object?
        public Entity placedObjectsInfoEntity;
        public Entity servicesEntity;
    }

    public struct PlacedObjectInfoElem : IBufferElementData
    {
        public int objectID;
        public ObjectType objectType;
        public int2 size;
        public int2 location; // pivotPoint
    }


    public struct ServiceTypeElem : IBufferElementData
    {
        public ServiceType Value;
    }
    
    public enum ObjectType
    {
        // something
    }

    public enum ServiceType
    {
        Electricity,
        Water,        
        Internet,      
        Ventilation
    }
}
