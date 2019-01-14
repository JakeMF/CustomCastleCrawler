using CustomCastleCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OSTBAC
{
    class Program
    {
        static void Main(string[] args)
        {
            //For Testing
            var art = new Artoria(); 

            //Real Main
            /*
            string userInput;

            Console.WriteLine("Welcome to the game 'Artoria'. You can start a new game by typing 'new' " + Environment.NewLine + "Load an existing save by typing 'load' or view instructions on how to play the game by typing 'help_me'");
            userInput = Console.ReadLine();
            userInput = userInput.ToLower();
            if (userInput == "new")
            {
                Artoria artoriaGame = new Artoria();
                while (userInput != "QUIT")
                {
                    userInput = Console.ReadLine();
                    string ret = artoriaGame.evaluateInput(userInput);
                    if (ret == "true")
                    {
                        Console.WriteLine(artoriaGame.genMessage());
                        userInput = string.Empty;
                    }
                    else if (ret == "false")
                    {
                        Console.WriteLine("You have entered an invalid command. You can type 'help_me' for instructions.");
                        userInput = string.Empty;
                    }
                    else if (ret == "QUIT")
                    {
                        userInput = "QUIT";
                    }
                }
            }
            else if (userInput == "load")
            {
                Console.WriteLine("What is the name you used?");
                userInput = Console.ReadLine();
                Artoria artoriaGame = new Artoria(userInput);
                while (userInput != "QUIT")
                {
                    userInput = Console.ReadLine();
                    string ret = artoriaGame.evaluateInput(userInput);
                    if (ret == "true")
                    {
                        Console.WriteLine(artoriaGame.genMessage());
                        userInput = string.Empty;
                    }
                    else if (ret == "false")
                    {
                        Console.WriteLine("You have entered an invalid command. You can type 'help_me' for instructions.");
                        userInput = string.Empty;
                    }
                    else if (ret == "QUIT")
                    {
                        userInput = "QUIT";
                    }
                }
            }
            else if (userInput == "help_me")
            {
                Console.WriteLine("Artoria is a text-based dungeon crawler created by Jake Farley. The goal of the game is to get the highest score.");
                Console.WriteLine("You score points by defeating monsters and picking up items, as you walk around the map " + Environment.NewLine + "you will encounter various enemies and find various items.");
                Console.WriteLine("As you fight enemies, you will take damage; be warned, there is no way to regain your health." + Environment.NewLine + "Once your health reaches zero, the game will end.");
                Console.WriteLine("You can move around the map by typing the four compass directions 'north', 'south', 'east', and 'west'");
                Console.WriteLine("If you encounter an enemy, attack them by typing 'attack' you can type 'surrender' to surrender yourself to the enemy.");
                Console.WriteLine("If you wish to quit the game, type 'StopPlayingArtoria'");
            }
            else
            {
                Console.WriteLine("Error: Invalid response, closing program.");
            }
            */
        }
    }
}
