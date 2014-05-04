﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SousChef;

namespace DeCuisine
{
    /// <summary>
    /// Class that allows the player to manipulate the game world through the command line
    /// </summary>
    static class CommandLinePlayer
    {

        /// <summary>
        /// read the arguments from the command line.
        /// </summary>
        /// <param name="args"></param>
        public static bool ReadArgs(string[] args, Server server)
        {
            bool success = true;
            switch (args[0])
            {
                // Everything under here is dev code. 
                case "add":
                    // "takes" two arguments, the first is the cooker id of where you want to 
                    // put the ingredient, the second is the ingredient id of what you want to add
                    if (args.Length < 3)
                    {
                        Console.WriteLine("add expects at least two arguments.");
                        break;
                    }
                    lock (server.Lock)
                    {
                        lock (server.Game)
                        {
                            server.Game.TestCookerAdd(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
                        }
                    }
                    break;
                case "listworld":
                    // list all objects ids in the game as well as there class 
                    lock (server.Lock)
                    {
                        server.Game.ListGameObjects();
                    }
                    break;

                case "listcooker":
                    // takes one argument, the cooker you want to list it's contents
                    if (args.Length < 2)
                    {
                        Console.WriteLine("list cooker expects at least one argument.");
                        break;
                    }
                    lock (server.Lock)
                    {
                        server.Game.ListCookerContents(Convert.ToInt32(args[1]));
                    }
                    break;
                case "listing":
                    // lists all the ingredients by name in the game world
                    lock (server.Lock)
                    {
                        server.Game.ListIngredients();
                    }
                    break;
                case "spawn":
                    // spawn stuff, see function definition for right argument format
                    lock (server.Lock)
                    {
                        CommandLinePlayer.Spawn(server.Game, args);
                    }
                    break;
                case "stresstest":
                    // spawn stuff, see function definition for right argument format
                    lock (server.Lock)
                    {
                        CommandLinePlayer.StressTest(server.Game);
                    }
                    break;
                default:
                    success = false;
                    break;
            }
            return success;
        }

        /// <summary>
        /// Technically, calls the collision function between the cooker and 
        /// ingredient. The collision function adds the ingredient to the cooker 
        /// list
        /// </summary>
        public static void TestCookerAdd(Dictionary<int,ServerGameObject> gameObjects, int cookerId, int ingredientId)
        {
            if (!sanityChecks(gameObjects, cookerId, GameObjectClass.Cooker))
                return;
            if (!sanityChecks(gameObjects, ingredientId, GameObjectClass.Ingredient))
                return;

            // final cooker specific check, is the ingredient alive?
            if (gameObjects[ingredientId].ToRender) // TODO: Don't use to render
                    gameObjects[cookerId].OnCollide(gameObjects[ingredientId]);
        }

        /// <summary>
        /// Lists all the game objects that exist in the world and various properties.
        /// </summary>
        public static void ListGameObjects(Dictionary<int, ServerGameObject> gameObjects)
        {
            Console.WriteLine("Object Id " + "\t" + "Name" + "\t\t" + "Location" + "\t\t" + "ToRender");
            foreach (var x in gameObjects)
                writeAttributes(x.Value, "");
        }

        /// <summary>
        /// Lists only the ingredients in the world and various properties
        /// </summary>
        public static void ListIngredients(Dictionary<int, ServerGameObject> gameObjects)
        {
            Console.WriteLine("Object Id " + "\t" + "Name" + "\t\t" + "Location" + "\t\t" + "ToRender");
            foreach (var x in gameObjects)
            {
                if (x.Value.ObjectClass == GameObjectClass.Ingredient)
                    writeAttributes(x.Value, "ing");
            }
        }

        /// <summary>
        /// Lists the contents of the current cooker.
        /// </summary>
        public static void ListCookerContents(Dictionary<int,ServerGameObject> gameObjects, int cookerId)
        {
            if (!sanityChecks(gameObjects, cookerId, GameObjectClass.Cooker))
                return; 

            // sanity checks passed list contents
            Console.WriteLine("Object Id " + "\t" + "Name");
            foreach (var x in ((ServerCooker)gameObjects[cookerId]).Contents)
            {
                Console.WriteLine(x.Id + "\t\t" + x.Type.Name);
            }
        }

        /// <summary>
        /// Stress test our system, AKA Spawn a metric shitload of things at once
        /// </summary>
        public static void StressTest(ServerGame game)
        {
            int numOfCookers = 0;
            int numOfIngredients = 50;
            for (int x = 0; x < numOfIngredients; x++ )
                SpawnIngredient(game, "orange");
            for (int x = 0; x < numOfCookers; x++)
                SpawnCooker(game);
        }

        /// <summary>
        /// processes spawn command arguments and calls the appropiate spawn functions
        /// </summary>
        public static void Spawn(ServerGame game, string[] args)
        {

            if (args.Length < 2) 
                Console.WriteLine("You need to specify what type of object to spawn");
            else
            {
                switch (args[1])
                {
                    case "ingredient":
                        spawnIngredientHelper(game, args);
                        break;
                    case "cooker":
                        spawnCookerHelper(game, args);
                        break;
                    default:
                        Console.WriteLine("WTF is a " + args[1] + ". I can't spawn that.");
                        break;
                }
            }
        }

        private static void spawnCookerHelper(ServerGame game, string[] args)
        {
            if (args.Length == 2) // spawn cooker
                SpawnCooker(game);
            else if (args.Length == 3) // spawn cooker type
                SpawnCooker(game, args[2]);
            else if (args.Length == 6) // spawn cooker type x y z
                SpawnCooker(game, args[2], Convert.ToDouble(args[3]), Convert.ToDouble(args[4]), Convert.ToDouble(args[5]));
            else
                Console.WriteLine("Spawn takes 1 arguments: one string that " +
                                  "that is the type of object you want to spawn. 2 arguments: the type of object and " +
                                  "the type of that object i.e. a banana. 5 arguments: the type of object and the " +
                                  "type of that object i.e. a blender and 3 doubles for the x, y and z spawn location");

        }

        private static void spawnIngredientHelper(ServerGame game, string[] args)
        {
            if (args.Length == 2) // spawn ingredient
                SpawnIngredient(game);
            else if (args.Length == 3) // spawn ingredient type
                SpawnIngredient(game, args[2]);
            else if (args.Length == 6) // spawn ingredient type x y z
                SpawnIngredient(game, args[2], Convert.ToDouble(args[3]), Convert.ToDouble(args[4]), Convert.ToDouble(args[5]));
            else
                Console.WriteLine("Spawn takes 1 arguments: one string that " +
                                  "that is the type of object you want to spawn. 2 arguments: the type of object and " +
                                  "the type of that object i.e. a banana. 5 arguments: the type of object and the " +
                                  "type of that object i.e. a blender and 3 doubles for the x, y and z spawn location");
        }

        /// <summary>
        /// Spawn a random ingredient at a random location.
        /// </summary>
        public static void SpawnIngredient(ServerGame game)
        {
            List<IngredientType> types = new List<IngredientType>(game.Config.Ingredients.Values);
            Random rand = new Random();
            IngredientType randIng = types[rand.Next(types.Count)];
            Tao.Ode.Ode.dVector3 spawnLoc = randomSpawn();
            new ServerIngredient(randIng, game, spawnLoc);
            Console.WriteLine("Made a " + randIng.Name + " at " + spawnLoc.X + " " + spawnLoc.Y + " " + spawnLoc.Z);
        }

        /// <summary>
        /// Spawn a the ingredient at a random location.
        /// </summary>
        public static void SpawnIngredient(ServerGame game, string type)
        {
            Tao.Ode.Ode.dVector3 spawnLoc = randomSpawn();
            new ServerIngredient(game.Config.Ingredients[type], game, spawnLoc);
            Console.WriteLine("Made a " + game.Config.Ingredients[type].Name + " at " + spawnLoc.X + " " + spawnLoc.Y + " " + spawnLoc.Z);
        }

        /// <summary>
        /// Spawn an ingredient of the type passed  at the location. 
        /// </summary>
        public static void SpawnIngredient(ServerGame game, string type, double x, double y, double z)
        {
            Tao.Ode.Ode.dVector3 spawnLoc = new Tao.Ode.Ode.dVector3(x, y, z);
            new ServerIngredient(game.Config.Ingredients[type], game, spawnLoc);
            Console.WriteLine("Made a " + game.Config.Ingredients[type].Name + " at " + spawnLoc.X + " " + spawnLoc.Y + " " + spawnLoc.Z);
        }

        /// <summary>
        /// Spawn a random ingredient at a random location.
        /// </summary>
        public static void SpawnCooker(ServerGame game)
        {
            List<CookerType> types = new List<CookerType>(game.Config.Cookers.Values);
            Random rand = new Random();
            CookerType randCooker = types[rand.Next(types.Count)];
            Tao.Ode.Ode.dVector3 spawnLoc = new Tao.Ode.Ode.dVector3(rand.Next(100), rand.Next(100), rand.Next(10, 100));
            new ServerCooker(randCooker, game, spawnLoc);
            Console.WriteLine("Made a " + randCooker.Name + " at " + spawnLoc.X + " " + spawnLoc.Y + " " + spawnLoc.Z);
        }

        /// <summary>
        /// Spawn a the ingredient at a random location.
        /// </summary>
        public static void SpawnCooker(ServerGame game, string type)
        {
            Tao.Ode.Ode.dVector3 spawnLoc = randomSpawn();
            new ServerCooker(game.Config.Cookers[type], game, spawnLoc);
            Console.WriteLine("Made a " + game.Config.Cookers[type].Name + " at " + spawnLoc.X + " " + spawnLoc.Y + " " + spawnLoc.Z);
        }

        /// <summary>
        /// Spawn an ingredient of the type passed  at the location. 
        /// </summary>
        public static void SpawnCooker(ServerGame game, string type, double x, double y, double z)
        {
            Tao.Ode.Ode.dVector3 spawnLoc = new Tao.Ode.Ode.dVector3(x, y, z);
            new ServerCooker(game.Config.Cookers[type], game, spawnLoc);
            Console.WriteLine("Made a " + game.Config.Cookers[type].Name + " at " + spawnLoc.X + " " + spawnLoc.Y + " " + spawnLoc.Z);
        }

        /// <summary>
        /// helper to check if the id passed in is the right type and is in the dict.
        /// check yo self before you wreck self
        /// </summary>
        private static bool sanityChecks(Dictionary<int, ServerGameObject> gameObjects, int objId, GameObjectClass objType)
        {
            if (!gameObjects.ContainsKey(objId))
            {
                Console.WriteLine("Yo dawg, that Id " + objId + " ain't be in the world son");
                return false;
            }

            if (gameObjects[objId].ObjectClass != objType)
            {
                Console.WriteLine("Yo dawg, object at id " + objId + " Be a " + gameObjects[objId].ObjectClass + 
               ". That ain't a " + objType + " son.");
                return false;
            }
            return true;
        }

        

        /// <summary>
        /// helper to check if the config file has the requested ingredient type to make
        /// </summary>
        private static bool sanityChecks(string key, Dictionary<string, IngredientType> config)
        {
            if (!config.ContainsKey(key))
                return false;
            return true;
        }
        /// <summary>
        /// helper to randomly spawn objects
        /// </summary>
        /// <returns></returns>
        private static Tao.Ode.Ode.dVector3 randomSpawn()
        {            
            return new Tao.Ode.Ode.dVector3( DC.random.Next(100), DC.random.Next(100), DC.random.Next(10, 100));
        }

        /// <summary>
        /// Write them in a similar format. I can't format well though so there's that.
        /// </summary>
        private static void writeAttributes(ServerGameObject obj, string format)
        {
            switch(format){
                case "ing":
                    Console.WriteLine(obj.Id + "\t\t" + ((ServerIngredient)obj).Type.Name + "\t\t" + 
                                      (int)obj.Position.X + " " + (int)obj.Position.Y + " " + (int)obj.Position.Z +
                                      "\t\t" + obj.ToRender);
                    break;
                default:
                    Console.WriteLine(obj.Id + "\t\t" + obj.ObjectClass + "\t\t" + (int)obj.Position.X + " " + 
                                      (int)obj.Position.Y + " " + (int)obj.Position.Z + "\t\t" + obj.ToRender);
                    break;
            }            
        }
    }
}