// TODO means the method is staying in this class for further research and most likely require Player objects
// To separate class - they depend on each other and need to be in one class with the dependant methods

namespace Poker
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class PokerTable : Form
    {
        #region Variables
        private const int CARDS_ON_THE_FIELD_COUNT = 17;
        private const int ALL_CARDS_COUNT = 52;

        private Player player;
        private Player firstBot;
        private Player secondBot;
        private Player thirdBot;
        private Player forthBot;
        private Player fifthBot;

        private ProgressBar progressBar = new ProgressBar();

        private DataBase dataBase = new DataBase();
        private List<Type> win = new List<Type>(); 

        private bool playerFoldedTurn = false;
        private bool playerTurn = true;
        private bool restart = false;
        private bool raising = false; // To separate class

        //private int Nm; // never used, but commented just in case
        private int call = 500; // To separate class
        private int foldedPlayers = 5;

        private Panel playerPanel = new Panel();

        //TODO Rid some variables by default constr values;
        //private int playerChips = 10000;
        private double playerType = -1;
        private bool playerFolded;
        private int playerCall = 0;
        private int playerRaise = 0;
        private double playerPower = 0;

        private double type;
        private double rounds = 0; // To separate class
        private double raise = 0; // To separate class

        private bool firstBotTurn = false;
        private bool secondBotTurn = false;
        private bool thirdBotTurn = false;
        private bool forthBotTurn = false;
        private bool fifthBotTurn = false;

        private bool firstBotFoldedTurn = false;
        private bool secondBotFoldedTurn = false;
        private bool thirdBotFoldedTurn = false;
        private bool forthBotFoldedTurn = false;
        private bool fifthBotFoldedTurn = false;

        private bool intsAdded;
        private bool changed;

        private int height;
        private int width;

        private int winners = 0;
        private int flop = 1;
        private int turn = 2;
        private int river = 3;
        private int end = 4;
        private int maxLeft = 6;

        private int last = 123;
        private int raisedTurn = 1;

        Type sorted; // TODO To separate class 

        private void PokerTable_Load(object sender, EventArgs e)
        {

        }

        string[] ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        int[] Reserve = new int[CARDS_ON_THE_FIELD_COUNT]; // To separate class

        Image[] Deck = new Image[ALL_CARDS_COUNT];

        PictureBox[] Holder = new PictureBox[ALL_CARDS_COUNT]; // To separate class

        Timer timer = new Timer();
        Timer updates = new Timer();

        int timeRemaining = 60;
        int index;
        int bigBlind = 500;
        int smallBlind = 250;
        int up = 10000000;
        int turnCount = 0;
        
        #endregion
        
        public PokerTable()
        {
            player = new Player();
            firstBot = new Player();
            secondBot = new Player();
            thirdBot = new Player();
            forthBot = new Player();
            fifthBot = new Player();

            dataBase.AddBools(playerFoldedTurn); 
            dataBase.AddBools(firstBotFoldedTurn); 
            dataBase.AddBools(secondBotFoldedTurn); 
            dataBase.AddBools(thirdBotFoldedTurn); 
            dataBase.AddBools(forthBotFoldedTurn); 
            dataBase.AddBools(fifthBotFoldedTurn);

            call = bigBlind;

            MaximizeBox = false;
            MinimizeBox = false;

            updates.Start();
            InitializeComponent();

            width = this.Width;
            height = this.Height;

            Shuffle();

            textBoxPot.Enabled = false;
            textBoxPlayerChips.Enabled = false;
            textBoxFirstBotChips.Enabled = false;
            textBoxSecondBotChips.Enabled = false;
            textBoxThirdBotChips.Enabled = false;
            textBoxForthBotChips.Enabled = false;
            textBoxFifthBotChips.Enabled = false;

            textBoxPlayerChips.Text = "playerChips : " + player.Chips.ToString();
            textBoxFirstBotChips.Text = "playerChips : " + firstBot.Chips.ToString();
            textBoxSecondBotChips.Text = "playerChips : " + secondBot.Chips.ToString();
            textBoxThirdBotChips.Text = "playerChips : " + thirdBot.Chips.ToString();
            textBoxForthBotChips.Text = "playerChips : " + forthBot.Chips.ToString();
            textBoxFifthBotChips.Text = "playerChips : " + fifthBot.Chips.ToString();

            timer.Interval = (1 * 1 * 1000);
            timer.Tick += timer_Tick;
            updates.Interval = (1 * 1 * 100);
            updates.Tick += Update_Tick;

            textBoxBigBlind.Visible = true;
            textBoxSmallBlind.Visible = true;
            buttonBigBlind.Visible = true;
            buttonSmallBlind.Visible = true;
            textBoxBigBlind.Visible = false;
            textBoxSmallBlind.Visible = false;
            buttonBigBlind.Visible = false;
            buttonSmallBlind.Visible = false;
            textBoxRaise.Text = (bigBlind * 2).ToString();
        }
        
        async Task Shuffle()
        {
            dataBase.AddBools(playerFoldedTurn);
            dataBase.AddBools(firstBotFoldedTurn);
            dataBase.AddBools(secondBotFoldedTurn);
            dataBase.AddBools(thirdBotFoldedTurn);
            dataBase.AddBools(forthBotFoldedTurn);
            dataBase.AddBools(fifthBotFoldedTurn);

            buttonCall.Enabled = false;
            buttonRaise.Enabled = false;
            buttonFold.Enabled = false;
            buttonCheck.Enabled = false;

            MaximizeBox = false;
            MinimizeBox = false;

            bool check = false;

            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");

            int horizontal = 580;
            int vertical = 480;

            Random r = new Random();

            for (index = ImgLocation.Length; index > 0; index--)
            {
                int j = r.Next(index);
                var k = ImgLocation[j];
                ImgLocation[j] = ImgLocation[index - 1];
                ImgLocation[index - 1] = k;
            }

            for (index = 0; index < CARDS_ON_THE_FIELD_COUNT; index++)
            {
                try
                {
                    Deck[index] = Image.FromFile(ImgLocation[index]);
                }
                catch (ArgumentException)
                {
                    throw new ArgumentException("The the file does not exists");
                }
                var charsToRemove = new string[]
                {
                    "Assets\\Cards\\", ".png"
                };

                foreach (var c in charsToRemove)
                {
                    ImgLocation[index] = ImgLocation[index].Replace(c, string.Empty);
                }

                Reserve[index] = int.Parse(ImgLocation[index]) - 1;
                Holder[index] = new PictureBox();
                Holder[index].SizeMode = PictureBoxSizeMode.StretchImage;
                Holder[index].Height = 130;
                Holder[index].Width = 80;
                this.Controls.Add(Holder[index]);
                Holder[index].Name = "pb" + index.ToString();
                await Task.Delay(200);

                #region Throwing Cards
                if (index < 2)
                {
                    if (Holder[0].Tag != null)
                    {
                        Holder[1].Tag = Reserve[1];
                    }
                    Holder[0].Tag = Reserve[0];
                    Holder[index].Image = Deck[index];
                    Holder[index].Anchor = (AnchorStyles.Bottom);
                    //Holder[i].Dock = DockStyle.Top;
                    Holder[index].Location = new Point(horizontal, vertical);
                    horizontal += Holder[index].Width;
                    this.Controls.Add(player.PlayerPanel);
                    player.PlayerPanel.Location = new Point(Holder[0].Left - 10, Holder[0].Top - 10);
                    PlayerPanelInitialization(player);
                }

                if (firstBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 2 && index < 4)
                    {
                        if (Holder[2].Tag != null)
                        {
                            Holder[3].Tag = Reserve[3];
                        }

                        Holder[2].Tag = Reserve[2];

                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }
                        check = true;
                        Holder[index].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        Holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[index].Location = new Point(horizontal, vertical);
                        horizontal += Holder[index].Width;
                        Holder[index].Visible = true;
                        this.Controls.Add(firstBot.PlayerPanel);
                        firstBot.PlayerPanel.Location = new Point(Holder[2].Left - 10, Holder[2].Top - 10);
                        PlayerPanelInitialization(firstBot);

                        if (index == 3)
                        {
                            check = false;
                        }
                    }
                }
                if (secondBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 4 && index < 6)
                    {
                        if (Holder[4].Tag != null)
                        {
                            Holder[5].Tag = Reserve[5];
                        }
                        Holder[4].Tag = Reserve[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }
                        check = true;
                        Holder[index].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        Holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[index].Location = new Point(horizontal, vertical);
                        horizontal += Holder[index].Width;
                        Holder[index].Visible = true;
                        this.Controls.Add(secondBot.PlayerPanel);
                        secondBot.PlayerPanel.Location = new Point(Holder[4].Left - 10, Holder[4].Top - 10);
                        PlayerPanelInitialization(secondBot);
                        if (index == 5)
                        {
                            check = false;
                        }
                    }
                }
                if (thirdBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 6 && index < 8)
                    {
                        if (Holder[6].Tag != null)
                        {
                            Holder[7].Tag = Reserve[7];
                        }
                        Holder[6].Tag = Reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }
                        check = true;
                        Holder[index].Anchor = (AnchorStyles.Top);
                        Holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[index].Location = new Point(horizontal, vertical);
                        horizontal += Holder[index].Width;
                        Holder[index].Visible = true;
                        this.Controls.Add(thirdBot.PlayerPanel);
                        thirdBot.PlayerPanel.Location = new Point(Holder[6].Left - 10, Holder[6].Top - 10);
                        PlayerPanelInitialization(thirdBot);
                        if (index == 7)
                        {
                            check = false;
                        }
                    }
                }
                if (forthBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 8 && index < 10)
                    {
                        if (Holder[8].Tag != null)
                        {
                            Holder[9].Tag = Reserve[9];
                        }
                        Holder[8].Tag = Reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }
                        check = true;
                        Holder[index].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        Holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[index].Location = new Point(horizontal, vertical);
                        horizontal += Holder[index].Width;
                        Holder[index].Visible = true;
                        this.Controls.Add(forthBot.PlayerPanel);
                        forthBot.PlayerPanel.Location = new Point(Holder[8].Left - 10, Holder[8].Top - 10);
                        PlayerPanelInitialization(forthBot);
                        if (index == 9)
                        {
                            check = false;
                        }
                    }
                }
                if (fifthBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (index >= 10 && index < 12)
                    {
                        if (Holder[10].Tag != null)
                        {
                            Holder[11].Tag = Reserve[11];
                        }
                        Holder[10].Tag = Reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }
                        check = true;
                        Holder[index].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        Holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[index].Location = new Point(horizontal, vertical);
                        horizontal += Holder[index].Width;
                        Holder[index].Visible = true;
                        this.Controls.Add(fifthBot.PlayerPanel);
                        fifthBot.PlayerPanel.Location = new Point(Holder[10].Left - 10, Holder[10].Top - 10);
                        PlayerPanelInitialization(fifthBot);
                        if (index == 11)
                        {
                            check = false;
                        }
                    }
                }
                if (index >= 12)
                {
                    Holder[12].Tag = Reserve[12];
                    if (index > 12) Holder[13].Tag = Reserve[13];
                    if (index > 13) Holder[14].Tag = Reserve[14];
                    if (index > 14) Holder[15].Tag = Reserve[15];
                    if (index > 15)
                    {
                        Holder[16].Tag = Reserve[16];

                    }
                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }
                    check = true;
                    if (Holder[index] != null)
                    {
                        Holder[index].Anchor = AnchorStyles.None;
                        Holder[index].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        Holder[index].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
                #endregion
                if (firstBot.Chips <= 0)
                {
                    firstBotFoldedTurn = true;
                    Holder[2].Visible = false;
                    Holder[3].Visible = false;
                }
                else
                {
                    firstBotFoldedTurn = false;
                    if (index == 3)
                    {
                        if (Holder[3] != null)
                        {
                            Holder[2].Visible = true;
                            Holder[3].Visible = true;
                        }
                    }
                }
                if (secondBot.Chips <= 0)
                {
                    secondBotFoldedTurn = true;
                    Holder[4].Visible = false;
                    Holder[5].Visible = false;
                }
                else
                {
                    secondBotFoldedTurn = false;
                    if (index == 5)
                    {
                        if (Holder[5] != null)
                        {
                            Holder[4].Visible = true;
                            Holder[5].Visible = true;
                        }
                    }
                }
                if (thirdBot.Chips <= 0)
                {
                    thirdBotFoldedTurn = true;
                    Holder[6].Visible = false;
                    Holder[7].Visible = false;
                }
                else
                {
                    thirdBotFoldedTurn = false;
                    if (index == 7)
                    {
                        if (Holder[7] != null)
                        {
                            Holder[6].Visible = true;
                            Holder[7].Visible = true;
                        }
                    }
                }
                if (forthBot.Chips <= 0)
                {
                    forthBotFoldedTurn = true;
                    Holder[8].Visible = false;
                    Holder[9].Visible = false;
                }
                else
                {
                    forthBotFoldedTurn = false;
                    if (index == 9)
                    {
                        if (Holder[9] != null)
                        {
                            Holder[8].Visible = true;
                            Holder[9].Visible = true;
                        }
                    }
                }
                if (fifthBot.Chips <= 0)
                {
                    fifthBotFoldedTurn = true;
                    Holder[10].Visible = false;
                    Holder[11].Visible = false;
                }
                else
                {
                    fifthBotFoldedTurn = false;
                    if (index == 11)
                    {
                        if (Holder[11] != null)
                        {
                            Holder[10].Visible = true;
                            Holder[11].Visible = true;
                        }
                    }
                }
                if (index == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    timer.Start();
                }
            }
            if (foldedPlayers == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                foldedPlayers = 5;
            }
            if (index == 17)
            {
                buttonRaise.Enabled = true;
                buttonCall.Enabled = true;
                buttonRaise.Enabled = true;
                buttonRaise.Enabled = true;
                buttonFold.Enabled = true;
            }
        }
        //Player Panel Initialization
        private void PlayerPanelInitialization(Player player)
        {
            player.PlayerPanel.BackColor = Color.DarkBlue;
            player.PlayerPanel.Height = 150;
            player.PlayerPanel.Width = 180;
            player.PlayerPanel.Visible = false;
        }

        async Task Turns()
        {
            #region Rotating
            if (!playerFoldedTurn)
            {
                if (playerTurn)
                {
                    FixCall(playerStatus, ref playerCall, ref playerRaise, 1);
                    //MessageBox.Show("Player'cardsCurrentValue turn");

                    pbTimer.Visible = true;
                    pbTimer.Value = 1000;

                    timeRemaining = 60;
                    up = 10000000;
                    timer.Start();

                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    //buttonRaise.Enabled = true;
                    //buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;

                    turnCount++;

                    FixCall(playerStatus, ref playerCall, ref playerRaise, 2);
                }
            }
            if (playerFoldedTurn || !playerTurn)
            {
                await AllIn();
                if (playerFoldedTurn && !playerFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        dataBase.BoolsRemoveAt(0);
                        dataBase.BoolsInsert(0, null);
                        maxLeft--;
                        playerFolded = true;
                    }
                }
                await CheckRaise(0, 0);

                pbTimer.Visible = false;
                buttonRaise.Enabled = false;
                buttonCall.Enabled = false;
                //buttonRaise.Enabled = false;
                //buttonRaise.Enabled = false;
                buttonFold.Enabled = false;

                timer.Stop();

                firstBot.Turn = true;

                if (!firstBotFoldedTurn)
                {
                    if (firstBot.Turn)
                    {
                        int firstBotCall = firstBot.PlayerCall;
                        int firstBotRaise = firstBot.Raise;
                        double firstBotType = firstBot.PlayerType;
                        int firstBotChips = firstBot.Chips;
                        double firstBotPower = firstBot.Power;
                        bool firstBotTurn = firstBot.Turn;

                        FixCall(b1Status, ref firstBotCall, ref firstBotRaise, 1);
                        FixCall(b1Status, ref firstBotCall, ref firstBotRaise, 2);
                        Rules(2, 3, "Bot 1", ref firstBotType, ref firstBotPower, firstBotFoldedTurn);
                        MessageBox.Show("Bot 1'cardsCurrentValue turn");
                        AI(2, 3, ref firstBotChips, ref firstBotTurn, ref firstBotFoldedTurn, b1Status, 0, firstBotPower, firstBot.PlayerType);
                        turnCount++;
                        last = 1;
                        firstBot.Turn = false;
                        secondBot.Turn = true;
                    }
                }
                if (firstBotFoldedTurn && !firstBot.PlayerFolded)
                {
                    dataBase.BoolsRemoveAt(1);
                    dataBase.BoolsInsert(1, null);
                    maxLeft--;
                    firstBot.PlayerFolded = true;
                }
                if (firstBotFoldedTurn || !firstBot.Turn)
                {
                    await CheckRaise(1, 1);
                    secondBot.Turn = true;
                }
                if (!secondBotFoldedTurn)
                {
                    if (secondBot.Turn)
                    {
                        int secondBotCall = secondBot.PlayerCall;
                        int secondBotRaise = secondBot.Raise;
                        double secondBotType = secondBot.PlayerType;
                        int secondBotChips = secondBot.Chips;
                        double secondBotPower = secondBot.Power;
                        bool secondBotTurn = secondBot.Turn;

                        FixCall(b2Status, ref secondBotCall, ref secondBotRaise, 1);
                        FixCall(b2Status, ref secondBotCall, ref secondBotRaise, 2);
                        Rules(4, 5, "Bot 2", ref secondBotType, ref secondBotPower, secondBotFoldedTurn);
                        MessageBox.Show("Bot 2'cardsCurrentValue turn");
                        AI(4, 5, ref secondBotChips, ref secondBotTurn, ref secondBotFoldedTurn, b2Status, 1, secondBotPower, secondBot.PlayerType);
                        turnCount++;
                        last = 2;
                        secondBot.Turn = false;
                        thirdBot.Turn = true;
                    }
                }
                if (secondBotFoldedTurn && !secondBot.PlayerFolded)
                {
                    dataBase.BoolsRemoveAt(2);
                    dataBase.BoolsInsert(2, null);
                    maxLeft--;
                    secondBot.PlayerFolded = true;
                }
                if (secondBotFoldedTurn || !secondBot.Turn)
                {
                    await CheckRaise(2, 2);
                    thirdBot.Turn = true;
                }
                if (!thirdBotFoldedTurn)
                {
                    if (thirdBot.Turn)
                    {
                        int thirdBotCall = thirdBot.PlayerCall;
                        int thirdBotRaise = thirdBot.Raise;
                        double thirdBotType = thirdBot.PlayerType;
                        int thirdBotChips = thirdBot.Chips;
                        double thirdBotPower = thirdBot.Power;
                        bool thirdBotTurn = thirdBot.Turn;

                        FixCall(b3Status, ref thirdBotCall, ref thirdBotRaise, 1);
                        FixCall(b3Status, ref thirdBotCall, ref thirdBotRaise, 2);
                        Rules(6, 7, "Bot 3", ref thirdBotType, ref thirdBotPower, thirdBotFoldedTurn);
                        MessageBox.Show("Bot 3'cardsCurrentValue turn");
                        AI(6, 7, ref thirdBotChips, ref thirdBotTurn, ref thirdBotFoldedTurn, b3Status, 2, thirdBotPower, thirdBot.PlayerType);
                        turnCount++;
                        last = 3;
                        thirdBot.Turn = false;
                        forthBot.Turn = true;
                    }
                }
                if (thirdBotFoldedTurn && !thirdBot.PlayerFolded)
                {
                    dataBase.BoolsRemoveAt(3);
                    dataBase.BoolsInsert(3, null);
                    maxLeft--;
                    thirdBot.PlayerFolded = true;
                }
                if (thirdBotFoldedTurn || !thirdBot.Turn)
                {
                    await CheckRaise(3, 3);
                    forthBot.Turn = true;
                }
                if (!forthBotFoldedTurn)
                {
                    if (forthBot.Turn)
                    {
                        int forthBotCall = forthBot.PlayerCall;
                        int forthBotRaise = forthBot.Raise;
                        double forthBotType = forthBot.PlayerType;
                        int forthBotChips = forthBot.Chips;
                        double forthBotPower = forthBot.Power;
                        bool forthBotTurn = forthBot.Turn;

                        FixCall(b4Status, ref forthBotCall, ref forthBotRaise, 1);
                        FixCall(b4Status, ref forthBotCall, ref forthBotRaise, 2);
                        Rules(8, 9, "Bot 4", ref forthBotType, ref forthBotPower, forthBotFoldedTurn);
                        MessageBox.Show("Bot 4'cardsCurrentValue turn");
                        AI(8, 9, ref forthBotChips, ref forthBotTurn, ref forthBotFoldedTurn, b4Status, 3, forthBotPower, forthBot.PlayerType);
                        turnCount++;
                        last = 4;
                        forthBot.Turn = false;
                        fifthBot.Turn = true;
                    }
                }
                if (forthBotFoldedTurn && !forthBot.PlayerFolded)
                {
                    dataBase.BoolsRemoveAt(4);
                    dataBase.BoolsInsert(4, null);
                    maxLeft--;
                    forthBot.PlayerFolded = true;
                }
                if (forthBotFoldedTurn || !forthBot.Turn)
                {
                    await CheckRaise(4, 4);
                    fifthBot.Turn = true;
                }
                if (!fifthBotFoldedTurn)
                {
                    if (fifthBot.Turn)
                    {
                        int fifthBotCall = fifthBot.PlayerCall;
                        int fifthBotRaise = fifthBot.Raise;
                        double fifthBotType = fifthBot.PlayerType;
                        int fifthBotChips = fifthBot.Chips;
                        double fifthBotPower = fifthBot.Power;
                        bool fifthBotTurn = fifthBot.Turn;

                        FixCall(b5Status, ref fifthBotCall, ref fifthBotRaise, 1);
                        FixCall(b5Status, ref fifthBotCall, ref fifthBotRaise, 2);
                        Rules(10, 11, "Bot 5", ref fifthBotType, ref fifthBotPower, fifthBotFoldedTurn);
                        MessageBox.Show("Bot 5'cardsCurrentValue turn");
                        AI(10, 11, ref fifthBotChips, ref fifthBotTurn, ref fifthBotFoldedTurn, b5Status, 4, fifthBotPower, fifthBot.PlayerType);
                        turnCount++;
                        last = 5;
                        fifthBot.Turn = false;
                    }
                }
                if (fifthBotFoldedTurn && !fifthBot.PlayerFolded)
                {
                    dataBase.BoolsRemoveAt(5);
                    dataBase.BoolsInsert(5, null);
                    maxLeft--;
                    fifthBot.PlayerFolded = true;
                }
                if (fifthBotFoldedTurn || !fifthBot.Turn)
                {
                    await CheckRaise(5, 5);
                    playerTurn = true;
                }
                if (playerFoldedTurn && !playerFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        dataBase.BoolsRemoveAt(0);
                        dataBase.BoolsInsert(0, null);
                        maxLeft--;
                        playerFolded = true;
                    }
                }
                #endregion
                await AllIn();
                if (!restart)
                {
                    await Turns();
                }
                restart = false;
            }
        }

        // Dependant on 'Reserve[]', 'Holder[]', 'rPairFromHand()', 'rPairTwoPair()', etc.
        void Rules(int cardOne, int cardTwo, string currentText, ref double cardsCurrentValue, ref double power, bool foldedTurn)
        {
            if (cardOne == 0 && cardTwo == 1)
            {
            }
            if (!foldedTurn || cardOne == 0 && cardTwo == 1 && playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables

                bool done = false;
                bool vf = false;

                int[] littleStraight = new int[5];
                int[] bigStraight = new int[7];

                bigStraight[0] = Reserve[cardOne];
                bigStraight[1] = Reserve[cardTwo];
                littleStraight[0] = bigStraight[2] = Reserve[12];
                littleStraight[1] = bigStraight[3] = Reserve[13];
                littleStraight[2] = bigStraight[4] = Reserve[14];
                littleStraight[3] = bigStraight[5] = Reserve[15];
                littleStraight[4] = bigStraight[6] = Reserve[16];

                var diamondStraight = bigStraight.Where(o => o % 4 == 0).ToArray();
                var clubsStraight = bigStraight.Where(o => o % 4 == 1).ToArray();
                var spicesStraight = bigStraight.Where(o => o % 4 == 2).ToArray();
                var heartStraight = bigStraight.Where(o => o % 4 == 3).ToArray();

                var diamondStraightValue = diamondStraight.Select(o => o / 4).Distinct().ToArray();
                var clubsStraightValue = clubsStraight.Select(o => o / 4).Distinct().ToArray();
                var spidesStraightValue = spicesStraight.Select(o => o / 4).Distinct().ToArray();
                var heartStraightValue = heartStraight.Select(o => o / 4).Distinct().ToArray();

                Array.Sort(bigStraight);
                Array.Sort(diamondStraightValue);
                Array.Sort(clubsStraightValue);
                Array.Sort(spidesStraightValue);
                Array.Sort(heartStraightValue);
                #endregion

                const int CARDS_ON_TABLE = 16;

                for (index = 0; index < CARDS_ON_TABLE; index++)
                {
                    if (Reserve[index] == int.Parse(Holder[cardOne].Tag.ToString()) 
                        && Reserve[index + 1] == int.Parse(Holder[cardTwo].Tag.ToString()))
                    {
                        //Pair from Hand current = 1

                        CheckForPairFromHand(ref cardsCurrentValue, ref power);

                        #region Pair or Two Pair from Table current = 2 || 0
                        CheckForPairTwoPair(ref cardsCurrentValue, ref power);
                        #endregion

                        #region Two Pair current = 2
                        CheckForTwoPair(ref cardsCurrentValue, ref power);
                        #endregion

                        #region Three of a kind current = 3
                        CheckThreeOfAKind(ref cardsCurrentValue, ref power, bigStraight);
                        #endregion

                        #region Straight current = 4
                        CheckStraight(ref cardsCurrentValue, ref power, bigStraight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        CheckFlush(ref cardsCurrentValue, ref power, ref vf, littleStraight);
                        #endregion

                        #region Full House current = 6
                        CheckForFullHouse(ref cardsCurrentValue, ref power, ref done, bigStraight);
                        #endregion

                        #region Four of a Kind current = 7
                        CheckForFourOfAKind(ref cardsCurrentValue, ref power, bigStraight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        CheckForStraightFlush(ref cardsCurrentValue, ref power, diamondStraightValue, clubsStraightValue, spidesStraightValue, heartStraightValue);
                        #endregion

                        #region High Card current = -1
                        CheckForHighCard(ref cardsCurrentValue, ref power);
                        #endregion
                    }
                }
            }
        }

        // Dependant only on 'win' and 'sorted'
        private void CheckForStraightFlush(ref double current, 
            ref double power, 
            int[] diamondStaight, 
            int[] spidesStraight, 
            int[] clubsStraight, 
            int[] heartStraight)
        {
            if (current >= -1)
            {
                if (diamondStaight.Length >= 5)
                {
                    if (diamondStaight[0] + 4 == diamondStaight[4])
                    {
                        current = 8;
                        power = diamondStaight.Max() / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 8
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (diamondStaight[0] == 0 && diamondStaight[1] == 9 
                        && diamondStaight[2] == 10 && diamondStaight[3] == 11
                        && diamondStaight[0] + 12 == diamondStaight[4])
                    {
                        current = 9;
                        power = diamondStaight.Max() / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 9
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (spidesStraight.Length >= 5)
                {
                    if (spidesStraight[0] + 4 == spidesStraight[4])
                    {
                        current = 8;
                        power = spidesStraight.Max() / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 8
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (spidesStraight[0] == 0 && spidesStraight[1] == 9 
                        && spidesStraight[2] == 10 && spidesStraight[3] == 11 
                        && spidesStraight[0] + 12 == spidesStraight[4])
                    {
                        current = 9;
                        power = spidesStraight.Max() / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 9
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (clubsStraight.Length >= 5)
                {
                    if (clubsStraight[0] + 4 == clubsStraight[4])
                    {
                        current = 8;
                        power = (clubsStraight.Max()) / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 8
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (clubsStraight[0] == 0 && clubsStraight[1] == 9 
                        && clubsStraight[2] == 10 && clubsStraight[3] == 11 
                        && clubsStraight[0] + 12 == clubsStraight[4])
                    {
                        current = 9;
                        power = clubsStraight.Max() / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 9
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (heartStraight.Length >= 5)
                {
                    if (heartStraight[0] + 4 == heartStraight[4])
                    {
                        current = 8;
                        power = heartStraight.Max() / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 8
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (heartStraight[0] == 0 && heartStraight[1] == 9 
                        && heartStraight[2] == 10 && heartStraight[3] == 11
                        && heartStraight[0] + 12 == heartStraight[4])
                    {
                        current = 9;
                        power = heartStraight.Max() / 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 9
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void CheckForFourOfAKind(ref double current, ref double power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && 
                        Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        current = 7;
                        power = (Straight[j] / 4) * 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 7
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 
                        && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        power = 13 * 4 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 7
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void CheckForFullHouse(ref double current, ref double power, ref bool done, int[] Straight)
        {
            if (current >= -1)
            {
                type = power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                power = 13 * 2 + current * 100;
                                win.Add(new Type()
                                {
                                    Power = power,
                                    Current = 6
                                });
                                sorted = win.OrderByDescending(op1 => op1.Current).
                                    ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                power = fh.Max() / 4 * 2 + current * 100;
                                win.Add(new Type()
                                {
                                    Power = power,
                                    Current = 6
                                });
                                sorted = win.OrderByDescending(op1 => op1.Current).
                                    ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }
                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }
                if (current != 6)
                {
                    power = type;
                }
            }
        }
        private void CheckFlush(ref double current, ref double power, ref bool vf, int[] Straight1)
        {
            if (current >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();

                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 
                        && Reserve[index] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f1.Max() / 4 && Reserve[index + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 4)//different cards in hand
                {
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 
                        && Reserve[index + 1] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 5)
                {
                    if (Reserve[index] % 4 == f1[0] % 4 && Reserve[index] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f1[0] % 4 
                        && Reserve[index + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f1.Min() / 4
                        && Reserve[index + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        power = f1.Max() + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4
                        && Reserve[index] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f2.Max() / 4 
                            && Reserve[index + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });

                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4
                        && Reserve[index] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 
                        && Reserve[index + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (Reserve[index] % 4 == f2[0] % 4 
                        && Reserve[index] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f2[0] % 4 
                        && Reserve[index + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f2.Min() / 4 
                        && Reserve[index + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        power = f2.Max() + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 
                        && Reserve[index] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f3.Max() / 4 
                            && Reserve[index + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 
                        && Reserve[index] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 
                        && Reserve[index + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (Reserve[index] % 4 == f3[0] % 4 
                        && Reserve[index] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f3[0] % 4
                        && Reserve[index + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f3.Min() / 4 
                        && Reserve[index + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        power = f3.Max() + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 
                        && Reserve[index] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f4.Max() / 4 
                            && Reserve[index + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 
                        && Reserve[index] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 
                        && Reserve[index + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type()
                            {
                                Power = power,
                                Current = 5
                            });
                            sorted = win.OrderByDescending(op1 => op1.Current).
                                ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (Reserve[index] % 4 == f4[0] % 4 
                        && Reserve[index] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f4[0] % 4 
                        && Reserve[index + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f4.Min() / 4 
                        && Reserve[index + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        power = f4.Max() + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        { 
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type()
                        {
                            Power = power,
                            Current = 5.5
                        });
                        sorted = win.OrderByDescending(op1 => op1.Current).
                            ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void CheckStraight(ref double current, ref double power, int[] Straight)
        {
            if (current >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            power = op.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            power = op[j + 4] + current * 100;
                            win.Add(new Type() { Power = power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }
                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 4 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void CheckThreeOfAKind(ref double current, ref double power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            power = 13 * 3 + current * 100;
                            win.Add(new Type() { Power = power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            win.Add(new Type() { Power = power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }
        private void CheckForTwoPair(ref double current, ref double power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (Reserve[index] / 4 != Reserve[index + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (Reserve[index] / 4 == Reserve[tc] / 4 && Reserve[index + 1] / 4 == Reserve[tc - k] / 4 ||
                                    Reserve[index + 1] / 4 == Reserve[tc] / 4 && Reserve[index] / 4 == Reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[index] / 4 == 0)
                                        {
                                            current = 2;
                                            power = 13 * 4 + (Reserve[index + 1] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[index + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = 13 * 4 + (Reserve[index] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[index + 1] / 4 != 0 && Reserve[index] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (Reserve[index] / 4) * 2 + (Reserve[index + 1] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void CheckForPairTwoPair(ref double current, ref double power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;

                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;

                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }
                        if (tc - k >= 12)
                        {
                            if (Reserve[tc] / 4 == Reserve[tc - k] / 4)
                            {
                                if (Reserve[tc] / 4 != Reserve[index] / 4 && Reserve[tc] / 4 != Reserve[index + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[index + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (Reserve[index] / 4) * 2 + 13 * 4 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[index] / 4 == 0)
                                        {
                                            current = 2;
                                            power = (Reserve[index + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[index + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (Reserve[tc] / 4) * 2 + (Reserve[index + 1] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[index] / 4 != 0)
                                        {
                                            current = 2;
                                            power = (Reserve[tc] / 4) * 2 + (Reserve[index] / 4) * 2 + current * 100;
                                            win.Add(new Type() { Power = power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (Reserve[index] / 4 > Reserve[index + 1] / 4)
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + Reserve[index] / 4 + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = Reserve[tc] / 4 + Reserve[index] / 4 + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                power = 13 + Reserve[index + 1] + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                power = Reserve[tc] / 4 + Reserve[index + 1] / 4 + current * 100;
                                                win.Add(new Type() { Power = power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }
                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void CheckForPairFromHand(ref double current, ref double power)
        {
            if (current >= -1)
            {
                bool msgbox = false;

                if (Reserve[index] / 4 == Reserve[index + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (Reserve[index] / 4 == 0)
                        {
                            current = 1;
                            power = 13 * 4 + current * 100;
                            win.Add(new Type() { Power = power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            power = (Reserve[index + 1] / 4) * 4 + current * 100;
                            win.Add(new Type() { Power = power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (Reserve[index + 1] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[index + 1] / 4 == 0)
                            {
                                current = 1;
                                power = 13 * 4 + Reserve[index] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (Reserve[index + 1] / 4) * 4 + Reserve[index] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                    if (Reserve[index] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[index] / 4 == 0)
                            {
                                current = 1;
                                power = 13 * 4 + Reserve[index + 1] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                power = (Reserve[tc] / 4) * 4 + Reserve[index + 1] / 4 + current * 100;
                                win.Add(new Type() { Power = power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }
        private void CheckForHighCard(ref double current, ref double power)
        {
            if (current == -1)
            {
                if (Reserve[index] / 4 > Reserve[index + 1] / 4)
                {
                    current = -1;
                    power = Reserve[index] / 4;
                    win.Add(new Type() { Power = power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    power = Reserve[index + 1] / 4;
                    win.Add(new Type() { Power = power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (Reserve[index] / 4 == 0 || Reserve[index + 1] / 4 == 0)
                {
                    current = -1;
                    power = 13;
                    win.Add(new Type() { Power = power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        // TODO
        void Winner(double current, double power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (Holder[j].Visible)
                    Holder[j].Image = Deck[j];
            }
            if (current == sorted.Current)
            {
                if (power == sorted.Power)
                {
                    winners++;
                    dataBase.AddCheckedWinners(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }
            if (currentText == lastly)//lastfixed
            {
                if (winners > 1)
                {
                    if (dataBase.Contains("Player"))
                    {
                        player.Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxPlayerChips.Text = player.Chips.ToString();
                        //playerPanel.Visible = true;

                    }
                    if (dataBase.Contains("Bot 1"))
                    {
                        firstBot.Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxFirstBotChips.Text = firstBot.Chips.ToString();
                        //b1Panel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 2"))
                    {
                        secondBot.Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxSecondBotChips.Text = secondBot.Chips.ToString();
                        //b2Panel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 3"))
                    {
                        thirdBot.Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxThirdBotChips.Text = thirdBot.Chips.ToString();
                        //b3Panel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 4"))
                    {
                        forthBot.Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxForthBotChips.Text = forthBot.Chips.ToString();
                        //b4Panel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 5"))
                    {
                        fifthBot.Chips += int.Parse(textBoxPot.Text) / winners;
                        textBoxFifthBotChips.Text = fifthBot.Chips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (dataBase.Contains("Player"))
                    {
                        player.Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 1"))
                    {
                        firstBot.Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 2"))
                    {
                        secondBot.Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b2Panel.Visible = true;

                    }
                    if (dataBase.Contains("Bot 3"))
                    {
                        thirdBot.Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 4"))
                    {
                        forthBot.Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }
                    if (dataBase.Contains("Bot 5"))
                    {
                        fifthBot.Chips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b5Panel.Visible = true;
                    }
                }
            }
        } // TODO
        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (raising)
            {
                turnCount = 0;
                raising = false;
                raisedTurn = currentTurn;
                changed = true;
            }
            else
            {
                if (turnCount >= maxLeft - 1 || !changed && turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        raise = 0;
                        call = 0;
                        raisedTurn = 123;
                        rounds++;

                        if (!playerFoldedTurn)
                            playerStatus.Text = "";
                        if (!firstBotFoldedTurn)
                            b1Status.Text = "";
                        if (!secondBotFoldedTurn)
                            b2Status.Text = "";
                        if (!thirdBotFoldedTurn)
                            b3Status.Text = "";
                        if (!forthBotFoldedTurn)
                            b4Status.Text = "";
                        if (!fifthBotFoldedTurn)
                            b5Status.Text = "";
                    }
                }
            }
            if (rounds == flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (Holder[j].Image != Deck[j])
                    {
                        Holder[j].Image = Deck[j];

                        PlayerRaiseInitializing();

                        PlayerCallInitializing();
                    }
                }
            }
            if (rounds == turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (Holder[j].Image != Deck[j])
                    {
                        Holder[j].Image = Deck[j];

                        PlayerRaiseInitializing(); // O

                        PlayerCallInitializing();
                    }
                }
            }
            if (rounds == river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (Holder[j].Image != Deck[j])
                    {
                        Holder[j].Image = Deck[j];

                        PlayerRaiseInitializing();

                        PlayerCallInitializing();
                    }
                }
            }
            if (rounds == end && maxLeft == 6)
            {
                double playerType = player.PlayerType;
                double firstBotType = firstBot.PlayerType;
                double secondBotType = secondBot.PlayerType;
                double thirdBotType = thirdBot.PlayerType;
                double forthBotType = forthBot.PlayerType;
                double fifthBotType = fifthBot.PlayerType;

                double playerPower = player.Power;
                double firstBotPower = firstBot.Power;
                double secondBotPower = secondBot.Power;
                double thirdBotPower = thirdBot.Power;
                double forthBotPower = forthBot.Power;
                double fifthBotPower = fifthBot.Power;

                string fixedLast = "qwerty";
                if (!playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldedTurn);
                }
                if (!b1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref firstBotType, ref firstBotPower, firstBotFoldedTurn);
                }
                if (!b2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref secondBotType, ref secondBotPower, secondBotFoldedTurn);
                }
                if (!b3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref thirdBotType, ref thirdBotPower, thirdBotFoldedTurn);
                }
                if (!b4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref forthBotType, ref forthBotPower, forthBotFoldedTurn);
                }
                if (!b5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref fifthBotType, ref fifthBotPower, fifthBotFoldedTurn);
                }

                Winner(player.PlayerType, playerPower, "Player", player.Chips, fixedLast);
                Winner(firstBot.PlayerType, firstBotPower, "Bot 1", firstBot.Chips, fixedLast);
                Winner(secondBot.PlayerType, secondBotPower, "Bot 2", secondBot.Chips, fixedLast);
                Winner(thirdBot.PlayerType, thirdBotPower, "Bot 3", thirdBot.Chips, fixedLast);
                Winner(forthBot.PlayerType, forthBotPower, "Bot 4", forthBot.Chips, fixedLast);
                Winner(fifthBot.PlayerType, fifthBotPower, "Bot 5", fifthBot.Chips, fixedLast);

                restart = true;

                playerFoldedTurn = true;
                playerFoldedTurn = false;
                firstBotFoldedTurn = false;
                secondBotFoldedTurn = false;
                thirdBotFoldedTurn = false;
                forthBotFoldedTurn = false;
                fifthBotFoldedTurn = false;

                if (player.Chips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.chipsQuantity != 0)
                    {
                        player.Chips = f2.chipsQuantity;
                        firstBot.Chips += f2.chipsQuantity;
                        secondBot.Chips += f2.chipsQuantity;
                        thirdBot.Chips += f2.chipsQuantity;
                        forthBot.Chips += f2.chipsQuantity;
                        fifthBot.Chips += f2.chipsQuantity;
                        playerFoldedTurn = false;
                        playerTurn = true;
                        buttonRaise.Enabled = true;
                        buttonFold.Enabled = true;
                        buttonCheck.Enabled = true;
                        buttonRaise.Text = "raise";
                    }
                }

                PlayerPanelVisibility();

                PlayerRaiseInitializing();

                PlayerCallInitializing();

                last = 0;
                call = bigBlind;
                raise = 0;
                ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                dataBase.ClearBools();
                rounds = 0;

                this.playerPower = 0; this.playerType = -1;

                PlayersPowerInitializing();

                PlayersTypeInitializing();

                dataBase.ClearInts();
                dataBase.ClearCheckedWinners();
                winners = 0;
                win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;

                for (int os = 0; os < CARDS_ON_THE_FIELD_COUNT; os++)
                {
                    Holder[os].Image = null;
                    Holder[os].Invalidate();
                    Holder[os].Visible = false;
                }

                textBoxPot.Text = "0";
                playerStatus.Text = "";

                await Shuffle();

                await Turns();
            }
        }

        private void PlayersTypeInitializing()
        {
            player.PlayerType = -1;
            firstBot.PlayerType = -1;
            secondBot.PlayerType = -1;
            thirdBot.PlayerType = -1;
            forthBot.PlayerType = -1;
            fifthBot.PlayerType = -1;
        }

        // TODO
        /// <summary>
        /// Making the players panel not visible 
        /// </summary>
        private void PlayerPanelVisibility()
        {
            player.PlayerPanel.Visible = false;
            firstBot.PlayerPanel.Visible = false;
            secondBot.PlayerPanel.Visible = false;
            thirdBot.PlayerPanel.Visible = false;
            forthBot.PlayerPanel.Visible = false;
            fifthBot.PlayerPanel.Visible = false;
        }

        // Dependant on 'call', 'raise and 'rounds'
        void FixCall(Label status, ref int cardCall, ref int cardRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cardRaise = int.Parse(changeRaise);
                    }
                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cardCall = int.Parse(changeCall);
                    }
                    if (status.Text.Contains("Check"))
                    {
                        cardRaise = 0;
                        cardCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cardRaise != raise && cardRaise <= raise)
                    {
                        call = Convert.ToInt32(raise) - cardRaise;
                    }
                    if (cardCall != call || cardCall <= call)
                    {
                        call = call - cardCall;
                    }
                    if (cardRaise == raise && raise > 0)
                    {
                        call = 0;
                        buttonCall.Enabled = false;
                        buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }

        // TODO
        async Task AllIn()
        {
            #region All in
            if (player.Chips <= 0 && !intsAdded)
            {
                if (playerStatus.Text.Contains("raise"))
                {
                    dataBase.AddInts(player.Chips);
                    intsAdded = true;
                }
                if (playerStatus.Text.Contains("Call"))
                {
                    dataBase.AddInts(player.Chips);
                    intsAdded = true;
                }
            }
            intsAdded = false;

            if (firstBot.Chips <= 0 && !firstBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    dataBase.AddInts(firstBot.Chips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (secondBot.Chips <= 0 && !secondBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    dataBase.AddInts(secondBot.Chips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (thirdBot.Chips <= 0 && !thirdBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    dataBase.AddInts(thirdBot.Chips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (forthBot.Chips <= 0 && !forthBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    dataBase.AddInts(forthBot.Chips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (fifthBot.Chips <= 0 && !fifthBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    dataBase.AddInts(fifthBot.Chips);
                    intsAdded = true;
                }
            }
            if (dataBase.IntsLenght() == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                dataBase.ClearInts();
            }
            #endregion

            var abc = dataBase.BoolsCount(x => x.Equals(false));

            #region LastManStanding
            if (abc == 1)
            {
                int index = dataBase.IndexOf(false);

                if (index == 0)
                {
                    player.Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = player.Chips.ToString();
                    playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    firstBot.Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = firstBot.Chips.ToString();
                    firstBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    secondBot.Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = secondBot.Chips.ToString();
                    secondBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    thirdBot.Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = thirdBot.Chips.ToString();
                    thirdBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    forthBot.Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = forthBot.Chips.ToString();
                    forthBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    fifthBot.Chips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = fifthBot.Chips.ToString();
                    fifthBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    Holder[j].Visible = false;
                }
                await Finish(1);
            }

            intsAdded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= end)
            {
                await Finish(2);
            }
            #endregion


        } // TODO
        async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }

            PlayerPanelVisibility();

            call = bigBlind;
            raise = 0;
            foldedPlayers = 5;
            type = 0;
            rounds = 0;

            PlayersPowerInitializing();

            raise = 0;

            PlayersTypeInitializing();

            NotBotTurn();

            firstBotFoldedTurn = false;
            secondBotFoldedTurn = false;
            thirdBotFoldedTurn = false;
            forthBotFoldedTurn = false;
            fifthBotFoldedTurn = false;

            PlayerNotFolded();

            playerFoldedTurn = false;
            playerTurn = true;
            restart = false;
            raising = false;

            PlayerCallInitializing();

            PlayerRaiseInitializing();

            height = 0;
            width = 0;
            winners = 0;
            flop = 1;
            turn = 2;
            river = 3;
            end = 4;
            maxLeft = 6;

            last = 123; raisedTurn = 1;

            dataBase.ClearBools();
            dataBase.ClearCheckedWinners();
            dataBase.ClearInts();
            win.Clear();

            sorted.Current = 0;
            sorted.Power = 0;

            textBoxPot.Text = "0";
            timeRemaining = 60; up = 10000000; turnCount = 0;
            playerStatus.Text = "";
            b1Status.Text = "";
            b2Status.Text = "";
            b3Status.Text = "";
            b4Status.Text = "";
            b5Status.Text = "";

            if (player.Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.chipsQuantity != 0)
                {
                    player.Chips = f2.chipsQuantity;
                    firstBot.Chips += f2.chipsQuantity;
                    secondBot.Chips += f2.chipsQuantity;
                    thirdBot.Chips += f2.chipsQuantity;
                    forthBot.Chips += f2.chipsQuantity;
                    fifthBot.Chips += f2.chipsQuantity;
                    playerFoldedTurn = false;
                    playerTurn = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    buttonCheck.Enabled = true;
                    buttonRaise.Text = "raise";
                }
            }

            ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);

            for (int os = 0; os < 17; os++)
            {
                Holder[os].Image = null;
                Holder[os].Invalidate();
                Holder[os].Visible = false;
            }

            await Shuffle();
            //await Turns();
        }

        // Variable setters
        private void PlayersPowerInitializing()
        {
            player.Power = 0;
            firstBot.Power = 0;
            secondBot.Power = 0;
            thirdBot.Power = 0;
            forthBot.Power = 0;
            fifthBot.Power = 0;
        }
        private void NotBotTurn()
        {
            firstBot.Turn = false;
            secondBot.Turn = false;
            thirdBot.Turn = false;
            forthBot.Turn = false;
            fifthBot.Turn = false;
        }
        private void PlayerNotFolded()
        {
            player.PlayerFolded = false;
            firstBot.PlayerFolded = false;
            secondBot.PlayerFolded = false;
            thirdBot.PlayerFolded = false;
            forthBot.PlayerFolded = false;
            fifthBot.PlayerFolded = false;
        }
        private void PlayerCallInitializing()
        {
            player.PlayerCall = 0;
            firstBot.PlayerCall = 0;
            secondBot.PlayerCall = 0;
            thirdBot.PlayerCall = 0;
            forthBot.PlayerCall = 0;
            fifthBot.PlayerCall = 0;
        }
        private void PlayerRaiseInitializing()
        {
            player.Raise = 0;
            firstBot.Raise = 0;
            secondBot.Raise = 0;
            thirdBot.Raise = 0;
            forthBot.Raise = 0;
            fifthBot.Raise = 0;
        }

        // Dependant on 'win', 'sorted', 'pStatus', 'b1Status' ... 'b5Status', 'Rules()', 'Winner()'
        void FixWinners()
        {
            win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";

            double playerType = player.PlayerType;
            double firstBotType = firstBot.PlayerType;
            double secondBotType = secondBot.PlayerType;
            double thirdBotType = thirdBot.PlayerType;
            double forthBotType = forthBot.PlayerType;
            double fifthBotType = fifthBot.PlayerType;

            double playerPower = player.Power;
            double firstBotPower = firstBot.Power;
            double secondBotPower = secondBot.Power;
            double thirdBotPower = thirdBot.Power;
            double forthBotPower = forthBot.Power;
            double fifthBotPower = fifthBot.Power;

            if (!playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldedTurn);
            }
            if (!b1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref firstBotType, ref firstBotPower, firstBotFoldedTurn);
            }
            if (!b2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref secondBotType, ref secondBotPower, secondBotFoldedTurn);
            }
            if (!b3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref thirdBotType, ref thirdBotPower, thirdBotFoldedTurn);
            }
            if (!b4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref forthBotType, ref forthBotPower, forthBotFoldedTurn);
            }
            if (!b5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref fifthBotType, ref fifthBotPower, fifthBotFoldedTurn);
            }

            Winner(player.PlayerType, player.Power, "Player", player.Chips, fixedLast);
            Winner(firstBot.PlayerType, firstBotPower, "Bot 1", firstBot.Chips, fixedLast);
            Winner(secondBot.PlayerType, secondBotPower, "Bot 2", secondBot.Chips, fixedLast);
            Winner(thirdBot.PlayerType, thirdBotPower, "Bot 3", thirdBot.Chips, fixedLast);
            Winner(forthBot.PlayerType, forthBotPower, "Bot 4", forthBot.Chips, fixedLast);
            Winner(fifthBot.PlayerType, fifthBotPower, "Bot 5", fifthBot.Chips, fixedLast);
        }

        // Dependant on 'Holder'
        void AI(int c1, int c2, ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, double botPower, double botCurrent)
        {
            if (!isBotFinalTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, botPower);
                }
            }
            if (isBotFinalTurn)
            {
                Holder[c1].Visible = false;
                Holder[c2].Visible = false;
            }
        }

        // Dependant only on 'HP' and 'PH'
        private void HighCard(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, double botPower)
        {
            HP(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, botPower, 20, 25);
        }
        private void PairTable(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, double botPower)
        {
            HP(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, botPower, 16, 25);
        }
        private void PairHand(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, double botPower)
        {
            Random pairHand = new Random();
            int pairHandCall = pairHand.Next(10, 16);
            int pairHandRaise = pairHand.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, pairHandCall, 6, pairHandRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                PH(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, pairHandCall, 7, pairHandRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                PH(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, pairHandCall, 9, pairHandRaise);
            }
        }
        private void TwoPair(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, double botPower)
        {
            Random twoPair = new Random();
            int twoPairCall = twoPair.Next(6, 11);
            int twoPairRaise = twoPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, twoPairCall, 3, twoPairRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                PH(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, twoPairCall, 4, twoPairRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                PH(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, twoPairCall, 4, twoPairRaise);
            }
        }
        private void ThreeOfAKind(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, double botPower)
        {
            Random threeOfAKind = new Random();
            int threeOfAKindCall = threeOfAKind.Next(3, 7);
            int threeOfAKindRaise = threeOfAKind.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, threeOfAKindCall, threeOfAKindRaise);
            }
            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, threeOfAKindCall, threeOfAKindRaise);
            }
            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, threeOfAKindCall, threeOfAKindRaise);
            }
        }
        private void Straight(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, double botPower)
        {
            Random straight = new Random();
            int straightCall = straight.Next(3, 6);
            int straightRaise = straight.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, straightCall, straightRaise);
            }
            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, straightCall, straightRaise);
            }
            if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, straightCall, straightRaise);
            }
        }
        private void Flush(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, double botPower)
        {
            Random flush = new Random();
            int flushCall = flush.Next(2, 6);
            int flushRaise = flush.Next(3, 7);
            Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, flushCall, flushRaise);
        }
        private void FullHouse(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, double botPower)
        {
            Random fullHouse = new Random();
            int fullHouseCall = fullHouse.Next(1, 5);
            int fullHouseRaise = fullHouse.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, fullHouseCall, fullHouseRaise);
            }
            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, fullHouseCall, fullHouseRaise);
            }
        }
        private void FourOfAKind(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, double botPower)
        {
            Random fourOfAKind = new Random();
            int fourOfAKindCall = fourOfAKind.Next(1, 4);
            int fourOfAKindRaise = fourOfAKind.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, fourOfAKindCall, fourOfAKindRaise);
            }
        }
        private void StraightFlush(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, double botPower)
        {
            Random straightFlush = new Random();
            int straightFlushCall = straightFlush.Next(1, 3);
            int straightFlushRaise = straightFlush.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref botChips, ref isBotTurn, ref isBotFinalTurn, statusLabel, name, straightFlushCall, straightFlushRaise);
            }
        }

        // Dependant on 'raising', 'call' and 'textBoxPot'
        private void Fold(ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel)
        {
            raising = false;
            statusLabel.Text = "Fold";
            isBotTurn = false;
            isBotFinalTurn = true;
        }
        private void Check(ref bool isBotTurn, Label botStatus)
        {
            botStatus.Text = "Check";
            isBotTurn = false;
            raising = false;
        }
        private void Call(ref int botChips, ref bool isBotTurn, Label statusLabel)
        {
            raising = false;
            isBotTurn = false;
            botChips -= call;
            statusLabel.Text = "Call " + call;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
        }
        private void Raised(ref int botChips, ref bool isBotTurn, Label statusLabel)
        {
            botChips -= Convert.ToInt32(raise);
            statusLabel.Text = "raise " + raise;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + Convert.ToInt32(raise)).ToString();
            call = Convert.ToInt32(raise);
            raising = true;
            isBotTurn = false;
        }

        // Independant
        private static double RoundN(int botChips, int n)
        {
            double a = Math.Round((botChips / n) / 100d, 0) * 100;
            return a;
        }

        // Dependant on 'call', 'raise', 'rounds' and 'RoundN()', 'Fold()', 'Check()', etc.
        private void HP(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(ref isBotTurn, statusLabel);
            }
            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(botChips, n))
                    {
                        Call(ref botChips, ref isBotTurn, statusLabel);
                    }
                    else
                    {
                        Fold(ref isBotTurn, ref isBotFinalTurn, statusLabel);
                    }
                }
                if (rnd == 2)
                {
                    if (call <= RoundN(botChips, n1))
                    {
                        Call(ref botChips, ref isBotTurn, statusLabel);
                    }
                    else
                    {
                        Fold(ref isBotTurn, ref isBotFinalTurn, statusLabel);
                    }
                }
            }
            if (rnd == 3)
            {
                if (raise == 0)
                {
                    raise = call * 2;
                    Raised(ref botChips, ref isBotTurn, statusLabel);
                }
                else
                {
                    if (raise <= RoundN(botChips, n))
                    {
                        raise = call * 2;
                        Raised(ref botChips, ref isBotTurn, statusLabel);
                    }
                    else
                    {
                        Fold(ref isBotTurn, ref isBotFinalTurn, statusLabel);
                    }
                }
            }
            if (botChips <= 0)
            {
                isBotFinalTurn = true;
            }
        }
        private void PH(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(ref isBotTurn, statusLabel);
                }
                if (call > 0)
                {
                    if (call >= RoundN(botChips, n1))
                    {
                        Fold(ref isBotTurn, ref isBotFinalTurn, statusLabel);
                    }
                    if (raise > RoundN(botChips, n))
                    {
                        Fold(ref isBotTurn, ref isBotFinalTurn, statusLabel);
                    }
                    if (!isBotFinalTurn)
                    {
                        if (call >= RoundN(botChips, n) && call <= RoundN(botChips, n1))
                        {
                            Call(ref botChips, ref isBotTurn, statusLabel);
                        }
                        if (raise <= RoundN(botChips, n) && raise >= (RoundN(botChips, n)) / 2)
                        {
                            Call(ref botChips, ref isBotTurn, statusLabel);
                        }
                        if (raise <= (RoundN(botChips, n)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = RoundN(botChips, n);
                                Raised(ref botChips, ref isBotTurn, statusLabel);
                            }
                            else
                            {
                                raise = call * 2;
                                Raised(ref botChips, ref isBotTurn, statusLabel);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(botChips, n1 - rnd))
                    {
                        Fold(ref isBotTurn, ref isBotFinalTurn, statusLabel);
                    }
                    if (raise > RoundN(botChips, n - rnd))
                    {
                        Fold(ref isBotTurn, ref isBotFinalTurn, statusLabel);
                    }
                    if (!isBotFinalTurn)
                    {
                        if (call >= RoundN(botChips, n - rnd) && call <= RoundN(botChips, n1 - rnd))
                        {
                            Call(ref botChips, ref isBotTurn, statusLabel);
                        }
                        if (raise <= RoundN(botChips, n - rnd) && raise >= (RoundN(botChips, n - rnd)) / 2)
                        {
                            Call(ref botChips, ref isBotTurn, statusLabel);
                        }
                        if (raise <= (RoundN(botChips, n - rnd)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = RoundN(botChips, n - rnd);
                                Raised(ref botChips, ref isBotTurn, statusLabel);
                            }
                            else
                            {
                                raise = call * 2;
                                Raised(ref botChips, ref isBotTurn, statusLabel);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    raise = RoundN(botChips, r - rnd);
                    Raised(ref botChips, ref isBotTurn, statusLabel);
                }
            }
            if (botChips <= 0)
            {
                isBotFinalTurn = true;
            }
        }

        // Dependant on 'call', 'raise', 'raising', 'TextBoxPot' and 'RoundN()', 'Fold()', 'Check()', etc.
        void Smooth(ref int botChips, ref bool isBotTurn, ref bool isBotFinalTurn, Label statusLabel, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);

            if (call <= 0)
            {
                Check(ref isBotTurn, statusLabel);
            }
            else
            {
                if (call >= RoundN(botChips, n))
                {
                    if (botChips > call)
                    {
                        Call(ref botChips, ref isBotTurn, statusLabel);
                    }
                    else if (botChips <= call)
                    {
                        raising = false;
                        isBotTurn = false;
                        botChips = 0;
                        statusLabel.Text = "Call " + botChips;
                        textBoxPot.Text = (int.Parse(textBoxPot.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (raise > 0)
                    {
                        if (botChips >= raise * 2)
                        {
                            raise *= 2;
                            Raised(ref botChips, ref isBotTurn, statusLabel);
                        }
                        else
                        {
                            Call(ref botChips, ref isBotTurn, statusLabel);
                        }
                    }
                    else
                    {
                        raise = call * 2;
                        Raised(ref botChips, ref isBotTurn, statusLabel);
                    }
                }
            }
            if (botChips <= 0)
            {
                isBotFinalTurn = true;
            }
        }

        /// <summary>
        /// UserInterface methods
        /// </summary>
        /// <param name="sender">unknown</param>
        /// <param name="e">unknown</param>
        /// <remarks>Depends on 'playerFoldedTurn', ''</remarks>
        // Have to make class UserInterface
        #region UserInterface
        // Dependant on 'playerFoldedTurn', 'timeRemaining', 'Turns()'
        private async void timer_Tick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                playerFoldedTurn = true;
                await Turns();
            }
            if (timeRemaining > 0)
            {
                timeRemaining--;
                pbTimer.Value = (timeRemaining / 6) * 100;
            }
        }
        // Dependant on 'playerChips', 'up', 'call', Player objects
        private void Update_Tick(object sender, object e)
        {
            if (player.Chips <= 0)
            {
                textBoxPlayerChips.Text = "Player Chips: 0";
            }
            if (firstBot.Chips <= 0)
            {
                textBoxFirstBotChips.Text = "1st Bot Chips: 0";
            }
            if (secondBot.Chips <= 0)
            {
                textBoxSecondBotChips.Text = "2nd Bot Chips: 0";
            }
            if (thirdBot.Chips <= 0)
            {
                textBoxThirdBotChips.Text = "3rd Bot Chips: 0";
            }
            if (forthBot.Chips <= 0)
            {
                textBoxForthBotChips.Text = "4th Bot Chips: 0";
            }
            if (fifthBot.Chips <= 0)
            {
                textBoxFifthBotChips.Text = "5th Bot Chips: 0";
            }

            textBoxPlayerChips.Text = "Player Chips: " + player.Chips.ToString();
            textBoxFirstBotChips.Text = "1st Bot Chips: " + firstBot.Chips.ToString();
            textBoxSecondBotChips.Text = "2nd Bot Chips: " + secondBot.Chips.ToString();
            textBoxThirdBotChips.Text = "3rd Bot Chips: " + thirdBot.Chips.ToString();
            textBoxForthBotChips.Text = "4th Bot Chips: " + forthBot.Chips.ToString();
            textBoxFifthBotChips.Text = "5th Bot Chips: " + fifthBot.Chips.ToString();

            if (player.Chips <= 0)
            {
                playerTurn = false;
                playerFoldedTurn = true;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                buttonCheck.Enabled = false;
            }
            if (up > 0)
            {
                up--;
            }
            if (player.Chips >= call)
            {
                buttonCall.Text = "Call " + call.ToString();
            }
            else
            {
                buttonCall.Text = "All in";
                buttonRaise.Enabled = false;
            }
            if (call > 0)
            {
                buttonCheck.Enabled = false;
            }
            if (call <= 0)
            {
                buttonCheck.Enabled = true;
                buttonCall.Text = "Call";
                buttonCall.Enabled = false;
            }
            if (player.Chips <= 0)
            {
                buttonRaise.Enabled = false;
            }
            int parsedValue;

            if (textBoxRaise.Text != "" && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (player.Chips <= int.Parse(textBoxRaise.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "raise";
                }
            }
            if (player.Chips < call)
            {
                buttonRaise.Enabled = false;
            }
        }
        // Dependant on 'playerTurn' and 'Turns()' 
        private async void BotFold_Click(object sender, EventArgs e)
        {
            playerStatus.Text = "Fold";
            playerTurn = false;
            playerFoldedTurn = true;
            await Turns();
        }
        // Dependant on 'playerTurn', 'call' and 'Turns()' 
        private async void BotCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                playerTurn = false;
                playerStatus.Text = "Check";
            }
            else
            {
                //playerStatus.Text = "All in " + playerChips;

                buttonCheck.Enabled = false;
            }
            await Turns();
        }
        // Dependant on 'playerChips', 'call', 'playerTurn', 'playerCall', 'Rules()', 'Turns()' 
        private async void BotCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldedTurn);

            if (player.Chips >= call)
            {
                player.Chips -= call;
                textBoxPlayerChips.Text = "playerChips : " + player.Chips.ToString();
                if (textBoxPot.Text != "")
                {
                    textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
                }
                else
                {
                    textBoxPot.Text = call.ToString();
                }
                playerTurn = false;
                playerStatus.Text = "Call " + call;
                playerCall = call;
            }
            else if (player.Chips <= call && call > 0)
            {
                textBoxPot.Text = (int.Parse(textBoxPot.Text) + player.Chips).ToString();
                playerStatus.Text = "All in " + player.Chips;
                player.Chips = 0;
                textBoxPlayerChips.Text = "playerChips : " + player.Chips.ToString();
                playerTurn = false;
                buttonFold.Enabled = false;
                playerCall = player.Chips;
            }
            await Turns();
        }
        // Dependant on 'playerChips', 'call', 'raise', 'raising', 'last', 'playerRaise', 'Rules()', 'Turns()' 
        private async void BotRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldedTurn);

            int parsedValue;
            if (textBoxRaise.Text != "" && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (player.Chips > call)
                {
                    if (raise * 2 > int.Parse(textBoxRaise.Text))
                    {
                        textBoxRaise.Text = (raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (player.Chips >= int.Parse(textBoxRaise.Text))
                        {
                            call = int.Parse(textBoxRaise.Text);
                            raise = int.Parse(textBoxRaise.Text);
                            playerStatus.Text = "raise " + call.ToString();
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
                            buttonCall.Text = "Call";
                            player.Chips -= int.Parse(textBoxRaise.Text);
                            raising = true;
                            last = 0;
                            playerRaise = Convert.ToInt32(raise);
                        }
                        else
                        {
                            call = player.Chips;
                            raise = player.Chips;
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + player.Chips).ToString();
                            playerStatus.Text = "raise " + call.ToString();
                            player.Chips = 0;
                            raising = true;
                            last = 0;
                            playerRaise = Convert.ToInt32(raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }
            playerTurn = false;
            await Turns();
        }
        // Dependant on 'playerChips' and Player objects
        private void BotAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAdd.Text == "") { }
            else
            {
                player.Chips += int.Parse(textBoxAdd.Text);
                firstBot.Chips += int.Parse(textBoxAdd.Text);
                secondBot.Chips += int.Parse(textBoxAdd.Text);
                thirdBot.Chips += int.Parse(textBoxAdd.Text);
                forthBot.Chips += int.Parse(textBoxAdd.Text);
                fifthBot.Chips += int.Parse(textBoxAdd.Text);
            }
            textBoxPlayerChips.Text = "playerChips : " + player.Chips.ToString();
        }
        // Dependant on 'bigBlind' and 'smallBlind'
        private void BotOptions_Click(object sender, EventArgs e)
        {
            textBoxBigBlind.Text = bigBlind.ToString();
            textBoxSmallBlind.Text = smallBlind.ToString();

            if (textBoxBigBlind.Visible == false)
            {
                textBoxBigBlind.Visible = true;
                textBoxSmallBlind.Visible = true;
                buttonBigBlind.Visible = true;
                buttonSmallBlind.Visible = true;
            }
            else
            {
                textBoxBigBlind.Visible = false;
                textBoxSmallBlind.Visible = false;
                buttonBigBlind.Visible = false;
                buttonSmallBlind.Visible = false;
            }
        }
        // Dependant on 'smallBlind'
        private void BotSmallBlind_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (textBoxSmallBlind.Text.Contains(",") || textBoxSmallBlind.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                textBoxSmallBlind.Text = smallBlind.ToString();
                return;
            }
            if (!int.TryParse(textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                textBoxSmallBlind.Text = smallBlind.ToString();
                return;
            }
            if (int.Parse(textBoxSmallBlind.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                textBoxSmallBlind.Text = smallBlind.ToString();
            }
            if (int.Parse(textBoxSmallBlind.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(textBoxSmallBlind.Text) >= 250 && int.Parse(textBoxSmallBlind.Text) <= 100000)
            {
                smallBlind = int.Parse(textBoxSmallBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        // Dependant on 'bigBlind'
        private void BotBigBlind_Click(object sender, EventArgs e)
        {
            int parsedValue;

            if (textBoxBigBlind.Text.Contains(",") || textBoxBigBlind.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                textBoxBigBlind.Text = bigBlind.ToString();
                return;
            }
            if (!int.TryParse(textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                textBoxSmallBlind.Text = bigBlind.ToString();
                return;
            }
            if (int.Parse(textBoxBigBlind.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                textBoxBigBlind.Text = bigBlind.ToString();
            }
            if (int.Parse(textBoxBigBlind.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(textBoxBigBlind.Text) >= 500 && int.Parse(textBoxBigBlind.Text) <= 200000)
            {
                bigBlind = int.Parse(textBoxBigBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        // Dependant on 'width' and 'height'
        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            width = this.Width;
            height = this.Height;
        }
        #endregion
    }
}