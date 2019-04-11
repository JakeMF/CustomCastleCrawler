using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace CustomCastleCrawler
{
    //Encrypter
    public sealed class Crypter
    {
        private string Key;
        
        //Constructor with a default key value, this should NEVER be used.
        public Crypter()
        {
            Key = "I'mADefaultKeyPleaseChangeMe";
        }

        //Function that takes the key as a parameter, this should be used 100% of the time.
        public Crypter(string key)
        {
            this.Key = key;
        }

        //method to encrypt a string, returns encrypted string.
        public string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            keyArray = UTF8Encoding.UTF8.GetBytes(Key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            foreach (KeySizes d in tdes.LegalKeySizes)
            {
                Console.WriteLine(d.MinSize);
                Console.WriteLine(d.MaxSize);
            }
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        //method to decrypt a string, returns decrypted string
        public string Decrypt(string cipherString)
        {
            byte[] keyArray;
            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            keyArray = UTF8Encoding.UTF8.GetBytes(Key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
    
    public sealed class GameData
    {
        public int XMax { get; set; }
        public int YMax { get; set; }
        public string GameName { get; set; }
        public string GameFlavorText { get; set; }
        public int StartingX { get; set; }
        public int StartingY { get; set; }
        public int MaxEstus { get; set; }
        public int PlayerMaxHealth { get; set; }
        public int PlayerMaxStamina { get; set; }
        public bool Debug { get; set; }

        public List<StartingClass> Classes = new List<StartingClass>();
        
        public GameData()
        {
            //Load the game's basic configurations from XML.
            LoadGameData();
        }

        //Function to load game settings from the XML.
        private void LoadGameData()
        {
            try
            {
                XDocument xdoc = XDocument.Load("GameSettings.xml");
                //Populate list with tile objects to store in array later

                //Map Size Settings
                foreach (var elem in xdoc.Root.Elements("MapSize"))
                {
                    XMax = Convert.ToInt16(elem.Element("XMax").Value);
                    YMax = Convert.ToInt16(elem.Element("YMax").Value);
                }
                //Starting Position
                foreach (var elem in xdoc.Root.Elements("StartingPosition"))
                {
                    StartingX = Convert.ToInt16(elem.Element("DefaultX").Value);
                    StartingY = Convert.ToInt16(elem.Element("DefaultY").Value);
                }
                //Balance Settings
                foreach (var elem in xdoc.Root.Elements("BalanceSettings"))
                {
                    MaxEstus = Convert.ToInt16(elem.Element("BaseMaxHeals").Value);
                    Debug = Convert.ToBoolean(elem.Element("Debug").Value);
                    PlayerMaxHealth = !string.IsNullOrEmpty(elem.Element("PlayerMaxHealth").Value) ? Convert.ToInt16(elem.Element("PlayerMaxHealth").Value) : 1200;
                    //If the max stamina is left blank, set it to 100.
                    PlayerMaxStamina = !string.IsNullOrEmpty(elem.Element("PlayerMaxStamina").Value) ? Convert.ToInt16(elem.Element("PlayerMaxStamina").Value) : 100;
                }
                //Flavor Settings
                foreach (var elem in xdoc.Root.Elements("FlavorSettings"))
                {
                    GameName = elem.Element("GameName").Value;
                    GameFlavorText = elem.Element("GameFlavorText").Value;
                }
                //Class Settings
                foreach (var elem in xdoc.Root.Elements("Classes"))
                {
                    foreach (var childElem in elem.Elements("Class"))
                    {
                        Classes.Add(new StartingClass(childElem.Element("Name").Value, childElem.Element("Armor").Value, childElem.Element("Weapon").Value, childElem.Element("ClassDescription").Value));
                    }
                }
            }
            catch (Exception exception)
            {
                //Ensure log file directory exists.
                System.IO.Directory.CreateDirectory("LogFiles/");

                //Set file path
                var path = "LogFiles/XMLErrorLog.txt";

                //Log error
                var returnString = new StringBuilder();

                returnString.AppendLine(string.Format("{0} - {1} error at: {2}", DateTime.Now, GameName, "LoadGameData()"));
                returnString.AppendLine(exception.Message);
                returnString.AppendLine(exception.StackTrace);
                returnString.AppendLine();

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(returnString);
                }

                MessageBox.Show("There was a problem loading your Game Settings, ensure your data is formatted properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Quit the application because their data was not loaded properly, the game will not work properly.
                Environment.Exit(0);
            }
        }
    }
    
    //Simple class to store the attributes of a starting class.
    public sealed class StartingClass
    {
        public string ClassWeapon;
        public string ClassArmor;
        public string ClassName;
        public string ClassDescription;
        
        //constructor that takes all values as a parameter.
        public StartingClass(string name, string armor, string weapon, string description)
        {
            ClassName = name;
            ClassWeapon = weapon;
            ClassArmor = armor;
            ClassDescription = description;
        }
    }

    //Class to represent a pair of (X,Y) coordinates
    public sealed class Coords
    {
        public int X;
        public int Y;
        private GameData MapDimensions;

        public Coords()
        {
            X = 0;
            Y = 0;
            MapDimensions = new GameData();
        }
        //Go West
        public bool West()
        {
            if (X > 0)
            {
                --X;
                return true;
            }
            return false;
        }
        //Go North
        public bool North()
        {
            if (Y > 0)
            {
                --Y;
                return true;
            }
            return false;
        }
        //Go East
        public bool East()
        {
            if (X + 1 < MapDimensions.XMax)
            {
                ++X;
                return true;
            }
            return false;
        }
        //Go South
        public bool South()
        {
            if (Y + 1 < MapDimensions.YMax)
            {
                ++Y;
                return true;
            }
            return false;
        }
    }

    //Class for one tile on the map, used in multidimensional array to represent X-Y coordinate plane
    public sealed class MapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Message { get; set; }
        public int EventID { get; set; }
        //Default Constructor
        public MapTile()
        {
            X = -1;
            Y = -1;
            Message = "You are unable to go that way.";
            EventID = -1;
        }

        //Constructor with minimum arguments
        public MapTile(int x, int y)
        {
            X = x;
            Y = y;
            Message = "You are unable to go that way.";
            EventID = -1;
        }

        //Constructor with all arguments
        public MapTile(int x, int y, string message, int eventID)
        {
            X = X;
            Y = Y;
            Message = message;
            EventID = eventID;
        }
    }

    //Class for an event
    public sealed class Event
    {
        public int EventID;
        public int EnemySpawn;
        public int ItemSpawn;
        public int NothingSpawn;
        public int WeaponChance;
        public int ArmorChance;
        public int ScoreItemChance;
        public int RestLocation;
        public int EnemySpawnZone;

        //Default constructor that initlizes all values.
        public Event()
        {
            EventID = -1;
            EnemySpawn = 0;
            ItemSpawn = 0;
            NothingSpawn = 0;
            WeaponChance = 0;
            ArmorChance = 0;
            ScoreItemChance = 0;
            RestLocation = 0;
            EnemySpawnZone = 0;
        }

        //Additional constructors are not needed because events should always be loaded from the XML.
    }

    //Class that extends C# Random()
    public sealed class MyRandom
    {
        private Random MyRand;
        //Default constructor.
        public MyRandom()
        {
            MyRand = new Random();
        }

        //roll a die with n number of sides (starts at 1)
        public int rollDie(int n)
        {
            // Incrementing parameter 'n' by 1 because the .Next function's max range is exclusive.
            n++;

            int num = 0;
            num = MyRand.Next(1, n);
            return num;
        }

        //roll a die with 10 sides, useful for % chances
        public int rollDTen()
        {
            return rollDie(10);
        }

        //Simulates coin toss
        public bool rollTrueFalse()
        {
            if (rollDie(2) == 1)
            {
                return true;
            }
            return false;
        }
    }

    //Class for every item in the game, inherited by Weapon and Armor
    public class Item
    {
        public string Name { get; set; }
        //Used as an ID field
        public int Value { get; set; }
        public string Description { get; set; }

        //Basic Constructor
        public Item()
        {
            Name = "Gold Ring.";
            Value = 2; 
            Description = "A simple gold band.";
        }

        //Constructor that takes parameters for all attributes
        public Item(string name, int value, string description)
        {
            Name = name;
            Value = value;
            Description = description;
        }
    }

    //Class for a weapon
    public class Weapon : Item
    {
        public int BDamage { get; set; }
        public int APDamage { get; set; }
        public int Evasion { get; set; }
        public int Rarity { get; set; }

        //Default constructor that creates your default gear.
        public Weapon()
        {
            Name = "Wooden Sword";
            Value = 2;
            BDamage = 5;
            APDamage = 0;
            Evasion = 0;
        }

        //Constructor with all values
        public Weapon(string name, int value, string description, int bDamage, int aPDamage, int evasion)
        {
            Name = name;
            Value = value;
            Description = description;
            BDamage = bDamage;
            APDamage = aPDamage;
            Evasion = evasion;
        }

    }

    //Class for an armor set
    public class Armor : Item
    {
        public int ArmorVal { get; set; }
        public int Evasion { get; set; }
        public int Rarity { get; set; }

        //Default constructor that creates your default gear.
        public Armor()
        {
            Name = "Rusted Chainmail";
            Value = 2;
            ArmorVal = 2;
            Evasion = 0;
        }

        //Constructor with all values.
        public Armor(string name, int value, string description, int armorVal, int evasion)
        {
            Name = name;
            Value = value;
            Description = description;
            ArmorVal = armorVal;
            Evasion = evasion;
        }
    }

    //Class for the player themselves
    public sealed class Player
    {
        public string Name { get; }
        private int MaxHealth;
        private int Health;
        private int MaxStamina;
        private int Stamina;
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public int Score { get; set; }
        public int EnemiesKilled = 0;
        public int MaxEstus;
        public int Estus;

        //Load game data to grab max estus.
        private GameData GameData;

        //Default constructor
        public Player()
        {
            //Load game settings
            GameData = new GameData();

            Name = "Nameless Hollow";
            MaxHealth = GameData.PlayerMaxHealth;
            Health = MaxHealth;
            MaxStamina = 100;
            Stamina = MaxStamina;
            Weapon = new Weapon();
            Armor = new Armor();
            Score = 0;
            MaxEstus = GameData.MaxEstus;
            Estus = MaxEstus;
        }

        //Constructor that will be used for new players
        public Player(string name, Weapon startingWeapon, Armor startingArmor)
        {
            //Load game settings
            GameData = new GameData();

            Name = name;
            MaxHealth = GameData.PlayerMaxHealth;
            Health = MaxHealth;
            MaxStamina = GameData.PlayerMaxStamina;
            Stamina = MaxStamina;

            Weapon = startingWeapon;
            Armor = startingArmor;
            Score = 0;

            MaxEstus = GameData.MaxEstus;
            Estus = MaxEstus;
        }

        //Constructor that will be used to load a saved game
        public Player(string name, int maxHealth, int health, int maxStamina, int stamina, Weapon weapon, Armor armor, int score)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            MaxStamina = maxStamina;
            Stamina = stamina;
            Weapon = weapon;
            Armor = armor;
            Score = score;
            //Players get 5 heals by default, like in Dark Souls 1. 
            MaxEstus = 5;
            Estus = MaxEstus;
        }

        //Function to get the player's data in a csv format to save.
        public string getSaveData()
        {
            string data;
            char delim = ',';
            
            //Return player values separated by delimiter
            return Name + delim + MaxHealth + delim + Health + delim + MaxStamina + delim + Stamina + delim + Weapon.Name + delim + Armor.Name + delim + Score;
        }
        
        //function to get the players evasion
        public int GetEvasion()
        {
            //A player's evasion is modified by their weapon and armor, with a base percentage of 20.
            int Evasion = Weapon.Evasion + Armor.Evasion + 20;
            if (Evasion > 0)
            {
                return Evasion;
            }
            else
            {
                return 0;
            }
        }

        //function that causes the player to take damage. Can be used to simply return health if 0 is passed.
        public int Injure(int dmg)
        {
            //Subtract damaged amount from current health.
            Health -= dmg;
            return Health;
        }

        //Function to use one heal, this checks available heals and either heals the user or informs them that they do not have any heals yet.
        public string DrinkEstus()
        {
            if(Estus > 0 && Estus <= MaxEstus)
            {
                Estus--;
                Health = MaxHealth;
                return "You drank from your health flask, you have been healed. " + Estus + " sips remaining.";
            }
            else
            {
                return "Your health flask is empty. You need to find a rest location to heal again.";
            } 
        }

        //Function to refil the player's healing item.
        public string RefillEstus()
        {
            Estus = MaxEstus;
            return "Your healing flask has been filled. " + Estus + " sips remaining.";
        }
        
        //Function to perform a stamin draining action. Returns true if action could be completed with current stam, false if not enough stam.
        public bool PerformAction(int staminaCost)
        {
            int tempStam = Stamina;
            Stamina -= staminaCost;

            if(Stamina >= 0)
            {
                return true;
            }
            else
            {
                Stamina = tempStam;
                return false;
            }
        }

        //Function to regen the player's stamina by a specified amount.
        public int RegenStamina(int regen)
        {
            Stamina += regen;

            if(Stamina > MaxStamina)
            {
                Stamina = MaxStamina;
            }

            return Stamina;
        }

        //Function to return the player's health and stamina as a pipe separated string.
        public string GetHealthAndStamina()
        {
            return Health + "/" + MaxHealth + "|" + Stamina + "/" + MaxStamina;
        }
    }

    //Class for enemies
    public sealed class Enemy
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        //Damage
        public int Damage { get; set; }
        public int ApDamage { get; set; }

        //Defense
        //Values are %/100
        public double Defence { get; set; }
        public int Evasion { get; set; }
        public int Score { get; set; }
        public int SpawnZone { get; set; }

        //Default constructor
        public Enemy()
        {
            Name = "Hollow Soldier";
            MaxHealth = 125;
            CurrentHealth = MaxHealth;
            //Damage
            Damage = 5;
            ApDamage = 2;
            
            //Defence
            Defence = 10;
            Evasion = 5;
        }

        //Constructor that takes parameters for all attributes
        public Enemy(string name, int maxHealth, int score, int damage, int apDamage, double defence, int evasion = 5)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;

            Damage = damage;
            ApDamage = apDamage;

            Defence = defence;
            Evasion = evasion;
            Score = score;
        }

        //Constructor to create a new enemy based on an enemy object.
        public Enemy(Enemy ene)
        {
            Name = ene.Name;
            MaxHealth = ene.MaxHealth;
            CurrentHealth = MaxHealth;

            Damage = ene.Damage;
            ApDamage = ene.ApDamage;

            Defence = ene.Defence;
            Evasion = ene.Evasion;
            Score = ene.Score;
        }

        //function to damage the enemy
        public int Injure(int dmg)
        {
            CurrentHealth -= dmg;
            return CurrentHealth;
        }

        //Function to get the enemy's health 
        public string GetHealth()
        {
            return CurrentHealth + "/" + MaxHealth;
        }
    }

    //Class that is the game object itself, is implemented in the main "runner" 
    public sealed class MainGame
    {
        private string GameName;
        public string PlayerName { get; set; }
        //Key 24 Bytes long
        private string EncryptionKey = "ART0R1A_ISNT_4_CH33T3RS!";
        private Coords LastCoordinates = new Coords();
        private Coords Coordinates = new Coords();

        private Player Player;
        public Enemy CurrentEnemy { get; set; }
        public bool ActiveEnemy { get; set; }

        //Variables to store data that needs to go from child to parent forms.
        public Weapon TempWeapon;
        public Armor TempArmor;
        public string TempMiscData;

        //Load map size
        public GameData GameConfigurations;
        //multi_array of MapTiles for the map
        private MapTile[,] Map;

        private List<Weapon> Weapons = new List<Weapon>();
        private List<Armor> Armors = new List<Armor>();
        private List<Item> Items = new List<Item>();
        private List<Enemy> Enemies = new List<Enemy>();
        private List<Event> Events = new List<Event>();

        private MyRandom RandomGen = new MyRandom();
        public bool PlayerDied;
        public int TurnCount = 0;

        //Default Constructor
        public MainGame()
        {
            //Load Game Settings
            GameConfigurations = new GameData();
            
            //Load map size
            Map = new MapTile[GameConfigurations.XMax, GameConfigurations.YMax];

            //Load Game Name
            GameName = GameConfigurations.GameName;

            //No active enemy
            ActiveEnemy = false;

            //Setup Coords at Starting Position
            Coordinates.X = GameConfigurations.StartingX;
            Coordinates.Y = GameConfigurations.StartingY;

            //ToDo:Check if need to x and y
            LastCoordinates.X = Coordinates.X;
            LastCoordinates.Y = Coordinates.Y;

            //Load Game Data From XML Files
            PopulateEnemies();
            PopulateItems();
            PopulateMap();
            PopulateEvents();
            
        }
        
        //Function that creates introduction string.
        public string StartGame(string playerName, bool newGame)
        {
            PlayerName = playerName;
            StringBuilder introMessage = new StringBuilder();

            if (newGame)
            {
                //Show User Introduction text.
                introMessage.AppendLine("Welcome to " + GameName + ", " + playerName + ".");

                //Add Custom Flavor Text
                introMessage.AppendLine(GameConfigurations.GameFlavorText);
                introMessage.AppendLine(Environment.NewLine);

                //Add Instructions
                introMessage.Append("You can move around the map by using the 'North', 'South', 'East', and 'West' buttons.");
                introMessage.Append("If you encounter an enemy, attack them by pressing the Sword icon, or surrender yourself to the enemy by pressing the Flag icon.");
                introMessage.Append("If you wish to quit the game, use the 'Quit' button located in the Menu Bar. You can also simply click the 'X' to close the window.'");
                introMessage.Append("If you wish to view more detailed information about Game mechanics, click the 'help' buttons located in the Menu Bar.");
                introMessage.AppendLine(Environment.NewLine);
            }
            else
            {
                //Show User Introduction text.
                introMessage.AppendLine("Welcome back to " + GameName + " " + playerName + ".");

                //This line is commented out now, but exists in case you wish to display the player's last position when they load an existing game.
                //introMessage.AppendLine("You are at X:" + Coordinates.x + " Y:" + Coordinates.y + ".");
                introMessage.AppendLine("Good luck, adventurer.'");
                introMessage.AppendLine(Environment.NewLine);

            }

            //if debug mode is on then skip the normal intro and give them information about the data loaded.
            if (GameConfigurations.Debug)
            {
                introMessage.Clear();
                introMessage.AppendLine("Debug mode is on, normal intro skipped.");
                introMessage.AppendLine(string.Format("Number of Map Tiles: {0}", Map.Length));
                introMessage.AppendLine(string.Format("Number of Events: {0}", Events.Count));
                introMessage.AppendLine(string.Format("Number of Enemies: {0}", Enemies.Count));
                introMessage.AppendLine(string.Format("Number of Weapons: {0}", Weapons.Count));
                introMessage.AppendLine(string.Format("Number of Armor sets: {0}", Armors.Count));
                introMessage.AppendLine(string.Format("Number of Score Items: {0}", Items.Count));
                introMessage.AppendLine();
                introMessage.AppendLine(string.Format("Starting Position: ({0},{1})", GameConfigurations.StartingX, GameConfigurations.StartingY));
                introMessage.AppendLine(Environment.NewLine);

            }
            return introMessage.ToString();
        }

        //Function load the game's map from XML and store it in the array of MapTile objects
        private void PopulateMap()
        {
            try
            {
                //Load XML document containing items.
                XDocument xdoc = XDocument.Load("Map.xml");

                //Populate list with tile objects to store in array later
                List<MapTile> tiles =
                    (
                        from elem in xdoc.Root.Elements("Tile")
                        select new MapTile
                        {
                            X = (int)elem.Element("XCoord"),
                            Y = (int)elem.Element("YCoord"),
                            Message = (string)elem.Element("Message"),
                            EventID = (int)elem.Element("EventID")
                        }).ToList();
                if (tiles.Any())
                {
                    var expectedTileCount = GameConfigurations.XMax * GameConfigurations.XMax;
                    if (tiles.Count < expectedTileCount)
                    {
                        //there were less tiles in the map than there should be. Inform the user before quitting the game.

                        MessageBox.Show(string.Format("There are less tiles in your map than required. You have {0} tiles, you need to have {1}.", tiles.Count, expectedTileCount), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Quit the application because their data was not loaded properly, the game will not work properly.
                        Environment.Exit(0);
                    }
                    foreach (MapTile currentTile in tiles)
                    {
                        Map[currentTile.X, currentTile.Y] = currentTile;
                    }
                }
                else
                {
                    //There was no map, because this is integral to gameplay inform the user and force quit the game.
                    MessageBox.Show("There were no tiles found in your map Your game must have map tiles to function.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Quit the application because their data was not loaded properly, the game will not work properly.
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "PopulateMap()", true);
                MessageBox.Show("There was a problem loading your Map, ensure your data is formatted properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Quit the application because their data was not loaded properly, the game will not work properly.
                Environment.Exit(0);
            }
        }

        //Function to load the game's Events from XML and store it in a list of Event objects.
        private void PopulateEvents()
        {
            try
            {
                //Load XML document containing items.
                XDocument xdoc = XDocument.Load("Events.xml");

                //Populate list with event objects to store in array later
                List<Event> events =
                    (
                        from childElem in xdoc.Root.Elements("Event")
                        select new Event
                        {
                            EventID = (int)childElem.Element("EventID"),
                            RestLocation = (int)childElem.Element("RestLocation"),
                            EnemySpawn = (int)childElem.Element("EnemySpawn"),
                            ItemSpawn = (int)childElem.Element("ItemSpawn"),
                            NothingSpawn = (int)childElem.Element("NothingSpawn"),
                            WeaponChance = (int)childElem.Element("WeaponChance"),
                            ArmorChance = (int)childElem.Element("ArmorChance"),
                            ScoreItemChance = (int)childElem.Element("ScoreItemChance"),
                            EnemySpawnZone = (int)childElem.Element("EnemySpawnZone")
                        }).ToList();
                Events = events;

                //There were no events, because this is integral to gameplay inform the user and force quit the game.
                if (!Events.Any())
                {
                    MessageBox.Show("There were no Events found. Your game must have Events to work.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Quit the application because their data was not loaded properly, the game will not work properly.
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "PopulateEvents()", true);
                MessageBox.Show("There was a problem loading your Events, ensure your data is formatted properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Quit the application because their data was not loaded properly, the game will not work properly.
                Environment.Exit(0);
            }
        }

        //Function load the game's items from XML and store them in the appropriate lists
        private void PopulateItems()
        {
            //Load XML document containing items.
            XDocument xdoc = XDocument.Load("Items.xml");
            try
            { 
            //Populate Weapons
                List<Weapon> weapons =
                    (
                        from elem in xdoc.Root.Elements("Weapon")
                        select new Weapon
                        {
                            Name = (string)elem.Element("Name"),
                            Value = (int)elem.Element("ScoreValue"),
                            BDamage = (int)elem.Element("BaseDMG"),
                            APDamage = (int)elem.Element("APDMG"),
                            Evasion = (int)elem.Element("Evasion"),
                            Rarity = (int)elem.Element("Rarity"),
                            Description = (string)elem.Element("Description")
                        }).ToList();
                Weapons = weapons;

                //There were no weapons, because this is integral to gameplay even without combat (starting class initialization) inform the user and force quit the game.
                if (!Weapons.Any())
                {
                    MessageBox.Show("There were no Weapons found. Your game must have Weapons to work properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Quit the application because their data was not loaded properly, the game will not work properly.
                    Environment.Exit(0);
                }

            }
            catch (Exception ex)
            {
                LogError(ex, "PopulateItems()", true);
                MessageBox.Show("There was a problem loading your Weapons, ensure your data is formatted properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Quit the application because their data was not loaded properly, the game will not work properly.
                Environment.Exit(0);
            }

            try
            {
                //Populate Armor
                List<Armor> armors =
                    (
                        from elem in xdoc.Root.Elements("Armor")
                        select new Armor
                        {
                            Name = (string)elem.Element("Name"),
                            Value = (int)elem.Element("ScoreValue"),
                            ArmorVal = (int)elem.Element("ArmorVal"),
                            Evasion = (int)elem.Element("Evasion"),
                            Rarity = (int)elem.Element("Rarity"),
                            Description = (string)elem.Element("Description")
                        }).ToList();
                Armors = armors;

                //There was no armor, because this is integral to gameplay even without combat (starting class initialization) inform the user and force quit the game.
                if (!Armors.Any())
                {
                    MessageBox.Show("There was no Armor found. Your game must have Armor to work properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Quit the application because their data was not loaded properly, the game will not work properly.
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "PopulateItems()", true);
                MessageBox.Show("There was a problem loading your Armor, ensure your data is formatted properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Quit the application because their data was not loaded properly, the game will not work properly.
                Environment.Exit(0);
            }

            try
            { 
                //Populate Items
                List<Item> items =
                (
                    from elem in xdoc.Root.Elements("Item")
                    select new Item
                    {
                        Name = (string)elem.Element("Name"),
                        Value = (int)elem.Element("ScoreValue"),
                        Description = (string)elem.Element("Description")
                    }).ToList();
                Items = items;

                //There were no score items, but because they are not integral to gameplay simply add a default item to ensure the game will function.
                if (!Items.Any())
                {
                    MessageBox.Show("There were no Score Items found. Adding a default score item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Items.Add(new Item());
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "PopulateItems()", true);
                MessageBox.Show("There was a problem loading your Score Items, ensure your data is formatted properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Quit the application because their data was not loaded properly, the game will not work properly.
                Environment.Exit(0);
            }
        }

        //Function load the game's enemies from XML and store them in a List of Enemy objects
        private void PopulateEnemies()
        {
            try
            {
                //Load XML document containing enemies.
                XDocument xdoc = XDocument.Load("Enemies.xml");

                //Populate Enemies
                List<Enemy> enemies =
                    (
                        from elem in xdoc.Root.Elements("Enemy")
                        select new Enemy
                        {
                            Name = (string)elem.Element("Name"),
                            MaxHealth = (int)elem.Element("Health"),
                            CurrentHealth = (int)elem.Element("Health"),
                            Damage = (int)elem.Element("BaseDMG"),
                            ApDamage = (int)elem.Element("APDMG"),
                            Defence = (int)elem.Element("Defence"),
                            Evasion = (int)elem.Element("Evasion"),
                            Score = (int)elem.Element("Points"),
                            SpawnZone = (int)elem.Element("SpawnZone")
                        }).ToList();
                Enemies = enemies;

                //There were no enemies, but because you could make a game without enemies do not force close the game, simply inform the user.
                if (!Enemies.Any())
                {
                    MessageBox.Show("There were no Enemies found, if this was not intended you should close the game and check your data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "PopulateEnemies()", true);
                MessageBox.Show("There was a problem loading your Enemies, ensure your data is formatted properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Quit the application because their data was not loaded properly, the game will not work properly.
                Environment.Exit(0);
            }
}

        //ToDo: TESTING
        //Function that will save a player's progress in a text file.
        public void SaveProgress(string notes)
        {
            //Ensure SaveData folder is created.
            System.IO.Directory.CreateDirectory("SaveData/");
            try
            {
                //Initialize encryption class with the key parameter.
                Crypter cryptic = new Crypter(EncryptionKey);
                string path = "SaveData/" + PlayerName.ToLower() + ".txt";
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    //Get the player's data as a string.
                    string saveData;
                    saveData = Player.getSaveData() + ',' + Coordinates.X + ',' + Coordinates.Y + ',' + notes;
                    string saveDataEncrypted = string.Empty;
                    
                    //Encrypt save data to prevent cheating.
                    saveDataEncrypted = cryptic.Encrypt(saveData);
                    
                    //Write data to a text file.
                    sw.WriteLine(saveDataEncrypted);
                    MessageBox.Show("Game saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            //Catch errors for logging
            catch (Exception ex)
            {
                LogError(ex, "SaveProgress()");
            }

        }

        //ToDo: Test Load Funcitonality 
        //Function that will load a player's save from a text file.
        public string LoadProgress(string name, bool secondPass)
        {
            try
            {
                //NEEDS TESTING
                name = name.ToLower();
                Crypter cryptic = new Crypter(EncryptionKey);
                string path = "SaveData/" + name + ".txt";
                if (File.Exists(path))
                {
                    List<string> saves = new List<string>();
                    string save;

                    var reader = new StreamReader(File.OpenRead(path));
                    while (!reader.EndOfStream)
                    {
                        save = reader.ReadLine();
                        saves.Add(cryptic.Decrypt(save));
                        foreach (string line in saves)
                        {
                            var splat = line.Split(',');
                            if (splat[0].ToLower() == name.ToLower())
                            {
                                if (splat.Count() == 11)
                                {
                                    //name, health, weapon name, armor name, score, x, y
                                    //Integer variables to store player statistics
                                    int mHealth;
                                    int cHealth;
                                    int mStamina;
                                    int cStamina;
                                    int score;
                                    //Integer variables to store coordinates
                                    int y;
                                    int x;
                                    //boolean to determine whether or not there were any missing values.
                                    bool missingVals = false;

                                    //convert the string values into integers, if the conversion fails then the save file must have been corrupted.
                                    if (!int.TryParse(splat[1], out mHealth))
                                    {
                                        missingVals = true;
                                    }

                                    if (!int.TryParse(splat[2], out cHealth))
                                    {
                                        missingVals = true;
                                    }

                                    if (!int.TryParse(splat[3], out mStamina))
                                    {
                                        missingVals = true;
                                    }

                                    if (!int.TryParse(splat[4], out cStamina))
                                    {
                                        missingVals = true;
                                    }

                                    string weapon = splat[5];
                                    string armor = splat[6];

                                    if (!int.TryParse(splat[7], out score))
                                    {
                                        missingVals = true;
                                    }

                                    //Check if user's location was properly loaded, if not inform them that their save has been corrupted, but still let them play with partial load.
                                    if (!int.TryParse(splat[8], out x))
                                    {
                                        missingVals = true;
                                    }
                                    if (!int.TryParse(splat[9], out y))
                                    {
                                        missingVals = true;
                                    }
                                    TempMiscData = splat[10];
                                    Weapon wep = FindWeapon(weapon);
                                    Armor arm = FindArmor(armor);
                                    Player p = new Player(name, mHealth, cHealth, mStamina, cStamina, wep, arm, score);

                                    //Set player attributes built from load data.
                                    Player = p;
                                    Coordinates.X = x;
                                    Coordinates.X = y;

                                    if (missingVals)
                                    {
                                        MessageBox.Show("Your save data has been corrupted, please start a new game.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                    reader.Close();
                                    return "success";
                                }
                                else
                                {
                                    //if there are not enough values in the array, the data is either from a version with different save data or has been manually modified.
                                    MessageBox.Show("Your save data has been corrupted, your data cannot be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    reader.Close();
                                    return "fail";
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Your save file could not be located. Make sure it is located in [AppDirectory]\\SaveData\\[CharacterName].txt", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (!secondPass)
                    {
                        DialogResult result1 = MessageBox.Show("Your save could not be found, would you like to try again?", GameName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if (result1 == DialogResult.Yes)
                        {
                            //User wants to try again, allow for one more try.
                            return "tryagain";
                        }
                        else if (result1 == DialogResult.No)
                        {
                            //If game could not be loaded ask if they want to start a new game.
                            DialogResult result2 = MessageBox.Show("Would you like to start a new game?", GameName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            //If they don't want to start a new game, close the program.
                            if (result2 == DialogResult.No)
                            {
                                Form currentForm = Form.ActiveForm;
                                currentForm.Close();
                            }
                            else
                            {
                                return "fail";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogError(ex, "LoadProgress()");
            }
            return "fail";
        }

        //Function to allow a player to select their starting class.
        public string SelectClass(string playerName, string playerClass)
        {
            //Get class information from the XML using LINQ
            StartingClass selectedClass = GameConfigurations.Classes.First(s => s.ClassName == playerClass);
            
            //Get player equipment from Find[Equipment]() functions.
            Weapon selectedWeapon = FindWeapon(selectedClass.ClassWeapon);
            Armor selectedArmor = FindArmor(selectedClass.ClassArmor);

            //Create the player object with the starting gear.
            Player = new Player(playerName, selectedWeapon, selectedArmor);

            return "You have selected the " + selectedClass.ClassName + " class.";
        }

        //Function to evaluate user's input
        public string EvaluateInput(string userInput)
        {
            var returnString = new StringBuilder();
            //returnString.AppendLine(System.Environment.NewLine);

            userInput = userInput.ToLower();
            if (userInput == "north")
            {
                if (!ActiveEnemy)
                {
                    //moving north
                    if(Coordinates.North())
                    {
                        //Moved successfully
                        returnString.AppendLine(GenMessage());
                    }
                    else
                    {
                        //Cannot go that way
                        returnString.AppendLine("You cannot move this way.");
                    }
                }
                else
                {
                    returnString.AppendLine("You cannot move when there is an enemy attacking you!");
                }
            }
            else if (userInput == "south")
            {
                if (!ActiveEnemy)
                {
                    //moving south
                    if(Coordinates.South())
                    {
                        //Moved successfully
                        returnString.AppendLine(GenMessage());
                    }
                    else
                    {
                        //Cannot go that way
                        returnString.AppendLine("You cannot move this way.");
                    }
                }
                else
                {
                    returnString.AppendLine("You cannot move when there is an enemy attacking you!");
                }
            }
            else if (userInput == "east")
            {
                if (!ActiveEnemy)
                {
                    //moving east
                    if(Coordinates.East())
                    {
                        //Moved successfully
                        returnString.AppendLine(GenMessage());
                    }
                    else
                    {
                        //Cannot go that way
                        returnString.AppendLine("You cannot move this way.");
                    }
                }
                else
                {
                    returnString.AppendLine("You cannot move when there is an enemy attacking you!");
                }
            }
            else if (userInput == "west")
            {
                if (!ActiveEnemy)
                {
                    //moving west
                    if(Coordinates.West())
                    {
                        //Moved successfully
                        returnString.AppendLine(GenMessage());
                    }
                    else
                    {
                        //Cannot go that way
                        returnString.AppendLine("You cannot move this way.");
                    }
                }
                else
                {
                    returnString.AppendLine("You cannot move when there is an enemy attacking you!");
                }
            }
            else
            {
                MessageBox.Show("ERROR: Unknown Input. | Method: EvaluateInput", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                returnString.AppendLine("Error Evaluating Input. Please restart application.");
            }
            return returnString.ToString();
        }

        //Function to generate the message output to the console.
        private string GenMessage()
        {
            try
            {
                //get the current map tile from the map container
                MapTile currentTile = Map[Coordinates.X, Coordinates.Y];
                if (!ActiveEnemy)
                {
                    //variable to format final return string
                    var returnString = new StringBuilder();
                    if (!GameConfigurations.Debug)
                    {
                        returnString.AppendLine(currentTile.Message);
                    }
                    else
                    {
                        returnString.AppendLine(string.Format("{0} ({1},{2})", currentTile.Message, Coordinates.X, Coordinates.Y));
                    }
                    //LINQ Query to grab the current event.
                    var eventQuery = from eve in Events where eve.EventID == currentTile.EventID select eve;
                    //If the user followed setup instructions correctly, there should only ever be one Event for each EventID
                    if(!eventQuery.Any())
                    {
                        MessageBox.Show(string.Format("No matching EventID found for ({0},{1}). EventID: {2}{3}Choosing random event.", currentTile.X, currentTile.Y, currentTile.EventID, Environment.NewLine), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SaveProgress("");
                    }
                    Event currentEvent = eventQuery.First();

                    //The EventID -1 is used to mark an unpassable location.
                    if (currentEvent.EventID == -1)
                    {
                        Coordinates.X = LastCoordinates.X;
                        Coordinates.Y = LastCoordinates.Y;
                        return returnString.ToString();
                    }
                    if (currentEvent.RestLocation != 1)
                    {
                        //Check for enemy encounter first.
                        //If EnemySpawn is greater than a random val 1-100 then an enemy was encountered.
                        if (currentEvent.EnemySpawn > RandomGen.rollDie(100))
                        {
                            //An Enemy was encountered
                            ActiveEnemy = true;

                            //LINQ Query to grab all enemies that can spawn in this zone.
                            var enemyQuery = from enem in Enemies where enem.SpawnZone == currentEvent.EnemySpawnZone select enem;

                            //Randomly pick an enemy from that list.
                            var index = RandomGen.rollDie(enemyQuery.Count()) - 1;
                            
                            if(index < 0)
                            {
                                index = 0;
                            }

                            if (enemyQuery.Any())
                            {
                                CurrentEnemy = new Enemy(enemyQuery.ElementAt(index));
                            }
                            else
                            {
                                //If there are no enemies period then inform the user they shouldn't have an event spawn an enemy and just return the message.
                                if (!Enemies.Any())
                                {
                                    MessageBox.Show(string.Format("EventID: {0} is spawning an enemy but your game has no enemies.", currentEvent.EventID), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return returnString.ToString();
                                }
                                //Inform the user here were no enemies with the matching SpawnZone.
                                MessageBox.Show(string.Format("Enemy generation failed, no enemies in spawn zone. {0}Current Tile: ({1},{2}) EventID: {3} EnemySpawnZone: {4}{5}Ensure there is at least one enemy with a matching SpawnZone." +
                                    "{6}Generating random enemy.", Environment.NewLine, Coordinates.X, Coordinates.Y, currentEvent.EventID, currentEvent.EnemySpawnZone, Environment.NewLine, Environment.NewLine), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                

                                //There were no enemies with the matching SpawnZone so pick a random enemy.
                                CurrentEnemy = new Enemy(Enemies[RandomGen.rollDie(Enemies.Count - 1)]);
                            }

                            returnString.AppendLine(CurrentEnemy.Name + " has attacked you!");
                        }
                        else if (currentEvent.ItemSpawn > RandomGen.rollDie(100))
                        {
                            //An item was found, determine what type of item.
                            var itemGenIndex = RandomGen.rollDie(100);
                            if (itemGenIndex <= currentEvent.WeaponChance)
                            {
                                //Index was between 0 and weapon max, a weapon was found.
                                TempWeapon = GetNewWeapon();

                                //Add the found items value to the player's score, whether they equip it or not.
                                Player.Score += TempWeapon.Value;

                                string returnContents = returnString.ToString();
                                returnString.Clear();

                                returnString.AppendLine("NewWeapon" + '|' + returnContents.ToString());
                            }
                            else if (itemGenIndex > currentEvent.WeaponChance && itemGenIndex <= currentEvent.ArmorChance)
                            {
                                //index was between weapon max and armor max, armor was found.
                                TempArmor = GetNewArmor();

                                //Add the found items value to the player's score, whether they equip it or not.
                                Player.Score += TempArmor.Value;

                                string returnContents = returnString.ToString();
                                returnString.Clear();

                                returnString.AppendLine("NewArmor" + '|' + returnContents.ToString());
                            }
                            else if (itemGenIndex > currentEvent.ArmorChance && itemGenIndex <= currentEvent.ScoreItemChance)
                            {
                                //index was between armor max and score item max, a score item was found.
                                returnString.AppendLine(GetNewScoreItem());
                            }
                        }
                        else
                        {
                            //Nothing Happened, return default tile message
                            return returnString.ToString();
                        }
                    }
                    else
                    {
                        //If this is a rest location then refil the player's heals.
                        returnString.AppendLine(Player.RefillEstus());
                    }
                    //set tempCoords to keep track of last tile.
                    LastCoordinates.X = Coordinates.X;
                    LastCoordinates.Y = Coordinates.Y;
                    return returnString.ToString();
                }

                //If the method didn't return for a normal reason, just return the tile message.
                return currentTile.Message;
            }
            catch (Exception ex)
            {
                LogError(ex, "GenMessage()");
                SaveProgress("Notes lost during last crash :(");
                return "Error! Your progress has been saved, please restart your game.";
            }
        }

        //function to generate a weapon
        private Weapon GetNewWeapon()
        {
            //Declare variables for weapon generation
            IEnumerable<Weapon> weapons;
            int index;
            //Generate value to determine what rarity weapon was found.
            int val = RandomGen.rollDie(6);
            //Switch val to determine which rarity
            switch (val)
            {
                case 1:
                case 2:
                case 3:

                    //a common weapon was found
                    weapons = from wep in Weapons where wep.Rarity == 1 select wep;
                    if (weapons.Any())
                    {
                        index = RandomGen.rollDie(weapons.Count());
                        return Weapons[index - 1];
                    }
                    else
                    {
                        MessageBox.Show("Weapon Generation Failed, no weapon found with rarity: 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new Weapon();
                    }
                case 4:
                case 5:

                    //An uncommon weapon was found
                    weapons = from wep in Weapons where wep.Rarity == 2 select wep;
                    if (weapons.Any())
                    {
                        index = RandomGen.rollDie(weapons.Count());
                        return Weapons[index - 1];
                    }
                    else
                    {
                        MessageBox.Show("Weapon Generation Failed, no weapon found with rarity: 2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new Weapon();
                    }
                case 6:
                    //A rare weapon was found.
                    weapons = from wep in Weapons where wep.Rarity == 3 select wep;
                    if (weapons.Any())
                    {
                        index = RandomGen.rollDie(weapons.Count());
                        return Weapons[index - 1];
                    }
                    else
                    {
                        MessageBox.Show("Weapon Generation Failed, no weapon found with rarity: 3", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new Weapon();
                    }
                default:
                    //Should never reach this point, but if it does then return a default weapon.
                    return new Weapon();
            }
        }

        //function to generate armor
        private Armor GetNewArmor()
        {
            //Declare variables for armor generation
            IEnumerable<Armor> armors;
            int index;
            //Generate value to determine what rarity armor was found.
            int val = RandomGen.rollDie(6);
            //Switch val to determine which rarity
            switch (val)
            {
                case 1:
                case 2:
                case 3:
                    //Common armor was found
                    armors = from arm in Armors where arm.Rarity == 1 select arm;
                    if (armors.Any())
                    {
                        index = RandomGen.rollDie(armors.Count());
                        return Armors[index - 1];
                    }
                    else
                    {
                        MessageBox.Show("Armor Generation Failed, no armor found with rarity: 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new Armor();
                    }
                case 4:
                case 5:
                    //Uncommon armor was found
                    armors = from arm in Armors where arm.Rarity == 2 select arm;
                    if (armors.Any())
                    {
                        index = RandomGen.rollDie(armors.Count());
                        return Armors[index - 1];
                    }
                    else
                    {
                        MessageBox.Show("Armor Generation Failed, no armor found with rarity: 2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new Armor();
                    }
                case 6:
                    //Rare armor was found.
                    armors = from arm in Armors where arm.Rarity == 3 select arm;
                    if (armors.Any())
                    {
                        index = RandomGen.rollDie(armors.Count());
                        return Armors[index - 1];
                    }
                    else
                    {
                        MessageBox.Show("Armor Generation Failed, no armor found with rarity: 2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new Armor();
                    }
                default:
                    //Should never reach this point, but if it does return a default armor.
                    return new Armor();
            }
        }

        private string GetNewScoreItem()
        {
            var returnString = new StringBuilder();

            //Get random value to determine what item was found
            var index = RandomGen.rollDie(Items.Count() - 1);

            Item item = Items.ElementAt(index);
            //Add item value to the player's score
            if(item.Value < 1)
            {
                item.Value = 0;
            }
            Player.Score += item.Value;

            //tell the player what they found.
            //Check if the item's name starts with a vowel so that you can use a or an properly
            if (item.Name.Substring(0, 1) == "a" || item.Name.Substring(0, 1) == "e" || item.Name.Substring(0, 1) == "i" || item.Name.Substring(0, 1) == "o" || item.Name.Substring(0, 1) == "u")
            {
                returnString.AppendLine(string.Format("You have found an {0}.", item.Name));
            }
            else
            {
                returnString.AppendLine(string.Format("You have found a {0}.", item.Name));
            }

            returnString.AppendLine(item.Description);
            if (item.Value > 1 || item.Value == 0)
            {
                returnString.AppendLine(item.Value + " points have been added to your score.");
            }
            else
            {
                returnString.AppendLine(item.Value + " point have been added to your score.");
            }

            return returnString.ToString();
        }

        //function to initiate a round of combat with the current enemy.
        public string BattleEnemy(string action)
        {
            try
            {
                //Stringbuilder to return
                var returnString = new StringBuilder();

                //Get Data for player and enemy: Attack Ratings, Defense Ratings, Evasion.

                //enemy data
                int enemyBaseDamage = CurrentEnemy.Damage;
                int enemyAPDamage = CurrentEnemy.ApDamage;

                //Turn values into doubles for multiplication
                double enemyPhysicalDef = CurrentEnemy.Defence / 100;

                int enemyDodgeChance = CurrentEnemy.Evasion;

                //player data
                int playerBaseDamage = Player.Weapon.BDamage;
                int playerAPDamage = Player.Weapon.APDamage;

                //Turn values into doubles for multiplication
                double playerDefence = Player.Armor.ArmorVal / 100;

                int playerEvasion = Player.GetEvasion();

                int playerDamageTaken = 0;
                int enemyDamageTaken = 0;

                //Boolean to store whether or not this attack was a block.
                bool playerBlocking = false;

                if (action == "block")
                {
                    //Player is blocking, so they take half damage the attack and regen some stamina.
                    playerBlocking = true;
                    enemyBaseDamage /= 2;
                    enemyAPDamage /= 2;
                    returnString.AppendLine("You have blocked the enemy's attack, you will take half damage and regain some stamina.");
                    Player.RegenStamina(20);
                }
                else
                {
                    if (!Player.PerformAction(10))
                    {
                        returnString.AppendLine("You do not have enough stamina to attack. Block to regen some stamina.");

                        //return pipe separated value to populate controls.
                        return Player.GetHealthAndStamina() + "|" + CurrentEnemy.GetHealth() + "|" + returnString.ToString();
                    }
                }

                //Calculate damage enemy deals
                playerDamageTaken += (enemyBaseDamage - Convert.ToInt32(Math.Ceiling((enemyBaseDamage * playerDefence))));
                playerDamageTaken += enemyAPDamage;


                //Calculate damage player deals
                enemyDamageTaken += (playerBaseDamage - Convert.ToInt32(Math.Ceiling((playerBaseDamage * enemyPhysicalDef))));
                enemyDamageTaken += playerAPDamage;


                if (playerEvasion > RandomGen.rollDie(100))
                {
                    //The player dodged the attack
                    returnString.AppendLine("You dodged the " + CurrentEnemy.Name + "'s attack!");
                }
                else
                {
                    //Check if player died
                    int playerHealthLeft = Player.Injure(playerDamageTaken);
                    if (playerHealthLeft < 1)
                    {
                        PlayerDied = true;
                        //EXIT GAME
                    }

                }

                //Do not perform any enemy dodge checks or damaging if the player was blocking.
                if (!playerBlocking)
                {
                    if (enemyDodgeChance > RandomGen.rollDie(100))
                    {
                        returnString.AppendLine("The " + CurrentEnemy.Name + " has dodged your attack!");
                    }
                    else
                    {
                        //Check if enemy died
                        int enemyHP = 0;
                        enemyHP = CurrentEnemy.Injure(enemyDamageTaken);
                        //if the enemy has less than 1 HP, they are dead.
                        if (!(enemyHP < 1))
                        {
                            returnString.AppendLine("You have traded blows with the enemy.");
                            returnString.AppendLine("The enemy remains with " + enemyHP + " health.");
                        }
                        else
                        {
                            //Add to player's score and kill count
                            Player.Score += CurrentEnemy.Score;
                            Player.EnemiesKilled++;

                            //Set temp data for population on main screen. 
                            TempMiscData = "You have defeated the " + CurrentEnemy.Name + ", " + CurrentEnemy.Score + " was added to your kill score." + Environment.NewLine;
                            ActiveEnemy = false;

                            //Regen half of the player's stamina outside of combat
                            Player.RegenStamina(GameConfigurations.PlayerMaxStamina / 2);
                        }
                    }
                }

                //Regen 5 stam per turn.
                Player.RegenStamina(5);

                return Player.GetHealthAndStamina() + "|" + CurrentEnemy.GetHealth() + "|" + returnString.ToString();
            }
            catch(Exception ex)
            {
                LogError(ex, "BattleEnemy()");
                SaveProgress("Notes lost during crash");
                return Player.GetHealthAndStamina() + "|" + CurrentEnemy.GetHealth() + "|" + "Error occured during combat, saving game. Please restart the game.";
            }
        }

        //function to find a weapon based on its name
        private Weapon FindWeapon(string weaponText)
        {
            var name = weaponText;
            Weapon weapon = new Weapon();
            foreach (Weapon wep in Weapons)
            {
                //If the name of the weapon matches, return the found weapon object
                if (name.ToLower() == (wep.Name).ToLower())
                {
                    weapon = wep;
                }
            }
            return weapon;
        }

        //Function to find a set of armor based on its name
        private Armor FindArmor(string name)
        {
            Armor armor = new Armor();
            foreach (Armor arm in Armors)
            {
                //If the name of the armor matches, return the found armor object
                if (name.ToLower() == (arm.Name).ToLower())
                {
                    armor = arm;
                }
            }
            return armor;
        }

        //Function to get the player's current equipment as a printable string
        public string GenerateEquipmentList()
        {
            var returnString = new StringBuilder();
            Weapon currentWeapon = Player.Weapon;
            Armor currentArmor = Player.Armor;
            
            //Weapon
            returnString.AppendLine("Weapon");
            returnString.AppendLine(currentWeapon.Name);
            returnString.AppendLine(" Rarity: " + currentWeapon.Rarity);
            returnString.AppendLine(currentWeapon.Description);
            returnString.AppendLine("DMG: " + currentWeapon.BDamage + "    APDMG: " + currentWeapon.APDamage);

            //Whitespace is important too
            returnString.AppendLine(Environment.NewLine);

            //Armor
            returnString.AppendLine("Armor");
            returnString.AppendLine(currentArmor.Name);
            returnString.AppendLine("Rarity: " + currentArmor.Rarity);
            returnString.AppendLine(currentArmor.Description);
            returnString.AppendLine("Armor Value: " + currentArmor.ArmorVal);


            return returnString.ToString();
        }

        //Function to get a printout of the player's statistics
        public string GenerateStatistics()
        {
            var returnString = new StringBuilder();

            returnString.AppendLine(PlayerName);
            returnString.AppendLine(Environment.NewLine);

            //Add player's gear
            returnString.AppendLine("Final Weapon: " + Player.Weapon.Name + " Rarity " + Player.Weapon.Rarity);
            returnString.AppendLine("Final Armor: " + Player.Armor.Name + " Rarity " + Player.Armor.Rarity);
            returnString.AppendLine(Environment.NewLine);

            //Add player's combat statistics
            returnString.AppendLine("Turns Survived: " + TurnCount);
            returnString.AppendLine("Enemies Defeated: " + Player.EnemiesKilled);
            returnString.AppendLine("Points earned: " + Player.Score);

            return returnString.ToString();
        }

        //Function to switch a player's current weapon with a new one.
        public void SwapWeapon(Weapon newWeapon)
        {
            Player.Weapon = newWeapon;
        }

        //Function to switch a player's current armor set with a new one.
        public void SwapArmor(Armor newArmor)
        {
            Player.Armor = newArmor;
        }
        
        //Function to heal the player and to return the results of the heal.
        public string healPlayer()
        {
            //Get text output from heal attempt.
            var healText = Player.DrinkEstus();

            return Player.GetHealthAndStamina() + "|" + CurrentEnemy.GetHealth() + "|" + healText;
        }

        //Function to attempt escaping from combat and return the results.
        public string EscapeAttempt()
        {
            try
            {
                var returnString = new StringBuilder();

                //Get player's evasion to factor into escape chance.
                decimal playerEvasion = Player.GetEvasion();

                //Player's evasion factors into escape chance on a factor of 0.5 with a base 50% chance to escape. 
                //If player has negative evasion (meaning they never evade attacks), this will lower their overall escape change.
                //If player has 50 evasion (meaning they evade half of all attacks), they will have a 75% chance to evade.
                //If player has -50 evasion, they will have a 25% chance to evade.
                var escapeChance = 50 - Math.Ceiling(playerEvasion / 2);

                if (playerEvasion > RandomGen.rollDie(100))
                {
                    //The player dodged the attack
                    returnString.AppendLine("You escaped the enemy!");

                    //Check if the enemy's name starts with a vowel so that you can use a or an properly
                    if (CurrentEnemy.Name.Substring(0, 1) == "a" || CurrentEnemy.Name.Substring(0, 1) == "e" || CurrentEnemy.Name.Substring(0, 1) == "i" || CurrentEnemy.Name.Substring(0, 1) == "o" || CurrentEnemy.Name.Substring(0, 1) == "u")
                    {
                        TempMiscData = "You have encountered an " + CurrentEnemy.Name + " and successfully escaped." + Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        TempMiscData = "You have encountered a " + CurrentEnemy.Name + " and successfully escaped." + Environment.NewLine + Environment.NewLine;
                    }

                    //You escaped correctly, there is no longer an active enemy.
                    ActiveEnemy = false;
                }
                else
                {
                    returnString.AppendLine("You failed to escape the enemy!");
                    //If player failed to dodge, then they took damage.
                    var playerDamageTaken = (CurrentEnemy.Damage - Convert.ToInt32(Math.Ceiling((decimal)(CurrentEnemy.Damage * (Player.Armor.ArmorVal / 100)))));
                    playerDamageTaken += CurrentEnemy.ApDamage;

                    //Check if player died
                    int playerHealthLeft = Player.Injure(playerDamageTaken);
                    if (playerHealthLeft < 1)
                    {
                        PlayerDied = true;
                        //EXIT GAME
                    }
                }

                return Player.GetHealthAndStamina() + "|" + CurrentEnemy.GetHealth() + "|" + returnString.ToString();
            }

            catch (Exception ex)
            {
                LogError(ex, "BattleEnemy()");
                return Player.GetHealthAndStamina() + "|" + CurrentEnemy.GetHealth() + "|" + "There was an error while attmpting to escape. You may continue to play, but escape attempts may fail.";
            }
        }

        //Function to return the player's current weapon.
        public Weapon GetPlayerWeapon()
        {
            return Player.Weapon;
        }

        //Function to return the player's current armor
        public Armor GetPlayerArmor()
        {
            return Player.Armor;
        }

        //Function to return a pipe separated string with the player's health and stamina. 
        public string GetPlayerHealthAndStamina()
        {
            return Player.GetHealthAndStamina();
        }

        //Function to log errors found in the application. Logs differently depending on what type of error.
        private void LogError(Exception exception, string function, bool xmlError = false)
        {
            //Ensure log file directory exists.
            System.IO.Directory.CreateDirectory("LogFiles/");
            var path = "LogFiles/errorLog.txt";
            if (xmlError)
            {
                path = "LogFiles/XMLErrorLog.txt";
            }

            //Log error
            var returnString = new StringBuilder();

            returnString.AppendLine(string.Format("{0} - {1} error at: {2}", DateTime.Now, GameConfigurations.GameName, function));
            returnString.AppendLine(exception.Message);
            returnString.AppendLine(exception.StackTrace);
            returnString.AppendLine();

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(returnString);
            }

            //Inform the user they encountered an error.
            if (!xmlError)
            {
                MessageBox.Show("An error was encountered, results written to log file found at " + path + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
