namespace Poker
{
    using System.Windows.Forms;

    public interface IPlayer
    {
        Panel PlayerPanel { get; set; }

        int PlayerChips { get; set; }

        double PlayerType { get; set; }

        bool PlayerFolded { get; set; }

        int PlayerCall { get; set; }

        int PlayerRaise { get; set; }

        double PlayerPower { get; set; }
    }
}