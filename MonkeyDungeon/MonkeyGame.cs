using isometricgame.GameEngine;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Systems;
using MonkeyDungeon.Components;
using MonkeyDungeon.Components.Implemented.Enemies.Goblins;
using MonkeyDungeon.GameSystems;
using MonkeyDungeon.Prefabs.Entities;
using MonkeyDungeon.Scenes.GameScenes;
using MonkeyDungeon.Scenes.Menus;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon
{
    public class MonkeyGame : Game
    {
        public static readonly string[] ENTITIES = new string[] 
        {
            EntityComponent.RACE_NAME_PLAYER,
            EC_Goblin.DEFAULT_RACE_NAME
        };
        
        internal EntityComponentFactory EntityComponentFactory { get; set; }

        public MonkeyGame(string GAME_DIR = "", string GAME_DIR_ASSETS = "", string GAME_DIR_WORLDS = "") 
            : base(1200, 900, "Monkey Dungeon", GAME_DIR, GAME_DIR_ASSETS, GAME_DIR_WORLDS)
        {
            SceneManagementService.AddScene("gameScene", new GameScene(this));

            SceneManagementService.AddScene("mainMenu", new MainMenuScene(this));

            SceneManagementService.SetScene("mainMenu");
        }

        protected override void RegisterCustomSystems()
        {
            EntityComponentFactory = new EntityComponentFactory(this);
            RegisterSystem(EntityComponentFactory);
        }

        protected override string[] GetShaders()
        {
            return new string[] { "shader", "shader_Scale" };
        }

        protected override void LoadContent()
        {
            LoadSprite("mainMenu", 1, 1200, 900);
            SpriteLibrary.GetSprite("mainMenu").SetSize(new OpenTK.Vector2(Width, Height));
            LoadSprite("creationMenu", 1, 1200, 900);
            SpriteLibrary.GetSprite("creationMenu").SetSize(new OpenTK.Vector2(Width, Height));

            LoadSprite("button", 1, 200, 100);
            LoadSprite("gamefont", 1, 18, 28, true, "font");
            LoadSprite("targetButton", 1, 50, 50);

            LoadSprite("statusBar", 1.25f, 400, 300);
            LoadSprite("resourceBar", 1, 128, 16);

            foreach (string name in ENTITIES)
                LoadEntity(name);

            TextDisplayer.LoadFont("font", SpriteLibrary.GetSpriteID("font"));
        }

        private void LoadEntity(string name)
        {
            LoadSprite(String.Format("{0}{1}", name, CreatureGameObject.Suffix_Head), 4, 32, 32);

            LoadSprite(String.Format("{0}{1}", name, CreatureGameObject.Suffix_Body), 4, 32, 32);

            LoadSprite(String.Format("{0}{1}", name, CreatureGameObject.Suffix_Unique), 4, 32, 32, false);
        }

        private void LoadSprite(string spriteName, float scale, int width, int height, bool throwIf_NotExists = true, string savedName=null)
        {
            string path = Path.Combine(
                            GAME_DIRECTORY_ASSETS,
                            spriteName + ".png"
                            );
            if (!throwIf_NotExists && !File.Exists(path))
                return;
            Sprite s;
            SpriteLibrary.RecordSprite(
                s = AssetProvider.ExtractSpriteSheet(
                    path,
                    (savedName == null) ? spriteName : savedName,
                    width,
                    height
                    )
                );
            s.Scale = scale;
        }
    }
}
