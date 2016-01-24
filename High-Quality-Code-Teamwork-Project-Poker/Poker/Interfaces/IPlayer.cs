namespace Poker
{
    using System.Windows.Forms;

    public interface IPlayer
    {
        Panel PlayerPanel { get; set; }

        int Chips { get; set; }

        double PlayerType { get; set; }

        bool PlayerFolded { get; set; }

        int PlayerCall { get; set; }

        int Raise { get; set; }

        double Power { get; set; }
    }
}