using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CustomCastleCrawler
{
    public sealed class GameSettings
    {
        public List<int> LoadMapSize()
        {
            var retList = new List<int>();
            XDocument xdoc = XDocument.Load("GameSettings.xml");
            //Populate list with tile objects to store in array later
            foreach (var elem in xdoc.Root.Elements("MapSize"))
            {
                Convert.ToInt16(elem.Element("XMax"));
                Convert.ToInt16(elem.Element("YMax"));
            }
            return retList;
        }
    }

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
    public sealed class MapSize
    {
        public MapSize()
        {
            LoadMapSize();
        }
        public int XMax { get; set; }
        public int YMax { get; set; }
        public List<int> LoadMapSize()
        {
            var retList = new List<int>();
            XDocument xdoc = XDocument.Load("GameSettings.xml");
            //Populate list with tile objects to store in array later
            foreach (var elem in xdoc.Root.Elements("MapSize"))
            {
                XMax = Convert.ToInt16(elem.Element("XMax").Value);
                YMax = Convert.ToInt16(elem.Element("YMax").Value);
            }
            return retList;
        }
    }

    //Class to represent a pair of (X,Y) coordinates
    class Coords
    {
        public int x = 0;
        public int y = 0;
        private MapSize MapDimensions = new MapSize();
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
        
    };
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
        private string Name;
        private int MaxHealth;
        private int Health;
        private Weapon Weapon = new Weapon();
        private Armor Armor = new Armor();
        private int Score = 0;
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
        public Player(string Name, int MaxHealth, int Health, Weapon Weapon, Armor Armor, int Score, int MaxWeight)
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
    public sealed class Artoria
    {
        private string PlayerName;
        //Key 24 Bytes long
        private string encryptionKey = "ART0R1A_ISNT_4_CH33T3RS!";
        private Coords lastCoordinates = new Coords();
        private Coords coordinates = new Coords();

        private Player player;
        private Enemy currentEnemy = null;
        private bool activeEnemy = false;

        //Load map size
        private MapSize MapDimensions = new MapSize();
        //multi_array of MapTiles for the map
        private MapTile[,] map;

        private List<Weapon> weapons = new List<Weapon>();
        private List<Armor> armors = new List<Armor>();
        private List<Item> items = new List<Item>();
        private List<Enemy> enemies = new List<Enemy>();
        private MyRandom randomGen = new MyRandom();
        private bool playerDied = false;

        //Default Constructor
        public Artoria()
        {
            //Set Default Values

            //Load map size
            map = new MapTile[MapDimensions.XMax, MapDimensions.YMax];

            //No active enemy
            activeEnemy = false;

            //Setup Coords at Starting Position
            coordinates.x = 7;
            coordinates.y = 7;

            //ToDo:Check if need to x and y
            lastCoordinates.x = coordinates.x;
            lastCoordinates.y = coordinates.y;

            //Load Game Data From XML Files
            PopulateEnemies();
            PopulateItems();
            PopulateMap();

            //Start Running Game
            //StartGame();
        }

        //Constructor for loading a saved game
        public Artoria(string PlayerName)
        {
            //Load Previous Game Data

            //Load map size
            map = new MapTile[MapDimensions.XMax, MapDimensions.YMax];

            //No active enemy
            activeEnemy = false;

            //Setup Coordinates 
            coordinates.x = 7;
            coordinates.y = 7;
            lastCoordinates = coordinates;

            //Load Game Data From XML
            PopulateItems();
            PopulateMap();
            PopulateEnemies();
            this.PlayerName = PlayerName;

            //Check if save game loaded properly
            if (!LoadProgress(PlayerName))
            {
                //Allow repeated attempt #1
                string response;
                Console.WriteLine("We could not load your save, would you like to try again? Y/N");
                response = Console.ReadLine();
                while (response.ToLower() != "n")
                {
                    //Allow repeated attempt #2
                    string name = "";
                    Console.WriteLine("Enter your name:");
                    response = Console.ReadLine();
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
                    {
                        if (!LoadProgress(name))
                        {
                            Console.WriteLine("We could not load your save, would you like to try again? Y/N");
                            response = Console.ReadLine();
                        }
                    }
                    else
                    {
                        //Load failed, start new game
                        Console.WriteLine("You have entered an invalid name, launching new game.");
                        StartGame();
                        response = "n";
                    }
                }
            }
            else
            {
                //Load was successful
                Console.WriteLine("Save successfully loaded. Welcome back, " + PlayerName);
            }
        }

        //Function that starts your first run of the game
        public void StartGame()
        {
            string playerName;
            Console.WriteLine("Welcome to Artoria, adventurer, what is your name?");
            playerName = Console.ReadLine();
            if (string.IsNullOrEmpty(playerName) || string.IsNullOrWhiteSpace(playerName))
            {
                Console.WriteLine("Please enter a name of at least 1 character.");
                playerName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(playerName) || string.IsNullOrEmpty(playerName))
                {
                    Console.WriteLine("You can't be named nothing, so I'll just call you adventurer.");
                    playerName = "Adventurer";
                }
            }
            Console.WriteLine(Environment.NewLine + "Welcome to Artoria, " + playerName);
            Console.WriteLine("Artoria is a text-based dungeon crawler created by Jake Farley. The goal of the game is to get the highest score.");
            Console.WriteLine("You score points by defeating monsters and picking up items, as you walk around the map " + Environment.NewLine + "you will encounter various enemies and find various items.");
            Console.WriteLine("As you fight enemies, you will take damage; be warned, there is no way to regain your health." + Environment.NewLine + "Once your health reaches zero, the game will end.");
            Console.WriteLine("You can move around the map by typing the four compass directions 'north', 'south', 'east', and 'west'.");
            Console.WriteLine("If you encounter an enemy, attack them by typing 'attack' you can type 'surrender' to surrender yourself to the enemy.");
            Console.WriteLine("If you wish to quit the game, type 'StopPlayingArtoria'" + Environment.NewLine);
            Console.WriteLine("If you wish to view more detailed information about Game mechanics, type 'help'." + Environment.NewLine);

            Console.WriteLine("Please select a starting class. There are four option available to you.");
            Console.WriteLine("'Soldier': A light melee focused class. Capable of dodging, but does moderate damage");
            Console.WriteLine("'Knight': A medium melee focused class. Dodges with difficulty, but does good damage.");
            Console.WriteLine("'Archer': An archery focused class. Dodges easily, but does little damage");
            Console.WriteLine("'Giant': A very heavy melee focused class. Never dodges, but does lots of damage");
            var chosenClass = Console.ReadLine().ToLower();
            if (chosenClass != "soldier" || chosenClass != "knight" || chosenClass != "archer" || chosenClass != "giant")
            {
                Console.WriteLine("Please select a starting class, if you do not make a selection you will be a Soldier.");
                Console.ReadLine().ToLower();
            }
            if (chosenClass != "soldier" || chosenClass != "knight" || chosenClass != "archer" || chosenClass != "giant")
            {
                chosenClass = "Soldier";
            }

            player = new Player(playerName, chosenClass);

            Console.WriteLine("Type a command to start.");

        }

        //Function load the game's map from XML and store it in the array of MapTile objects
        void PopulateMap()
        {
            map[0, 0] = new MapTile(0, 0, "You are in Blighttown.", 7);

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
            foreach(MapTile currentTile in tiles)
            {
                map[currentTile.X, currentTile.Y] = currentTile;
            }
        }

        //Function load the game's items from XML and store them in a List
        void PopulateItems()
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
            this.weapons = weapons;

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
            this.armors = armors;

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
            this.items = items;
        }
        
        //Function load the game's enemies from XML and store them in a List
        void PopulateEnemies()
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
            this.enemies = enemies;
        }

        //ToDo: TESTING
        //Function that will save a player's progress in a text file.
        void SaveProgress()
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
                Crypter cryptic = new Crypter(encryptionKey);
                string path = "SaveData/" + PlayerName + ".txt";
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    string saveData;
                    saveData = player.getSaveData() + ',' + coordinates.x + ',' + coordinates.y;
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

        //Function that will load a player's save from a text file.
        bool LoadProgress(string name)
        {
            //NEEDS TESTING
            Crypter cryptic = new Crypter(encryptionKey);
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

                            int weight;
                            if (!int.TryParse(splat[7], out weight))
                            {
                                weight = 100;
                                Console.WriteLine("Your save data has been corrupted.");
                            }

                            Weapon wep = FindWeapon(weapon);
                            Armor arm = FindArmor(armor);
                            Player p = new Player(name, 100, cHealth, wep, arm, score, weight);
                            player = p;
                            coordinates.x = x;
                            coordinates.y = y;

                            found = true;

                            return true;
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("Error: Your save could not be found.");
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
        public string EvaluateInput(string userInput)
        {
            userInput = userInput.ToLower();
            if (userInput == "north")
            {
                if (!activeEnemy)
                {
                    //moving north
                    coordinates.north();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "south")
            {
                if (!activeEnemy)
                {
                    //moving south
                    coordinates.south();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "east")
            {
                if (!activeEnemy)
                {
                    //moving east
                    coordinates.east();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "west")
            {
                if (!activeEnemy)
                {
                    //moving west
                    coordinates.west();
                }
                else
                {
                    Console.WriteLine("You cannot move when there is an enemy attacking you!\nType 'Attack' to attack the enemy. Type 'help_me' for detailed instructions.");
                    return "false";
                }
            }
            else if (userInput == "attack")
            {
                if (activeEnemy)
                {
                    BattleEnemy();
                }
                else
                {
                    Console.WriteLine("There is no enemy attacking you, move around to find enemies.\nType 'help_me' for detailed instructions.");
                }

                if (playerDied)
                {
                    Console.WriteLine("You have been killed by " + currentEnemy.Name);
                    return "QUIT";
                }
                return "battle";
            }
            else if (userInput == "estus")
            {
                //Drink Estus
                player.DrinkEstus();
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
                if (activeEnemy)
                {
                    Console.WriteLine("You have surrendered your life to the " + currentEnemy.Name);
                    playerDied = true;
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
        //COMMENTED OUT FOR TESTING
        /*
        public string genMessage()
        {
            //get the current map tile from the map container
            //!!IMPORTANT!! Map was created (Y,X) not (X,Y) I'm dumb but remember. 5/30/18
            MapTile currentTile = map[coordinates.y, coordinates.x];
            string message = currentTile.getMessage();
            int eventID = currentTile.getEventID();
            if (!activeEnemy)
            {
                //string variable to fill with text;
                string retStr = "";
                //stringstream to format final return string
                string returnString;
                //3 different areas each with event id(1-3), event ids are added for additional custom events
                switch (eventID)
                {
                    case 1:
                        //Blighttown
                        retStr = genBlighttown();
                        returnString = message + "\n" + retStr;
                        retStr = returnString;
                        break;
                    case 2:
                        //Irithyll Dungeon
                        retStr = genIrithyllDungeon();
                        returnString = message + "\n" + retStr;
                        retStr = returnString;
                        break;
                    case 3:
                        //Catacombs
                        retStr = genCatacombs();
                        returnString = message + "\n" + retStr;
                        retStr = returnString;
                        break;
                    case 4:
                        //Tomb of the Giants
                        retStr = genTombOfTheGiants();
                        returnString = message + "\n" + retStr;
                        retStr = returnString;
                        break;
                    //Special Cases
                    case 50:
                        //retStr = "This appears to be a safe place, nothing can surprise you here, but you will also find nothing here."; //OLD
                        retStr = genBonfire();
                        returnString = message + "\n" + retStr;
                        retStr = returnString;
                        break;
                    case -1:
                        //player cannot go here so reset their coordinates
                        coordinates.x = lastCoordinates.x;
                        coordinates.y = lastCoordinates.y;
                        returnString = message;
                        return returnString;
                    default:
                        // Case 0 is just print message(currently unused)
                        retStr = message;
                        break;
                }
                //set tempCoords to keep track of last tile.
                lastCoordinates = coordinates;
                lastCoordinates.x = coordinates.x;
                lastCoordinates.y = coordinates.y;
                return retStr;
            }
            return message;
        }
        
        
        #region Gen Area Methods
        //TODO: Update methods with proper enemy spawn chances and proper item spawns

        //function to determine what happens wheny you land on an Undead Settlement tile
        string genUndeadSettlement()
        {
            int val = randomGen.rollDTen();
            if (val <= 3)
            {
                //chance for nothing, or for healing potion or infuse mat.
                return "Nothing happened, you should keep moving.\n";
            }
            else if (val > 3 && val <= 7)
            {
                //Generate Enemy
                int nVal = randomGen.rollDTen();
                switch (nVal)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:                     //Name, Health, Souls, PhysDef, FireDef, LightningDef, MagicDef, DarkDef, PhysicalDmg, FireDmg, LightningDmg, MagicDmg, DarkDmg, Evasion Chance %.
                        currentEnemy = new Enemy("Peasant Holow", 121, 60, -0.24, -0.24, -0.24, -0.24, -0.24, 200);
                        break;
                    case 5:
                    case 6:
                    case 7:
                        currentEnemy = new Enemy("Hollow Slave", 114, 320, -0.17, -0.22, -0.13, -0.14, -0.14, 237);
                        break;
                    case 8:
                    case 9:
                        currentEnemy = new Enemy("Hollow Manservant", 436, 320, 0.02, -0.07, -0.02, -0.02, -0.02, 334);
                        break;
                    case 10:
                        currentEnemy = new Enemy("Evangelist", 519, 380, 0, 0.17, 0.03, 0.03, 0.03, 298);
                        break;
                }
                activeEnemy = true;


                string ret;
                ret = currentEnemy.Name + " has attacked you!" + "\n" + "Type 'attack' to attack.";
                return ret;
            }
            else if (val > 7)
            {
                //Generate Item
                int nVal = randomGen.rollDTen();
                switch (nVal)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        //weapon
                        getWeapon();
                        break;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        //armor
                        getArmor();
                        break;
                }
                return "";
            }
            return "";
        }
        
        //function for a bonfire tile
        string genBonfire()
        {
            string response = "You are comforted by the warmth of the Bonfire. You may stay as long as you like.";

            Console.WriteLine("You found a bonfire and decided to rest, your health has been restored.");

            //Heal player
            player.BonfireRest();
            Console.WriteLine();
            saveProgress();
            //response = Console.ReadLine(); //For when bonfires allow other options.

            return response;
        }

        #endregion
        
        */
        //function to generate a weapon
        void GetWeapon()
        {
            //TODO: Use DS weapons
            int val = randomGen.rollDie(100);

            if (val > 1 && val < 5)
            {
                //Iron Short
                player.SwapWeapon(weapons[1]);
            }
            else if (val > 5 && val < 10)
            {
                //Iron Long
                player.SwapWeapon(weapons[2]);
            }
            else if (val > 10 && val < 15)
            {
                //Steel Short
                player.SwapWeapon(weapons[3]);
            }
            else if (val > 15 && val < 35)
            {
                //Steel Long
                player.SwapWeapon(weapons[4]);
            }
            else if (val > 35 && val < 60)
            {
                //Bronze
                player.SwapWeapon(weapons[5]);
            }
            else if (val > 60 && val < 80)
            {
                //Ench Iron
                player.SwapWeapon(weapons[6]);
            }
            else if (val > 80 && val < 95)
            {
                //Ench Steel
                player.SwapWeapon(weapons[7]);
            }
            else if (val > 95 && val < 100)
            {
                //Obsidian
                player.SwapWeapon(weapons[8]);
            }
        }

        //function to generate armor
        void GetArmor()
        {
            //TODO: Use DS armor 
            int val = randomGen.rollDie(100);

            if (val > 1 && val < 10)
            {
                //Iron Plate
                player.SwapArmor(armors[1]);
            }
            else if (val > 10 && val < 20)
            {
                //Steel Plate
                player.SwapArmor(armors[2]);
            }
            else if (val > 20 && val < 50)
            {
                //Ench Iron
                player.SwapArmor(armors[3]);
            }
            else if (val > 50 && val < 70)
            {
                //Ench Steel
                player.SwapArmor(armors[4]);
            }
            else if (val > 70 && val < 90)
            {
                //Heavy Steel
                player.SwapArmor(armors[5]);
            }
            else if (val > 90 && val <= 100)
            {
                //Master Steel
                player.SwapArmor(armors[6]);
            }
        }

        //function to initiate a round of combat with the current enemy.
        void BattleEnemy()
        {
            //Get Data for player and enemy: Attack Ratings, Defense Ratings, Evasion.
            #region Get Values
            //enemy data
            int enemyBaseDamage = currentEnemy.Damage;
            int enemyAPDamage = currentEnemy.ApDamage;

            //Turn values into doubles for multiplication
            double enemyPhysicalDef = currentEnemy.Defence;

            int enemyDodgeChance = currentEnemy.Evasion;

            //player data
            int playerBaseDamage = player.GetWeapon().BDamage;
            int playerAPDamage = player.GetWeapon().APDamage;

            //Turn values into doubles for multiplication
            double playerDefence = player.GetWeapon().BDamage;

            int playerEvasion = player.GetEvasion();
            
            #endregion

            int playerDamageTaken = 0;
            int enemyDamageTaken = 0;

            //Calculate damage enemy deals
            playerDamageTaken += (enemyBaseDamage - Convert.ToInt32(Math.Ceiling((enemyBaseDamage * playerDefence))));
            playerDamageTaken += enemyAPDamage;


            //Calculate damage player deals
            enemyDamageTaken += (playerBaseDamage - Convert.ToInt32(Math.Ceiling((playerBaseDamage * enemyPhysicalDef))));
            enemyDamageTaken += playerAPDamage;


            if (playerEvasion > randomGen.rollDie(100))
            {
                Console.WriteLine("You dodged the " + currentEnemy.Name + "'s attack!");
            }
            else
            {
                if (enemyDodgeChance > randomGen.rollDie(100))
                {
                    Console.WriteLine("The" + currentEnemy.Name + " has dodged your attack!");
                }
                else
                {
                    Console.WriteLine("You have traded blows with the " + currentEnemy.Name + "." + Environment.NewLine + "You have taken " + playerDamageTaken + " damage and dealt " + enemyDamageTaken + " damage.");
                }
            }

            //Check if player died
            int playerHealthLeft = player.Injure(playerDamageTaken);
            if (playerHealthLeft < 1)
            {
                playerDied = true;
                //EXIT GAME
            }
            else
            {
                Console.WriteLine("You have " + playerHealthLeft + " health left.");
            }

            //Check if enemy died
            int enemyHP = 0;
            enemyHP = currentEnemy.Injure(enemyDamageTaken);
            if (!(enemyHP < 1))
            {
                Console.WriteLine("The enemy remains with " + enemyHP + " health.");
            }
            else
            {
                Console.WriteLine("You have defeated the " + currentEnemy.Name + ", " + currentEnemy.Score + " was added to your kill score.");
                activeEnemy = false;
            }
        }

        //function to quit the game
        string QuitGame()
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
        Weapon FindWeapon(string weaponText)
        {
            var name = weaponText;
            Weapon weapon = new Weapon();
            foreach (Weapon wep in weapons)
            {

                if (name.ToLower() == (wep.getName()).ToLower())
                {
                    weapon = wep;
                }
            }
            return weapon;
        }

        //Function to find a set of armor based on its name
        Armor FindArmor(string name)
        {
            Armor armor = new Armor();
            foreach (Armor arm in armors)
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
