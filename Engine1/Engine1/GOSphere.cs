using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Engine1
{
    class GOSphere : GameObject
    {
        private float R;
        public float Radius
        {
            get
            {
                return R;
            }
            set
            {
                R = value;
                SphereCollisionHandler modifier = (SphereCollisionHandler)collisionHandler;
                if (modifier is SphereCollisionHandler)
                modifier.Radius = value;
            }
        }
        private Vector3 SphereCenter;
        public override Vector3 Position
        {
            get { return SphereCenter; }
            set { collisionHandler.Position = value; SphereCenter = value; }
        }

        //TODO:: shader data?
        private bool Colored;
        private static VertexPositionNormalColor[] ColoredVertices;
        private static VertexPositionNormal[] Vertices;
        private static short[] Indices;
        //test list;
        
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        //BasicEffect basicEffect;

        public GOSphere(int tessilation = 20, bool colored = false, float myMass = 1.0f)
        {
            //create all required objects
            collisionHandler = new SphereCollisionHandler(this);

            //set details
            Position = Vector3.Zero;
            Colored = colored;
            if (colored)
                CreateVerteciesColored(tessilation);
            else
                CreateVertecies(tessilation);

            Radius = 1.0f;
            mass = myMass;
            physicsHandler.mass = myMass;
        }



        private void CreateVerteciesColored(int tessilation)
        {
            if (ColoredVertices != null)
                return;
            float fRotationVal = 2 * MathHelper.Pi / tessilation;
            List<VertexPositionNormalColor> vertexList = new List<VertexPositionNormalColor>();
            
            vertexList.Add(new VertexPositionNormalColor( Vector3.Up, Vector3.Up, Color.Blue));
            int halfTess = tessilation / 2;
            for (int y = 1; y < halfTess; y++)
            {
                for (int x = 0; x < tessilation; x++)
                {
                    Color newCol;
                    if (x == 0)
                        newCol = Color.White;
                    else
                        newCol = new Color((float)(y + 1) / halfTess,
                                            0f,
                                            (float)halfTess / y);

                    Vector3 newVect = Vector3.Transform(
                                    Vector3.Transform(Vector3.Up,
                                    Matrix.CreateRotationZ(y * fRotationVal)),
                                    Matrix.CreateRotationY(x * fRotationVal));
                    vertexList.Add(new VertexPositionNormalColor(newVect, newVect, newCol));
                }
            }
            vertexList.Add(new VertexPositionNormalColor(-Vector3.Up, -Vector3.Up, Color.Red));

            //AVertices = vertexList.ToArray();
            ColoredVertices = vertexList.ToArray();
            CreateIndices(tessilation, vertexList.Count);
            
            vertexList.Clear();
        }



        private void CreateVertecies(int tessilation)
        {
            if (Vertices != null)
                return;
            float fRotationVal = 2 * MathHelper.Pi / tessilation;
            List<VertexPositionNormal> vertexList = new List<VertexPositionNormal>();
                vertexList.Add(new VertexPositionNormal(Vector3.Up, Vector3.Up));
                int halfTess = tessilation / 2;
                for (int y = 1; y < halfTess; y++)
                {
                    for (int x = 0; x < tessilation; x++)
                    {
                        Vector3 newVect = Vector3.Transform(
                                        Vector3.Transform(Vector3.Up,
                                        Matrix.CreateRotationZ(y * fRotationVal)),
                                        Matrix.CreateRotationY(x * fRotationVal));
                        vertexList.Add(new VertexPositionNormal(newVect, newVect));
                    }
                }
            vertexList.Add(new VertexPositionNormal(- Vector3.Up, - Vector3.Up));

            //AVertices = vertexList.ToArray();
            Vertices = vertexList.ToArray();
            CreateIndices(tessilation, vertexList.Count);

            vertexList.Clear();
        }



        private void CreateIndices(int tessilation, int numVerticies)
        {
            List<short> indexList = new List<short>();
            //Cap on top
            for (int x = 0; x < tessilation; x++)
            {
                indexList.Add(0);
                indexList.Add((short)((x + 1)));
                indexList.Add((short)(1 + (x + 1) % tessilation));
            }

            //create main body
            for (int y = 0; y < (tessilation / 2) - 2; y++)
            {
                for (int x = 0; x < tessilation; x++)
                {
                    int xnext = (x + 1) % tessilation;
                    int ynext = (y + 1);
                    //TODO:: Test amount of polygons created 
                    indexList.Add((short)(1 + xnext + (y * tessilation)));
                    indexList.Add((short)(1 + (x + (y * tessilation))));
                    indexList.Add((short)(1 + xnext + (ynext * tessilation)));

                    indexList.Add((short)(1 + x + (y * tessilation)));
                    indexList.Add((short)(1 + x + (ynext * tessilation)));
                    indexList.Add((short)(1 + xnext + (ynext * tessilation)));
                }
            }

            //cap on bottom
            for (int x = 0; x < tessilation; x++)
            {
                indexList.Add((short)(numVerticies - 2 - x ));
                indexList.Add((short)(numVerticies - 2 - (x + 1) % tessilation));
                indexList.Add((short)(numVerticies - 1));
            }

            Indices = indexList.ToArray();

            indexList.Clear();
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

            //This line leaks the memeory! 
            if (Colored)
            {

                vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalColor),
                                                ColoredVertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPositionNormalColor>(ColoredVertices);
                basicEffect.VertexColorEnabled = true;
            }
            else
            {
                vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormal),
                                                Vertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData(Vertices);
                basicEffect.VertexColorEnabled = false;
            }

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), 
                                            Indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData<short>(Indices);
            graphicsDevice.Indices = indexBuffer;

            //Temporary, store and restore basicEffect
            basicEffect.World = (Matrix.CreateScale(Radius) * Matrix.CreateTranslation(Position));

            foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Indices.Length / 3);
                //graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, AVertices.Length / 3);
            }

            //This line fixes the leak, but I don't think this is good practice
            Dispose();
        }
    }
}
