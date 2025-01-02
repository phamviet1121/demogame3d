using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Orbitami.PacSystem
{
    [CustomEditor(typeof(PointAndClickSystem))]
    public class PointAndClickCreateEditor : Editor
    {
        private PointAndClickSystem PAC_SYSTEM;
        public GUIStyle buttonStyleNatureTerrain;
        public GUIStyle buttonStyleNatureWater;
        public GUIStyle buttonStyleNatureSky;
        public GUIStyle buttonStyleStructureTerrain;
        public GUIStyle buttonStyleStructureWater;
        public GUIStyle buttonStyleStructureSky;
        public GUIStyle header_style;
        private CreateRotationMode rotationType = CreateRotationMode.None;
        private static Texture noTerrain;
        private static Texture systemBanner;
        public static Texture2D toggleUseAll_Normal_NatureTerrain;
        public static Texture2D toggleUseAll_Hover_NatureTerrain;
        public static Texture2D toggleUseAll_Normal_NatureWater;
        public static Texture2D toggleUseAll_Hover_NatureWater;
        public static Texture2D toggleUseAll_Normal_NatureSky;
        public static Texture2D toggleUseAll_Hover_NatureSky;
        public static Texture2D toggleUseAll_Normal_StructureTerrain;
        public static Texture2D toggleUseAll_Hover_StructureTerrain;
        public static Texture2D toggleUseAll_Normal_StructureWater;
        public static Texture2D toggleUseAll_Hover_StructureWater;
        public static Texture2D toggleUseAll_Normal_StructureSky;
        public static Texture2D toggleUseAll_Hover_StructureSky;
        public RaycastHit creation_hit_info;
        public Vector3 ray_position;
        public RaycastHit click_event_ray_hit;
        public bool validCollisionCheck;
        public bool validCreationPosition;
        public bool mass_creation;
        public int create_amount;        
        
        [MenuItem("Assets/Orbitami Entertainment/PAC System/Add To Scene")]
        private static void AddPACSystemToScene()
        {
            PointAndClickSystem system_in_scene = FindObjectOfType<PointAndClickSystem>();

            if (!system_in_scene)
            {
                GameObject new_system = new GameObject("PAC System");
                new_system.AddComponent<PointAndClickSystem>();
            }
            else 
            {
                PACSystemEditorAudioPlayback.PlaySound(system_in_scene.notificationSound);
                system_in_scene.notification_header_message = "PAC SYSTEM ALREADY IN THE SCENE";
                system_in_scene.notification_sub_message = "You are attempting to add the PAC System to the scene, but there " +
                    "is already a PAC System in the scene.";
                system_in_scene.displayNotificationWindow = true;
            }
        }

        public override void OnInspectorGUI()
        {
            if (SystemInScene())
            {
                if (PAC_SYSTEM == null)
                {
                    PAC_SYSTEM = (PointAndClickSystem)target;                    
                    
                    if(PAC_SYSTEM.PACSystemPrefabCategories != null)
                    {
                        PAC_SYSTEM.arraySize = PAC_SYSTEM.PACSystemPrefabCategories.Length;
                    }
                }
                else 
                {
                    //This checks the local system defines for Unity and if the PAC System define symbol is not located there the system will add it.
                    CheckDefines();
                    
                    CheckAudioReferences();

                    CheckSurfaceSetting();
                    
                    //This locates and sets the reference to the images needed to display in the inspector.
                    ReferenceImagesInspector();

                    //This displays the appropriate inspector images after they have been referenced.
                    DisplayInspectorImages();
                    
                    //This is to check for changes the user may have made to the active scene objects, such as, deleting an object from the scene
                    CheckForChangesInActiveSceneObjects();

                    //This checks the auto spacing setting
                    CheckAutoSpacing();

                    CheckPrefabArrayCount();                    
                }
            }
            
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            //This checks the PAC System reference and sets it if null
            if (PAC_SYSTEM == null)
            {
                PAC_SYSTEM = (PointAndClickSystem)target;
            }
            
            //This sets the tool button dimensions to the defined settings.
            SetToolButtonDimensions();
            
            //This applies the pertaining button style to the tool button.
            SetToolButtonStyle();
            
            //This locates and sets the reference to the images needed for the scene view.
            ReferenceImagesScene();
            
            //This checks for the click event that comes from the user clicking Shift + Right Mouse button in the scene.
            CheckClickEvent();

            #region PAC Windows Settings
            if(PAC_SYSTEM.showPACSystemControlPanel)
            {
                GUI.backgroundColor = new Color(0f, 0f, 0f, 0.95f);
                Rect windowRect = new Rect(10, 25, 385, 500);
                windowRect = GUILayout.Window(9999, windowRect, DisplayControlPanel, "PAC CONTROL PANEL");                
            }            

            if(PAC_SYSTEM.displaySceneWindow)
            {
                float x = (Screen.width - 500) / 2;
                float y = (Screen.height - 175) / 2;
                Rect windowRect_scene = new Rect(x, y, 500, 125);
                windowRect_scene = GUILayout.Window(9998, windowRect_scene, DisplaySystemWindow, "PAC SYSTEM MESSAGE - ISSUE FOUND");
            }

            if (PAC_SYSTEM.displayNotificationWindow)
            {
                float x = (Screen.width - 500) / 2;
                float y = (Screen.height - 175) / 2;
                Rect windowRect_notification = new Rect(x, y, 500, 125);
                windowRect_notification = GUILayout.Window(9997, windowRect_notification, DisplayNotificationWindow, "PAC SYSTEM NOTIFICATION");
            }

            if (PAC_SYSTEM.displayRemoveAllWarningWindow)
            {
                float x = (Screen.width - 500) / 2;
                float y = (Screen.height - 175) / 2;
                Rect windowRect_warning = new Rect(x, y, 500, 125);
                windowRect_warning = GUILayout.Window(9996, windowRect_warning, DisplayWarningWindow, "PAC SYSTEM WARNING");
            }

            #region Optional Scene Elements Without Window
            //Handles.BeginGUI();            
            //GUILayout.BeginArea(new Rect(5, 25, 700, 700));
            //GUILayout.BeginVertical();
            ////Optional Scene Elements
            //GUILayout.EndVertical();
            //GUILayout.EndArea();
            //Handles.EndGUI();
            #endregion
            #endregion
        }

        #region OnScene Methods
        private void SetToolButtonDimensions()
        {
            if (PAC_SYSTEM.toolButtonWidth != 55f)
            {
                PAC_SYSTEM.toolButtonWidth = 55f;
            }

            if (PAC_SYSTEM.toolButtonHeight != 55f)
            {
                PAC_SYSTEM.toolButtonHeight = 55f;
            }
        }
        private void SetToolButtonStyle()
        {
            buttonStyleNatureTerrain = new GUIStyle(GUI.skin.button);
            buttonStyleNatureTerrain.alignment = TextAnchor.UpperLeft;
            buttonStyleNatureTerrain.normal.background = toggleUseAll_Normal_NatureTerrain;
            buttonStyleNatureTerrain.normal.textColor = Color.white;
            buttonStyleNatureTerrain.hover.background = toggleUseAll_Hover_NatureTerrain;
            buttonStyleNatureTerrain.fontSize = 11;
            buttonStyleNatureTerrain.fixedWidth = PAC_SYSTEM.toolButtonWidth;
            buttonStyleNatureTerrain.fixedHeight = PAC_SYSTEM.toolButtonHeight;

            buttonStyleNatureWater = new GUIStyle(GUI.skin.button);
            buttonStyleNatureWater.alignment = TextAnchor.UpperLeft;
            buttonStyleNatureWater.normal.background = toggleUseAll_Normal_NatureWater;
            buttonStyleNatureWater.normal.textColor = Color.white;
            buttonStyleNatureWater.hover.background = toggleUseAll_Hover_NatureWater;
            buttonStyleNatureWater.fontSize = 11;
            buttonStyleNatureWater.fixedWidth = PAC_SYSTEM.toolButtonWidth;
            buttonStyleNatureWater.fixedHeight = PAC_SYSTEM.toolButtonHeight;

            buttonStyleNatureSky = new GUIStyle(GUI.skin.button);
            buttonStyleNatureSky.alignment = TextAnchor.UpperLeft;
            buttonStyleNatureSky.normal.background = toggleUseAll_Normal_NatureSky;
            buttonStyleNatureSky.normal.textColor = Color.white;
            buttonStyleNatureSky.hover.background = toggleUseAll_Hover_NatureSky;
            buttonStyleNatureSky.fontSize = 11;
            buttonStyleNatureSky.fixedWidth = PAC_SYSTEM.toolButtonWidth;
            buttonStyleNatureSky.fixedHeight = PAC_SYSTEM.toolButtonHeight;

            buttonStyleStructureTerrain = new GUIStyle(GUI.skin.button);
            buttonStyleStructureTerrain.alignment = TextAnchor.UpperLeft;
            buttonStyleStructureTerrain.normal.background = toggleUseAll_Normal_StructureTerrain;
            buttonStyleStructureTerrain.normal.textColor = Color.white;
            buttonStyleStructureTerrain.hover.background = toggleUseAll_Hover_StructureTerrain;
            buttonStyleStructureTerrain.fontSize = 11;
            buttonStyleStructureTerrain.fixedWidth = PAC_SYSTEM.toolButtonWidth;
            buttonStyleStructureTerrain.fixedHeight = PAC_SYSTEM.toolButtonHeight;

            buttonStyleStructureWater = new GUIStyle(GUI.skin.button);
            buttonStyleStructureWater.alignment = TextAnchor.UpperLeft;
            buttonStyleStructureWater.normal.background = toggleUseAll_Normal_StructureWater;
            buttonStyleStructureWater.normal.textColor = Color.white;
            buttonStyleStructureWater.hover.background = toggleUseAll_Hover_StructureWater;
            buttonStyleStructureWater.fontSize = 11;
            buttonStyleStructureWater.fixedWidth = PAC_SYSTEM.toolButtonWidth;
            buttonStyleStructureWater.fixedHeight = PAC_SYSTEM.toolButtonHeight;

            buttonStyleStructureSky = new GUIStyle(GUI.skin.button);
            buttonStyleStructureSky.alignment = TextAnchor.UpperLeft;
            buttonStyleStructureSky.normal.background = toggleUseAll_Normal_StructureSky;
            buttonStyleStructureSky.normal.textColor = Color.white;
            buttonStyleStructureSky.hover.background = toggleUseAll_Hover_StructureSky;
            buttonStyleStructureSky.fontSize = 11;
            buttonStyleStructureSky.fixedWidth = PAC_SYSTEM.toolButtonWidth;
            buttonStyleStructureSky.fixedHeight = PAC_SYSTEM.toolButtonHeight;
        }
        private void ReferenceImagesScene()
        {
            if (toggleUseAll_Normal_NatureTerrain == null)
                toggleUseAll_Normal_NatureTerrain = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Normal_NatureTerrain.png", typeof(Texture2D));
            
            if (toggleUseAll_Hover_NatureTerrain == null)
                toggleUseAll_Hover_NatureTerrain = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Hover_NatureTerrain.png", typeof(Texture2D));
            
            if (toggleUseAll_Normal_NatureWater == null)
                toggleUseAll_Normal_NatureWater = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Normal_NatureWater.png", typeof(Texture2D));
            
            if (toggleUseAll_Hover_NatureWater == null)
                toggleUseAll_Hover_NatureWater = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Hover_NatureWater.png", typeof(Texture2D));
            
            if (toggleUseAll_Normal_NatureSky == null)
                toggleUseAll_Normal_NatureSky = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Normal_NatureSky.png", typeof(Texture2D));
            
            if (toggleUseAll_Hover_NatureSky == null)
                toggleUseAll_Hover_NatureSky = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Hover_NatureSky.png", typeof(Texture2D));
            
            if (toggleUseAll_Normal_StructureTerrain == null)
                toggleUseAll_Normal_StructureTerrain = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Normal_StructureTerrain.png", typeof(Texture2D));
            
            if (toggleUseAll_Hover_StructureTerrain == null)
                toggleUseAll_Hover_StructureTerrain = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Hover_StructureTerrain.png", typeof(Texture2D));
            
            if (toggleUseAll_Normal_StructureWater == null)
                toggleUseAll_Normal_StructureWater = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Normal_StructureWater.png", typeof(Texture2D));
            if (toggleUseAll_Hover_StructureWater == null)
                toggleUseAll_Hover_StructureWater = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Hover_StructureWater.png", typeof(Texture2D));
            
            if (toggleUseAll_Normal_StructureSky == null)
                toggleUseAll_Normal_StructureSky = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Normal_StructureSky.png", typeof(Texture2D));
            if (toggleUseAll_Hover_StructureSky == null)
                toggleUseAll_Hover_StructureSky = (Texture2D)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/SceneWindowImages/PACSystem_Hover_StructureSky.png", typeof(Texture2D));
        }        

        //Create item logic methods
        private void CheckClickEvent()
        {
            Event e = Event.current;

            //OPTIMIZE BY USING PrefabData[] prefab_data = null;

            if (e.type == EventType.MouseDown && e.button == 1 && e.shift)
            {                
                if (PAC_SYSTEM.systemMode == SystemState.Idle)
                {
                    PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                    PAC_SYSTEM.notification_header_message = "INCORRECT SYSTEM MODE";
                    PAC_SYSTEM.notification_sub_message = "Please first set the system mode to 'Create Mode' first before attempting to place an item.";
                    PAC_SYSTEM.displayNotificationWindow = true;
                    return;
                }
                if (PAC_SYSTEM.avoidCollisions == true)
                {
                    if(PAC_SYSTEM.surface == null)
                    {
                        PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                        PAC_SYSTEM.notification_header_message = "USING AVOID COLLISIONS AND NO SURFACE ASSIGNED";
                        PAC_SYSTEM.notification_sub_message = "Please make sure you have assigned a surface in the inspector for the PAC System if you have " +
                            "chosen to use the 'Avoid Collisions' option.";
                        PAC_SYSTEM.displayNotificationWindow = true;
                        return;
                    }                    
                }
                if (PAC_SYSTEM.systemMode == SystemState.CreateMode)
                {
                    if(PAC_SYSTEM.PACSystemPrefabCategories != null)
                    {
                        if (PAC_SYSTEM.PACSystemPrefabCategories.Length <= 0)
                        {
                            PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                            PAC_SYSTEM.notification_header_message = "NO SYSTEM PREFABS DEFINED";
                            PAC_SYSTEM.notification_sub_message = "The system detected the system mode 'Create Mode' is active, however, you must first assign " +
                                "prefabs in the inspector for the PAC System before you can place any objects in the scene.";
                            PAC_SYSTEM.displayNotificationWindow = true;
                            return;
                        }
                    }

                    Ray worldRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                    if (Physics.Raycast(worldRay, out RaycastHit hitInfo, Mathf.Infinity))
                    {
                        Debug.Log("Raycast Hit Object: " + hitInfo.transform.name);
                        GameObject detected_object = hitInfo.transform.gameObject;
                        Transform parent = detected_object.transform.parent;
                        Vector3 click_event_hit_point = hitInfo.point;
                        click_event_ray_hit = hitInfo;

                        //Delete mode - If the object is found in the active arrays it will be removed from the array and the scene
                        if (detected_object != null)
                        {
                            //Debug.Log("Detected object found: " + detected_object.name);

                            if (parent != null)
                            {
                                Debug.Log("Parent object detected: " + parent.name);

                                if(parent.name != "PAC System - " + PAC_SYSTEM.categories[PAC_SYSTEM.category_index])
                                {
                                    if (!SystemDeleteObject(parent.gameObject))
                                    {
                                        //Debug.Log("Calling create active prefab type from detected parent.");
                                        CreateActivePrefabType(click_event_hit_point, hitInfo.normal);
                                    }
                                }
                                else if (!SystemDeleteObject(detected_object))
                                {
                                    //Debug.Log("Calling create active prefab type from detected object and not parent.");
                                    CreateActivePrefabType(click_event_hit_point, hitInfo.normal);
                                }
                            }
                            else if (!SystemDeleteObject(detected_object))
                            {
                                //Debug.Log("Calling create active prefab type from detected object and not parent.");
                                CreateActivePrefabType(click_event_hit_point, hitInfo.normal);
                            }
                        }

                        e.Use();
                    }
                }
            }
        }                
        private void CreateActivePrefabType(Vector3 click_event_hit_point, Vector3 click_event_normal)
        {
            //Debug.Log("Inside: CreateActivePrefabType()");
            bool item_defined = false;            
            int sealevel_error_count = 0;
            int neighbor_error_count = 0;
            int using_item_count = 0;
            int invalid_collision_count = 0;
            int invalid_creation_raycast_count = 0;
            int successful_create_amount = 0;

            for (int i = 0; i < GetPrefabArray().Length; i++)
            {
                int prefab_index = i;

                if (GetPrefabArray()[prefab_index].useItem)
                {
                    item_defined = true;
                    using_item_count++;
                    
                    create_amount = GetPrefabArray()[i].createAmount;
                    //Debug.Log("Using Prefab: " + GetPrefabArray()[prefab_index].itemName);

                    //Check the creation amount to ensure that an amount has been set by the user.
                    //If there is not an amount defined the system will inform the user of this error.
                    if (create_amount <= 0)
                    {
                        PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                        PAC_SYSTEM.notification_header_message = "CREATE AMOUNT LESS THAN OR EQUAL TO ZERO";
                        PAC_SYSTEM.notification_sub_message = "Please define a create amount. The create amount for the selected object/s is less than zero or set to zero.";
                        PAC_SYSTEM.displayNotificationWindow = true;
                        return;
                    }
                    
                    //If the prefab is null the system will stop progress on instantiating the object and inform the user of the error.
                    if (GetPrefabArray()[prefab_index].prefab == null)
                    {
                        PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                        PAC_SYSTEM.notification_header_message = "PREFAB IS NULL OR MISSING";
                        PAC_SYSTEM.notification_sub_message = "Please assign the prefab. The prefab for the selected category " 
                            + GetPrefabArray()[prefab_index].itemName + " is null/empty.";
                        PAC_SYSTEM.displayNotificationWindow = true;
                        return;
                    }
                    
                    //Begin the creation of the defined amount of items.
                    for (int a = 0; a < create_amount; a++)
                    {
                        SetCreationRayPosition(click_event_hit_point);                        

                        //Set the position on the ground the raycast from the sky has hit. Ref: creation_hit_info
                        if (CreationRaycast())
                        {                            
                            if (CreationCollisionCheck())
                            {
                                //Debug.Log("Creation Surface Validated");

                                if (CreationCheckNeighbor())
                                {
                                    //Debug.Log("Nearest Neighbor Position Validated");

                                    if (CreationSeaLevelPosition(prefab_index))
                                    {
                                        //Debug.Log("Sea Level Position Validated");

                                        #region Prefab Instantiation
                                        PAC_SYSTEM.item = Instantiate(GetPrefabArray()[prefab_index].prefab);
                                        PAC_SYSTEM.item.name = GetPrefabArray()[prefab_index].itemName;
                                        successful_create_amount++;
                                        #endregion

                                        #region Collider Check And Creation
                                        if (PAC_SYSTEM.systemAddsCollidersAtCreation == true)
                                        {
                                            bool hasCollider = false;

                                            if (PAC_SYSTEM.item.TryGetComponent(out BoxCollider box_collider))
                                            {
                                                hasCollider = true;
                                            }

                                            if (PAC_SYSTEM.item.TryGetComponent(out CapsuleCollider capsule_collider))
                                            {
                                                hasCollider = true;
                                            }

                                            if (PAC_SYSTEM.item.TryGetComponent(out SphereCollider sphere_collider))
                                            {
                                                hasCollider = true;
                                            }

                                            if (PAC_SYSTEM.item.TryGetComponent(out WheelCollider wheel_collider))
                                            {
                                                hasCollider = true;
                                            }

                                            if (PAC_SYSTEM.item.TryGetComponent(out MeshCollider mesh_collider))
                                            {
                                                hasCollider = true;
                                            }

                                            if (hasCollider == false)
                                            {
                                                MeshCollider meshcollider = null;
                                                bool filterfoundonparent;
                                                bool meshcolliderfoundonparent;

                                                //CHECK PARENT FOR MESH FILTER
                                                filterfoundonparent = PAC_SYSTEM.item.TryGetComponent(out MeshFilter filter);

                                                //MESH FILTER WAS NOT FOUND ON PARENT
                                                if (filterfoundonparent == false)
                                                {
                                                    //CHECK CHILDREN FOR MESH FILTER
                                                    filter = PAC_SYSTEM.item.GetComponentInChildren<MeshFilter>();
                                                }

                                                //MESH FILTER WAS NOT FOUND ON THE PARENT OR CHILDREN - USER WANTS A COLLIDER - CREATE PRIMITIVE BOX COLLIDER
                                                //BOX COLLIDER DIMENSIONS BASED OFF OF MESH SIZE
                                                if (filter == null)
                                                {
                                                    BoxCollider boxcollider = PAC_SYSTEM.item.AddComponent<BoxCollider>();
                                                    //Vector3 objectSize =
                                                    //Vector3.Scale(PAC_SYSTEM.item.transform.localScale, filter.sharedMesh.bounds.size);

                                                    //CREATE PRIMITIVE BOX COLLIDER WITH DEFAULT VALUE OF 1
                                                    boxcollider.size = new Vector3(1f, 1f, 1f);

                                                    #region OPTIONAL FEATURE
                                                    //OPTIONAL STANDARD AND PRO FEATURE
                                                    //BoxCollider boxcollider = PAC_SYSTEM.item.AddComponent<BoxCollider>();
                                                    //Vector3 objectSize =
                                                    //Vector3.Scale(PAC_SYSTEM.item.transform.localScale, filter.sharedMesh.bounds.size);
                                                    //Debug.Log("(Child) Created Object Size: " + objectSize);
                                                    //boxcollider.size = new Vector3(objectSize.x * 0.25f, objectSize.y, objectSize.z * 0.25f);
                                                    //boxcollider.center = new Vector3(0f, objectSize.y * 0.5f, 0f);
                                                    #endregion
                                                }
                                                //MESH FILTER WAS FOUND ON THE PARENT OR CHILDREN - USER WANTS A COLLIDER - ADD MESH COLLIDER TO PARENT
                                                else
                                                {
                                                    //CHECK PARENT FOR EXISTING MESH COLLIDER
                                                    meshcolliderfoundonparent = PAC_SYSTEM.item.TryGetComponent(out MeshCollider meshCollider);

                                                    //MESH COLLIDER WAS NOT FOUND ON PARENT
                                                    if (meshcolliderfoundonparent == false)
                                                    {
                                                        //CHECK CHILDREN FOR MESH COLLIDER
                                                        meshcollider = PAC_SYSTEM.item.GetComponentInChildren<MeshCollider>();
                                                    }

                                                    //MESH COLLIDER NOT FOUND ON PARENT OR CHILDREN
                                                    if (meshcollider == null)
                                                    {
                                                        //ADD MESH COLLIDER TO PARENT
                                                        meshCollider = PAC_SYSTEM.item.AddComponent<MeshCollider>();
                                                        meshCollider.sharedMesh = filter.sharedMesh;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Item Setup Checks
                                        if (CreationRotation(creation_hit_info, click_event_normal, prefab_index, mass_creation))
                                        {
                                            #region Item and Container Parenting
                                            if (SetParentContainer())
                                            {
                                                #region Height Mode Control And Setting Final Position
                                                if (CreationHeightMode(prefab_index))
                                                {
                                                    #region Scale Control
                                                    if (GetPrefabArray()[i].randomScale)
                                                    {
                                                        float min = GetPrefabArray()[prefab_index].minScale;
                                                        float max = GetPrefabArray()[prefab_index].maxScale;
                                                        float scale = UnityEngine.Random.Range(min, max);
                                                        PAC_SYSTEM.item.transform.localScale = new Vector3(scale, scale, scale);
                                                    }
                                                    #endregion

                                                    #region Item Info Component - Require For Item Detection At Runtime
                                                    bool info_found = PAC_SYSTEM.item.TryGetComponent(out ItemInfo _);

                                                    if (info_found)
                                                    {
                                                        ItemInfo info = PAC_SYSTEM.item.GetComponent<ItemInfo>();
                                                        info.itemCategory = PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].categoryName;
                                                    }
                                                    else
                                                    {
                                                        ItemInfo info = PAC_SYSTEM.item.AddComponent<ItemInfo>();
                                                        info.itemCategory = PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].categoryName;
                                                    }
                                                    #endregion

                                                    #region Final Positioning and Activation
                                                    PAC_SYSTEM.item.transform.position = PAC_SYSTEM.final_position;
                                                    PAC_SYSTEM.item.SetActive(true);
                                                    #endregion

                                                    #region Adding Active Item Data Referencing
                                                    ActiveItemData item_data = new ActiveItemData();
                                                    item_data.activeItem = PAC_SYSTEM.item;
                                                    item_data.activeItemPosition = PAC_SYSTEM.item.transform.position;

                                                    if (AddActiveItemData(item_data))
                                                    {
                                                        //Debug.Log("Item Creation Successful. Active item data added to the PAC System.");
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    Debug.Log("Creation Height Mode Error");
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                Debug.Log("Creation Parent Container Error");
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            Debug.Log("Creation Rotation Error");
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        sealevel_error_count++;
                                    }
                                }
                                else
                                {
                                    neighbor_error_count++;
                                }
                            }
                            else
                            {
                                invalid_collision_count++;
                            }
                        }
                        else
                        {
                            invalid_creation_raycast_count++;
                        }
                    }
                }
            }

            if (!item_defined)
            {
                PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                PAC_SYSTEM.notification_header_message = "NO PREFABS IN ACTIVE PREFAB CATEGORY MARKED AS 'USE ITEM'";
                PAC_SYSTEM.notification_sub_message = "The system could not find any items of the active prefab category with 'Use Item' checked as true.";
                PAC_SYSTEM.displayNotificationWindow = true;
                return;
            }

            Debug.Log("SEA LEVEL INVALID PLACEMENT: " + sealevel_error_count + " OJBECTS NOT CREATED");
            Debug.Log("NEIGHBOR INVALID PLACEMENT: " + neighbor_error_count + " OJBECTS NOT CREATED");
            Debug.Log("INVALID COLLISION PLACEMENT: " + invalid_collision_count + " OJBECTS NOT CREATED");
            Debug.Log("INVALID RAYCAST PLACEMENT: " + invalid_creation_raycast_count + " OJBECTS NOT CREATED");
            Debug.Log("TOTAL ATTEMPTED CREATE AMOUNT: " + create_amount * using_item_count);
            Debug.Log("TOTAL CREATED AMOUNT: " + successful_create_amount);

            if (sealevel_error_count == create_amount * using_item_count)
            {                
                PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                PAC_SYSTEM.notification_header_message = "SEA LEVEL ERROR";
                PAC_SYSTEM.notification_sub_message = "The system attempted to create the objects for you but each objects position was above or below the defined " +
                    "sea level. To resolve this issue try increasing the placement radius in the PAC Control Panel, change the sea level mode to " +
                    "'Above and Below Sea Level', or adjust the defined sea level. The system attempted to create: " + create_amount * using_item_count + " . The sea level error count is: " +
                    sealevel_error_count;
                PAC_SYSTEM.displayNotificationWindow= true;
            }

            if (neighbor_error_count == create_amount * using_item_count)
            {
                PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                PAC_SYSTEM.notification_header_message = "NEIGHBOR DISTANCE ERROR";
                PAC_SYSTEM.notification_sub_message = "The system attempted to create the objects for you but each objects position was too close to an existing " +
                    "object. To resolve this issue try increasing the min neighbor distance in the PAC Control Panel, or increase the placement radius. " +
                    "The system attempted to create: " + create_amount * using_item_count + " . The neighbor error count is: " +
                    neighbor_error_count;
                PAC_SYSTEM.displayNotificationWindow = true;
            }            

            if (invalid_creation_raycast_count >= create_amount * using_item_count)
            {
                PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                PAC_SYSTEM.notification_header_message = "NO ITEMS WERE CREATED DUE TO INVALID RAYCASTING";
                PAC_SYSTEM.notification_sub_message = "The system attempted to create the objects for you but each of the objects creation raycast didn't hit " +
                    "a surface/collider. If you are using the active surface type 'All Surfaces' please make sure that the surface has a collider component " +
                    "attached.";
                PAC_SYSTEM.displayNotificationWindow = true;                
            }

            if (invalid_collision_count >= create_amount * using_item_count)
            {
                PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                PAC_SYSTEM.notification_header_message = "NO ITEMS WERE CREATED DUE TO INVALID COLLISIONS";
                PAC_SYSTEM.notification_sub_message = "The system attempted to create the objects for you but each of the objects creation raycast collided " +
                    "with a scene object. Try reducing the creation amount for the prefabs settings, or increase the placement radius if you are mass creating.";
                PAC_SYSTEM.displayNotificationWindow = true;
            }
        }

        //Scene GUI Windows
        private void DisplayControlPanel(int windowID)
        {
            header_style = new GUIStyle();            
            header_style.alignment = TextAnchor.MiddleCenter;
            header_style.normal.textColor = Color.white;
            header_style.fontSize = 12;
            header_style.fontStyle = FontStyle.Bold;
            header_style.wordWrap = true;

            var sub_label = new GUIStyle();
            sub_label.alignment = TextAnchor.MiddleCenter;
            sub_label.normal.textColor = Color.yellow;
            sub_label.fontSize = 10;
            sub_label.fontStyle = FontStyle.Bold;

            string tooltip_sea_level = "Definition: The sea level is what the system uses to provide more control when placing items in the scene. When placing " +
                "a large amount of items in a scene, the system will utilize the defined sea level for limiting the placement of " +
                "items to the defined 'Sea Level Mode' in the prefab settings.";
            string tooltip_placement_radius = "Definition: The placement radius will be what the system uses when it recognizes mass creation. " +
            "The placement radius is the total size of the creation field range on the X and Z axes from the point clicked. ";
            string tooltip_min_neighbor_distance = "Definition: The minimum neighbor distance is not a static value in which the objects will be placed from " +
                "each other. This value is only defines how close an object can be to its neighbor. The object can be placed further than the defined value.";
            string tooltip_min_auto_adjust = "Definition: When turned on, the system will adjust the minimum neighbor distance to be 5% " +
                "of the placement radius.";            

            if (PAC_SYSTEM.PACSystemPrefabCategories.Length > 0)
            {
                PAC_SYSTEM.categories = new string[PAC_SYSTEM.PACSystemPrefabCategories.Length];

                for (int i = 0; i < PAC_SYSTEM.PACSystemPrefabCategories.Length; i++)
                {
                    if (PAC_SYSTEM.PACSystemPrefabCategories[i].categoryName == "")
                    {
                        PAC_SYSTEM.PACSystemPrefabCategories[i].categoryName = "Please name category at index: " + i;
                    }
                    else 
                    {
                        PAC_SYSTEM.categories[i] = PAC_SYSTEM.PACSystemPrefabCategories[i].categoryName;
                    }                    
                }
            }
            else
            {
                PAC_SYSTEM.categories = new string[1];
                PAC_SYSTEM.categories[0] = "No System Prefabs Defined";
            }

            //GUILayout.Label(PAC_SYSTEM.notification_sub_message, header_style);
            GUILayout.Space(5f);
            GUILayout.Label("System Core Settings", header_style);            
            PAC_SYSTEM.systemMode = (SystemState)EditorGUILayout.EnumPopup("System Mode", PAC_SYSTEM.systemMode);
            PAC_SYSTEM.category_index = EditorGUILayout.Popup("Active Prefab Category", PAC_SYSTEM.category_index, PAC_SYSTEM.categories);
            //PAC_SYSTEM.activeCreationType = (ItemType)EditorGUILayout.EnumPopup("Active Creation Type", PAC_SYSTEM.activeCreationType);            
            //PAC_SYSTEM.terrain = (Terrain)EditorGUILayout.ObjectField("World Terrain", PAC_SYSTEM.terrain, typeof(Terrain), true);
            PAC_SYSTEM.seaLevel = EditorGUILayout.FloatField(new GUIContent("Sea Level", tooltip_sea_level), PAC_SYSTEM.seaLevel);
            GUILayout.Label("Mass Placement - When Create Amount Is Greater Than 1", header_style);
            PAC_SYSTEM.placementRadius = EditorGUILayout.FloatField
                (new GUIContent("Placement Radius", tooltip_placement_radius), PAC_SYSTEM.placementRadius);
            PAC_SYSTEM.minimumNeighborDistance = EditorGUILayout.FloatField
                (new GUIContent("Min Neighbor Distance", tooltip_min_neighbor_distance), PAC_SYSTEM.minimumNeighborDistance);
            PAC_SYSTEM.autoAdjustMinNeighborDistance = GUILayout.Toggle
                (PAC_SYSTEM.autoAdjustMinNeighborDistance, new GUIContent("Auto Adjust Min Neighbor Distance", tooltip_min_auto_adjust));
            
            GUILayout.Space(5f);
            
            GUILayout.Label("Remove All Active Prefab Category Objects From Scene", header_style);
            
            ShowActiveRemoveAllButton();            

            GUILayout.Label("Remove All PAC System Placed Objects From Scene", header_style);

            ShowRemoveEverythingButton();
            
            GUILayout.Space(5f);

            GUILayout.Label("Edit All Active Prefab Category Settings", header_style);
            
            GUILayout.Label("Active Prefab Category: " + GetItemTypeStringName(), sub_label);            

            ShowEditAllButtons();

            
            //GUILayout.BeginArea(new Rect(5, 55, 300, 700));
            //GUILayout.BeginVertical();

            //GUILayout.EndVertical();
            //GUILayout.EndArea();
        }
        private void DisplaySystemWindow(int windowID)
        {
            var header_style = new GUIStyle();
            header_style.alignment = TextAnchor.MiddleCenter;
            header_style.normal.textColor = Color.white;
            header_style.fontSize = 15;
            header_style.fontStyle = FontStyle.Bold;

            var sub_label = new GUIStyle();
            sub_label.alignment = TextAnchor.MiddleCenter;
            sub_label.wordWrap = true;
            sub_label.normal.textColor = Color.yellow;
            sub_label.fontSize = 15;
            sub_label.fontStyle = FontStyle.Bold;            

            GUILayout.Space(10);
            GUILayout.Label(PAC_SYSTEM.system_header_message, header_style);
            GUILayout.Space(10);
            GUILayout.Label("What should I do to resolve the issue?", header_style);
            GUILayout.Space(10);
            GUILayout.Label(PAC_SYSTEM.system_sub_message, sub_label);
            GUILayout.Space(10);
            
            //if(GUILayout.Button("I Understand"))
            //{

            //}

            //GUILayout.BeginArea(new Rect(5, 55, 300, 700));
            //GUILayout.BeginVertical();

            //GUILayout.EndVertical();
            //GUILayout.EndArea();
        }
        private void DisplayNotificationWindow(int windowID)
        {
            var header_style = new GUIStyle();
            header_style.alignment = TextAnchor.MiddleCenter;
            header_style.normal.textColor = Color.white;
            header_style.fontSize = 15;
            header_style.fontStyle = FontStyle.Bold;

            var label_style = new GUIStyle();
            label_style.alignment = TextAnchor.MiddleCenter;
            label_style.normal.textColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            label_style.fontSize = 15;
            label_style.fontStyle = FontStyle.Normal;

            var sub_label = new GUIStyle();
            sub_label.alignment = TextAnchor.MiddleCenter;
            sub_label.wordWrap = true;
            sub_label.normal.textColor = Color.yellow;
            sub_label.fontSize = 15;
            sub_label.fontStyle = FontStyle.Normal;

            GUILayout.Space(10);
            GUILayout.Label(PAC_SYSTEM.notification_header_message, header_style);
            GUILayout.Space(10);
            GUILayout.Label("See details below:", label_style);
            GUILayout.Space(10);
            GUILayout.Label(PAC_SYSTEM.notification_sub_message, sub_label);
            GUILayout.Space(10);

            if (GUILayout.Button("Got It"))
            {
                PAC_SYSTEM.displayNotificationWindow = false;
            }

            //GUILayout.BeginArea(new Rect(5, 55, 300, 700));
            //GUILayout.BeginVertical();

            //GUILayout.EndVertical();
            //GUILayout.EndArea();
        }
        private void DisplayWarningWindow(int windowID)
        {
            var header_style = new GUIStyle();
            header_style.alignment = TextAnchor.MiddleCenter;
            header_style.normal.textColor = Color.white;
            header_style.fontSize = 15;
            header_style.fontStyle = FontStyle.Bold;

            var sub_label = new GUIStyle();
            sub_label.alignment = TextAnchor.MiddleCenter;
            sub_label.wordWrap = true;
            sub_label.normal.textColor = Color.yellow;
            sub_label.fontSize = 15;
            sub_label.fontStyle = FontStyle.Bold;

            GUILayout.Space(10);
            GUILayout.Label(PAC_SYSTEM.notification_header_message, header_style);
            GUILayout.Space(10);
            GUILayout.Label("See details below:", header_style);
            GUILayout.Space(10);
            GUILayout.Label(PAC_SYSTEM.notification_sub_message, sub_label);
            GUILayout.Space(10);

            if (GUILayout.Button("Cancel"))
            {
                PAC_SYSTEM.displayRemoveAllWarningWindow = false;
            }

            if (GUILayout.Button("Continue"))
            {
                PAC_SYSTEM.clearAll = true;
                PAC_SYSTEM.displayRemoveAllWarningWindow = false;
            }

            //GUILayout.BeginArea(new Rect(5, 55, 300, 700));
            //GUILayout.BeginVertical();

            //GUILayout.EndVertical();
            //GUILayout.EndArea();
        }
        private void ShowActiveRemoveAllButton()
        {
            if (PAC_SYSTEM.PACSystemPrefabCategories != null)
            {
                if (PAC_SYSTEM.PACSystemPrefabCategories.Length > 0)
                {
                    if(PAC_SYSTEM.category_index < PAC_SYSTEM.PACSystemPrefabCategories.Length)
                    {
                        if (GUILayout.Button("Remove All Active: " + PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].categoryName))
                        {
                            GameObject container =
                                GameObject.Find("PAC System - " + PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].categoryName);

                            DestroyImmediate(container);

                            for (int i = 0; i < PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].objectPool.Count; i++)
                            {
                                DestroyImmediate(PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].objectPool[i].activeItem);
                            }
                        }
                    }                    
                }
                else
                {
                    if (GUILayout.Button("No Prefabs Defined"))
                    {

                    }
                }                
            }            
        }
        private void ShowRemoveEverythingButton()
        {
            if (PAC_SYSTEM.PACSystemPrefabCategories.Length > 0)
            {
                if (GUILayout.Button("Remove Everything From The Scene"))
                {
                    PACSystemEditorAudioPlayback.PlaySound(PAC_SYSTEM.notificationSound);
                    PAC_SYSTEM.notification_header_message = "PAC SYSTEM - REMOVE ALL WARNING";
                    PAC_SYSTEM.notification_sub_message = "You have chosen to remove everything placed by the PAC System. Please confirm that this is what you " +
                        "want to do.";
                    PAC_SYSTEM.displayRemoveAllWarningWindow = true;
                }

                if (PAC_SYSTEM.clearAll)
                {
                    for (int i = 0; i < PAC_SYSTEM.PACSystemPrefabCategories.Length; i++)
                    {
                        GameObject container;
                        int object_pool_count = PAC_SYSTEM.PACSystemPrefabCategories[i].objectPool.Count;

                        if (object_pool_count > 0)
                        {
                            for (int a = 0; a < object_pool_count; a++)
                            {
                                int index = a;
                                //Debug.Log("Prefab Index: " + i + "Object Pool Index To Remove: " + index);
                                DestroyImmediate(PAC_SYSTEM.PACSystemPrefabCategories[i].objectPool[a].activeItem);
                            }

                            container =
                                GameObject.Find("PAC System - " + PAC_SYSTEM.PACSystemPrefabCategories[i].categoryName);

                            if (container != null)
                            {
                                DestroyImmediate(container);
                            }

                            PAC_SYSTEM.PACSystemPrefabCategories[i].objectPool.Clear();
                        }
                    }
                }

                PAC_SYSTEM.clearAll = false;
            }
            else
            {
                if (GUILayout.Button("No Items In The Scene"))
                {

                }
            }
        }
        private bool SystemDeleteObject(GameObject detected_object)
        {
            if (GetActiveList() != null)
            {
                for (int i = 0; i < GetActiveList().Count; i++)
                {
                    if (GetActiveList()[i].activeItem == detected_object)
                    {
                        Debug.Log("Object Removed: " + detected_object.name);
                        DestroyImmediate(detected_object);
                        GetActiveList().RemoveAt(i);                        
                        return true;
                    }
                }
                Debug.Log("The system attempted to remove the object but the object was not found in the PAC System. Proceeding with creating objects.");
                return false;
            }
            return false;
        }

        //PAC System Control Panel Edit All Buttons
        private void ShowEditAllButtons()
        {
            if (GetPrefabArray() != null)
            {
                if (GetPrefabArray().Length > 0)
                {
                    PAC_SYSTEM.editAllUseItem = GUILayout.Toggle(PAC_SYSTEM.editAllUseItem, "Use Item");
                    PAC_SYSTEM.editAllCreateAmount = EditorGUILayout.IntField("Create Amount", PAC_SYSTEM.editAllCreateAmount);
                    PAC_SYSTEM.editAllRandomScale = GUILayout.Toggle(PAC_SYSTEM.editAllRandomScale, "Random Scale");
                    PAC_SYSTEM.editAllRandomScaleMin = EditorGUILayout.FloatField("Random Scale Min", PAC_SYSTEM.editAllRandomScaleMin);
                    PAC_SYSTEM.editAllRandomScaleMax = EditorGUILayout.FloatField("Random Scale Max", PAC_SYSTEM.editAllRandomScaleMax);
                    PAC_SYSTEM.editAllRotationMode = (CreateRotationMode)EditorGUILayout.EnumPopup("Rotation Mode", PAC_SYSTEM.editAllRotationMode);
                    PAC_SYSTEM.editAllMinRotation = EditorGUILayout.FloatField("Random Rotation Min", PAC_SYSTEM.editAllMinRotation);
                    PAC_SYSTEM.editAllMaxRotation = EditorGUILayout.FloatField("Random Rotation Max", PAC_SYSTEM.editAllMaxRotation);
                    PAC_SYSTEM.editAllHeightMode = (CreateHeightMode)EditorGUILayout.EnumPopup("Height Mode", PAC_SYSTEM.editAllHeightMode);
                    PAC_SYSTEM.editAllSeaLevelMode = (CreateSeaLevelMode)EditorGUILayout.EnumPopup("Sea Level Mode", PAC_SYSTEM.editAllSeaLevelMode);
                    GUILayout.Space(5f);
                    //GUILayout.Label("Global Height Mode Values", header_style);
                    PAC_SYSTEM.editAllStaticHeightFromSurface = EditorGUILayout.FloatField("Static Height", PAC_SYSTEM.editAllStaticHeightFromSurface);
                    PAC_SYSTEM.editAllSurfaceRangeMinHeight = EditorGUILayout.FloatField("Surface Range Min", PAC_SYSTEM.editAllSurfaceRangeMinHeight);
                    PAC_SYSTEM.editAllSurfaceRangeMaxHeight = EditorGUILayout.FloatField("Surface Range Max", PAC_SYSTEM.editAllSurfaceRangeMaxHeight);
                    PAC_SYSTEM.editAllWorldSpaceMinHeight = EditorGUILayout.FloatField("World Space Min", PAC_SYSTEM.editAllWorldSpaceMinHeight);
                    PAC_SYSTEM.editAllWorldSpaceMaxHeight = EditorGUILayout.FloatField("World Space Max", PAC_SYSTEM.editAllWorldSpaceMaxHeight);

                    if(PAC_SYSTEM.editAllCreateAmount <= 0)
                    {
                        PAC_SYSTEM.editAllCreateAmount = 1;
                    }
                    if (PAC_SYSTEM.editAllRandomScaleMin <= 0)
                    {
                        PAC_SYSTEM.editAllRandomScaleMin = 1;
                    }
                    if (PAC_SYSTEM.editAllRandomScaleMax <= 0)
                    {
                        PAC_SYSTEM.editAllRandomScaleMax = 2;
                    }
                    if (PAC_SYSTEM.editAllMinRotation == 0)
                    {
                        PAC_SYSTEM.editAllMinRotation = -180;
                    }
                    if (PAC_SYSTEM.editAllMaxRotation == 0)
                    {
                        PAC_SYSTEM.editAllMaxRotation = 180;
                    }

                    GUILayout.Space(5f);
                    if (GUILayout.Button("Apply Settings"))
                    {
                        string message = string.Empty;
                        bool randomScaleMinSetToDefault = false;
                        bool randomScaleMaxSetToDefault = false;
                        bool rotationMinSetToDefault = false;
                        bool rotationMaxSetToDefault = false;
                        bool staticSetToDefault = false;                        
                        bool maxRangeSetToDefault = false;
                        
                        for (int i = 0; i < GetPrefabArray().Length; i++)
                        {
                            GetPrefabArray()[i].useItem = PAC_SYSTEM.editAllUseItem;
                            GetPrefabArray()[i].createAmount = PAC_SYSTEM.editAllCreateAmount;

                            if (PAC_SYSTEM.editAllRandomScaleMin == 0)
                            {
                                PAC_SYSTEM.editAllRandomScaleMin = 1;
                                randomScaleMinSetToDefault = true;
                            }

                            if (PAC_SYSTEM.editAllRandomScaleMax == 0)
                            {
                                PAC_SYSTEM.editAllRandomScaleMax = 2;
                                randomScaleMaxSetToDefault = true;
                            }

                            GetPrefabArray()[i].randomScale = PAC_SYSTEM.editAllRandomScale;
                            GetPrefabArray()[i].minScale = PAC_SYSTEM.editAllRandomScaleMin;
                            GetPrefabArray()[i].maxScale = PAC_SYSTEM.editAllRandomScaleMax;

                            if (PAC_SYSTEM.editAllMinRotation == 0)
                            {
                                PAC_SYSTEM.editAllMinRotation = -180;
                                rotationMinSetToDefault = true;
                            }

                            if (PAC_SYSTEM.editAllMaxRotation == 0)
                            {
                                PAC_SYSTEM.editAllMaxRotation = 180;
                                rotationMaxSetToDefault = true;
                            }

                            GetPrefabArray()[i].rotationMode = PAC_SYSTEM.editAllRotationMode;
                            GetPrefabArray()[i].minRotation = PAC_SYSTEM.editAllMinRotation;
                            GetPrefabArray()[i].maxRotation = PAC_SYSTEM.editAllMaxRotation;

                            if (PAC_SYSTEM.editAllHeightMode == CreateHeightMode.StaticHeightFromSurface)
                            {
                                if (PAC_SYSTEM.editAllStaticHeightFromSurface == 0)
                                {
                                    PAC_SYSTEM.editAllStaticHeightFromSurface = 1;
                                    staticSetToDefault = true;
                                }

                                GetPrefabArray()[i].staticCreateHeight = PAC_SYSTEM.editAllStaticHeightFromSurface;
                            }

                            if (PAC_SYSTEM.editAllHeightMode == CreateHeightMode.HeightRangeFromSurface)
                            {
                                if (PAC_SYSTEM.editAllSurfaceRangeMinHeight == 0 & PAC_SYSTEM.editAllSurfaceRangeMaxHeight == 0)
                                {
                                    PAC_SYSTEM.editAllSurfaceRangeMaxHeight = 1;
                                    maxRangeSetToDefault = true;

                                }
                                

                                GetPrefabArray()[i].surfaceRangeMinHeight = PAC_SYSTEM.editAllSurfaceRangeMinHeight;
                                GetPrefabArray()[i].surfaceRangeMaxHeight = PAC_SYSTEM.editAllSurfaceRangeMaxHeight;
                            }

                            if (PAC_SYSTEM.editAllHeightMode == CreateHeightMode.WorldSpaceHeightRange)
                            {
                                GetPrefabArray()[i].worldSpaceMinHeight = PAC_SYSTEM.editAllWorldSpaceMinHeight;
                                GetPrefabArray()[i].worldSpaceMaxHeight = PAC_SYSTEM.editAllWorldSpaceMaxHeight;
                            }

                            GetPrefabArray()[i].heightMode = PAC_SYSTEM.editAllHeightMode;

                            GetPrefabArray()[i].seaLevelMode = PAC_SYSTEM.editAllSeaLevelMode;

                            if (randomScaleMinSetToDefault)
                            {
                                message += "Random scale minimum was detected to be at 0. It has been set to a default of 1 to prevent error. " +
                                    "Please adjust the edit all random scale minimum to your desired value. ";
                            }

                            if (randomScaleMaxSetToDefault)
                            {
                                message += "Random scale maximum was detected to be at 0. It has been set to a default of 2 to prevent error. " +
                                    "Please adjust the edit all random scale maximum to your desired value. ";
                            }

                            if (rotationMinSetToDefault)
                            {
                                message += "Rotation minimum was detected to be at 0. It has been set to a default of -180 to prevent error. " +
                                        "Please adjust the edit all rotation minimum to your desired value. ";
                            }

                            if (rotationMaxSetToDefault)
                            {
                                message += "Rotation maximum was detected to be at 0. It has been set to a default of 180 to prevent error. " +
                                        "Please adjust the edit all rotation maximum to your desired value. ";
                            }

                            if (staticSetToDefault)
                            {
                                message += "Static create height was detected to be at 0. It has been set to a default of 1 for you to prevent error. " +
                                        "Please adjust the edit all static create height to your desired value. ";
                            }
                            
                            if (maxRangeSetToDefault)
                            {
                                message += "Surface range maximum height was detected to be at 0. It has been set to a default of 1 for you to prevent error. " +
                                            "Please adjust the edit all height range maximum height to your desired value. ";
                            }
                        }

                        message += "The changes for all " +
                                PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].categoryName + " have been applied.";
                        
                        Debug.Log(message);
                    }
                }
                else
                {
                    if (GUILayout.Button("No " + GetItemTypeStringName() + " Prefabs"))
                    {

                    }
                }
            }
        }
        #endregion

        #region OnInspector Methods
        private void CheckDefines()
        {
            PAC_SYSTEM.defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';');

            List<string> temp = new List<string>();

            for (int i = 0; i < PAC_SYSTEM.defines.Length; i++)
            {
                temp.Add(PAC_SYSTEM.defines[i]);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] == "PAC_SYSTEM")
                {
                    PAC_SYSTEM.systemDefine = "PAC_SYSTEM";
                }
            }

            if (PAC_SYSTEM.systemDefine != "PAC_SYSTEM")
            {
                temp.Add("PAC_SYSTEM");
                string defines = string.Empty;
                for (int i = 0; i < temp.Count; i++)
                {
                    defines += temp[i] + ";";
                }
                Debug.Log("PAC_SYSTEM define added to Unity Player Settings system define symbols.");
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
                PAC_SYSTEM.systemDefine = "PAC_SYSTEM";
            }
        }
        private void CheckAudioReferences()
        {
            if (PAC_SYSTEM.notificationSound == null)
            {
                AudioClip system_notification_sound = (AudioClip)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/Audio/NotificationSound.wav", typeof(AudioClip));

                //The system found the audio file at the designated path.
                if (system_notification_sound != null)
                {
                    PAC_SYSTEM.notificationSound = system_notification_sound;
                }
                //The system or the user has assigned an audio file and the scene window will close.
                else if (PAC_SYSTEM.notificationSound != null)
                {
                    PAC_SYSTEM.displaySceneWindow = false;
                }
                //The system did not find the audio file at the designated path and the reference remains null or empty.
                else
                {
                    PAC_SYSTEM.system_header_message = "MISSING SYSTEM NOTIFICATION AUDIO FILE";
                    PAC_SYSTEM.system_sub_message = "The system attempted to automatically reference the notification sound audio clip but it couldn't be " +
                        "located at path: Assets/PACSystem/Audio/NotificationSound.wav. Please make sure this was not removed. If it was removed then either re-import " +
                        "the asset or implement your own audio file. Just drag your audio file into the reference for the 'Notification Sound' in " +
                        "the inspector.";
                    PAC_SYSTEM.displaySceneWindow = true;
                }                
            }            
        }                
        private void CheckForChangesInActiveSceneObjects()
        {
            if(GetActiveList() != null)
            {
                if (GetActiveList().Count > 0)
                {
                    for (int i = 0; i < GetActiveList().Count; i++)
                    {
                        if (GetActiveList()[i].activeItem == null)
                        {
                            GetActiveList().RemoveAt(i);
                        }
                    }
                }                
            }
        }
        private void CheckAutoSpacing()
        {
            //This detects the auto spacing setting and adjusts the creation radius and item spacing if the creation radius set to 0
            if (PAC_SYSTEM.autoAdjustMinNeighborDistance)
            {
                if (PAC_SYSTEM.placementRadius <= 0)
                {
                    PAC_SYSTEM.placementRadius = 25f;
                }
                PAC_SYSTEM.minimumNeighborDistance = PAC_SYSTEM.placementRadius * 0.05f;
            }
            //This is if auto spacing is turned off and sets the creation radius and item spacing if creation radius is set to 0
            else
            {
                if (PAC_SYSTEM.placementRadius <= 0)
                {
                    PAC_SYSTEM.placementRadius = 25f;
                    PAC_SYSTEM.minimumNeighborDistance = PAC_SYSTEM.placementRadius * 0.05f;
                }
            }
        }        
        private void ReferenceImagesInspector()
        {
            if (noTerrain == null)
                noTerrain = (Texture)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/InspectorImages/PAC_System_NoTerrain.png", typeof(Texture));
            if (systemBanner == null)
                systemBanner = (Texture)AssetDatabase.LoadAssetAtPath
                    ("Assets/PACSystem/InspectorImages/PAC_System_Banner.png", typeof(Texture));
        }
        private void DisplayInspectorImages()
        {
            GUILayout.Box(systemBanner, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        }                
        private void CheckSurfaceSetting()
        {
            if(PAC_SYSTEM.parentToSurface)
            {
                if (PAC_SYSTEM.surface == null)
                {
                    if(PAC_SYSTEM.displayNotificationWindow == false)
                    {
                        PAC_SYSTEM.notification_header_message = "SURFACE IS MISSING";
                        PAC_SYSTEM.notification_sub_message = "You have chosen to use the setting 'Parent To Surface' but there is not a surface assigned in the " +
                            "inspector. Please assign a surface in the inspector or uncheck the box for 'Parent To Surface'.";
                        PAC_SYSTEM.displayNotificationWindow = true;
                    }                    
                }
                else 
                {
                    PAC_SYSTEM.displayNotificationWindow = false;
                }
            }
        }
        private bool SystemInScene()
        {
            PointAndClickSystem system_in_scene = FindObjectOfType<PointAndClickSystem>();

            if (system_in_scene != null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        private void CheckPrefabArrayCount()
        {
            if (PAC_SYSTEM.PACSystemPrefabCategories != null)
            {
                if (PAC_SYSTEM.arraySize != PAC_SYSTEM.PACSystemPrefabCategories.Length)
                {
                    //OBJECT ADDED
                    if (PAC_SYSTEM.arraySize < PAC_SYSTEM.PACSystemPrefabCategories.Length)
                    {
                        int lastindex = PAC_SYSTEM.PACSystemPrefabCategories.Length - 1;

                        PAC_SYSTEM.PACSystemPrefabCategories[lastindex] = null;
                    }

                    PAC_SYSTEM.arraySize = PAC_SYSTEM.PACSystemPrefabCategories.Length;
                }
            }
        }
        #endregion

        #region Creation Methods
        private void SetCreationRayPosition(Vector3 hit_point)
        {
            ray_position = Vector3.zero;

            //Check the defined creation amount to determine mass creation and the raycast position.
            if (create_amount == 1)
            {
                mass_creation = false;
                ray_position = new Vector3(hit_point.x, hit_point.y, hit_point.z);
                Debug.Log("Not mass creating. Ray position: " + ray_position);
            }
            else if (create_amount > 1)
            {
                mass_creation = true;
                float placement_radius = PAC_SYSTEM.placementRadius * 0.45f;
                //Mass create detected and the system will generate a random raycast within the defined creation radius.
                float minX = hit_point.x - placement_radius;
                float maxX = hit_point.x + placement_radius;
                float y = 1000;
                float minZ = hit_point.z - placement_radius;
                float maxZ = hit_point.z + placement_radius;

                float randomX = UnityEngine.Random.Range(minX, maxX);
                float randomZ = UnityEngine.Random.Range(minZ, maxZ);
                ray_position = new Vector3(randomX, y, randomZ);
            }
            
            //Debug.Log("System Ray Position: " + ray_position);            
        }
        private bool CreationRaycast()
        {
            //Debug.Log("Creation Raycast Position: " + ray_position);
            
            if (mass_creation)
            {
                if (Physics.Raycast(ray_position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {                    
                    creation_hit_info = hit;                    
                    return true;
                }
                else
                {
                    //Debug.Log("Creation Raycast Didn't Hit Anything.");               
                    return false;
                }
            }
            else
            {
                creation_hit_info = click_event_ray_hit;
                return true;
            }
        }
        private bool CreationCollisionCheck()
        {
            if (PAC_SYSTEM.avoidCollisions)
            {
                GameObject detected_object = creation_hit_info.transform.gameObject;
                Transform parent = creation_hit_info.transform.parent;

                if (detected_object != null)
                {
                    if (PAC_SYSTEM.surface != detected_object.transform)
                    {
                        Debug.Log("Creation Raycast Hit An Invalid Object: " + detected_object.name);
                        validCollisionCheck = false;
                    }
                    else
                    {
                        validCollisionCheck = true;
                    }
                }
                else if (parent != null)
                {
                    if (PAC_SYSTEM.surface != parent)
                    {
                        Debug.Log("(PARENT) Creation Raycast Hit An Invalid Object:" + parent.name);
                        validCollisionCheck = false;
                    }
                    else
                    {
                        validCollisionCheck = true;
                    }
                }
            }
            //Not avoiding collisions
            else
            {
                validCollisionCheck = true;
            }
            
            return validCollisionCheck;
        }        
        private bool CreationCheckNeighbor()
        {
            if(mass_creation)
            {
                //A neighbor distance value too low will result in the item not being instantiated.
                if (GetActiveList().Count > 0)
                {
                    for (int b = 0; b < GetActiveList().Count; b++)
                    {
                        //Vector3 height = new Vector3(hit.point.x, prefab_data[i].staticCreateHeight, hit.point.z);
                        float neighbor_distance = Vector3.Distance(creation_hit_info.point, GetActiveList()[b].activeItemPosition);

                        if (neighbor_distance < PAC_SYSTEM.minimumNeighborDistance)
                        {
                            //Debug.Log("The item/s to be created was/were too close to a neighboring item placed by the PAC System. The " +
                            //"item/s was/were not created.");
                            return false;
                        }
                    }
                    return true;
                }
                //This is if the item is the first one to be created of its type.
                else
                {
                    return true;
                }
            }
            return true;
        }
        private bool CreationSeaLevelPosition(int prefab_index)
        {
            bool valid = false;
            //Debug.Log("Object Creation Raycast Position: " + creation_hit_info.point);

            if (GetPrefabArray()[prefab_index].seaLevelMode == CreateSeaLevelMode.AboveSeaLevelOnly)
            {
                if (creation_hit_info.point.y <= PAC_SYSTEM.seaLevel)
                {
                    valid = false;
                }
                else
                {                   
                    valid = true;
                }
            }
            if (GetPrefabArray()[prefab_index].seaLevelMode == CreateSeaLevelMode.BelowSeaLevelOnly)
            {
                if (creation_hit_info.point.y >= PAC_SYSTEM.seaLevel)
                {
                    valid = false;
                }
                else
                {
                    valid = true;
                }
            }
            else if (GetPrefabArray()[prefab_index].seaLevelMode == CreateSeaLevelMode.AboveAndBelowSeaLevel)
            {                
                valid = true;
            }
            
            return valid;
        }
        private bool CreationRotation(RaycastHit hit, Vector3 original_normal, int prefab_index, bool mass_creation)
        {
            if (!mass_creation)
            {
                PAC_SYSTEM.hit_normal = original_normal;
            }
            else if (mass_creation)
            {
                PAC_SYSTEM.hit_normal = hit.normal;
            }

            rotationType = GetPrefabArray()[prefab_index].rotationMode;
            
            float minRotation = GetPrefabArray()[prefab_index].minRotation;
            float maxRotation = GetPrefabArray()[prefab_index].maxRotation;

            float final_rotation_value = UnityEngine.Random.Range(minRotation, maxRotation);

            if (!SetItemRotation(rotationType, final_rotation_value))
            {
                PAC_SYSTEM.notification_header_message = "ITEM ROTATION ERROR";
                PAC_SYSTEM.notification_sub_message = "The system attempted to set the rotation for the " +
                    PAC_SYSTEM.item.name + " but the rotation type " + rotationType + " was not recognized. " +
                    "The item has been destroyed and the PAC System updated.";
                PAC_SYSTEM.displayNotificationWindow = true;
                DestroyImmediate(PAC_SYSTEM.item);
                return false;
            }
            else 
            {
                return true;
            }
        }
        private bool CreationHeightMode(int prefab_index)
        {
            if (GetPrefabArray()[prefab_index].heightMode == CreateHeightMode.StaticHeightFromSurface)
            {
                PAC_SYSTEM.final_position =
                    new Vector3(creation_hit_info.point.x, creation_hit_info.point.y +
                    GetPrefabArray()[prefab_index].staticCreateHeight, creation_hit_info.point.z);
                return true;
            }
            else if (GetPrefabArray()[prefab_index].heightMode == CreateHeightMode.HeightRangeFromSurface)
            {
                float random_y = UnityEngine.Random.Range(GetPrefabArray()[prefab_index].surfaceRangeMinHeight, GetPrefabArray()[prefab_index].surfaceRangeMaxHeight);
                PAC_SYSTEM.final_position = new Vector3(creation_hit_info.point.x, creation_hit_info.point.y + random_y, creation_hit_info.point.z);
                return true;
            }
            else if (GetPrefabArray()[prefab_index].heightMode == CreateHeightMode.WorldSpaceHeightRange)
            {
                float random_y = UnityEngine.Random.Range(GetPrefabArray()[prefab_index].worldSpaceMinHeight, GetPrefabArray()[prefab_index].worldSpaceMaxHeight);
                PAC_SYSTEM.final_position = new Vector3(creation_hit_info.point.x, random_y, creation_hit_info.point.z);
                return true;
            }
            else if (GetPrefabArray()[prefab_index].heightMode == CreateHeightMode.AtPointAndClick)
            {
                PAC_SYSTEM.final_position = creation_hit_info.point;
                return true;
            }            
            else 
            {
                return false;
            }
        }
        #endregion

        #region Case Switch Methods
        private List<ActiveItemData> GetActiveList()
        {
            if (PAC_SYSTEM.PACSystemPrefabCategories != null)
            {
                if (PAC_SYSTEM.category_index < PAC_SYSTEM.PACSystemPrefabCategories.Length)
                {
                    if (PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].objectPool != null)
                    {
                        return PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].objectPool;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else 
            {
                return null;
            }
        }
        private PrefabData[] GetPrefabArray()
        {
            if(PAC_SYSTEM.category_index < PAC_SYSTEM.PACSystemPrefabCategories.Length)
            {
                if (PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index] != null)
                {
                    return PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].prefabs;
                }
                else 
                {
                    return null;
                }                
            }
            return null;
        }
        private bool SetParentContainer()
        {
            string container_name = 
                "PAC System - " + PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].categoryName;
            
            GameObject created_objects_container = GameObject.Find(container_name);

            //If the container is not found in the hierarchy then create a container
            if (created_objects_container == null)
            {
                created_objects_container = new GameObject(container_name);                
            }

            //If the creation of the container was successful then proceed
            if (created_objects_container != null)
            {
                //If using parent to surface then the current container for the objects will be parented to the new or existing creation surface clicked
                if (PAC_SYSTEM.parentToSurface)
                {
                    created_objects_container.transform.SetParent(PAC_SYSTEM.surface);
                }
            }
            //If the creation of the container was not successful then abort and display notification message
            else
            {
                PAC_SYSTEM.notification_header_message = "ITEM PARENT CONTAINER ERROR";
                PAC_SYSTEM.notification_sub_message = "The system attempted to set the parent container for the " +
                    PAC_SYSTEM.item.name + " but the system couldn't find the object continer. The item was destroyed " +
                    "and the PAC System updated.";
                PAC_SYSTEM.displayNotificationWindow = true;
                DestroyImmediate(PAC_SYSTEM.item);
                return false;
            }
            
            PAC_SYSTEM.item.transform.SetParent(created_objects_container.transform);

            return true;
        }
        private bool SetItemRotation(CreateRotationMode rotation_mode, float final_rotation_value)
        {
            switch (rotation_mode)
            {
                case CreateRotationMode.AlignWithSurfaceOnly:
                    //Align the entity with the terrain or creation surface.
                    PAC_SYSTEM.item.transform.rotation = Quaternion.FromToRotation(PAC_SYSTEM.item.transform.up, PAC_SYSTEM.hit_normal);
                    return true;
                case CreateRotationMode.RandomX:
                    PAC_SYSTEM.item.transform.localEulerAngles = new Vector3(final_rotation_value, 0f, 0f);                    
                    return true;
                case CreateRotationMode.RandomYWithSurfaceAlignment:
                    PAC_SYSTEM.item.transform.rotation = Quaternion.FromToRotation(PAC_SYSTEM.item.transform.up, PAC_SYSTEM.hit_normal);
                    PAC_SYSTEM.item.transform.localEulerAngles = new Vector3(0f, final_rotation_value, 0f);
                    return true;
                case CreateRotationMode.RandomY:
                    PAC_SYSTEM.item.transform.localEulerAngles = new Vector3(0f, final_rotation_value, 0f);
                    return true;
                case CreateRotationMode.RandomZ:
                    PAC_SYSTEM.item.transform.localEulerAngles = new Vector3(0f, 0f, final_rotation_value);
                    return true;
                case CreateRotationMode.RandomXAndY:
                    PAC_SYSTEM.item.transform.localEulerAngles = new Vector3(final_rotation_value, final_rotation_value, 0f);
                    return true;
                case CreateRotationMode.RandomXAndZ:
                    PAC_SYSTEM.item.transform.localEulerAngles = new Vector3(final_rotation_value, 0f, final_rotation_value);
                    return true;
                case CreateRotationMode.RandomYAndZ:
                    PAC_SYSTEM.item.transform.localEulerAngles = new Vector3(0f, final_rotation_value, final_rotation_value);
                    return true;
                case CreateRotationMode.None:                    
                    return true;
                default:
                    return false;
            }
        }
        private bool AddActiveItemData(ActiveItemData item_data)
        {
            if (PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].objectPool != null)
            {
                PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].objectPool.Add(item_data);
                return true;
            }
            else 
            {
                PAC_SYSTEM.notification_header_message = "ACTIVE CREATION CATEGORY OBJECT POOL NOT INTIALIZED";
                PAC_SYSTEM.notification_sub_message = "The system attempted to add the item data for the " +
                    PAC_SYSTEM.item.name + " but the active creation category object pool was not recognized. The item was destroyed " +
                    "and the PAC System updated.";
                PAC_SYSTEM.displayNotificationWindow = true;
                DestroyImmediate(PAC_SYSTEM.item);
                return false;
            }            
        }        
        private string GetItemTypeStringName()
        {
            if (PAC_SYSTEM.category_index < PAC_SYSTEM.PACSystemPrefabCategories.Length)
            {
                if (PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index] != null)
                {
                    return PAC_SYSTEM.PACSystemPrefabCategories[PAC_SYSTEM.category_index].categoryName;
                }
                else 
                {
                    return "No Prefab At Category Index";
                }                
            }
            
            else 
            {
                return "No System Prefabs Defined";
            }
        }
        #endregion

        #region Extra Methods
        private void UnpackPrefab()
        {
            if (PrefabUtility.GetPrefabAssetType(PAC_SYSTEM.transform.gameObject) != PrefabAssetType.NotAPrefab)
            {
                PrefabUtility.UnpackPrefabInstance
                    (PAC_SYSTEM.transform.gameObject, PrefabUnpackMode.Completely, InteractionMode.UserAction);                
            }
        }
        private void ClearConsole()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
        
        #endregion
    }
    public static class PACSystemEditorAudioPlayback
    {
        private static MethodInfo playMethod;
        private static MethodInfo stopMethod;
        public static void PlaySound(AudioClip clip, int startTime = 0, bool loop = false)
        {
            if (playMethod == null)
            {
                Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
                Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
                playMethod = audioUtilClass.GetMethod(
                    "PlayPreviewClip",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.DeclaredOnly,
                    null,
                    new System.Type[] {
                typeof(AudioClip),
                typeof(int),
                typeof(bool),
                    },
                    null
                );
            }
            playMethod.Invoke(
               null,
               new object[] {
                clip, startTime, loop
               }
           );
        }
        public static void StopSound()
        {
            if (stopMethod == null)
            {
                Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
                Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
                stopMethod = audioUtilClass.GetMethod(
                    "StopAllPreviewClips",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.DeclaredOnly
                );
            }
            stopMethod.Invoke(null, null);
        }
    }
}