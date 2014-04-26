﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SousChef;
using Breakneck_Brigade.Graphics;
using System.IO;

namespace Breakneck_Brigade
{
    class ClientCooker //: ClientGameObject
    {
        public CookerType Type { get; set; }
        public List<ClientIngredient> Contents { get; set; }
        
        public ClientCooker()
        {

        }

        /*
        public ClientCooker(int id, CookerType type, Vector4 transform, ClientGame game)
            : base(id, new Vector4(), game)
        {
            construct(type);
        }

        public override string ModelName
        {
            get { return Type.Name; }
        }

        //called by ClientGameObject.Deserialize
        public ClientCooker(int id, BinaryReader reader, ClientGame game) 
            : base(id, reader, game)
        {
            Type = game.Config.Cookers[reader.ReadString()];
            base.finilizeConstruction();
        }

        private void construct(CookerType type)
        {
            Type = type;
            base.finilizeConstruction();
        }

        private void update()
        {
            // add any client specific update here
        }

        /// <summary>
        /// Update everything pertaining to the ingriedient. Note this is 
        /// mainly for ease of testing, shouldn't be called by the stream
        /// </summary>
        private void update(Vector4 transform, int cleanliness)
        {
            base.Update(transform);
            this.update();
        }


        public override void StreamUpdate(BinaryReader reader)
        {
            base.StreamUpdate(reader);
            if (reader.ReadBoolean()) // Ingredient added
            {
                processIngredientsAdded(reader);
            }
            this.update();
        }

        private void processIngredientsAdded(BinaryReader reader)
        {
            this.Contents.Clear();
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
                Contents.Add((ClientIngredient)Game.gameObjects[reader.ReadInt32()]); //TODO: Find the effect to play and then play it.
        }

        public override void Render()
        {
            base.Render();
        }*/
    }


}
