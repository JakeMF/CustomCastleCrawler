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
    
    //OLD Map Code: enum mapSize { Rows = 20, Columns = 20 };

    //Class to load and store the maximum size of the map.
    //ToDo: Ensure XML file is setup properly.
    public sealed class GameData
    {
        public int XMax { get; set; }
        public int YMax { get; set; }
        public string GameName { get; set; }
        public string GameFlavorText { get; set; }
        public int StartingX { get; set; }
        public int StartingY { get; set; }
        public int MaxEstus { get; set; }
        public List<StartingClass> Classes = new List<StartingClass>();
        
        public GameData()
        {
            LoadGameData();
        }
        private void LoadGameData()
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
            //Basic Settings
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
    }
    //Simple class to store the attributes of a starting class.
    public sealed class StartingClass
    {
        public string ClassWeapon;
        public string ClassArmor;
        public string ClassName;
        public string ClassDescription;
        
        public StartingClass(string name, string armor, string weapon, string Description)
        {
            ClassName = name;
            ClassWeapon = weapon;
            ClassArmor = armor;
            ClassDescription = Description;
        }
    }

    //Class to represent a pair of (X,Y) coordinates
    public sealed class Coords
    {
        public int x;
        public int y;
        private GameData MapDimensions;

        public Coords()
        {
            x = 0;
            y = 0;
            MapDimensions = new GameData();
        }
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
        public int EnemySpawn;
        public int ItemSpawn;
        public int NothingSpawn;
        public int WeaponChance;
        public int ArmorChance;
        public int ScoreItemChance;
        public int RestLocation;
        public int EnemySpawnZone;

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
    }

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
        public Weapon(string Name, int Value, string Description, int BDamage, int APDamage, int Evasion)
        {
            this.Name = Name;
            this.Value = Value;
            this.Description = Description;
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
        public Armor(string Name, int Value, string Description, int ArmorVal, int Evasion)
        {
            this.Name = Name;
            this.Value = Value;
            this.Description = Description;
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
        private int MaxStamina;
        private int Stamina;
        public Weapon Weapon { get; set; }
        public Armor Armor { get; set; }
        public int Score { get; set; }
        public int EnemiesKilled = 0;
        public int Estus;
        public int MaxEstus;
        //Default constructor
        public Player()
        {
            Name = "Nameless Hollow";
            MaxHealth = 1200;
            Health = MaxHealth;
            MaxStamina = 100;
            Stamina = MaxStamina;
            Weapon = new Weapon();
            Armor = new Armor();
            Score = 0;
            MaxEstus = 5;
            Estus = MaxEstus;
        }

        //Constructor that will be used for new players
        public Player(string Name, Weapon StartingWeapon, Armor StartingArmor)
        {
            this.Name = Name;
            MaxHealth = 1200;
            Health = MaxHealth;
            MaxStamina = 100;
            Stamina = MaxStamina;

            Weapon = StartingWeapon;
            Armor = StartingArmor;
            Score = 0;

            MaxEstus = 5;
            Estus = MaxEstus;
        }

        //Constructor that will be used to load a saved game
        public Player(string Name, int MaxHealth, int Health, int MaxStamina, int Stamina, Weapon Weapon, Armor Armor, int Score)
        {
            this.Name = Name;
            this.MaxHealth = MaxHealth;
            this.Health = Health;
            this.MaxStamina = MaxStamina;
            this.Stamina = Stamina;
            this.Weapon = Weapon;
            this.Armor = Armor;
            this.Score = Score;
            //Players get 5 heals by default, like in Dark Souls 1. 
            MaxEstus = 5;
            Estus = MaxEstus;
        }

        //Function to get the player's data in a csv format to save.
        public string getSaveData()
        {
            string data;
            char delim = ',';

            data = Name + delim + MaxHealth + delim + Health + delim + MaxStamina + delim + Stamina + delim + Weapon.Name + delim + Armor.Name + delim + Score;
            return data;
        }
        
        //function to get the players evasion
        public int GetEvasion()
        {
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
            Health -= dmg;
            return Health;
        }

        public string DrinkEstus()
        {
            if(Estus > 0 && Estus < MaxEstus)
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

        public string RefillEstus()
        {
            Estus = MaxEstus;
            return "Your healing flask has been filled. " + Estus + " sips remaining.";
        }
        
        //Function to perform a stamin draining action. Returns true if action could be completed with current stam, false if not enough stam.
        public bool PerformAction(int StaminaCost)
        {
            int tempStam = Stamina;
            Stamina -= StaminaCost;

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

        public int RegenStamina(int Regen)
        {
            Stamina += Regen;

            if(Stamina > MaxStamina)
            {
                Stamina = MaxStamina;
            }

            return Stamina;
        }

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
        public Enemy(string Name, int MaxHealth, int Score, int Damage, int ApDamage, double Defence, int Evasion = 2)
        {
            this.Name = Name;
            this.MaxHealth = MaxHealth;
            CurrentHealth = MaxHealth;

            this.Damage = Damage;
            this.ApDamage = ApDamage;

            this.Defence = Defence;
            this.Evasion = Evasion;
            this.Score = Score;
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

        public string GetHealth()
        {
            return CurrentHealth + "/" + MaxHealth;
        }
    }

    //Class that is the game object itself, is implemented in the main "runner" 
    public class MainGame
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
        private GameData GameConfigurations = new GameData();
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
            //Set Default Values

            //Load map size
            Map = new MapTile[GameConfigurations.XMax, GameConfigurations.YMax];

            //Load Game Name
            GameName = GameConfigurations.GameName;

            //No active enemy
            ActiveEnemy = false;

            //Setup Coords at Starting Position
            Coordinates.x = GameConfigurations.StartingX;
            Coordinates.y = GameConfigurations.StartingY;

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
        //Function that creates introduction string.
        public string StartGame(string playerName, bool newGame)
        {
            PlayerName = playerName;
            StringBuilder introMessage = new StringBuilder();

            if (newGame)
            {
                //Show User Introduction text.
                introMessage.AppendLine("Welcome to " + GameName + " " + playerName + ".");

                //Add Custom Flavor Text
                introMessage.AppendLine(GameConfigurations.GameFlavorText);
                introMessage.AppendLine(Environment.NewLine);

                //Add Instructions
                introMessage.Append("You can move around the map by using the four arrow buttons 'north', 'south', 'east', and 'west'.");
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
            return introMessage.ToString();
        }

        //Function load the game's map from XML and store it in the array of MapTile objects
        void PopulateMap()
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
        void PopulateEvents()
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

        }

        //Function load the game's items from XML and store them in the appropriate lists
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
        }

        //ToDo: TESTING
        //Function that will save a player's progress in a text file.
        public void SaveProgress(string Notes)
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
                    saveData = Player.getSaveData() + ',' + Coordinates.x + ',' + Coordinates.y + ',' + Notes;
                    string saveDataEncrypted = string.Empty;
                    saveDataEncrypted = cryptic.Encrypt(saveData);
                    sw.WriteLine(saveDataEncrypted);
                    MessageBox.Show("Game saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Save could not be Completed, error notes saved to log file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //ToDo: Test Load Funcitonality 
        //Function that will load a player's save from a text file.
        public string LoadProgress(string name, bool secondPass)
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
                    foreach (string line in saves)
                    {
                        var splat = line.Split(',');
                        if (splat[0] == name)
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
                                Coordinates.x = x;
                                Coordinates.y = y;

                                if(missingVals)
                                {
                                    MessageBox.Show("Your save data has been corrupted, please start a new game.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                return "success";
                            }
                            else
                            {
                                //if there are not enough values in the array, the data is either from a version with different save data or has been manually modified.
                                MessageBox.Show("Your save data has been corrupted, your data cannot be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            return "fail";
        }

        public string SelectClass(string playerName, string playerClass)
        {
            StartingClass selectedClass = GameConfigurations.Classes.First(s => s.ClassName == playerClass);

            Weapon selectedWeapon = FindWeapon(selectedClass.ClassWeapon);
            Armor selectedArmor = FindArmor(selectedClass.ClassArmor);

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
                    if(Coordinates.north())
                    {
                        //Moved successfully
                        returnString.AppendLine(genMessage());
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
                    if(Coordinates.south())
                    {
                        //Moved successfully
                        returnString.AppendLine(genMessage());
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
                    if(Coordinates.east())
                    {
                        //Moved successfully
                        returnString.AppendLine(genMessage());
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
                    if(Coordinates.west())
                    {
                        //Moved successfully
                        returnString.AppendLine(genMessage());
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
                MessageBox.Show("ERROR: Unknown Input. | Method: EvaluateInput | Line: 1105", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                returnString.AppendLine("Error Evaluating Input. Please restart application.");
            }
            return returnString.ToString();
        }

        //Function to generate the message output to the console.
        string genMessage()
        {
            //get the current map tile from the map container
            //!!IMPORTANT!! Map was created (Y,X) not (X,Y) I'm dumb but remember. 5/30/18
            //I have changed it back to x,y.
            MapTile currentTile = Map[Coordinates.x, Coordinates.y];
            if (!ActiveEnemy)
            {
                //variable to format final return string
                var returnString = new StringBuilder();
                returnString.AppendLine(currentTile.Message);

                //LINQ Query to grab the current event.
                var eventQuery = from eve in Events where eve.EventID == currentTile.EventID select eve;
                //If the user followed setup instructions correctly, there should only ever be one Event for each EventID
                Event currentEvent = eventQuery.First();

                //The EventID -1 is used to mark an unpassable location.
                if(currentEvent.EventID == -1)
                {
                    Coordinates.x = LastCoordinates.x;
                    Coordinates.y = LastCoordinates.y;
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
                        var index = RandomGen.rollDie(enemyQuery.Count() - 1);
                        CurrentEnemy = new Enemy(enemyQuery.ElementAt(index));

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

                            string returnContents = returnString.ToString();
                            returnString.Clear();

                            returnString.AppendLine("NewWeapon" + '|' + returnContents.ToString());
                        }
                        else if (itemGenIndex > currentEvent.WeaponChance && itemGenIndex <= currentEvent.ArmorChance)
                        {
                            //index was between weapon max and armor max, armor was found.
                            TempArmor = GetNewArmor();

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
                    returnString.AppendLine(Player.RefillEstus());
                }
                //set tempCoords to keep track of last tile.
                LastCoordinates.x = Coordinates.x;
                LastCoordinates.y = Coordinates.y;
                return returnString.ToString();
            }

            //If the method didn't return for a normal reason, just return the tile message.
            return currentTile.Message;
        }

        //function to generate a weapon
        Weapon GetNewWeapon()
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
                    index = RandomGen.rollDie(weapons.Count());
                    return Weapons[index - 1];
                case 3:
                case 4:
                    //An uncommon weapon was found
                    weapons = from wep in Weapons where wep.Rarity == 2 select wep;
                    index = RandomGen.rollDie(weapons.Count());
                    return Weapons[index - 1];
                case 5:
                case 6:
                    //A rare weapon was found.
                    weapons = from wep in Weapons where wep.Rarity == 3 select wep;
                    index = RandomGen.rollDie(weapons.Count());
                    return Weapons[index - 1];
                default:
                    //Should never reach this point...
                    return new Weapon();
            }
        }

        //function to generate armor
        Armor GetNewArmor()
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
                    index = RandomGen.rollDie(armors.Count());
                    return Armors[index - 1];
                case 3:
                case 4:
                    //Uncommon armor was found
                    armors = from arm in Armors where arm.Rarity == 2 select arm;
                    index = RandomGen.rollDie(armors.Count());
                    return Armors[index - 1];
                case 5:
                case 6:
                    //Rare armor was found.
                    armors = from arm in Armors where arm.Rarity == 3 select arm;
                    index = RandomGen.rollDie(armors.Count());
                    return Armors[index - 1];
                default:
                    //Should never reach this point...
                    return new Armor();
            }
        }

        string GetNewScoreItem()
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
        public string BattleEnemy(string action)
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
                playerBlocking = true;
                enemyBaseDamage /= 2;
                enemyAPDamage /= 2;
                returnString.AppendLine("You have blocked the enemy's attack, you will take half damage and regain some stamina.");
                Player.RegenStamina(20);
            }
            else
            {
                if(!Player.PerformAction(10))
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
                    }
                }
            }

            //Regen 5 stam per turn.
            Player.RegenStamina(5);

            return Player.GetHealthAndStamina() + "|"  + CurrentEnemy.GetHealth() + "|" + returnString.ToString();
        }

        //function to find a weapon based on its name
        Weapon FindWeapon(string weaponText)
        {
            var name = weaponText;
            Weapon weapon = new Weapon();
            foreach (Weapon wep in Weapons)
            {

                if (name.ToLower() == (wep.Name).ToLower())
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
            foreach (Armor arm in Armors)
            {
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
            returnString.AppendLine(System.Environment.NewLine);

            //Armor
            returnString.AppendLine("Armor");
            returnString.AppendLine(currentArmor.Name);
            returnString.AppendLine("Rarity: " + currentArmor.Rarity);
            returnString.AppendLine(currentArmor.Description);
            returnString.AppendLine("Armor Value: " + currentArmor.ArmorVal);


            return returnString.ToString();
        }

        public string GenerateStatistics()
        {
            var returnString = new StringBuilder();

            returnString.AppendLine(Player.Name);
            returnString.AppendLine(System.Environment.NewLine);
            returnString.AppendLine("Final Weapon: " + Player.Weapon.Name + " Rarity " + Player.Weapon.Rarity);
            returnString.AppendLine("Final Armor: " + Player.Armor.Name + " Rarity " + Player.Armor.Rarity);
            returnString.AppendLine(System.Environment.NewLine);
            returnString.AppendLine("Turns Survived: " + TurnCount);
            returnString.AppendLine("Enemies Defeated: " + Player.EnemiesKilled);
            returnString.AppendLine("Points earned: " + Player.Score);

            return returnString.ToString();

        }

        public void SwapWeapon(Weapon newWeapon)
        {
            Player.Weapon = newWeapon;
        }

        public void SwapArmor(Armor newArmor)
        {
            Player.Armor = newArmor;
        }

        public string healPlayer()
        {
            return Player.GetHealthAndStamina() + "|" + CurrentEnemy.GetHealth() + "|" + Player.DrinkEstus();
        }

        public string EscapeAttempt()
        {
            var returnString = new StringBuilder();

            //Get player's evasion to factor into escape chance.
            decimal playerEvasion = Player.GetEvasion();

            //Player's evasion factors into escape chance on a factor of 0.5 with a base 50% chance to escape. 
            //If palyer has negative evasion (meaning they never evade attacks), this will lower their overall escape change.
            //If player has 50 evasion (meaning they evade half of all attacks), they will have a 75% chance to evade.
            //If player has -50 evasion, they will have a 25% chance to evade.
            var escapeChance = 50 - Math.Ceiling(playerEvasion / 2);

            if (playerEvasion > RandomGen.rollDie(100))
            {
                //The player dodged the attack
                returnString.AppendLine("You escaped the enemy!");
                if (CurrentEnemy.Name.Substring(1, 1) == "a" || CurrentEnemy.Name.Substring(1, 1) == "e" || CurrentEnemy.Name.Substring(1, 1) == "i" || CurrentEnemy.Name.Substring(1, 1) == "o" || CurrentEnemy.Name.Substring(1, 1) == "u")
                {
                    TempMiscData = "You have encountered an " + CurrentEnemy.Name + " and successfully escaped." + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    TempMiscData = "You have encountered a " + CurrentEnemy.Name + " and successfully escaped." + Environment.NewLine + Environment.NewLine;
                }
                
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

        public Weapon GetPlayerWeapon()
        {
            return Player.Weapon;
        }

        public Armor GetPlayerArmor()
        {
            return Player.Armor;
        }

        public string GetPlayerHealthAndStamina()
        {
            return Player.GetHealthAndStamina();
        }
    }
}
