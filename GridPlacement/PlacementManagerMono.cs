using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Odysape
{
    class PlacementManagerMono : MonoBehaviour
    {
        public Vector2Int landDimensions;
    }

    class PlacementManagerMonoBaker : Baker<PlacementManagerMono>
    {
        public override void Bake(PlacementManagerMono authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlacementManager
            {
                landDimensions = new int2(authoring.landDimensions.x , authoring.landDimensions.y)
            });
            DynamicBuffer<PlacementDataElem> mainBuffer = AddBuffer<PlacementDataElem>(entity);
            for (int y = 0; y < 8; y++) // 8 must be changed
            {
                for (int x = 0; x < 8; x++) // 8 must be changed
                {
                    Entity placedObjectsInfoChildEntity = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddBuffer<PlacedObjectInfoElem>(placedObjectsInfoChildEntity);
                    Entity servicesChildEntity = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddBuffer<ServiceTypeElem>(servicesChildEntity);
                    mainBuffer.Add(new PlacementDataElem
                    {
                        position = new int3(x,0,y),
                        isEmpty = true,
                        isStackable = true,
                        placedObjectsInfoEntity = placedObjectsInfoChildEntity,
                        servicesEntity = servicesChildEntity
                    });
                }
            }
            
        }
    }
}
