﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SousChef;
using System.IO;

namespace DeCuisine
{
    class ServerIngredient : ServerGameObject, IIngredient 
    {
        public IngredientType Type { get; set; }
        public int Cleanliness { get; set; }

        public ServerIngredient (int id, IngredientType type, Server server)
            : base(id, new Vector4(), server)
        {
            this.Type = type;
        }

        public ServerIngredient(int id, IngredientType type, Vector4 transform, Server server)
            : base(id, transform, server)
        {
            this.Type = type;
        }

        void writeSerialization(StreamWriter stream)
        {

        }

        public override void Update()
        {
            //move the object
        }

    }
}
