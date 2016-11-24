using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Engine1
{
    class GOPlane : GameObject
    {
        private float Width;
        private float Length;
        private Vector3 normalVector;
        protected Vector3 normal
        {
            get { return normalVector; }
            set {
                normalVector = value;
                if (collisionHandler != null && collisionHandler is HalfSpaceCollisionHandler)
                {
                    HalfSpaceCollisionHandler temp = (HalfSpaceCollisionHandler)collisionHandler;
                    temp.normal = value;
                }
            }
        }

        private Vector3 planeCenter;
        public override Vector3 Position
        {
            get { return planeCenter; }
            set { collisionHandler.Position = value; planeCenter = value; }
        }

        //TODO:: Add quaternions or something to handle a change in normal
        private static VertexPositionNormal[] Vertices;
        private static short[] Indices;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public GOPlane() : this(1f, 1f)
        {
        }

        public GOPlane(float length, float width)
        {
            anchorPosition = Vector3.Zero;
            InitializePoints(length, width);
            collisionHandler = new HalfSpaceCollisionHandler(this);
            normal = Vector3.Up;
            mass = float.PositiveInfinity; //make mass always infinity to start, as this is probably ground
        }

        //Initialize Points will make the thing double the size
        private void InitializePoints(float length, float width)
        {
            Width = width / 2;
            Length = length / 2;

            Vertices = new VertexPositionNormal[4];

            Vertices[0] = new VertexPositionNormal(new Vector3(Width, 0f, Length), Vector3.Up);
            Vertices[1] = new VertexPositionNormal(new Vector3(Width, 0f, -Length), Vector3.Up);
            Vertices[2] = new VertexPositionNormal(new Vector3(-Width, 0f, Length), Vector3.Up);
            Vertices[3] = new VertexPositionNormal(new Vector3(-Width, 0f, -Length), Vector3.Up);
            
            CreateIndices();
        }

        private void CreateIndices()
        {
            Indices = new short[6];

            Indices[0] = 3;
            Indices[1] = 2;
            Indices[2] = 1;
            Indices[3] = 0;
            Indices[4] = 2;
            Indices[5] = 1;
        }

        public override void Dispose()
        {
            if (vertexBuffer != null)
                vertexBuffer.Dispose();
            if (indexBuffer != null)
                indexBuffer.Dispose();
        }

        public override void Draw(BasicEffect basicEffect)
        {
            GraphicsDevice graphicsDevice = basicEffect.GraphicsDevice;

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormal),
                                                Vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(Vertices);
            basicEffect.VertexColorEnabled = false;

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short),
                                            Indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(Indices);
            graphicsDevice.Indices = indexBuffer;

            basicEffect.World = ( Matrix.CreateTranslation(Position));

            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.Length / 3);
            }

            Dispose();
        }
    }
}
