using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Engine1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // TEST STUFF
        VertexBuffer vertexBuffer;

        BasicEffect basicEffect;
        //Matrix world = Matrix.Identity;
        //Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 15), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        //Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 4, 800f / 480f, 0.01f, 100f);

        Camera camera;

        GOSphere testSphere;
        GOSphere testSphere2;
        GOSphere testSphere3;
        GOPlane testPlane;

        bool ScreenActive = false;

        //Make Player object? later when cam is done;
        float playerSpeed = 10.0f;
        float mouseSensitivity = 0.04f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void OnActivated(object sender, System.EventArgs args)
        {
            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, System.EventArgs args)
        {
            ScreenActive = false;
            base.OnActivated(sender, args);
        }
        
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
   
            basicEffect = new BasicEffect(GraphicsDevice);

            camera = new Camera(new Vector3(0, 1, -15), new Vector3(0f, 0f, 1f), MathHelper.Pi / 4,
                GraphicsDevice.Viewport.AspectRatio, 0.01f, 100f);
            //800f / 480f, 0.01f, 100f);


            //TODO:: bug with the penetration depth? maybe sep. velocity
            testSphere = new GOSphere(100, false);
            testSphere.gravityEnabled = true;
            testSphere.Position = new Vector3(0f, -8f, 0f);
            testSphere.Velocity = new Vector3(1f, 0f, 0f);
            testSphere.Radius = 2f;
            testSphere.mass = 1f;
            testSphere.restitution = 1f;

            testPlane = new GOPlane(100, 100);
            testPlane.gravityEnabled = false;
            testPlane.Position = new Vector3(0f, 0f, 0f);
            testPlane.restitution = 1f;

            testSphere2 = new GOSphere(100, false);
            testSphere2.gravityEnabled = true;
            testSphere2.Position = new Vector3(6f, 0f, 0f);
            testSphere2.Velocity = new Vector3(0f, 0f, 0f);
            testSphere2.Radius = 2f;
            testSphere2.mass = 1f;
            testSphere2.restitution = 1f;

            //testSphere3 = new GOSphere(100, false);
            //testSphere3.gravityEnabled = true;
            //testSphere3.Position = new Vector3(0f, 9f, 0f);
            //testSphere3.Velocity = new Vector3(0f, 0f, 0f);
            //testSphere3.Radius = 5f;
            //testSphere3.mass = 10f;
            //testSphere3.restitution = 0.95f;

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            int midpointX = GraphicsDevice.Viewport.Width / 2;
            int midpointY = GraphicsDevice.Viewport.Height / 2;

            if (IsActive)
            {
                if(!ScreenActive)
                {
                    ScreenActive = true;
                    Mouse.SetPosition(midpointX, midpointY);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                    return;
                }
                //TODO:: create controls handler
                Vector3 movementVector = Vector3.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    movementVector.X += 1f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    movementVector.X += -1f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    movementVector.Z += 1f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    movementVector.Z += -1f;
                }

                if (movementVector != Vector3.Zero)
                {
                    movementVector.Normalize();
                    movementVector *= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    camera.MovePlayer(movementVector);
                }

                //Test Code until mouse controls are made
                //do off mouse position? set mouse to center and check how far it's moved?

                int xdiff = midpointX - Mouse.GetState().X;
                int ydiff = midpointY - Mouse.GetState().Y;

                if (xdiff != 0)
                {
                    camera.RotateCameraHorizontal(xdiff * mouseSensitivity * 
                        (float)gameTime.ElapsedGameTime.TotalSeconds);
                }

                if ( ydiff !=0 )
                {
                    camera.RotateCameraVertical(-ydiff * mouseSensitivity *
                        (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
               
                //set the mouse to screen center
                Mouse.SetPosition(midpointX, midpointY);

                handlePhysics(gameTime);

                base.Update(gameTime);
            }
        }

        private void handlePhysics(GameTime gameTime)
        {
            testSphere.UpdatePhysics(gameTime); // pass events? 
            testSphere2.UpdatePhysics(gameTime);
            //testSphere3.UpdatePhysics(gameTime);
            testPlane.UpdatePhysics(gameTime);

            for (int i = 0; i < PhysicsGlobals.collisionObjects.Count - 1; i++)
            {
                for (int j = i + 1; j < PhysicsGlobals.collisionObjects.Count; j++)
                {
                    ContactResolver.ResolveContact(PhysicsGlobals.collisionObjects[i],
                                                   PhysicsGlobals.collisionObjects[j],
                                                   gameTime);
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //basicEffect.World = world;
            //basicEffect.View = view;
            //basicEffect.Projection = projection;
            basicEffect.World = camera.world;
            basicEffect.View = camera.getView();
            basicEffect.Projection = camera.Projection;
            //basicEffect.VertexColorEnabled = true;
            basicEffect.EnableDefaultLighting();

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //rasterizerState.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rasterizerState;
            basicEffect.PreferPerPixelLighting = true; //makes it buttery smooth

            testSphere.Draw(basicEffect);
            testSphere2.Draw(basicEffect);
            //testSphere3.Draw(basicEffect);
            testPlane.Draw(basicEffect);

            //foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            //}

            base.Draw(gameTime);
        }
    }
}
