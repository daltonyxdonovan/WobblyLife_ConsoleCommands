using BepInEx;
using Steamworks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        public string command = "";
        public string popup = "";
        public int popupTimer = 0;
        bool showConsole = true;
        bool doneStartingUp = false;


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
            commandText.fontSize = 20;
            commandText.color = Color.white;
            commandText.alignment = TextAlignmentOptions.Center;
            commandText.rectTransform.position = new Vector3(0,-500, 0);
            commandText.enableWordWrapping = false;

            popupObject = new GameObject("popupObject");
            popupObject.transform.SetParent(canvas.transform);
            popupObject.AddComponent<TextMeshProUGUI>();
            popupText = popupObject.GetComponent<TextMeshProUGUI>();
            popupText.text = "";
            popupText.fontSize = 20;
            popupText.color = Color.white;
            popupText.alignment = TextAlignmentOptions.Center;
            popupText.rectTransform.position = new Vector3(0, 0, 0);
            popupText.enableWordWrapping = false;
            Debug.Log("attached console");
            Debug.Log(canvas);
            Debug.Log(commandText);
            Debug.Log(popupText);
        }

        public void Log(string text)
        {
            Debug.Log(text);
            popupTimer = 1200;
            popupText.text = text;
            
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
                commandText.rectTransform.position = new Vector3(0, -500, 0);
                //set scale of both to 1,1,1
                popupText.rectTransform.localScale = new Vector3(1, 1, 1);
                commandText.rectTransform.localScale = new Vector3(1, 1, 1);
                //set local position of both to 0,0,0
                popupText.rectTransform.localPosition = new Vector3(0, 0, 0);
                commandText.rectTransform.localPosition = new Vector3(0, 400, 0);
                if (popupTimer > 0)
                {
                    popupTimer--;
                    
                }
                if (popupTimer == 0)
                {
                    popupText.text = "";
                }
                commandText.text = command;
                popupText.text = popup;


                if (Input.GetKeyDown(KeyCode.Slash))
                {
                    showConsole = !showConsole;
                    if (showConsole)
                    {
                        commandText.color = Color.white;
                        command = "/";
                    }
                    else
                    {
                        commandText.color = Color.clear;
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

                    command = "";
                    showConsole = false;
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
