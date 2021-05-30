using isometricgame.GameEngine;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_UI.Scenes.Menus;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI
{
    public abstract class MonkeyDungeon_Game_Client : Game
    {
        public readonly string DEFAULT_RACE;
        internal EventScheduler UI_EventScheduler => EventScheduler;

        internal Multiplayer_Expectation_Context Expectation_Context { get; private set; }
        internal Multiplayer_Relay Client_RecieverEndpoint_UI { get; private set; }
        
        public MonkeyDungeon_Game_Client(string defaultRace, string GAME_DIR = "", string GAME_DIR_ASSETS = "", string GAME_DIR_WORLDS = "") 
            : base(1000, 600, "Monkey Dungeon", GAME_DIR, GAME_DIR_ASSETS, GAME_DIR_WORLDS)
        {
            DEFAULT_RACE = defaultRace;
            Expectation_Context = new Multiplayer_Expectation_Context();

            //TODO: Change to Load_Entity_Factory();
            Handle_Load_Entities();

            GameScene gameScene = new GameScene(this);
            
            SceneManagementService.AddScene("gameScene", gameScene);

            SceneManagementService.AddScene("mainMenu", new MainMenuScene(this));

            SceneManagementService.SetScene("mainMenu");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            Client_RecieverEndpoint_UI?.Flush_Messages();
        }

        protected abstract void Handle_Load_Entities();

        internal void Create_Local_Game() 
            => Client_RecieverEndpoint_UI = Handle_Create_Local_Game();
        protected abstract Multiplayer_Relay Handle_Create_Local_Game();

        internal void Link_Endpoint()
            => Expectation_Context.Link(Client_RecieverEndpoint_UI);

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

            LoadSprite("statusBar", 1.25f, 400, 299);
            LoadSprite("resourceBar", 1, 128, 16);
            LoadSprite("announcement", 1, 300, 200);
            LoadSprite("AbilityPoint", 1, 32, 32);
            LoadSprite("EndTurnButton", 4, 32, 32);
            LoadSprite("BridgePath", 1, 1920, 1080);

            TextDisplayer.LoadFont("font", (int)SpriteLibrary.GetSpriteID("font"));
        }

        protected void LoadEntity(string name)
        {
            LoadSprite(String.Format("{0}{1}", name, CreatureGameObject.Suffix_Head), 4, 32, 32);

            LoadSprite(String.Format("{0}{1}", name, CreatureGameObject.Suffix_Body), 4, 32, 32);

            LoadSprite(String.Format("{0}{1}", name, CreatureGameObject.Suffix_Unique), 4, 32, 32, false);
        }

        protected void LoadSprite(string spriteName, float scale, int width, int height, bool throwIf_NotExists = true, string savedName = null)
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
