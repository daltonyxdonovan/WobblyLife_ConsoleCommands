using BepInEx;
using Steamworks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace WobblyLife_ConsoleCommands
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        AchievementManager achievementManager;
        public Canvas canvas;
        public GameObject commandObject;
        public GameObject popupObject;
        public TextMeshProUGUI commandText;
        public TextMeshProUGUI popupText;
        public string command = "press / to open console";
        public string popup = "";
        public int popupTimer = 0;
        bool showConsole = false;
        bool doneStartingUp = false;
        RewardMoneyData rewardMoneyData = FindObjectOfType<RewardMoneyData>();

        WobblyAchievement[] achievements = {
            WobblyAchievement.COMPLETE_JOB_JELLY,
	        // Token: 0x0400215A RID: 8538
            WobblyAchievement.COMPLETE_JOB_PIZZA,
            // Token: 0x0400215B RID: 8539
            WobblyAchievement.COMPLETE_JOB_BURGER,
            // Token: 0x0400215C RID: 8540
            WobblyAchievement.COMPLETE_JOB_POWER_PLANT,
            // Token: 0x0400215D RID: 8541
            WobblyAchievement.COMPLETE_JOB_EMERGENCY,
            // Token: 0x0400215E RID: 8542
            WobblyAchievement.COMPLETE_JOB_NEWROUND,
            // Token: 0x0400215F RID: 8543
            WobblyAchievement.COMPLETE_JOB_FURNITURE,
            // Token: 0x04002160 RID: 8544
            WobblyAchievement.COMPLETE_JOB_PIZZA_UFO,
            // Token: 0x04002161 RID: 8545
            WobblyAchievement.COMPLETE_JOB_GARBAGE,
            // Token: 0x04002162 RID: 8546
            WobblyAchievement.COMPLETE_RACE_KART,
            // Token: 0x04002163 RID: 8547
            WobblyAchievement.COMPLETE_RACE_PLANE,
            // Token: 0x04002164 RID: 8548
            WobblyAchievement.COMPLETE_RACE_BOAT,
            // Token: 0x04002165 RID: 8549
            WobblyAchievement.COMPLETE_TEMPLE_PUZZLE,
            // Token: 0x04002166 RID: 8550
            WobblyAchievement.BUY_FIRST_HOUSE,
            // Token: 0x04002167 RID: 8551
            WobblyAchievement.HAVE_1000_IN_THE_BANK,
            // Token: 0x04002168 RID: 8552
            WobblyAchievement.HAVE_5000_IN_THE_BANK,
            // Token: 0x04002169 RID: 8553
            WobblyAchievement.HAVE_10000_IN_THE_BANK,
            // Token: 0x0400216A RID: 8554
            WobblyAchievement.COLLECT_ALL_PRESENTS_ON_WOBBLY_ISLAND,
            // Token: 0x0400216B RID: 8555
            WobblyAchievement.FEED_MONSTER_25_TOXIC_BARRELS,
            // Token: 0x0400216C RID: 8556
            WobblyAchievement.FEED_MONSTER_50_TOXIC_BARRELS,
            // Token: 0x0400216D RID: 8557
            WobblyAchievement.COMPLETE_JOB_FARM_PLOW,
            // Token: 0x0400216E RID: 8558
            WobblyAchievement.COMPLETE_JOB_FARM_SEED,
            // Token: 0x0400216F RID: 8559
            WobblyAchievement.COMPLETE_JOB_FARM_HARVEST,
            // Token: 0x04002170 RID: 8560
            WobblyAchievement.PROCESS_URANIUM_IN_MINE_MACHINE,
            // Token: 0x04002171 RID: 8561
            WobblyAchievement.BUY_FIRST_PET,
            // Token: 0x04002172 RID: 8562
            WobblyAchievement.UNLOCK_GHOST_PET,
            // Token: 0x04002173 RID: 8563
            WobblyAchievement.CHOOSE_WISELY,
            // Token: 0x04002174 RID: 8564
            WobblyAchievement.COMPLETE_JOB_QUIZ_MASTER,
            // Token: 0x04002175 RID: 8565
            WobblyAchievement.COMPLETE_JOB_FIRE_FIGHTER,
            // Token: 0x04002176 RID: 8566
            WobblyAchievement.COMPLETE_JOB_WOOD_CUTTER,
            // Token: 0x04002177 RID: 8567
            WobblyAchievement.COMPLETE_JOB_SCIENCE_MACHINE,
            // Token: 0x04002178 RID: 8568
            WobblyAchievement.COMPLETE_BUILD_UFO_MISSION,
            // Token: 0x04002179 RID: 8569
            WobblyAchievement.COMPLETE_FIRST_MUSEUM_COLLECTION,
            // Token: 0x0400217A RID: 8570
            WobblyAchievement.COMPLETE_JOB_TAXI,
            // Token: 0x0400217B RID: 8571
            WobblyAchievement.COMPLETE_JOB_ICE_CREAM,
            // Token: 0x0400217C RID: 8572
            WobblyAchievement.COMPLETE_ANCIENT_TRIALS,
            // Token: 0x0400217D RID: 8573
            WobblyAchievement.COMPLETE_JOB_DISCO,
            // Token: 0x0400217E RID: 8574
            WobblyAchievement.COMPLETE_JOB_FISHING,
            // Token: 0x0400217F RID: 8575
            WobblyAchievement.COLLECT_ALL_FISH_ON_WOBBLY_ISLAND,
            // Token: 0x04002180 RID: 8576
            WobblyAchievement.COMPLETE_JELLY_CAR_MISSION,
            // Token: 0x04002181 RID: 8577
            WobblyAchievement.COMPLETE_JOB_CONSTRUCTION_RESOURCES,
            // Token: 0x04002182 RID: 8578
            WobblyAchievement.COMPLETE_JOB_CONSTRUCTION_BUILDING,
            // Token: 0x04002183 RID: 8579
            WobblyAchievement.COMPLETE_JOB_CONSTRUCTION_DESTRUCTION


        };

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            achievementManager = AchievementManager.Instance;

        }

        public void Start()
        {
            

        }

        public void AttachConsole()
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

        public void Log(string text)
        {
            Debug.Log(text);
            popupTimer = 300;
            popup = text;
        }
        
        public void AddMoney(int amount)
        {
            MoneyBag moneyBag = FindObjectOfType<MoneyBag>();
            moneyBag.SetMoney(moneyBag.GetMoney() + amount);

            Log($"added money\n${moneyBag.GetMoney()}");
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

        public void Update()
        {
            
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
            }
            else
            {
                popupText.rectTransform.position = new Vector3(0, 0, 0);
                commandText.rectTransform.position = new Vector3(0, 0, 0);
                popupText.rectTransform.localScale = new Vector3(1, 1, 1);
                commandText.rectTransform.localScale = new Vector3(1, 1, 1);
                popupText.rectTransform.localPosition = new Vector3(0, 0, 0);
                commandText.rectTransform.localPosition = new Vector3(0, -500, 0);

                if (popupTimer > 0)
                {
                    popupTimer--;
                    
                }
                if (popupTimer <= 0)
                {
                    popup = "";
                }

                commandText.text = command;
                popupText.text = popup;


                if (Input.GetKeyDown(KeyCode.Slash))
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
                        command = "press / to enter commands";
                    }
                    commandText.text = command;
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

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (command.ToLower().StartsWith("/test"))
                    {
                        Log("what the fuck man");
                    }
                    else if (command.ToLower().StartsWith("/achget"))
                    {
                        UnlockAchievements();
                    }
                    else if (command.ToLower().StartsWith("/cash"))
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

                    showConsole = false;
                    command = "press / to open console";
                }

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

            }
        }
    }
}
