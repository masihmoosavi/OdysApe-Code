using Unity.Entities;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Odysape
{
    public static class PlacementUtility
    {
        private static int2 landDimensions;

        public static void SetLandDimensions (int2 dimentions)
        {
            landDimensions = dimentions;
        }

        public static bool IsInsideField (int2 ObjectLocation , int2 ObjectSize)
        {
            if (ObjectLocation.x + ObjectSize.x - 1 < landDimensions.x)
            {
                if (ObjectLocation.y + ObjectSize.y - 1 < landDimensions.y)
                {
                    return true;
                }
            }
            return false;
        }

        public static int PositionToIndex(int2 position)
        {
            if (position.x >= 0 && position.x < landDimensions.x && position.y >= 0 && position.y < landDimensions.y)
            {
                int index = position.y * landDimensions.x + position.x;
                return index;
            }
            else
            {
                return -1;
            }
        }

        public static bool CanPlaceObject (DynamicBuffer<PlacementDataElem> buffer,
                                           BufferLookup<PlacedObjectInfoElem> placedObjectsLookup,
                                           int2 objectLocation,
                                           int2 objectSize,
                                           ObjectType ObjectType
                                          )
        {
            for (int j = objectLocation.y; j < objectLocation.y + objectSize.y; j++)
            {
                for (int i = objectLocation.x; i < objectLocation.x + objectSize.x; i++)
                {
                    if (!IsInsideField(objectLocation, objectSize)) return false;
                    int index = j * landDimensions.x + i;
                    PlacementDataElem data = buffer[index];
                    if (data.isEmpty)
                    {
                        continue;
                    }
                    else if (data.isStackable)
                    {
                        DynamicBuffer<PlacedObjectInfoElem> placedObjectsBuffer = placedObjectsLookup[data.placedObjectsInfoEntity];
                        // require more process
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    
        public static bool HasService (DynamicBuffer<PlacementDataElem> buffer,
                                       BufferLookup<ServiceTypeElem> servicesLookup,
                                       int2 startLocation,
                                       int2 areaSize,
                                       ServiceType serviceType
                                      )
        {
            for (int j = startLocation.y; j < startLocation.y + areaSize.y; j++)
            {
                for (int i = startLocation.x; i < startLocation.x + areaSize.x; i++)
                {
                    if (i < landDimensions.x && j < landDimensions.y)
                    {
                        int index = j * landDimensions.x + i;
                        PlacementDataElem data = buffer[index];
                        DynamicBuffer<ServiceTypeElem> services = servicesLookup[data.servicesEntity];
                        foreach (var service in services)
                        {
                            if (service.Value == serviceType)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    
        public static bool BuildObject (DynamicBuffer<PlacementDataElem> buffer,
                                        BufferLookup<PlacedObjectInfoElem> placedObjectsLookup,
                                        bool isStackable,
                                        int objectID,
                                        ObjectType objectType,
                                        int2 objectLocation,
                                        int2 objectSize
                                       )
        {
            for (int j = objectLocation.y; j < objectLocation.y + objectSize.y; j++)
            {
                for (int i = objectLocation.x; i < objectLocation.x + objectSize.x; i++)
                {
                    if (!IsInsideField(objectLocation, objectSize)) return false;
                    int index = j * landDimensions.x + i;
                    PlacementDataElem data = buffer[index];
                    data.isEmpty = false;
                    data.isStackable = isStackable;
                    DynamicBuffer<PlacedObjectInfoElem> placedObjectsBuffer = placedObjectsLookup[data.placedObjectsInfoEntity];
                    placedObjectsBuffer.Add(new PlacedObjectInfoElem
                    {
                        objectID = objectID,
                        objectType = objectType,
                        size = objectSize,
                        location = objectLocation
                    });
                }
            }
            return true;
        }

        public static void RemoveObject (DynamicBuffer<PlacementDataElem> buffer,
                                         BufferLookup<PlacedObjectInfoElem> placedObjectsLookup,
                                         int objectID,
                                         int2 objectLocation,
                                         int2 objectSize)
        {
            for (int j = objectLocation.y; j < objectLocation.y + objectSize.y; j++)
            {
                for (int i = objectLocation.x; i < objectLocation.x + objectSize.x; i++)
                {
                    int index = j * landDimensions.x + i;
                    PlacementDataElem data = buffer[index];
                    data.isStackable = true;
                    DynamicBuffer<PlacedObjectInfoElem> placedObjectsBuffer = placedObjectsLookup[data.placedObjectsInfoEntity];
                    for (int k = 0 ; k < placedObjectsBuffer.Length; k++)
                    {
                        if (placedObjectsBuffer[k].objectID == objectID)
                        {
                            placedObjectsBuffer.RemoveAt(k);
                            break;
                        }
                    }
                    if (placedObjectsBuffer.Length == 0)
                    {
                        data.isEmpty = true;
                    }
                }
            }
        }

        public static bool HasNeighborOfType (DynamicBuffer<PlacementDataElem> buffer,
                                              BufferLookup<PlacedObjectInfoElem> placedObjectsLookup,
                                              int2 objectLocation,
                                              int2 objectSize,
                                              ObjectType neighborType
                                             )
        {
            for (int j = objectLocation.y; j < objectLocation.y + objectSize.y; j++) // left side
            {
                int index = PositionToIndex(new int2 (objectLocation.x - 1 , j));
                if (index != -1)
                {
                    PlacementDataElem data = buffer[index];
                    DynamicBuffer<PlacedObjectInfoElem> placedObjectsInfo = placedObjectsLookup[data.placedObjectsInfoEntity];
                    foreach(var obj in placedObjectsInfo)
                    {
                        if (obj.objectType == neighborType)
                        {
                            return true;
                        }
                    }
                }
            }

            for (int j = objectLocation.y; j < objectLocation.y + objectSize.y; j++) // right side
            {
                int index = PositionToIndex(new int2(objectLocation.x + objectSize.x , j));
                if (index != -1)
                {
                    PlacementDataElem data = buffer[index];
                    DynamicBuffer<PlacedObjectInfoElem> placedObjectsInfo = placedObjectsLookup[data.placedObjectsInfoEntity];
                    foreach (var obj in placedObjectsInfo)
                    {
                        if (obj.objectType == neighborType)
                        {
                            return true;
                        }
                    }
                }
            }

            for (int i = objectLocation.x; i < objectLocation.x + objectSize.x; i++) // down side
            {
                int index = PositionToIndex(new int2(i, objectLocation.y - 1));
                if (index != -1)
                {
                    PlacementDataElem data = buffer[index];
                    DynamicBuffer<PlacedObjectInfoElem> placedObjectsInfo = placedObjectsLookup[data.placedObjectsInfoEntity];
                    foreach (var obj in placedObjectsInfo)
                    {
                        if (obj.objectType == neighborType)
                        {
                            return true;
                        }
                    }
                }
            }

            for (int i = objectLocation.x; i < objectLocation.x + objectSize.x; i++) // up side
            {
                int index = PositionToIndex(new int2(i, objectLocation.y + objectSize.y));
                if (index != -1)
                {
                    PlacementDataElem data = buffer[index];
                    DynamicBuffer<PlacedObjectInfoElem> placedObjectsInfo = placedObjectsLookup[data.placedObjectsInfoEntity];
                    foreach (var obj in placedObjectsInfo)
                    {
                        if (obj.objectType == neighborType)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void AddService (DynamicBuffer<PlacementDataElem> buffer,
                                       BufferLookup<ServiceTypeElem> servicesLookup,
                                       int2 startLocation,
                                       int2 areaSize,
                                       ServiceType serviceType
                                      )
        {
            for (int j = startLocation.y; j < startLocation.y + areaSize.y; j++)
            {
                for (int i = startLocation.x; i < startLocation.x + areaSize.x; i++)
                {
                    if (i < landDimensions.x && j < landDimensions.y)
                    {
                        int index = j * landDimensions.x + i;
                        PlacementDataElem data = buffer[index];
                        DynamicBuffer<ServiceTypeElem> servicesBuffer = servicesLookup[data.servicesEntity];
                        bool hasService = false;
                        foreach (var service in servicesBuffer)
                        {
                            if (service.Value == serviceType)
                            {
                                hasService = true;
                                break;
                            }
                        }
                        if (!hasService)
                        {
                            servicesBuffer.Add(new ServiceTypeElem
                            {
                                Value = serviceType
                            });
                        }
                    }
                }
            }
        }

        public static void RemoveService(DynamicBuffer<PlacementDataElem> buffer,
                                         BufferLookup<ServiceTypeElem> servicesLookup,
                                         int2 startLocation,
                                         int2 areaSize,
                                         ServiceType serviceType
                                        )
        {
            for (int j = startLocation.y; j < startLocation.y + areaSize.y; j++)
            {
                for (int i = startLocation.x; i < startLocation.x + areaSize.x; i++)
                {
                    if (i < landDimensions.x && j < landDimensions.y)
                    {
                        int index = j * landDimensions.x + i;
                        PlacementDataElem data = buffer[index];
                        DynamicBuffer<ServiceTypeElem> servicesBuffer = servicesLookup[data.servicesEntity];
                        for (int k = servicesBuffer.Length - 1 ; k >= 0 ; k--)
                        {
                            if (servicesBuffer[k].Value == serviceType)
                            {
                                servicesBuffer.RemoveAt(k);
                            }
                        }
                    }
                }
            }
        }

        public static bool CanBuild()
        {
            return true;
        }

    }
}
