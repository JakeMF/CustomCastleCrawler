﻿using System;
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
        private string key;
        public Crypter()
        {
            key = "I'mADefaultKeyPleaseChangeMe";
        }
        public Crypter(string key)
        {
            this.key = key;
        }
        //method to encrypt a string, returns encrypted string.
        public string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            keyArray = UTF8Encoding.UTF8.GetBytes(key);

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

            keyArray = UTF8Encoding.UTF8.GetBytes(key);

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

    #region MapClasses
    //OLD Map Code: enum mapSize { Rows = 20, Columns = 20 };

    //Class to load and store the maximum size of the map.
    //ToDo: Ensure XML file is setup properly.
    public sealed class GameData
    {
        public GameData()
        {
            LoadGameData();
        }
        public int XMax { get; set; }
        public int YMax { get; set; }
        public string GameName { get; set; }
        public string GameFlavorText { get; set; }
        public Coords StartingPosition { get; set; }
        public void LoadGameData()
        {
            var retList = new List<int>();
            XDocument xdoc = XDocument.Load("GameSettings.xml");
            //Populate list with tile objects to store in array later
            foreach (var elem in xdoc.Root.Elements("MapSize"))
            {
                XMax = Convert.ToInt16(elem.Element("XMax").Value);
                YMax = Convert.ToInt16(elem.Element("YMax").Value);
                GameName = elem.Element("GameName").Value;
                GameFlavorText = elem.Element("GameName").Value;
            }

            foreach(var elem in xdoc.Root.Elements("StartingPosition"))
            {
                StartingPosition.x = Convert.ToInt16(elem.Element("DefaultX").Value);
                StartingPosition.y = Convert.ToInt16(elem.Element("DefaultY").Value);
            }
            
        }
    }

    //Class to represent a pair of (X,Y) coordinates
    public sealed class Coords
    {
        public int x = 0;
        public int y = 0;
        private GameData MapDimensions = new GameData();
        //Go West
        public bool west()
        {
            if (x > 0)
            {
                --x;
                return true;
            }
            return false;
        }
        //Go North
        public bool north()
        {
            if (y > 0)
            {
                --y;
                return true;
            }
            return false;
        }
        //Go East
        public bool east()
        {
            if (x + 1 < MapDimensions.YMax)
            {
                ++x;
                return true;
            }
            return false;
        }
        //Go South
        public bool south()
        {
            if (y + 1 < MapDimensions.XMax)
            {
                ++y;
                return true;
            }
            return false;
        }
    }

    //Class for one tile on the map, used in multidimensional array to represent X-Y coordinate plane
    class MapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Message { get; set; }
        public int EventID { get; set; }
        public int SpawnZone { get; set; }
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
        public MapTile(int X, int Y, string Message, int EventID)
        {
            this.X = X;
            this.Y = Y;
            this.Message = Message;
            this.EventID = EventID;
        }

        //function to return coords as a vector with 2 elements
        public List<int> getCoords()
        {
            List<int> coords = new List<int>();
            coords.Add(X);
            coords.Add(Y);

            return coords;
        }
        
    }

    public sealed class Event
    {
        public int EventID;
        public string Description;
        public int EnemySpawn;
        public int ItemSpawn;
        public int NothingSpawn;
        public int WeaponChance;
        public int ArmorChance;
        public int ScoreItemChance;

        public Event()
        {
            EventID = -1;
            Description = "This event was not created properly.";
            EnemySpawn = 0;
            ItemSpawn = 0;
            NothingSpawn = 0;
            WeaponChance = 0;
            ArmorChance = 0;
            ScoreItemChance = 0;
        }
    }
    #endregion

    //Class that extends C# Random()
    public sealed class MyRandom
    {
        private Random myRand;
        //Default constructor.
        public MyRandom()
        {
            myRand = new Random();
        }
        //roll a die with n number of sides (starts at 1)
        public int rollDie(int n)
        {
            int num = 0;
            num = myRand.Next(1, n);
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
            Name = "This Item's name was lost long ago.";
            Value = -1; //-1 will output "This item's value is more unknown."
            Description = "";
        }
        //Constructor that takes parameters for all attributes
        public Item(string Name, int Value, string Description)
        {
            this.Name = Name;
            this.Value = Value;
            this.Description = Description;
        }
        //Accessor method for name
        public string getName()
        {
            return Name;
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
            Value = 0;
            BDamage = 5;
            APDamage = 1;
            Evasion = 0;
        }
        //Constructor with all values
        public Weapon(string Name, int Value, int BDamage, int APDamage, int Evasion)
        {
            this.Name = Name;
            this.Value = Value;
            this.BDamage = BDamage;
            this.APDamage = APDamage;
            this.Evasion = Evasion;
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
            Name = "Chainmail";
            Value = 0;
            ArmorVal = 5;
            Evasion = 0;
        }
        //Constructor with all values, no default constructor because gear would be useless otherwise.
        public Armor(string Name, int Value, int ArmorVal, int Evasion)
        {
            this.Name = Name;
            this.Value = Value;
            this.ArmorVal = ArmorVal;
            this.Evasion = Evasion;
        }
    }

    //Class for the player themselves
    public sealed class Player
    {
        public string Name { get; }
        private int MaxHealth;
        private int Health;
        private Weapon Weapon = new Weapon();
        private Armor Armor = new Armor();
        public int Score = 0;
        private int Estus;
        private int MaxEstus;
        //Default constructor
        public Player()
        {
            Name = "Nameless Hollow";
            MaxHealth = 1200;
            Health = MaxHealth;
            MaxEstus = 5;
            Estus = MaxEstus;
        }

        //Constructor that will be used for new players
        //ToDo: Use dynamic classes
        public Player(string Name, string chosenClass)
        {
            this.Name = Name;
            MaxHealth = 1200;
            Health = MaxHealth;
            MaxEstus = 5;
            Estus = MaxEstus;

            switch (chosenClass.ToLower())
            {
                case "solder":
                    Armor = new Armor("Hard Leather Armor", 5, 5, 5);
                    Weapon = new Weapon("Longsword", 5, 5, 2, 0);
                    break;
                case "knight":
                    Armor = new Armor("Silver Knight Armor", 10, 8, -5);
                    Weapon = new Weapon("Claymore", 18, 8, 4, -5);
                    break;
                case "archer":
                    Armor = new Armor("Leather Armor", 3, 3, 10);
                    Weapon = new Weapon("Longbow", 15, 0, 6, 25);
                    break;
                case "giant":
                    Armor = new Armor("Giant Armor", 15, 10, -10);
                    Weapon = new Weapon("Great Club", 25, 14, 2, -10);
                    break;
                default:
                    //Default to Soldier
                    Armor = new Armor("Hard Leather Armor", 5, 5, 5);
                    Weapon = new Weapon("Longsword", 5, 5, 2, 0);
                    break;
            }
        }

        //Constructor that will be used to load a saved game
        public Player(string Name, int MaxHealth, int Health, Weapon Weapon, Armor Armor, int Score)
        {
            this.Name = Name;
            this.MaxHealth = MaxHealth;
            this.Health = Health;
            this.Weapon = Weapon;
            this.Armor = Armor;
            this.Score = Score;
        }

        //Function to get the player's data in a csv format to save.
        public string getSaveData()
        {
            string data;
            char delim = ',';

            data = Name + delim + MaxHealth + delim + Health + delim + Weapon.getName() + delim + Armor.getName() + delim + Score;
            return data;
        }

        //function to get the players weapon
        public Weapon GetWeapon()
        {
            return Weapon;
        }

        //function to get the players armor
        public Armor GetArmor()
        {
            return Armor;
        }

        //function to get the players evasion
        public int GetEvasion()
        {
            int Evasion = Weapon.Evasion + Armor.Evasion;
            if (Evasion > 0)
            {
                return Evasion;
            }
            else
            {
                return 0;
            }
        }

        //function that causes the player to take damage
        public int Injure(int dmg)
        {
            Health -= dmg;
            return Health;
        }

        //function to replace players weapon
        public void SwapWeapon(Weapon newWeapon)
        {
            string response;

            Console.WriteLine("You have found a(n) " + newWeapon.getName() + " here is how it compares to your " + Weapon.getName() + ":");
            Console.WriteLine();
            Console.WriteLine("Standard   " + Weapon.BDamage + " -> " + newWeapon.BDamage);
            Console.WriteLine("Armor Piercing     " + Weapon.APDamage + " -> " + newWeapon.APDamage);
            Console.WriteLine();
            Console.WriteLine("Do you want to swap weapons?(Y/N)");

            response = Console.ReadLine();
            if (response.ToLower() == "y")
            {
                Weapon = newWeapon;
                Console.WriteLine("You are now using the " + newWeapon.getName());
            }
            else
            {
                Console.WriteLine("Are you sure? The weapon will be lost forever.");
                response = Console.ReadLine();
                if (response.ToLower() == "y")
                {
                    Weapon = newWeapon;
                    Console.WriteLine("You are now using the " + newWeapon.getName());
                }
                else
                {
                    Console.WriteLine("You left the " + newWeapon.getName() + " behind.");
                }
            }
        }

        //function to replace players weapon
        public void SwapArmor(Armor newArmor)
        {
            string response;

            Console.WriteLine("You have found a(n) " + newArmor.getName() + " here is how it compares to your " + Weapon.getName() + ":");
            Console.WriteLine();
            Console.WriteLine("Physical   " + Armor.ArmorVal + " -> " + newArmor.ArmorVal);
            Console.WriteLine();
            Console.WriteLine("Would you like to swap armor sets?(Y/N)");

            response = Console.ReadLine();
            if (response.ToLower() == "y")
            {
                Armor = newArmor;
                Console.WriteLine("You are now wearing the " + newArmor.getName());
            }
            else
            {
                Console.WriteLine("Are you sure? The armor will be lost forever.");
                response = Console.ReadLine();
                if (response.ToLower() == "y")
                {
                    Armor = newArmor;
                    Console.WriteLine("You are now wearing the " + newArmor.getName());
                }
                else
                {
                    Console.WriteLine("You left the " + newArmor.getName() + " behind.");
                }
            }
        }
        
        //Function to rest at bonfire
        public void BonfireRest()
        {
            //Reset player's health to max
            Health = MaxHealth;

            //Do anything else that should happen at bonfire.
            Estus = MaxEstus;
        }

        public void DrinkEstus()
        {
            Estus--;
            Health += 600;

            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }
    }

    //Class for enemies
    public sealed class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
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
            Health = 125;
            CurrentHealth = Health;
            //Damage
            Damage = 5;
            ApDamage = 2;
            
            //Defence
            Defence = 10;
            Evasion = 5;
        }
        //Constructor that takes parameters for all attributes
        public Enemy(string Name, int Health, int Score, int Damage, int ApDamage, double Defence, int Evasion = 2)
        {
            this.Name = Name;
            this.Health = Health;
            CurrentHealth = Health;

            this.Damage = Damage;
            this.ApDamage = ApDamage;

            this.Defence = Defence;
            this.Evasion = Evasion;
            this.Score = Score;
        }

        //function to damage the enemy
        public int Injure(int dmg)
        {
            CurrentHealth -= dmg;
            return CurrentHealth;
        }
    }

    //Class that is the game object itself, is implemented in the main "runner" 
    public static class MainGame
    {
        private static string GameName;
        private static string PlayerName;
        //Key 24 Bytes long
        private static string EncryptionKey = "ART0R1A_ISNT_4_CH33T3RS!";
        private static Coords LastCoordinates = new Coords();
        private static Coords Coordinates = new Coords();

        private static Player Player;
        private static Enemy CurrentEnemy = null;
        private static bool ActiveEnemy = false;

        //Load map size
        private static GameData GameConfigurations = new GameData();
        //multi_array of MapTiles for the map
        private static MapTile[,] Map;

        private static List<Weapon> Weapons = new List<Weapon>();
        private static List<Armor> Armors = new List<Armor>();
        private static List<Item> Items = new List<Item>();
        private static List<Enemy> Enemies = new List<Enemy>();
        private static List<Event> Events = new List<Event>();
        private static MyRandom RandomGen = new MyRandom();
        private static bool PlayerDied = false;

        //Default Constructor
        static MainGame()
        {
            //Set Default Values

            //Load map size
            Map = new MapTile[GameConfigurations.XMax, GameConfigurations.YMax];

            //Load Game Name
            GameName = GameConfigurations.GameName;

            //No active enemy
            ActiveEnemy = false;

            //Setup Coords at Starting Position
            Coordinates.x = GameConfigurations.StartingPosition.x;
            Coordinates.y = GameConfigurations.StartingPosition.y;

            //ToDo:Check if need to x and y
            LastCoordinates.x = Coordinates.x;
            LastCoordinates.y = Coordinates.y;

            //Load Game Data From XML Files
            PopulateEnemies();
            PopulateItems();
            PopulateMap();
            PopulateEvents();

            //Start Running Game
            //StartGame();
        }

        //ToDo rework to fit new load methods
        //Function that starts your first run of the game
        public static string StartGame(string playerName, bool newGame)
        {
            PlayerName = playerName;
            StringBuilder returnString = new StringBuilder();
            if (newGame)
            {
                //Open Class Choice Form
                Form currentForm = Form.ActiveForm;
                currentForm.Hide();

                using (frmClassSelection classSelection = new frmClassSelection(playerName))
                    classSelection.ShowDialog();
                currentForm.Show();

                //Show User Introduction text.
                returnString.AppendLine("Welcome to " + GameName + " " + playerName + ".");

                //Add Custom Flavor Text
                returnString.AppendLine(GameConfigurations.GameFlavorText);

                //Add Instructions
                returnString.AppendLine("You can move around the map by using the four arrow buttons 'north', 'south', 'east', and 'west'.");
                //ToDo: Add good instructions
                returnString.AppendLine("If you encounter an enemy, attack them by pressing the Sword icon, or surrender yourself to the enemy by pressing the Flag icon.");
                returnString.AppendLine("If you wish to quit the game, type 'StopPlayingArtoria'");
                returnString.AppendLine("If you wish to view more detailed information about Game mechanics, type 'help'.");

                

                return returnString.ToString();
            }
            else
            {
                return "";
            }
        }

        //Function load the game's map from XML and store it in the array of MapTile objects
        static void PopulateMap()
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
                        EventID = (int)elem.Element("EventID"),
                        SpawnZone = (int)elem.Element("SpawnZone")
                    }).ToList();
            foreach(MapTile currentTile in tiles)
            {
                Map[currentTile.X, currentTile.Y] = currentTile;
            }
        }

        //Function to load the game's Events from XML and store it in a list of Event objects.
        static void PopulateEvents()
        {
            //Load XML document containing items.
            XDocument xdoc = XDocument.Load("Events.xml");

            //Populate list with tile objects to store in array later
            List<Event> events =
                (
                    from elem in xdoc.Root.Elements("Event")
                    select new Event
                    {
                        EventID = (int)elem.Element("EventID"),
                        Description = (string)elem.Element("Description"),
                        EnemySpawn = (int)elem.Element("XCoord"),
                        ItemSpawn = (int)elem.Element("YCoord"),
                        NothingSpawn = (int)elem.Element("XCoord"),
                        WeaponChance = (int)elem.Element("YCoord"),
                        ArmorChance = (int)elem.Element("XCoord"),
                        ScoreItemChance = (int)elem.Element("YCoord")
                    }).ToList();
            Events = events;
        }

        //Function load the game's items from XML and store them in the appropriate lists
        static void PopulateItems()
        {
            //Load XML document containing items.
            XDocument xdoc = XDocument.Load("Items.xml");

            //Populate Weapons
            List<Weapon> weapons =
                (
                    from elem in xdoc.Root.Elements("Weapon")
                    select new Weapon
                    {
                        Name = (string)elem.Element("Name"),
                        Value = (int)elem.Element("GoldValue"),
                        BDamage = (int)elem.Element("BaseDMG"),
                        APDamage = (int)elem.Element("APDMG"),
                        Evasion = (int)elem.Element("Evasion"),
                        Rarity = (int)elem.Element("Rarity"),
                        Description = (string)elem.Element("Description")
                    }).ToList();
            Weapons = weapons;

            //Populate Armor
            List<Armor> armors =
                (
                    from elem in xdoc.Root.Elements("Armor")
                    select new Armor
                    {
                        Name = (string)elem.Element("Name"),
                        Value = (int)elem.Element("GoldValue"),
                        ArmorVal = (int)elem.Element("ArmorVal"),
                        Evasion = (int)elem.Element("Evasion"),
                        Rarity = (int)elem.Element("Rarity"),
                        Description = (string)elem.Element("Description")
                    }).ToList();
            Armors = armors;

            //Populate Items
            List<Item> items =
                (
                    from elem in xdoc.Root.Elements("Item")
                    select new Item
                    {
                        Name = (string)elem.Element("Name"),
                        Value = (int)elem.Element("GoldValue"),
                        Description = (string)elem.Element("Description")
                    }).ToList();
            Items = items;
        }

        //Function load the game's enemies from XML and store them in a List of Enemy objects
        static void PopulateEnemies()
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
                        Health = (int)elem.Element("Health"),
                        Damage = (int)elem.Element("BaseDMG"),
                        ApDamage = (int)elem.Element("APDMG"),
                        Defence = (int)elem.Element("Defence"),
                        Evasion = (int)elem.Element("Evasion"),
                        Score = (int)elem.Element("Points"),
                        SpawnZone = (int)elem.Element("SpawnZone")
                    }).ToList();
            Enemies = enemies;
        }

        //ToDo: TESTING
        //Function that will save a player's progress in a text file.
        static void SaveProgress()
        {
            //TODO:
            //Change to this 
            //File.WriteAllText(path, saveDataEncrypted);
            //From AddressBook Project.
            //Add estus kindle level.
            //NEEDS TESTING
            System.IO.Directory.CreateDirectory("SaveData/");
            try
            {
                Crypter cryptic = new Crypter(EncryptionKey);
                string path = "SaveData/" + Player.Name + ".txt";
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    string saveData;
                    saveData = Player.getSaveData() + ',' + Coordinates.x + ',' + Coordinates.y;
                    string saveDataEncrypted = string.Empty;
                    saveDataEncrypted = cryptic.Encrypt(saveData);
                    sw.WriteLine(saveDataEncrypted);
                    Console.WriteLine("Saved successfully.");
                }
            }
            catch (Exception ex)
            {
                System.IO.Directory.CreateDirectory("LogFiles/");
                string logTitle = "Log instance from: " + DateTime.Now.Month + '/' + DateTime.Now.Day + '/' + DateTime.Now.Year;
                string path = "LogFiles/errorLog.txt";
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(logTitle + Environment.NewLine + ex.Message + Environment.NewLine + "Stack Trace:" + Environment.NewLine + ex.StackTrace + Environment.NewLine);
                }
                Console.WriteLine("Save could not be Completed, error notes saved to log file.");
            }

        }

        //ToDo: Test Load Funcitonality
        //Function that will load a player's save from a text file.
        public static bool LoadProgress(string name, bool secondPass)
        {
            //NEEDS TESTING
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
                    bool found = false;
                    foreach (string line in saves)
                    {
                        var splat = line.Split(',');
                        if (splat[0] == name)
                        {
                            //name, health, weapon name, armor name, score, x, y

                            int cHealth;

                            if (!int.TryParse(splat[1], out cHealth))
                            {
                                cHealth = 100;
                            }

                            string weapon = splat[2];
                            string armor = splat[3];

                            int score;
                            if (!int.TryParse(splat[4], out score))
                            {
                                score = 0;
                            }

                            int x;
                            if (!int.TryParse(splat[5], out x))
                            {
                                x = 7;
                                Console.WriteLine("Your save data has been corrupted.");
                            }

                            int y;
                            if (!int.TryParse(splat[6], out y))
                            {
                                y = 7;
                                Console.WriteLine("Your save data has been corrupted.");
                            }
                            
                            Weapon wep = FindWeapon(weapon);
                            Armor arm = FindArmor(armor);
                            Player p = new Player(name, 100, cHealth, wep, arm, score);
                            Player = p;
                            Coordinates.x = x;
                            Coordinates.y = y;

                            found = true;

                            return true;
                        }
                    }
                    if (!found)
                    {
                        if (!secondPass)
                        {
                            DialogResult result1 = MessageBox.Show("Your save could not be found, would you like to try again?", GameName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if (result1 == DialogResult.Yes)
                            {
                                //User wants to try again, allow for one more try.
                                LoadProgress(name, true);
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
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: The file could not be loaded.");
            }
            return false;
        }

        //Function to evaluate user's input
        static public string EvaluateInput(string userInput)
        {
            userInput = userInput.ToLower();
            if (userInput == "north")
            {
                if (!ActiveEnemy)
                {
                    //moving north
                    Coordinates.north();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "south")
            {
                if (!ActiveEnemy)
                {
                    //moving south
                    Coordinates.south();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "east")
            {
                if (!ActiveEnemy)
                {
                    //moving east
                    Coordinates.east();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "west")
            {
                if (!ActiveEnemy)
                {
                    //moving west
                    Coordinates.west();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "attack")
            {
                if (ActiveEnemy)
                {
                    BattleEnemy();
                }
                else
                {
                    Console.WriteLine("There is no enemy attacking you, move around to find enemies.\nType 'help_me' for detailed instructions.");
                }

                if (PlayerDied)
                {
                    Console.WriteLine("You have been killed by " + CurrentEnemy.Name);
                    return "QUIT";
                }
                return "battle";
            }
            else if (userInput == "estus")
            {
                //Drink Estus
                Player.DrinkEstus();
            }
            else if (userInput == "stopplayingartoria")
            {
                string ret = QuitGame();
                return ret;
            }
            else if (userInput == "help_me")
            {
                Console.WriteLine("You can move around the map typing the following commands: 'north', 'south', 'east', and 'west'\nIf you encounter an enemy, attack them by typing 'attack' If you wish to quit the game, type 'StopPlayingArtoria'");
                return "helped";
            }
            else if (userInput == "surrender")
            {
                if (ActiveEnemy)
                {
                    Console.WriteLine("You have surrendered your life to the " + CurrentEnemy.Name);
                    PlayerDied = true;
                    return "QUIT";
                }
                else
                {
                    Console.WriteLine("There is no enemy to surrender to.");
                    return "failedsurrender";
                }
            }
            else
            {
                return "false";
            }
            return "true";
        }

        //Function to generate the message output to the console.
        static public string genMessage()
        {
            //get the current map tile from the map container
            //!!IMPORTANT!! Map was created (Y,X) not (X,Y) I'm dumb but remember. 5/30/18
            MapTile currentTile = Map[Coordinates.y, Coordinates.x];
            if (!ActiveEnemy)
            {
                //variable to format final return string
                var returnString = currentTile.Message;

                //LINQ Query to grab the current event.
                var eventQuery = from eve in Events where eve.EventID == currentTile.EventID select eve;
                //If the user followed setup instructions correctly, there should only ever be one Event for each EventID
                Event currentEvent = eventQuery.First();

                //Check for enemy encounter first.
                //If EnemySpawn is greater than a random val 1-100 then an enemy was encountered.
                if (currentEvent.EnemySpawn > RandomGen.rollDie(100))
                {
                    //An Enemy was encountered
                    ActiveEnemy = true;

                    //LINQ Query to grab all enemies that can spawn in this zone.
                    var enemyQuery = from enem in Enemies where enem.SpawnZone == currentEvent.EnemySpawn select enem;

                    //Randomly pick an enemy from that list.
                    var index = RandomGen.rollDie(enemyQuery.Count() - 1);
                    CurrentEnemy = enemyQuery.ElementAt(index);
                    
                    returnString = CurrentEnemy.Name + " has attacked you!" + "\n" + "Type 'attack' to attack.";
                    return returnString;
                }
                else if (currentEvent.ItemSpawn > RandomGen.rollDie(100))
                {
                    //An item was found, determine what type of item.
                    var itemGenIndex = RandomGen.rollDie(100);
                    if(itemGenIndex <= currentEvent.WeaponChance)
                    {
                        //Index was between 0 and weapon max, a weapon was found.
                        GetWeapon();
                    }
                    else if (itemGenIndex >currentEvent.WeaponChance && itemGenIndex <= currentEvent.ArmorChance)
                    {
                        //index was between weapon max and armor max, armor was found.
                        GetArmor();
                    }
                    else if(itemGenIndex > currentEvent.ArmorChance && itemGenIndex <= currentEvent.ScoreItemChance)
                    {
                        //index was between armor max and score item max, a score item was found.
                        returnString += GetScoreItem();
                    }
                }
                else
                {
                    //Nothing Happened, return default tile message
                    return returnString;
                }

                //set tempCoords to keep track of last tile.
                LastCoordinates = Coordinates;
                LastCoordinates.x = Coordinates.x;
                LastCoordinates.y = Coordinates.y;
                return returnString;
            }
            //If the method didn't return for a normal reason, just return the tile message.
            return currentTile.Message;
        }

        //function to generate a weapon
        static void GetWeapon()
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
                    //a common weapon was found
                    weapons = from wep in Weapons where wep.Rarity == 1 select wep;
                    index = RandomGen.rollDie(weapons.Count() - 1);
                    Player.SwapWeapon(Weapons[index]);
                    break;
                case 3:
                case 4:
                    //An uncommon weapon was found
                    weapons = from wep in Weapons where wep.Rarity == 2 select wep;
                    index = RandomGen.rollDie(weapons.Count() - 1);
                    Player.SwapWeapon(Weapons[index]);
                    break;
                case 5:
                case 6:
                    //A rare weapon was found.
                    weapons = from wep in Weapons where wep.Rarity == 3 select wep;
                    index = RandomGen.rollDie(weapons.Count() - 1);
                    Player.SwapWeapon(Weapons[index]);
                    break;
                default:
                    //Should never reach this point...
                    break;
            }
        }

        //function to generate armor
        static void GetArmor()
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
                    //Common armor was found
                    armors = from arm in Armors where arm.Rarity == 1 select arm;
                    index = RandomGen.rollDie(armors.Count() - 1);
                    Player.SwapArmor(Armors[index]);
                    break;
                case 3:
                case 4:
                    //Uncommon armor was found
                    armors = from arm in Armors where arm.Rarity == 2 select arm;
                    index = RandomGen.rollDie(armors.Count() - 1);
                    Player.SwapArmor(Armors[index]);
                    break;
                case 5:
                case 6:
                    //Rare armor was found.
                    armors = from arm in Armors where arm.Rarity == 3 select arm;
                    index = RandomGen.rollDie(armors.Count() - 1);
                    Player.SwapArmor(Armors[index]);
                    break;
                default:
                    //Should never reach this point...
                    break;
            }
        }

        static string GetScoreItem()
        {
            //Get random value to determine what item was found
            var index = RandomGen.rollDie(Items.Count() - 1);

            Item item = Items.ElementAt(index);
            //Add item value to the player's score
            Player.Score += item.Value;

            //tell the player what they found.
            return "You have found one " + item.Name + ".\n" + item.Description + "\n" + item.Value + " points have been added to your score.";
        }

        //function to initiate a round of combat with the current enemy.
        static void BattleEnemy()
        {
            //Get Data for player and enemy: Attack Ratings, Defense Ratings, Evasion.
            #region Get Values
            //enemy data
            int enemyBaseDamage = CurrentEnemy.Damage;
            int enemyAPDamage = CurrentEnemy.ApDamage;

            //Turn values into doubles for multiplication
            double enemyPhysicalDef = CurrentEnemy.Defence;

            int enemyDodgeChance = CurrentEnemy.Evasion;

            //player data
            int playerBaseDamage = Player.GetWeapon().BDamage;
            int playerAPDamage = Player.GetWeapon().APDamage;

            //Turn values into doubles for multiplication
            double playerDefence = Player.GetWeapon().BDamage;

            int playerEvasion = Player.GetEvasion();
            
            #endregion

            int playerDamageTaken = 0;
            int enemyDamageTaken = 0;

            //Calculate damage enemy deals
            playerDamageTaken += (enemyBaseDamage - Convert.ToInt32(Math.Ceiling((enemyBaseDamage * playerDefence))));
            playerDamageTaken += enemyAPDamage;


            //Calculate damage player deals
            enemyDamageTaken += (playerBaseDamage - Convert.ToInt32(Math.Ceiling((playerBaseDamage * enemyPhysicalDef))));
            enemyDamageTaken += playerAPDamage;


            if (playerEvasion > RandomGen.rollDie(100))
            {
                Console.WriteLine("You dodged the " + CurrentEnemy.Name + "'s attack!");
            }
            else
            {
                if (enemyDodgeChance > RandomGen.rollDie(100))
                {
                    Console.WriteLine("The" + CurrentEnemy.Name + " has dodged your attack!");
                }
                else
                {
                    Console.WriteLine("You have traded blows with the " + CurrentEnemy.Name + "." + Environment.NewLine + "You have taken " + playerDamageTaken + " damage and dealt " + enemyDamageTaken + " damage.");
                }
            }

            //Check if player died
            int playerHealthLeft = Player.Injure(playerDamageTaken);
            if (playerHealthLeft < 1)
            {
                PlayerDied = true;
                //EXIT GAME
            }
            else
            {
                Console.WriteLine("You have " + playerHealthLeft + " health left.");
            }

            //Check if enemy died
            int enemyHP = 0;
            enemyHP = CurrentEnemy.Injure(enemyDamageTaken);
            if (!(enemyHP < 1))
            {
                Console.WriteLine("The enemy remains with " + enemyHP + " health.");
            }
            else
            {
                Console.WriteLine("You have defeated the " + CurrentEnemy.Name + ", " + CurrentEnemy.Score + " was added to your kill score.");
                ActiveEnemy = false;
            }
        }

        //function to quit the game
        static string QuitGame()
        {
            string input;
            Console.WriteLine("Are you sure you want to Quit?(Y/N)");
            input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                bool keepRunning = true;
                do
                {
                    Console.WriteLine("Do you want to save your game?(Y/N)");
                    input = Console.ReadLine();
                    if (input.ToLower() == "y")
                    {
                        SaveProgress();
                        Console.WriteLine("Thank you for playing Artoria, created by Jake Farley.");
                        keepRunning = false;
                    }
                    else if (input.ToLower() == "n")
                    {
                        Console.WriteLine("Thank you for playing Artoria, created by Jake Farley.");
                        keepRunning = false;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid response.");
                    }
                } while (keepRunning);
            }
            else if (input.ToLower() == "n")
            {
                Console.WriteLine("You are no longer quitting the game.");
                return "true"; //true because evalUserInput returns true
            }
            return "QUIT";
        }

        //function to find a weapon based on its name
        static Weapon FindWeapon(string weaponText)
        {
            var name = weaponText;
            Weapon weapon = new Weapon();
            foreach (Weapon wep in Weapons)
            {

                if (name.ToLower() == (wep.getName()).ToLower())
                {
                    weapon = wep;
                }
            }
            return weapon;
        }

        //Function to find a set of armor based on its name
        static Armor FindArmor(string name)
        {
            Armor armor = new Armor();
            foreach (Armor arm in Armors)
            {
                if (name.ToLower() == (arm.getName()).ToLower())
                {
                    armor = arm;
                }
            }
            return armor;
        }

    }
}