namespace Poker
{
    using System.Windows.Forms;

    public class Player : IPlayer
    {
        public Player(int playerChips, double playerType, bool playerFolded, int playerCall, int playerRaise, double playerPower)
        {
            this.PlayerPanel = new Panel();
            this.PlayerChips = playerChips;
            this.PlayerType = playerType;
            this.PlayerFolded = playerFolded;
            this.PlayerCall = playerCall;
            this.PlayerRaise = playerRaise;
            this.PlayerPower = playerPower;

        }

        public Panel PlayerPanel { get; set; }

        public int PlayerChips { get; set; }

        public double PlayerType { get; set; }

        public bool PlayerFolded { get; set; }
        
        public int PlayerCall { get; set; }

        public int PlayerRaise { get; set; }
        
        public double PlayerPower { get; set; }
    }
}