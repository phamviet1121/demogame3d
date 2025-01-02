#define PAC_SYSTEM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orbitami.PacSystem
{
    public class PointAndClickSystem : MonoBehaviour
    {
        [Tooltip("This will display or hide the PAC System Control Panel. The control panel contains the functionality when placing objects in " +
           "the scene. To create objects in the scene use the key combination of shift and right mouse button.")]
        public bool showPACSystemControlPanel;
        [Tooltip("When this is checked, the system will automatically add a collider to your prefab/s during creation. If the prefab doesn't have a mesh the system will create " +
            "a box collider for the prefab. In order to utilize the PAC System click to delete feature, the prefab must have a collider on the parent object.")]
        public bool systemAddsCollidersAtCreation;
        public bool avoidCollisions;
        public bool parentToSurface;
        [HideInInspector]
        public SystemState systemMode;
        
        [Tooltip("The notification sound can be modified. If you have a different sound you would like to use then just assign the sound you want " +
            "to use here in this reference.")]
        public AudioClip notificationSound;
        public Transform surface;
        
        //[Tooltip("The terrain is ONLY assigned when the option 'My World Uses A Terrain' is set to 'Yes' in the PAC Control Panel.")]
        //public Terrain terrain;
        //[HideInInspector]
        //public TerrainData terrainData;
        [HideInInspector]
        public float seaLevel;
        
        
        [HideInInspector]        
        public float placementRadius;
        [HideInInspector]
        public float minimumNeighborDistance;
        [HideInInspector]
        public bool autoAdjustMinNeighborDistance;

        [HideInInspector]
        public bool editAllUseItem;
        [HideInInspector]
        public int editAllCreateAmount;
        [HideInInspector]
        public bool editAllRandomScale;
        [HideInInspector]
        public float editAllRandomScaleMin;
        [HideInInspector]
        public float editAllRandomScaleMax;
        [HideInInspector]
        public CreateRotationMode editAllRotationMode;
        [HideInInspector]
        public float editAllMinRotation;
        [HideInInspector]
        public float editAllMaxRotation;
        [HideInInspector]
        public CreateHeightMode editAllHeightMode;
        [HideInInspector]
        public CreateSeaLevelMode editAllSeaLevelMode;
        [HideInInspector]
        public float editAllStaticHeightFromSurface;
        [HideInInspector]
        public float editAllSurfaceRangeMinHeight;
        [HideInInspector]
        public float editAllSurfaceRangeMaxHeight;
        [HideInInspector]
        public float editAllWorldSpaceMinHeight;
        [HideInInspector]
        public float editAllWorldSpaceMaxHeight;        

        [HideInInspector]
        public int category_index;
        [HideInInspector]
        public string[] categories;
        [HideInInspector]
        public int arraySize;

        public SystemDatabase[] PACSystemPrefabCategories;
        //public System.Type[] types = {typeof(PrefabData), typeof(ActiveItemData) };
        
        [Header("Developer Options & Object Pools- Scene UI Element Settings")]
        [HideInInspector]
        public bool displaySceneWindow;
        [HideInInspector]
        public bool clearAll;
        [HideInInspector]
        public bool displayNotificationWindow;
        [HideInInspector]
        public bool displayRemoveAllWarningWindow;
        [HideInInspector]
        public string system_header_message;
        [HideInInspector]
        public string system_sub_message;
        [HideInInspector]
        public string notification_header_message;
        [HideInInspector]
        public string notification_sub_message;
        [HideInInspector]
        public List<ActiveItemData> current_active_data = new List<ActiveItemData>();
        [HideInInspector]
        public PrefabData[] current_prefab_data;
        
        [HideInInspector]
        public GameObject item;
        [HideInInspector]
        public Vector3 hit_normal;
        [HideInInspector]
        public Vector3 final_position;        
        [HideInInspector]
        public float toolButtonWidth;
        [HideInInspector]
        public float toolButtonHeight;
        [HideInInspector]
        public bool terrainUpToDate;        
        [HideInInspector]
        public string[] defines;
        [HideInInspector]
        public string systemDefine;        

        //This method can be used to fire a raycast from your player or origin source.
        //This method can be called from any of your other scripts as long as you implement 'using Orbitami.PacSystem' at the top of your script
        public static void FireRaycast(Vector3 ray_origin, Vector3 direction, float distance)
        {
            Debug.DrawRay(ray_origin, direction * distance, Color.green, 0.5f);
            
            if (Physics.Raycast(ray_origin, direction, out RaycastHit hit, distance))
            {
                //Check to see if we have a null result when attempting to grab the ItemInfo component from the transform hit by the raycast
                if (hit.transform.GetComponent<ItemInfo>())
                {
                    Debug.Log("The object hit by the raycast contained a ItemInfo component. The system will now scan for the active item.");
                    
                    //Create and set a reference to the ItemInfo component.
                    ItemInfo info = hit.transform.GetComponent<ItemInfo>();

                    //Scan the PAC System active scene object pools based on the information from the ItemInfo component.
                    ScanActiveSceneItems(hit.transform.gameObject, info.itemCategory);                    
                }
                else 
                {
                    Debug.Log("The raycast did not hit an item that has the ItemInfo component attached to it.");
                }
            }
        }

        //Fire a raycast from your player object or origin source. You can use the FireRaycast() method above or implement your own.
        //Grab the <ItemInfo> component attached to the active object in the scene.
        //Obtain the (item_type) parameter info from the <ItemInfo> component you obtained from the raycast
        //Call this method and pass in the (gameobject the raycast hit) and the data for the (item_type).
        public static void ScanActiveSceneItems(GameObject item, string item_category)
        {
            //Determine the item type from the passed in parameter.
            Debug.Log("Determining scan type: " + item + " " + item_category);

            //Locate the existing reference to the PAC System in the scene by finding it using the object type.
            PointAndClickSystem PAC = FindObjectOfType<PointAndClickSystem>();

            if (PAC != null)
            {
                //Scan the appropriate PAC System object pool to locate the item in the object pool array.
                PAC.ValidateItem(item, item_category);
            }
            else
            {
                Debug.LogError("The PAC System could not find the PAC System game object in the scene. Did it accidentally get removed?");
            }
        }
        public void ValidateItem(GameObject item, string item_category)
        {
            Debug.Log("Validating " + PACSystemPrefabCategories[category_index].categoryName + " item.");
            
            //Iterate trough the PAC System active nature terrain array to look for the detected 'item'.
            for (int i = 0; i < PACSystemPrefabCategories[category_index].objectPool.Count; i++)
            {
                //Check to see if the current PAC System active nature terrain index equals the 'item'.
                if(PACSystemPrefabCategories[category_index].objectPool[i].activeItem == item)
                {
                    Debug.Log("The item: " + item.name + " was found in the active creation category object pool.");

                    //THE ACTIVE SCENE OBJECT HIT BY THE RAYCAST WAS FOUND IN THE PAC SYSTEM ACTIVE OBJECT POOLS.
                    //IMPLEMENT YOUR LOGIC HERE.
                }
            }
        }
        
        //public void OnValidate()
        //{
        //    
        //}
    }

    [System.Serializable]
    public class PrefabData
    {
        [Header("Please give the item a general display name.")]
        public string itemName;
        [Header("Assign the prefab here.")]
        public GameObject prefab;
        [Header("Are you using this item for creation?")]
        public bool useItem;
        [Header("How many of this item do you want to create?")]
        public int createAmount;
        [Header("When created do you want the scale to be randomized?")]
        public bool randomScale;
        [Header("When using a random scale what should the minimum scale be?")]
        public float minScale;
        [Header("When using a random scale what should the maximum scale be?")]
        public float maxScale;        
        [Header("When created what kind of rotation should it have?")]
        public CreateRotationMode rotationMode;
        [Header("When created what is the minimum rotation when using a rotation mode?")]
        public float minRotation;
        [Header("When created what is the maximum rotation when using a rotation mode?")]
        public float maxRotation;
        [Header("When created what type of height mode will be used?")]
        public CreateHeightMode heightMode;
        [Header("When created what type of sea level mode will be used?")]
        public CreateSeaLevelMode seaLevelMode;
        [Header("This is for the static height from surface mode.")]
        public float staticCreateHeight;
        [Header("This is for the height range from surface height mode.")]
        public float surfaceRangeMinHeight;
        [Header("This is for the height range from surface height mode.")]
        public float surfaceRangeMaxHeight;
        [Header("This is ONLY set when using height range height mode.")]
        public float worldSpaceMinHeight;
        [Header("This is ONLY set when using height range height mode.")]
        public float worldSpaceMaxHeight;
    }
    [System.Serializable]
    public class ActiveItemData
    {
        public GameObject activeItem;
        public Vector3 activeItemPosition;
    }    
    [System.Serializable]
    public class SystemDatabase
    {        
        public string categoryName;
        public PrefabData[] prefabs;        
        [HideInInspector]
        public List<ActiveItemData> objectPool = new List<ActiveItemData>();
    }
    public enum SystemState
    {
        Idle,
        CreateMode        
    }
    public enum CreateRotationMode
    {
        None,
        AlignWithSurfaceOnly,
        RandomX,
        RandomYWithSurfaceAlignment,
        RandomY,
        RandomZ,
        RandomXAndY,
        RandomXAndZ,
        RandomYAndZ
    }
    public enum CreateHeightMode
    {
        AtPointAndClick,
        StaticHeightFromSurface,
        HeightRangeFromSurface,
        WorldSpaceHeightRange
    }
    public enum CreateSeaLevelMode
    {
        AboveSeaLevelOnly,
        BelowSeaLevelOnly,
        AboveAndBelowSeaLevel
    }
}