﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SousChef;
using Tao.OpenGl;

namespace Breakneck_Brigade.Graphics
{
    /*
     * A container class for data associated with a model
     * Ryan George
     */
    class Model
    {
        public      List<AObject3D>     Meshes          { get; private set; }

        public Model()
        {
            Meshes          = new List<AObject3D>();
            Transformation  = new Matrix4();
            _position       = new Vector4();
            _scale          = new Vector4();
            _rotation       = new Vector4();
        }

        public void Render()
        {
            updateMatrix();
            Gl.glPushMatrix();
            Gl.glLoadMatrixf(Transformation.glArray);
            foreach(AObject3D m in Meshes)
            {
                m.Render();
            }
            Gl.glPopMatrix();
        }

        /// <summary>
        /// Updates the matrix to reflect the properties of the model
        /// </summary>
        private void updateMatrix()
        {
            //Translate to location
            Transformation.TranslationMat(Position.X, Position.Y, Position.Z);
            
            //Rotate to proper orientation: 
            Transformation = Transformation*Matrix4.MakeRotateZ(Rotation.Z);
            Transformation = Transformation*Matrix4.MakeRotateY(Rotation.Y);
            Transformation = Transformation*Matrix4.MakeRotateX(Rotation.X);

            //Scale
            Transformation = Transformation*Matrix4.MakeScalingMat(Scale.X, Scale.Y, Scale.Z);
        }
    }
}
