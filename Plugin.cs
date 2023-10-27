#region IMPORTS
using BepInEx;
using Steamworks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Reflection;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HawkNetworking;
using Unity.Audio;
using System;
#endregion

namespace WobblyLife_ConsoleCommands
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        #region VARIABLES
        public static Plugin instance;

        public static Plugin Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Plugin();
                }
                return instance;
            }
        }
        int noclipticker = 0;
        RagdollController ragdoll;
        AchievementManager achievementManager;
        float vertSpeed = 0.1f;
        float flightSpeed = 1f;
        bool flight = false;
        public Canvas canvas;
        public GameObject commandObject;
        public GameObject popupObject;
        public TextMeshProUGUI commandText;
        public TextMeshProUGUI popupText;
        public string command = "press / or Up arrow to open console";
        public string popup = "";
        public int popupTimer = 0;
        bool showConsole = false;
        bool doneStartingUp = false;
        RewardMoneyData rewardMoneyData = FindObjectOfType<RewardMoneyData>();
        public RewardManager rewardManager = FindObjectOfType<RewardManager>();
        PlayerControllerEmployment pce = FindObjectOfType<PlayerControllerEmployment>();
        WobblyAchievement[] achievements = {
            WobblyAchievement.COMPLETE_JOB_JELLY,
	        // Token: 0x040024C0 RID: 9408
	        WobblyAchievement.COMPLETE_JOB_PIZZA,
	        // Token: 0x040024C1 RID: 9409
	        WobblyAchievement.COMPLETE_JOB_BURGER,
	        // Token: 0x040024C2 RID: 9410
	        WobblyAchievement.COMPLETE_JOB_POWER_PLANT,
	        // Token: 0x040024C3 RID: 9411
	        WobblyAchievement.COMPLETE_JOB_EMERGENCY,
	        // Token: 0x040024C4 RID: 9412
	        WobblyAchievement.COMPLETE_JOB_NEWROUND,
	        // Token: 0x040024C5 RID: 9413
	        WobblyAchievement.COMPLETE_JOB_FURNITURE,
	        // Token: 0x040024C6 RID: 9414
	        WobblyAchievement.COMPLETE_JOB_PIZZA_UFO,
	        // Token: 0x040024C7 RID: 9415
	        WobblyAchievement.COMPLETE_JOB_GARBAGE,
	        // Token: 0x040024C8 RID: 9416
	        WobblyAchievement.COMPLETE_RACE_KART,
	        // Token: 0x040024C9 RID: 9417
	        WobblyAchievement.COMPLETE_RACE_PLANE,
	        // Token: 0x040024CA RID: 9418
	        WobblyAchievement.COMPLETE_RACE_BOAT,
	        // Token: 0x040024CB RID: 9419
	        WobblyAchievement.COMPLETE_TEMPLE_PUZZLE,
	        // Token: 0x040024CC RID: 9420
	        WobblyAchievement.BUY_FIRST_HOUSE,
	        // Token: 0x040024CD RID: 9421
	        WobblyAchievement.HAVE_1000_IN_THE_BANK,
	        // Token: 0x040024CE RID: 9422
	        WobblyAchievement.HAVE_5000_IN_THE_BANK,
	        // Token: 0x040024CF RID: 9423
	        WobblyAchievement.HAVE_10000_IN_THE_BANK,
	        // Token: 0x040024D0 RID: 9424
	        WobblyAchievement.COLLECT_ALL_PRESENTS_ON_WOBBLY_ISLAND,
	        // Token: 0x040024D1 RID: 9425
	        WobblyAchievement.FEED_MONSTER_25_TOXIC_BARRELS,
	        // Token: 0x040024D2 RID: 9426
	        WobblyAchievement.FEED_MONSTER_50_TOXIC_BARRELS,
	        // Token: 0x040024D3 RID: 9427
	        WobblyAchievement.COMPLETE_JOB_FARM_PLOW,
	        // Token: 0x040024D4 RID: 9428
	        WobblyAchievement.COMPLETE_JOB_FARM_SEED,
	        // Token: 0x040024D5 RID: 9429
	        WobblyAchievement.COMPLETE_JOB_FARM_HARVEST,
	        // Token: 0x040024D6 RID: 9430
	        WobblyAchievement.PROCESS_URANIUM_IN_MINE_MACHINE,
	        // Token: 0x040024D7 RID: 9431
	        WobblyAchievement.BUY_FIRST_PET,
	        // Token: 0x040024D8 RID: 9432
	        WobblyAchievement.UNLOCK_GHOST_PET,
	        // Token: 0x040024D9 RID: 9433
	        WobblyAchievement.CHOOSE_WISELY,
	        // Token: 0x040024DA RID: 9434
	        WobblyAchievement.COMPLETE_JOB_QUIZ_MASTER,
	        // Token: 0x040024DB RID: 9435
	        WobblyAchievement.COMPLETE_JOB_FIRE_FIGHTER,
	        // Token: 0x040024DC RID: 9436
	        WobblyAchievement.COMPLETE_JOB_WOOD_CUTTER,
	        // Token: 0x040024DD RID: 9437
	        WobblyAchievement.COMPLETE_JOB_SCIENCE_MACHINE,
	        // Token: 0x040024DE RID: 9438
	        WobblyAchievement.COMPLETE_BUILD_UFO_MISSION,
	        // Token: 0x040024DF RID: 9439
	        WobblyAchievement.COMPLETE_FIRST_MUSEUM_COLLECTION,
	        // Token: 0x040024E0 RID: 9440
	        WobblyAchievement.COMPLETE_JOB_TAXI,
	        // Token: 0x040024E1 RID: 9441
	        WobblyAchievement.COMPLETE_JOB_ICE_CREAM,
	        // Token: 0x040024E2 RID: 9442
	        WobblyAchievement.COMPLETE_ANCIENT_TRIALS,
	        // Token: 0x040024E3 RID: 9443
	        WobblyAchievement.COMPLETE_JOB_DISCO,
	        // Token: 0x040024E4 RID: 9444
	        WobblyAchievement.COMPLETE_JOB_FISHING,
	        // Token: 0x040024E5 RID: 9445
	        WobblyAchievement.COLLECT_ALL_FISH_ON_WOBBLY_ISLAND,
	        // Token: 0x040024E6 RID: 9446
	        WobblyAchievement.COMPLETE_JELLY_CAR_MISSION,
	        // Token: 0x040024E7 RID: 9447
	        WobblyAchievement.COMPLETE_JOB_CONSTRUCTION_RESOURCES,
	        // Token: 0x040024E8 RID: 9448
	        WobblyAchievement.COMPLETE_JOB_CONSTRUCTION_BUILDING,
	        // Token: 0x040024E9 RID: 9449
	        WobblyAchievement.COMPLETE_JOB_CONSTRUCTION_DESTRUCTION,
	        // Token: 0x040024EA RID: 9450
	        WobblyAchievement.COMPLETE_JOB_WEATHER,
	        // Token: 0x040024EB RID: 9451
	        WobblyAchievement.COMPLETE_JOB_ARTSTUDIO,
	        // Token: 0x040024EC RID: 9452
	        WobblyAchievement.COMPLETE_MISSION_DREAM,
	        // Token: 0x040024ED RID: 9453
	        WobblyAchievement.COMPLETE_MISSION_WEATHERMACHINE


        };
        GameObject player;
        Vector3 playerPos = new Vector3(0, 0, 0);
        int ticker = 600;
        bool returnedFromGame = false;
        bool automatic = false;
        public int peopleAllowed = 100;
        public TextMeshProUGUI playerLimitText;
        PlayerController playerController;
        Color transparent = new Color(0, 0, 0, 0);
        public bool rangeHack = false;
        public bool notDone = true;

        #endregion

        #region METHODS
        public int GetPeopleAllowed()
        {
            return peopleAllowed;
        }

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            achievementManager = AchievementManager.Instance;
        }

        public GameObject GetPlayerCharacter()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                return GameObject.FindGameObjectsWithTag("Player")[0];
            }
            return null;
        }

        public void Teleport(Vector3 locationPos)
        {
            GameObject player = GetPlayerCharacter();
            player.transform.position = locationPos;
            Log($"teleported to\n{locationPos}");
        }
        
        public void TeleportAll(Vector3 locationPos)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            float yOffset = 0f;
            for (int i = 0; i < players.Length; i++)
            {
                players[i].transform.position = new Vector3(locationPos.x, locationPos.y + yOffset, locationPos.z);
                yOffset += 1.5f;
            }
            Log($"teleported all to\n{locationPos}");
        }

        public void StartArcade()
        {
            ArcadeInstance.Instance.GetLobbyInstance().ServerSetArcadeMode(ArcadeMode.Game);
        }

        public void StartAutomaticArcade()
        {
            int modes = 4;
            ArcadeInstance.Instance.GetLobbyInstance().GetLobbyInfoData().officalArcadeModeIndex = UnityEngine.Random.RandomRange(0,3);
            ArcadeInstance.Instance.GetLobbyInstance().ServerSetArcadeMode(ArcadeMode.Game);
        }

        public void ChangeSpeed(float mult)
        {
            PlayerCharacterMovement playerCharacterMovement = GameObject.Find("PlayerCharacter(Clone)").GetComponent<PlayerCharacterMovement>();
            playerCharacterMovement.SetSpeedMultiplier(mult);
        }

        public void GetAllClothes()
        {
            ClothingManager clothingManager = ClothingManager.Instance;

            //GetAllClothes() returns ClothingPiece[] which is an array of all the clothing pieces, we need it to be a list
            
            List<ClothingPiece> unlockedClothing = new List<ClothingPiece>();
            unlockedClothing = clothingManager.GetAllClothes(ClothingSelectionType.Hat).ToList();
            unlockedClothing.AddRange(clothingManager.GetAllClothes(ClothingSelectionType.Bottom).ToList());
            unlockedClothing.AddRange(clothingManager.GetAllClothes(ClothingSelectionType.Outfit).ToList());
            unlockedClothing.AddRange(clothingManager.GetAllClothes(ClothingSelectionType.Top).ToList());

            //PlayerControllerUnlocker.UnlockClothing(object caller, ClothingPiece clothingPiece) is the function that unlocks the clothing
            //we need to call it for every clothing piece in the list

            PlayerController playerController = GetPlayer();
            PlayerControllerUnlocker playerControllerUnlocker = new PlayerControllerUnlocker();
            for (int i = 0; i < unlockedClothing.Count; i++)
            {
                playerControllerUnlocker.UnlockClothing(this, unlockedClothing[i]);
            }


            Log("Unlocked all clothing! - WIP");
        }

        public void AttachConsole()
        {
            try
            {
                canvas = GameObject.Find("Game-Canvas").GetComponent<Canvas>();
                commandObject = new GameObject("commandObject");
                commandObject.transform.SetParent(canvas.transform);
                commandObject.AddComponent<TextMeshProUGUI>();
                commandText = commandObject.GetComponent<TextMeshProUGUI>();
                commandText.text = "press / to open console";
                commandText.fontSize = 40;
                commandText.color = Color.white;
                commandText.fontStyle = FontStyles.Bold;
                commandText.alignment = TextAlignmentOptions.Center;
                commandText.rectTransform.position = new Vector3(0,-500, 0);
                commandText.enableWordWrapping = false;

                popupObject = new GameObject("popupObject");
                popupObject.transform.SetParent(canvas.transform);
                popupObject.AddComponent<TextMeshProUGUI>();
                popupText = popupObject.GetComponent<TextMeshProUGUI>();
                popupText.text = "";
                popupText.fontSize = 40;
                popupText.color = Color.white;
                commandText.fontStyle = FontStyles.Bold;
                popupText.alignment = TextAlignmentOptions.Center;
                popupText.rectTransform.position = new Vector3(0, 0, 0);
                popupText.enableWordWrapping = false;
                Debug.Log("attached console");
                
            }
            catch (System.Exception e)
            {
                //Debug.Log("failed to attach console");
            }
            

        }

        public void SetJoinable()
        {
            HawkNetworking.HawkNetworkManager hawk = FindObjectOfType<HawkNetworking.HawkNetworkManager>();
            hawk.SetJoinable(hawk, true);
            //Debug.Log("Set hawk.isJoinable to true hopefull");

        }

        public void Log(string text)
        {
            Debug.Log(text);
            popupTimer = 300;
            popup = text;
        }
        
        public PlayerController GetPlayer()
        {
            return FindObjectOfType<PlayerController>();
        }

        public void ServerRewardMoneyBag(PlayerController playerController, params object[] args)
        {
            int money = (int)args[0];
            Vector3 vector;
            if (args.Length > 1)
            {
                vector = (Vector3)args[1];
            }
            else
            {
                Transform playerTransform = playerController.GetPlayerTransform();
                if (!playerTransform)
                {
                    return;
                }
                Collider[] componentsInChildren = playerTransform.GetComponentsInChildren<Collider>();
                if (componentsInChildren.Length == 0)
                {
                    return;
                }
                Bounds bounds = componentsInChildren[0].bounds;
                for (int i = 1; i < componentsInChildren.Length; i++)
                {
                    bounds.Encapsulate(componentsInChildren[i].bounds);
                }
                vector = playerTransform.position + Vector3.up * (2.5f + bounds.extents.y);
            }
            Quaternion quaternion;
            if (args.Length > 2)
            {
                quaternion = (Quaternion)args[2];
            }
            else
            {
                quaternion = Quaternion.identity;
            }
            if (money == 0)
            {
                return;
            }
            if (money < 0)
            {
                Debug.LogError("Money Bags cannot contain negative money");
                return;
            }
            NetworkPrefab.SpawnNetworkPrefab("Game/Prefabs/Props/MoneyBag.prefab", delegate(HawkNetworking.HawkNetworkBehaviour moneyNetworkBehaviour)
            {
                if (moneyNetworkBehaviour != null)
                {
                    MoneyBag component = moneyNetworkBehaviour.GetComponent<MoneyBag>();
                    if (component)
                    {
                        component.SetMoney(money);
                    }
                }
            }, new Vector3?(vector), new Quaternion?(quaternion), null, true, true, false, true);
        }

        public void AddMoney(int amount)
        {
            ServerRewardMoneyBag(GetPlayer(), amount);
        }

        public void UnlockAchievements()
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            for (int i = 0; i < achievements.Length; i++)
            {
                achievementManager.UnlockAchievement(achievements[i], player);
            }
            Log("unlocked all achievements, enjoy (; <3 daltonyx");
        }

        public void Free()
        {
            ShopPropItem[] propItems;
            propItems = FindObjectsOfType<ShopPropItem>();
            for (int i = 0; i < propItems.Length; i++)
            {
                propItems[i].cost = 0;
            }

            var instance = new Harmony("tester");
            instance.PatchAll(typeof(PatchPetShop));

            Log("free has been ran- test patching");

        }

        public void UnlockAllHouses()
        {
            SavePlayerPersistentData playerPersistentData = FindObjectOfType<PlayerController>().GetPlayerPersistentData();
            if (playerPersistentData == null)
            {
                Log("Can't find PlayerController :(");
                return;
            }
            List<System.Guid> houses = playerPersistentData.HousesData.GetHouses(SceneManager.GetActiveScene());
            PlayerControllerUnlocker playerControllerUnlocker = FindObjectOfType<PlayerControllerUnlocker>();
            if (playerControllerUnlocker == null)
            {
                Log("Can't find PlayerControllerUnlocker :(");
                return;
            }
            for (int i = 0; i < houses.Count; i++)
            {
                UnitySingleton<BuyableHouseManager>.Instance.HouseUnlocked(FindObjectOfType<PlayerController>(), houses[i]);
                PlayerControllerUnlocker.PlayerControllerUnlockerHouseDelegate playerControllerUnlockerHouseDelegate = playerControllerUnlocker.onHouseUnlocked;
                if (playerControllerUnlockerHouseDelegate == null)
		        {
			        Log("PlayerControllerUnlockerHouseDelegate was not found :(");
		        }
		        playerControllerUnlockerHouseDelegate(SceneManager.GetActiveScene(), houses[i]);
                playerPersistentData.HousesData.AddHouse(SceneManager.GetActiveScene(), houses[i]);
                playerControllerUnlocker.UnlockHouse(SceneManager.GetActiveScene(), houses[i]);
                
            }
            Log("Unlocked all houses(?) - WIP");

        }

        public GameObject FindWaypoint()
        {
            Log(GetPlayerName());
            GameObject freemodePlayerController = GameObject.Find("FreemodePlayerController_" + GetPlayerName());
            
            if (freemodePlayerController.transform.childCount > 0)
                return freemodePlayerController.transform.GetChild(0).gameObject;

            Log("Couldn't find waypoint");
            return null;
        }

        public string GetPlayerName()
        {
            PlayerController pc = FindObjectOfType<PlayerController>();
            Log(pc.GetPlayerName());
            return pc.GetPlayerName();
        }
        
        public void SpawnTextPet(string text)
        {
            PlayerControllerPet playerControllerPet = GetPlayer().GetPlayerControllerPet();
            MiscDataScriptableObject miscData = UnitySingleton<PersistentContentManager>.Instance.GetMiscData();
            Pet petPrefab = miscData.GetGhostPet();
            petPrefab.Setup(GetPlayer());
            petPrefab.SetPetName(text);
            petPrefab.SetPetNameVisible(true);
            Color transparent = new Color(0, 0, 0, 255);
            petPrefab.SetColour(transparent);
            Log("Set color and created petPrefab");
            
            PetData petData = new PetData();
            petData.petName = text;
            petData.petColor = transparent;
            petData.guid = new System.Guid();
            Log("Created a new petData and GUID");

            if (petPrefab)
            {
                Log("Setting pet to slot...");
                playerControllerPet.SetPetSlot(0, petPrefab, new PetData?(petData));
                playerControllerPet.SetActivePet(0);
                //playerControllerPet.ServerSetActivePet(petData.guid, new PetData?(petData), null, false);
            }
        }

        public void SetIndestructable()
	    {
            Log("Finished attempt to set all player vehicles to indestructible! LMK");
			return;
		}
        #endregion

        public void Update()
        {
            #region STARTUP
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                doneStartingUp = true;
                return;
            }
            if (!doneStartingUp)
            {
                return;
            }

            if (canvas == null)
            {
                AttachConsole();

                popupText.rectTransform.position = new Vector3(0, 0, 0);
                commandText.rectTransform.position = new Vector3(0, 0, 0);
                popupText.rectTransform.localScale = new Vector3(1, 1, 1);
                commandText.rectTransform.localScale = new Vector3(1, 1, 1);
                popupText.rectTransform.localPosition = new Vector3(0, 0, 0);
                commandText.rectTransform.localPosition = new Vector3(0, -500, 0);
            }
            #endregion
            else
            {

                #region UPDATE INIT / REFRESH
                if (noclipticker > 0)
                    noclipticker--;
                if (!playerController)
                    playerController = GetPlayer();

                if (popupTimer > 0)
                {
                    popupTimer--;
                    
                }
                else
                {
                    popup = "";
                }

                if (rangeHack && notDone)
                {
                    CameraFocusFree cameraFocusFree = FindObjectOfType<CameraFocusFree>();
                    if (cameraFocusFree && notDone)
                    {
                        cameraFocusFree.SetLockDistance(99999);
                        cameraFocusFree.SetLockDistanceEnabled(false);
                        notDone = false;
                    }
                }

                commandText.text = command;
                popupText.text = popup;


                if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    showConsole = !showConsole;
                    if (showConsole)
                    {
                        //commandText.color = Color.white;
                        command = "/";
                    }
                    else
                    {
                        //commandText.color = Color.clear;
                        command = "press / or Up arrow to enter commands";
                    }
                    commandText.text = command;
                }

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    rangeHack = true;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) && noclipticker == 0)
                {
                    PlayerCharacterMovement playerCharacterMovement = FindObjectOfType<PlayerCharacterMovement>();
                    playerCharacterMovement.SetNoClipEnabled(!playerCharacterMovement.IsNoClipEnabled());
                    Log("NoClip set to " + playerCharacterMovement.IsNoClipEnabled());
                    noclipticker = 50;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    commandText.color = Color.white;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    commandText.color = Color.clear;
                }

                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    if (command.Length > 1)
                    {
                        command = command.Remove(command.Length - 1);
                        commandText.text = command;
                    }
                    else
                    {
                        command = "/";
                        commandText.text = command;
                    }
                }
                #endregion

                #region COMMANDS
                if (Input.GetKeyDown(KeyCode.Return))
                {


                    if (command.ToLower().StartsWith("/test") || command.ToLower().StartsWith("test"))
                    {
                        Log("what the fuck man");
                    }
                    else if (command.ToLower().StartsWith("/achget") || command.ToLower().StartsWith("achget"))
                    {
                        UnlockAchievements();
                    }
                    else if (command.ToLower().StartsWith("/chat") || command.ToLower().StartsWith("chat"))
                    {
                        string[] strings = command.Split();
                        //every index of strings that is not 0 is the _same string_
                        string message = "";
                        
                        for (int i = 0; i < strings.Length; i++)
                        {

                            message += strings[i];
                            message += " ";

                        }
                        Log("message to spawn: " + message);
                        SpawnTextPet(message);
                        Log("Spawned text pet? WIP");

                    }
                    else if (command.ToLower().StartsWith("/indestructible") || command.ToLower().StartsWith("indestructible"))
                    {
                        var instance = new Harmony("indestructibleVehicle");
                        instance.PatchAll(typeof(PatchPlayerVehicle));
                        SetIndestructable();
                    }
                    else if (command.ToLower().StartsWith("/cash") || command.ToLower().StartsWith("cash"))
                    {
                        string[] strings = command.Split();
                        int choice = int.Parse(strings[1]);
                        if (choice < 0 || choice > 2147000000)
                        {
                            Log("Choose an amount 0 - 2147000000");
                            return;
                        }
                        AddMoney(choice);
                    }
                    else if (command.ToLower().StartsWith("/start") || command.ToLower().StartsWith("start"))
                    {
                        StartArcade();
                    }
                    else if (command.ToLower().StartsWith("/maxpets") || command.ToLower().StartsWith("maxpets"))
                    {
                        
                       

                        
                    }
                    else if (command.ToLower().StartsWith("/jump") || command.ToLower().StartsWith("jump"))
                    {
                        string[] strings = command.Split();
                        float choice = float.Parse(strings[1]);
                        if (choice < 0 || choice > 100)
                        {
                            Log("Choose an amount 0 - 100");
                            return;
                        }
                        PlayerCharacterMovement playerCharacterMovement = FindObjectOfType<PlayerCharacterMovement>();
                        playerCharacterMovement.SetJumpMultiplier(choice);
                        Log("Set Jump Multiplier to " + choice + "!");

                        
                    }
                    else if (command.ToLower().StartsWith("/explode") || command.ToLower().StartsWith("explode"))
                    {
                        
                        PlayerCharacterMovement playerCharacterMovement = FindObjectOfType<PlayerCharacterMovement>();
                        //playerController.
                        //playerCharacterMovement.Explode();

                        
                    }
                    else if (command.ToLower().StartsWith("/map") || command.ToLower().StartsWith("map"))
                    {
                        ArcadeInstance.Instance.GetLobbyInstance().GetLobbyInfoData().officalArcadeModeIndex = UnityEngine.Random.RandomRange(0,3);
                        while (ArcadeInstance.Instance.GetLobbyInstance().GetLobbyInfoData().officalArcadeModeIndex == 1)
                        {
                            ArcadeInstance.Instance.GetLobbyInstance().GetLobbyInfoData().officalArcadeModeIndex = UnityEngine.Random.RandomRange(0, 3);
                        }
                        Log("Map set to " + ArcadeInstance.Instance.GetLobbyInstance().GetLobbyInfoData().officalArcadeModeIndex + "!");
                        StartArcade();

                    }
                    else if (command.ToLower().StartsWith("/start2") || command.ToLower().StartsWith("start2"))
                    {
                        StartAutomaticArcade();

                    }
                    else if (command.ToLower().StartsWith("/auto") || command.ToLower().StartsWith("auto"))
                    {
                        automatic = !automatic;
                        Log("Automatic: " + automatic);
                    }
                    else if (command.ToLower().StartsWith("/users") || command.ToLower().StartsWith("users"))
                    {
                        string[] strings = command.Split();
                        int choice = int.Parse(strings[1]);
                        if (choice < 0 || choice > 100)
                        {
                            Log("Choose an amount 0 - 100");
                            return;
                        }
                        peopleAllowed = choice;
                        Log("People allowed set to " + choice + "!");

                        var instance = new Harmony("tester");
                        instance.PatchAll(typeof(PatchGameInstance));
                        var instance2 = new Harmony("tester2");
                        instance2.PatchAll(typeof(PatchSteamP2PNetworkManager));
                    }
                    else if (command.ToLower().StartsWith("/speed") || command.ToLower().StartsWith("speed"))
                    {
                        string[] strings = command.Split();
                        float choice = float.Parse(strings[1]);

                        ChangeSpeed(choice);
                        Log("Speed multiplier set to " + choice + "!");
                    }
                    else if (command.ToLower().StartsWith("/tp") || command.ToLower().StartsWith("tp"))
                    {
                        string[] strings = command.Split();
                        int choice = int.Parse(strings[1]);

                        GameObject devTeleportManager = GameObject.Find("DeveloperTeleportManager");

                        if (choice > devTeleportManager.transform.childCount - 1 || choice < 0)
                        {
                            Log("Choose a number between 0 and " + (devTeleportManager.transform.childCount - 1));
                            return;
                        }
                        for (int i = 0; i < devTeleportManager.transform.childCount; i++)
                        {
                            if (choice == i)
                            {
                                Teleport(devTeleportManager.transform.GetChild(i).position);
                            }
                        }
                    }
                    else if (command.ToLower().StartsWith("/tp2") || command.ToLower().StartsWith("tp2"))
                    {
                        string[] strings = command.Split();
                        int choice = int.Parse(strings[1]);

                        GameObject devTeleportManager = GameObject.Find("DeveloperTeleportManager");

                        if (choice > devTeleportManager.transform.childCount - 1 || choice < 0)
                        {
                            Log("Choose a number between 0 and " + (devTeleportManager.transform.childCount - 1));
                            return;
                        }
                        for (int i = 0; i < devTeleportManager.transform.childCount; i++)
                        {
                            if (choice == i)
                            {
                                TeleportAll(devTeleportManager.transform.GetChild(i).position);
                            }
                        }
                    }
                    else if (command.ToLower().StartsWith("/pets") || command.ToLower().StartsWith("pets"))
                    {
                        Free();
                    }
                    else if (command.ToLower().StartsWith("/house") || command.ToLower().StartsWith("house"))
                    {
                        var instance = new Harmony("tester");
                        instance.PatchAll(typeof(PatchPlayerControllerUnlockerHouse));

                        Log("All houses are unlocked - WIP");

                        UnlockAllHouses();
                    }
                    else if (command.ToLower().StartsWith("/car") || command.ToLower().StartsWith("car"))
                    {
                        var instance = new Harmony("tester");
                        instance.PatchAll(typeof(PatchPlayerControllerUnlockerVehicle));

                        Log("All vehicles are unlocked - WIP");
                    }
                    else if (command.ToLower().StartsWith("/clothes") || command.ToLower().StartsWith("clothes"))
                    {
                        var instance = new Harmony("tester");
                        instance.PatchAll(typeof(PatchPlayerControllerUnlockerClothing));
                        instance.PatchAll(typeof(PatchClothesShop));
                        Log("All clothes are unlocked - WIP");
                    }
                    else if (command.ToLower().StartsWith("/presents") || command.ToLower().StartsWith("presents"))
                    {
                        var instance = new Harmony("tester");
                        instance.PatchAll(typeof(PatchPlayerControllerUnlockerPresent));

                        Log("All presents are unlocked - WIP");
                    }
                    else if (command.ToLower().StartsWith("flight") || command.ToLower().StartsWith("/flight"))
                    {
                        string[] strings = command.Split();
                        bool choice = bool.Parse(strings[1]);
                        PlayerCharacterMovement playerCharacterMovement = FindObjectOfType<PlayerCharacterMovement>();
                        playerCharacterMovement.SetNoClipEnabled(choice);
                        Log("Set noClip to " + choice + "!");
                        
                    }
                    else if (command.ToLower().StartsWith("tp3") || command.ToLower().StartsWith("/tp3"))
                    {
                        GameObject waypoint = new GameObject();
                        Log(GetPlayerName());
                        waypoint = FindWaypoint();
                        if (waypoint != null)
                        {
                            Teleport(waypoint.transform.position);
                            Log("Teleported to waypoint");
                        }
                        else
                        {
                            Log("No waypoint found");
                        }

                    }
                    else if (command.ToLower().StartsWith("range") || command.ToLower().StartsWith("/range"))
                    {
                        
                        CameraFocusFree cameraFocusFree = FindObjectOfType<CameraFocusFree>();
                        cameraFocusFree.SetLockDistance(99999);
                        cameraFocusFree.SetLockDistanceEnabled(false);

                    }

                    showConsole = false;
                    command = "press / or Up arrow to open console";
                }
                #endregion

                #region INPUTS
                if (showConsole)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        command += "a";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.B))
                    {
                        command += "b";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.C))
                    {
                        command += "c";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        command += "d";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        command += "e";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.F))
                    {
                        command += "f";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.G))
                    {
                        command += "g";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.H))
                    {
                        command += "h";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.I))
                    {
                        command += "i";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.J))
                    {
                        command += "j";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.K))
                    {
                        command += "k";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.L))
                    {
                        command += "l";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.M))
                    {
                        command += "m";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.N))
                    {
                        command += "n";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.O))
                    {
                        command += "o";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.P))
                    {
                        command += "p";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        command += "q";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        command += "r";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        command += "s";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.T))
                    {
                        command += "t";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.U))
                    {
                        command += "u";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.V))
                    {
                        command += "v";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.W))
                    {
                        command += "w";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.X))
                    {
                        command += "x";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Y))
                    {
                        command += "y";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        command += "z";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        command += "0";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        command += "1";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        command += "2";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        command += "3";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        command += "4";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        command += "5";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        command += "6";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha7))
                    {
                        command += "7";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha8))
                    {
                        command += "8";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        command += "9";
                        commandText.text = command;
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        command += " ";
                        commandText.text = command;
                    }
                }
                #endregion 
            }
        }
    }

    #region PATCHES

    class PatchPetShop
    {
        [HarmonyPatch(typeof(PetShop), "IsFree")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool IsFreePrefix(ref bool __result)
        {
            __result = true;
            return false;
        }

        [HarmonyPatch(typeof(PetShop), "CanAfford")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool CanAffordPrefix(ref bool __result)
        {
            __result = false;
            return false;
        }

        [HarmonyPatch(typeof(PetShop), "GetPetCost")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool GetPetCostPrefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }

    class PatchPlayerControllerUnlockerClothing
    {
        [HarmonyPatch(typeof(PlayerControllerUnlocker), "IsClothingUnlocked")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool IsClothingUnlockedPrefix(ref bool __result)
        {
		    __result = true;
            return false;
        }
    }

    class PatchClothesShop
    {
        [HarmonyPatch(typeof(ClothesShop), "GetOverallPrice")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool GetOverallPricePrefix(ref int __result)
        {
		    __result = 0;
            return false;
        }

        [HarmonyPatch(typeof(ClothesShop), "GetItemPrice")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool GetItemPricePrefix(ref int __result)
        {
		    __result = 0;
            return false;
        }

    }

    class PatchPlayerControllerUnlockerVehicle
    {
        [HarmonyPatch(typeof(PlayerControllerUnlocker), "IsVehicleUnlocked", new System.Type[] {typeof(PlayerVehicle) })] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool IsVehicleUnlockedPrefix(ref PlayerVehicle vehicle, ref bool __result)
        {
		    __result = true;
            return false;
        }

         [HarmonyPatch(typeof(PlayerControllerUnlocker), "IsVehicleUnlocked", new System.Type[] {typeof(System.Guid) })] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool IsVehicleUnlocked2Prefix(ref System.Guid guid, ref bool __result)
        {
		    __result = true;
            return false;
        }
    }

    class PatchPlayerControllerUnlockerPresent
    {
        [HarmonyPatch(typeof(PlayerControllerUnlocker), "IsPresentUnlocked")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool IsPresentUnlockedPrefix(ref bool __result)
        {
		    __result = true;
            return false;
        }

        [HarmonyPatch(typeof(PlayerControllerUnlocker), "HasUnlockedAllPresentsInActiveScene")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]
        static bool HasUnlockedAllPresentsInActiveScenePrefix(ref bool __result)
        {
		    __result = true;
            return false;
        }
    }

    class PatchPlayerControllerUnlockerHouse
    {
        [HarmonyPatch(typeof(PlayerControllerUnlocker), "IsHouseUnlocked", new System.Type[] { typeof(Scene), typeof(System.Guid) })]
        [HarmonyPrefix]
        static bool IsHouseUnlockedPrefix(ref Scene scene, ref System.Guid houseGUID, ref bool __result)
        {
            __result = true;
            return false;
        }

        [HarmonyPatch(typeof(PlayerControllerUnlocker), "IsHouseUnlocked", new System.Type[] { typeof(BuyableHouse) })]
        [HarmonyPrefix]
        static bool IsHouseUnlocked2Prefix(ref BuyableHouse house, ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    class PatchHawkNetworkManager
    {

        [HarmonyPatch(typeof(HawkNetworking.HawkNetworkManager), "IsJoinable")]
        [HarmonyPrefix]
        static bool IsJoinablePrefix(ref bool __result)
        {
            __result = true;
            return false;
        }
        
    }

    class PatchSteamP2PNetworkManager
    {
        [HarmonyPatch(typeof(SteamP2PNetworkManager), "GetMaxPlayerCount")]
        [HarmonyPrefix]
        static bool GetMaxPlayerCountPrefix(ref int __result)
        {
            __result = Plugin.Instance.peopleAllowed;
            return false;
        }

    }

    class PatchPlayerVehicle
    {
        [HarmonyPatch(typeof(PlayerVehicle), "IsIndestructible")]
        [HarmonyPrefix]
        static bool IsIndestructiblePrefix(ref bool __result)
        {
            __result = true;
            return false;
        }

        [HarmonyPatch(typeof(PlayerVehicle), "IsIndestructible")]
        [HarmonyPostfix]
        static bool IsIndestructiblePostfix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    class PatchLobbySelectCanvas
    {
        [HarmonyPatch(typeof(LobbySelectCanvas), "IsFullOrInvalid")]
        [HarmonyPrefix]
        static bool IsIndestructiblePrefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }

    class PatchGameInstance
    {
        [HarmonyPatch(typeof(GameInstance), "GetMaxAllowedPlayers_Arcade")]
        [HarmonyPrefix]
        static bool GetMaxPlayerCount1Prefix(ref int __result)
        {
            __result = Plugin.Instance.peopleAllowed;
            return false;
        }
        [HarmonyPatch(typeof(GameInstance), "GetMaxAllowedPlayers_Game")]
        [HarmonyPrefix]
        static bool GetMaxPlayerCount2Prefix(ref int __result)
        {
            __result = Plugin.Instance.peopleAllowed;
            return false;
        }
    }
    #endregion

}
