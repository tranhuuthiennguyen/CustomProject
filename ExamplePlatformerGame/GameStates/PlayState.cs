﻿# region Description

/* Play state will load all game contents including character specifically player and enemy
 * other managers might be needed
 */

#endregion


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CustomProject.Entities.TileMap;
using CustomProject.Entities;
using CustomProject.Models;
using System.Collections.Generic;
using CustomProject.Entities.Characters;

namespace CustomProject.GameStates
{
    public class PlayState : GameState
    {
        #region Fields

        private Color _backgroundColor = Color.CornflowerBlue;

        private TileSet _tileSet;

        private Map _map;

        private SpriteFont spriteFont;

        private Player _player1;

        private MovableSprite _player2;

        private Tile _tile;

        private List<MovableSprite> _movablesprites;

        private List<Character> _characters;

        private MouseState pastMouse;

        #endregion

        public PlayState(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {

        }

        public override void Initialize()
        {

        }

        public override void LoadContent(ContentManager content)
        {
            #region Load file contents

            var block = content.Load<Texture2D>("Block");

            _texture = content.Load<Texture2D>("TileSets/Mossy - TileSet");

            #endregion


            #region Setup Animations for gameobjects

            var animations = new Dictionary<string, Animation>()
            {

                {"IdleRight", new Animation(content.Load<Texture2D>("Characters/Wizard/IdleRight"), 19) },
                {"IdleLeft", new Animation(content.Load<Texture2D>("Characters/Wizard/IdleLeft"), 20) },
                {"WalkLeft", new Animation(content.Load<Texture2D>("Characters/Wizard/WalkLeft"), 20) },
                {"WalkRight", new Animation(content.Load<Texture2D>("Characters/Wizard/WalkRight"), 20) },
                {"JumpRight", new Animation(content.Load<Texture2D>("Characters/Wizard/JumpRight"), 8) },
                {"JumpLeft", new Animation(content.Load<Texture2D>("Characters/Wizard/JumpLeft"), 8) }
            };

            var enemyAnimations = new Dictionary<string, Animation>()
            {
                {"Idle", new Animation(content.Load<Texture2D>("Characters/Enemy/GreenSlime"), 30) }
            };

            var fireballAnimations = new Dictionary<string, Animation>()
            {
                {"ShootRight", new Animation(content.Load<Texture2D>("Attacks/fireball"), 5) }
            };

            #endregion


            #region Assigning GameObject

            var fireball = new Bullet(fireballAnimations);

            var enemy = new Enemy(enemyAnimations)
            {
                Position = new Vector2(700, 700),
                HealthTexture = block,
                Identity = "enemy",
                HealthBarColor = Color.Red,
                Health = 125
            };

            _player1 = new Player(animations)
            {
                Position = new Vector2(300, 600),
                Bullet = fireball,
                Identity = "player",
                HealthTexture = block,
                HealthBarColor = Color.Green,
                Health = 100,
                Input = new Input()
                {
                    Left = Keys.A,
                    Right = Keys.D,
                    Jump = Keys.Space,
                    Attack = Keys.E
                }
            };

            #endregion


            #region Adding GameObject to Lists

            _sprites = new List<Sprite>()
            {
                _player1,
                enemy
            };

            #endregion

            #region Setup Map

            _tileSet = new TileSet(_graphicsDevice, _texture, 7);
            _map = new Map("map1.txt", _tileSet, spriteFont);
            _map.LoadMapFile();

            #endregion


            /// Add tiles into sprite list
            _sprites.AddRange(_map.Tiles);
        }
        public override void UnloadContent()
        {

        }


        /// <summary>
        /// Update Game Loop here
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            if(mouse.LeftButton == ButtonState.Pressed && pastMouse.LeftButton == ButtonState.Released)
            {

            }

            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime, _sprites);
            }

            pastMouse = mouse;
        }

        /// <summary>
        /// Draw on screen
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(_backgroundColor);

            spriteBatch.Begin(SpriteSortMode.BackToFront);

            _map.Draw(gameTime, spriteBatch);

            foreach (var sprite in _sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
