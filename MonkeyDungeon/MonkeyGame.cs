using isometricgame.GameEngine.Tools;
using MonkeyDungeon.GameFeatures.Multiplayer;
using MonkeyDungeon.GameFeatures.Multiplayer.Local_Recievers;
using MonkeyDungeon_UI;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using OpenTK;

namespace MonkeyDungeon_Core
{
    public class MonkeyDungeon_Game : MonkeyDungeon_Game_Client
    {
        internal Local_Session Local_Session { get; private set; }
        internal Local_Reciever Client { get; private set; }
        
        public MonkeyDungeon_Game(string GAME_DIR = "", string GAME_DIR_ASSETS = "", string GAME_DIR_WORLDS = "") 
            : base(MD_VANILLA_RACES.RACE_MONKEY, GAME_DIR, GAME_DIR_ASSETS, GAME_DIR_WORLDS)
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (!EventScheduler.IsActive)
                Local_Session?.On_Update_Frame(e.Time);
        }
        
        protected override void Handle_Load_Entities()
        {
            foreach (string race in MD_VANILLA_RACES.STRINGS)
                LoadEntity(race);
            foreach (string particle in MD_VANILLA_PARTICLES.STRINGS)
                LoadSprite(particle, 4, 32, 32);
        }

        protected override Multiplayer_Relay Handle_Create_Local_Game()
        {
            Local_Session = new Local_Session(
                Client = new Local_Reciever(),
                new Local_Reciever()
                );
            return Client;
        }
    }
}
