namespace Poker
{
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class Player : IPlayer
    {
        public Player(
            int chips = 10000, 
            double playerType = -1,
            bool playerFolded = false,
            int playerCall = 0, 
            int raise = 0, double power = 0,
            bool turn = false)
        {
            this.PlayerPanel = new Panel();
            this.Chips = chips;
            this.PlayerType = playerType;
            this.PlayerFolded = playerFolded;
            this.PlayerCall = playerCall;
            this.Raise = raise;
            this.Power = power;
            this.Turn = turn;

        }

        public Panel PlayerPanel { get; set; }

        public int Chips { get; set; }

        public double PlayerType { get; set; }

        public bool PlayerFolded { get; set; }
        
        public int PlayerCall { get; set; }

        public int Raise { get; set; }
        
        public double Power { get; set; }

        public bool Turn { get; set; }
    }
}