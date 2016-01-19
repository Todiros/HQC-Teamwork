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

        private List<bool?> bools = new List<bool?>();

        private List<Type> win = new List<Type>();

        private List<string> checkWinners = new List<string>();

        private List<int> ints = new List<int>();

        private bool playerFoldedTurn = false;
        private bool playerTurn = true;
        private bool restart = false;
        private bool raising = false;

        //private int Nm; // never used, but commented just in case
        private int call = 500;
        private int foldedPlayers = 5;

        private Panel playerPanel = new Panel();

        private int playerChips = 10000;
        private double playerType = -1;
        private bool playerFolded;
        private int playerCall = 0;
        private int playerRaise = 0;
        private double playerPower = 0;

        private double type;
        private double rounds = 0;
        private double raise = 0;

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

        Poker.Type sorted;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        string[] ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        /*string[] ImgLocation ={
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/
        int[] Reserve = new int[CARDS_ON_THE_FIELD_COUNT];

        Image[] Deck = new Image[ALL_CARDS_COUNT];

        PictureBox[] Holder = new PictureBox[ALL_CARDS_COUNT];

        Timer timer = new Timer();
        Timer updates = new Timer();

        // variable 't' changed to 'time'
        int timeRemaining = 60;
        // variable 'i' changed to 'index'
        int index;
        // variable 'bb' changed to 'bigBlind'
        int bigBlind = 500;
        // variable 'sb' changed to smallBlind'
        int smallBlind = 250;
        // TODO: figure out what 'up' is. Its connected with 'time' somehow.
        int up = 10000000;
        int turnCount = 0;
        #endregion

        public PokerTable()
        {
            player = new Player(playerChips, playerType, playerFolded, playerCall, playerRaise, playerPower);
            firstBot = new Player(playerChips, playerType, playerFolded, playerCall, playerRaise, playerPower);
            secondBot = new Player(playerChips, playerType, playerFolded, playerCall, playerRaise, playerPower);
            thirdBot = new Player(playerChips, playerType, playerFolded, playerCall, playerRaise, playerPower);
            forthBot = new Player(playerChips, playerType, playerFolded, playerCall, playerRaise, playerPower);
            fifthBot = new Player(playerChips, playerType, playerFolded, playerCall, playerRaise, playerPower);

            //bools.Add(playerFoldedTurn); bools.Add(firstBotFoldedTurn); bools.Add(secondBotFoldedTurn); bools.Add(thirdBotFoldedTurn); bools.Add(forthBotFoldedTurn); bools.Add(fifthBotFoldedTurn);
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

            textBoxPlayerChips.Text = "playerChips : " + playerChips.ToString();
            textBoxFirstBotChips.Text = "playerChips : " + firstBot.PlayerChips.ToString();
            textBoxSecondBotChips.Text = "playerChips : " + secondBot.PlayerChips.ToString();
            textBoxThirdBotChips.Text = "playerChips : " + thirdBot.PlayerChips.ToString();
            textBoxForthBotChips.Text = "playerChips : " + forthBot.PlayerChips.ToString();
            textBoxFifthBotChips.Text = "playerChips : " + fifthBot.PlayerChips.ToString();

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
            bools.Add(playerFoldedTurn);
            bools.Add(firstBotFoldedTurn);
            bools.Add(secondBotFoldedTurn);
            bools.Add(thirdBotFoldedTurn);
            bools.Add(forthBotFoldedTurn);
            bools.Add(fifthBotFoldedTurn);

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

                Deck[index] = Image.FromFile(ImgLocation[index]);
                var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };

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
                    player.PlayerPanel.BackColor = Color.DarkBlue;
                    player.PlayerPanel.Height = 150;
                    player.PlayerPanel.Width = 180;
                    player.PlayerPanel.Visible = false;
                }

                if (firstBot.PlayerChips > 0)
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
                        firstBot.PlayerPanel.BackColor = Color.DarkBlue;
                        firstBot.PlayerPanel.Height = 150;
                        firstBot.PlayerPanel.Width = 180;
                        firstBot.PlayerPanel.Visible = false;

                        if (index == 3)
                        {
                            check = false;
                        }
                    }
                }
                if (secondBot.PlayerChips > 0)
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
                        secondBot.PlayerPanel.BackColor = Color.DarkBlue;
                        secondBot.PlayerPanel.Height = 150;
                        secondBot.PlayerPanel.Width = 180;
                        secondBot.PlayerPanel.Visible = false;
                        if (index == 5)
                        {
                            check = false;
                        }
                    }
                }
                if (thirdBot.PlayerChips > 0)
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
                        thirdBot.PlayerPanel.BackColor = Color.DarkBlue;
                        thirdBot.PlayerPanel.Height = 150;
                        thirdBot.PlayerPanel.Width = 180;
                        thirdBot.PlayerPanel.Visible = false;
                        if (index == 7)
                        {
                            check = false;
                        }
                    }
                }
                if (forthBot.PlayerChips > 0)
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
                        forthBot.PlayerPanel.BackColor = Color.DarkBlue;
                        forthBot.PlayerPanel.Height = 150;
                        forthBot.PlayerPanel.Width = 180;
                        forthBot.PlayerPanel.Visible = false;
                        if (index == 9)
                        {
                            check = false;
                        }
                    }
                }
                if (fifthBot.PlayerChips > 0)
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
                        fifthBot.PlayerPanel.BackColor = Color.DarkBlue;
                        fifthBot.PlayerPanel.Height = 150;
                        fifthBot.PlayerPanel.Width = 180;
                        fifthBot.PlayerPanel.Visible = false;
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
                if (firstBot.PlayerChips <= 0)
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
                if (secondBot.PlayerChips <= 0)
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
                if (thirdBot.PlayerChips <= 0)
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
                if (forthBot.PlayerChips <= 0)
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
                if (fifthBot.PlayerChips <= 0)
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
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
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

                firstBotTurn = true;

                if (!firstBotFoldedTurn)
                {
                    if (firstBotTurn)
                    {
                        int firstBotCall = firstBot.PlayerCall;
                        int firstBotRaise = firstBot.PlayerRaise;
                        double firstBotType = firstBot.PlayerType;
                        int firstBotChips = firstBot.PlayerChips;
                        double firstBotPower = firstBot.PlayerPower;

                        FixCall(b1Status, ref firstBotCall, ref firstBotRaise, 1);
                        FixCall(b1Status, ref firstBotCall, ref firstBotRaise, 2);
                        Rules(2, 3, "Bot 1", ref firstBotType, ref firstBotPower, firstBotFoldedTurn);
                        MessageBox.Show("Bot 1'cardsCurrentValue turn");
                        AI(2, 3, ref firstBotChips, ref firstBotTurn, ref firstBotFoldedTurn, b1Status, 0, firstBotPower, firstBot.PlayerType);
                        turnCount++;
                        last = 1;
                        firstBotTurn = false;
                        secondBotTurn = true;
                    }
                }
                if (firstBotFoldedTurn && !firstBot.PlayerFolded)
                {
                    bools.RemoveAt(1);
                    bools.Insert(1, null);
                    maxLeft--;
                    firstBot.PlayerFolded = true;
                }
                if (firstBotFoldedTurn || !firstBotTurn)
                {
                    await CheckRaise(1, 1);
                    secondBotTurn = true;
                }
                if (!secondBotFoldedTurn)
                {
                    if (secondBotTurn)
                    {
                        int secondBotCall = secondBot.PlayerCall;
                        int secondBotRaise = secondBot.PlayerRaise;
                        double secondBotType = secondBot.PlayerType;
                        int secondBotChips = secondBot.PlayerChips;
                        double secondBotPower = secondBot.PlayerPower;

                        FixCall(b2Status, ref secondBotCall, ref secondBotRaise, 1);
                        FixCall(b2Status, ref secondBotCall, ref secondBotRaise, 2);
                        Rules(4, 5, "Bot 2", ref secondBotType, ref secondBotPower, secondBotFoldedTurn);
                        MessageBox.Show("Bot 2'cardsCurrentValue turn");
                        AI(4, 5, ref secondBotChips, ref secondBotTurn, ref secondBotFoldedTurn, b2Status, 1, secondBotPower, secondBot.PlayerType);
                        turnCount++;
                        last = 2;
                        secondBotTurn = false;
                        thirdBotTurn = true;
                    }
                }
                if (secondBotFoldedTurn && !secondBot.PlayerFolded)
                {
                    bools.RemoveAt(2);
                    bools.Insert(2, null);
                    maxLeft--;
                    secondBot.PlayerFolded = true;
                }
                if (secondBotFoldedTurn || !secondBotTurn)
                {
                    await CheckRaise(2, 2);
                    thirdBotTurn = true;
                }
                if (!thirdBotFoldedTurn)
                {
                    if (thirdBotTurn)
                    {
                        int thirdBotCall = thirdBot.PlayerCall;
                        int thirdBotRaise = thirdBot.PlayerRaise;
                        double thirdBotType = thirdBot.PlayerType;
                        int thirdBotChips = thirdBot.PlayerChips;
                        double thirdBotPower = thirdBot.PlayerPower;

                        FixCall(b3Status, ref thirdBotCall, ref thirdBotRaise, 1);
                        FixCall(b3Status, ref thirdBotCall, ref thirdBotRaise, 2);
                        Rules(6, 7, "Bot 3", ref thirdBotType, ref thirdBotPower, thirdBotFoldedTurn);
                        MessageBox.Show("Bot 3'cardsCurrentValue turn");
                        AI(6, 7, ref thirdBotChips, ref thirdBotTurn, ref thirdBotFoldedTurn, b3Status, 2, thirdBotPower, thirdBot.PlayerType);
                        turnCount++;
                        last = 3;
                        thirdBotTurn = false;
                        forthBotTurn = true;
                    }
                }
                if (thirdBotFoldedTurn && !thirdBot.PlayerFolded)
                {
                    bools.RemoveAt(3);
                    bools.Insert(3, null);
                    maxLeft--;
                    thirdBot.PlayerFolded = true;
                }
                if (thirdBotFoldedTurn || !thirdBotTurn)
                {
                    await CheckRaise(3, 3);
                    forthBotTurn = true;
                }
                if (!forthBotFoldedTurn)
                {
                    if (forthBotTurn)
                    {
                        int forthBotCall = forthBot.PlayerCall;
                        int forthBotRaise = forthBot.PlayerRaise;
                        double forthBotType = forthBot.PlayerType;
                        int forthBotChips = forthBot.PlayerChips;
                        double forthBotPower = forthBot.PlayerPower;

                        FixCall(b4Status, ref forthBotCall, ref forthBotRaise, 1);
                        FixCall(b4Status, ref forthBotCall, ref forthBotRaise, 2);
                        Rules(8, 9, "Bot 4", ref forthBotType, ref forthBotPower, forthBotFoldedTurn);
                        MessageBox.Show("Bot 4'cardsCurrentValue turn");
                        AI(8, 9, ref forthBotChips, ref forthBotTurn, ref forthBotFoldedTurn, b4Status, 3, forthBotPower, forthBot.PlayerType);
                        turnCount++;
                        last = 4;
                        forthBotTurn = false;
                        fifthBotTurn = true;
                    }
                }
                if (forthBotFoldedTurn && !forthBot.PlayerFolded)
                {
                    bools.RemoveAt(4);
                    bools.Insert(4, null);
                    maxLeft--;
                    forthBot.PlayerFolded = true;
                }
                if (forthBotFoldedTurn || !forthBotTurn)
                {
                    await CheckRaise(4, 4);
                    fifthBotTurn = true;
                }
                if (!fifthBotFoldedTurn)
                {
                    if (fifthBotTurn)
                    {
                        int fifthBotCall = fifthBot.PlayerCall;
                        int fifthBotRaise = fifthBot.PlayerRaise;
                        double fifthBotType = fifthBot.PlayerType;
                        int fifthBotChips = fifthBot.PlayerChips;
                        double fifthBotPower = fifthBot.PlayerPower;

                        FixCall(b5Status, ref fifthBotCall, ref fifthBotRaise, 1);
                        FixCall(b5Status, ref fifthBotCall, ref fifthBotRaise, 2);
                        Rules(10, 11, "Bot 5", ref fifthBotType, ref fifthBotPower, fifthBotFoldedTurn);
                        MessageBox.Show("Bot 5'cardsCurrentValue turn");
                        AI(10, 11, ref fifthBotChips, ref fifthBotTurn, ref fifthBotFoldedTurn, b5Status, 4, fifthBotPower, fifthBot.PlayerType);
                        turnCount++;
                        last = 5;
                        fifthBotTurn = false;
                    }
                }
                if (fifthBotFoldedTurn && !fifthBot.PlayerFolded)
                {
                    bools.RemoveAt(5);
                    bools.Insert(5, null);
                    maxLeft--;
                    fifthBot.PlayerFolded = true;
                }
                if (fifthBotFoldedTurn || !fifthBotTurn)
                {
                    await CheckRaise(5, 5);
                    playerTurn = true;
                }
                if (playerFoldedTurn && !playerFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
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

                int[] Straight1 = new int[5];
                int[] Straight = new int[7];

                Straight[0] = Reserve[cardOne];
                Straight[1] = Reserve[cardTwo];
                Straight1[0] = Straight[2] = Reserve[12];
                Straight1[1] = Straight[3] = Reserve[13];
                Straight1[2] = Straight[4] = Reserve[14];
                Straight1[3] = Straight[5] = Reserve[15];
                Straight1[4] = Straight[6] = Reserve[16];

                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();

                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();

                Array.Sort(Straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);
                #endregion

                const int CARDS_ON_TABLE = 16;
                for (index = 0; index < CARDS_ON_TABLE; index++)
                {
                    if (Reserve[index] == int.Parse(Holder[cardOne].Tag.ToString()) && Reserve[index + 1] == int.Parse(Holder[cardTwo].Tag.ToString()))
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
                        CheckThreeOfAKind(ref cardsCurrentValue, ref power, Straight);
                        #endregion

                        #region Straight current = 4
                        CheckStraight(ref cardsCurrentValue, ref power, Straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        CheckFlush(ref cardsCurrentValue, ref power, ref vf, Straight1);
                        #endregion

                        #region Full House current = 6
                        CheckForFullHouse(ref cardsCurrentValue, ref power, ref done, Straight);
                        #endregion

                        #region Four of a Kind current = 7
                        CheckForFourOfAKind(ref cardsCurrentValue, ref power, Straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        CheckForStraightFlush(ref cardsCurrentValue, ref power, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        CheckForHighCard(ref cardsCurrentValue, ref power);
                        #endregion
                    }
                }
            }
        }
        private void CheckForStraightFlush(ref double current, ref double power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        power = (st1.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        power = (st1.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        power = (st2.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        power = (st2.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        power = (st3.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        power = (st3.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        power = (st4.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 8 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        power = (st4.Max()) / 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 9 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        current = 7;
                        power = (Straight[j] / 4) * 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        power = 13 * 4 + current * 100;
                        win.Add(new Type() { Power = power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                                win.Add(new Type() { Power = power, Current = 6 });
                                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                power = fh.Max() / 4 * 2 + current * 100;
                                win.Add(new Type() { Power = power, Current = 6 });
                                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f1.Max() / 4 && Reserve[index + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f1[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f1.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f1[0] % 4 && Reserve[index + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f1.Min() / 4 && Reserve[index + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        power = f1.Max() + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f2.Max() / 4 && Reserve[index + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f2.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (Reserve[index] % 4 == f2[0] % 4 && Reserve[index] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f2[0] % 4 && Reserve[index + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f2.Min() / 4 && Reserve[index + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        power = f2.Max() + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f3.Max() / 4 && Reserve[index + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f3.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (Reserve[index] % 4 == f3[0] % 4 && Reserve[index] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f3[0] % 4 && Reserve[index + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f3.Min() / 4 && Reserve[index + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        power = f3.Max() + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (Reserve[index] % 4 == Reserve[index + 1] % 4 && Reserve[index] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[index] / 4 < f4.Max() / 4 && Reserve[index + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (Reserve[index] % 4 != Reserve[index + 1] % 4 && Reserve[index] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[index + 1] % 4 != Reserve[index] % 4 && Reserve[index + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            power = Reserve[index + 1] + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            power = f4.Max() + current * 100;
                            win.Add(new Type() { Power = power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (Reserve[index] % 4 == f4[0] % 4 && Reserve[index] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[index + 1] % 4 == f4[0] % 4 && Reserve[index + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        power = Reserve[index + 1] + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[index] / 4 < f4.Min() / 4 && Reserve[index + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        power = f4.Max() + current * 100;
                        win.Add(new Type() { Power = power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (Reserve[index] / 4 == 0 && Reserve[index] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[index + 1] / 4 == 0 && Reserve[index + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        power = 13 + current * 100;
                        win.Add(new Type() { Power = power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
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
                    checkWinners.Add(currentText);
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
                    if (checkWinners.Contains("Player"))
                    {
                        playerChips += int.Parse(textBoxPot.Text) / winners;
                        textBoxPlayerChips.Text = playerChips.ToString();
                        //playerPanel.Visible = true;

                    }
                    if (checkWinners.Contains("Bot 1"))
                    {
                        firstBot.PlayerChips += int.Parse(textBoxPot.Text) / winners;
                        textBoxFirstBotChips.Text = firstBot.PlayerChips.ToString();
                        //b1Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 2"))
                    {
                        secondBot.PlayerChips += int.Parse(textBoxPot.Text) / winners;
                        textBoxSecondBotChips.Text = secondBot.PlayerChips.ToString();
                        //b2Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 3"))
                    {
                        thirdBot.PlayerChips += int.Parse(textBoxPot.Text) / winners;
                        textBoxThirdBotChips.Text = thirdBot.PlayerChips.ToString();
                        //b3Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 4"))
                    {
                        forthBot.PlayerChips += int.Parse(textBoxPot.Text) / winners;
                        textBoxForthBotChips.Text = forthBot.PlayerChips.ToString();
                        //b4Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 5"))
                    {
                        fifthBot.PlayerChips += int.Parse(textBoxPot.Text) / winners;
                        textBoxFifthBotChips.Text = fifthBot.PlayerChips.ToString();
                        //b5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (checkWinners.Contains("Player"))
                    {
                        playerChips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 1"))
                    {
                        firstBot.PlayerChips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b1Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 2"))
                    {
                        secondBot.PlayerChips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b2Panel.Visible = true;

                    }
                    if (checkWinners.Contains("Bot 3"))
                    {
                        thirdBot.PlayerChips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b3Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 4"))
                    {
                        forthBot.PlayerChips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b4Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 5"))
                    {
                        fifthBot.PlayerChips += int.Parse(textBoxPot.Text);
                        //await Finish(1);
                        //b5Panel.Visible = true;
                    }
                }
            }
        }
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

                double playerPower = player.PlayerPower;
                double firstBotPower = firstBot.PlayerPower;
                double secondBotPower = secondBot.PlayerPower;
                double thirdBotPower = thirdBot.PlayerPower;
                double forthBotPower = forthBot.PlayerPower;
                double fifthBotPower = fifthBot.PlayerPower;

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

                Winner(player.PlayerType, playerPower, "Player", player.PlayerChips, fixedLast);
                Winner(firstBot.PlayerType, firstBotPower, "Bot 1", firstBot.PlayerChips, fixedLast);
                Winner(secondBot.PlayerType, secondBotPower, "Bot 2", secondBot.PlayerChips, fixedLast);
                Winner(thirdBot.PlayerType, thirdBotPower, "Bot 3", thirdBot.PlayerChips, fixedLast);
                Winner(forthBot.PlayerType, forthBotPower, "Bot 4", forthBot.PlayerChips, fixedLast);
                Winner(fifthBot.PlayerType, fifthBotPower, "Bot 5", fifthBot.PlayerChips, fixedLast);

                restart = true;

                playerFoldedTurn = true;
                playerFoldedTurn = false;
                firstBotFoldedTurn = false;
                secondBotFoldedTurn = false;
                thirdBotFoldedTurn = false;
                forthBotFoldedTurn = false;
                fifthBotFoldedTurn = false;

                if (playerChips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        playerChips = f2.a;
                        firstBot.PlayerChips += f2.a;
                        secondBot.PlayerChips += f2.a;
                        thirdBot.PlayerChips += f2.a;
                        forthBot.PlayerChips += f2.a;
                        fifthBot.PlayerChips += f2.a;
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
                bools.Clear();
                rounds = 0;

                this.playerPower = 0; this.playerType = -1;

                PlayersPowerInitializing();

                PlayersTypeInitializing();

                ints.Clear();
                checkWinners.Clear();
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

        // Making the players panel not visible 
        private void PlayerPanelVisibility()
        {
            player.PlayerPanel.Visible = false;
            firstBot.PlayerPanel.Visible = false;
            secondBot.PlayerPanel.Visible = false;
            thirdBot.PlayerPanel.Visible = false;
            forthBot.PlayerPanel.Visible = false;
            fifthBot.PlayerPanel.Visible = false;
        }

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
        async Task AllIn()
        {
            #region All in
            if (player.PlayerChips <= 0 && !intsAdded)
            {
                if (playerStatus.Text.Contains("raise"))
                {
                    ints.Add(playerChips);
                    intsAdded = true;
                }
                if (playerStatus.Text.Contains("Call"))
                {
                    ints.Add(playerChips);
                    intsAdded = true;
                }
            }
            intsAdded = false;

            if (firstBot.PlayerChips <= 0 && !firstBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    ints.Add(firstBot.PlayerChips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (secondBot.PlayerChips <= 0 && !secondBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    ints.Add(secondBot.PlayerChips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (thirdBot.PlayerChips <= 0 && !thirdBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    ints.Add(thirdBot.PlayerChips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (forthBot.PlayerChips <= 0 && !forthBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    ints.Add(forthBot.PlayerChips);
                    intsAdded = true;
                }
                intsAdded = false;
            }
            if (fifthBot.PlayerChips <= 0 && !fifthBotFoldedTurn)
            {
                if (!intsAdded)
                {
                    ints.Add(fifthBot.PlayerChips);
                    intsAdded = true;
                }
            }
            if (ints.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                ints.Clear();
            }
            #endregion

            var abc = bools.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = bools.IndexOf(false);
                if (index == 0)
                {
                    playerChips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = playerChips.ToString();
                    playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    firstBot.PlayerChips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = firstBot.PlayerChips.ToString();
                    firstBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    secondBot.PlayerChips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = secondBot.PlayerChips.ToString();
                    secondBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    thirdBot.PlayerChips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = thirdBot.PlayerChips.ToString();
                    thirdBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    forthBot.PlayerChips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = forthBot.PlayerChips.ToString();
                    forthBot.PlayerPanel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    fifthBot.PlayerChips += int.Parse(textBoxPot.Text);
                    textBoxPlayerChips.Text = fifthBot.PlayerChips.ToString();
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


        }
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

            bools.Clear();
            checkWinners.Clear();
            ints.Clear();
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

            if (playerChips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    playerChips = f2.a;
                    firstBot.PlayerChips += f2.a;
                    secondBot.PlayerChips += f2.a;
                    thirdBot.PlayerChips += f2.a;
                    forthBot.PlayerChips += f2.a;
                    fifthBot.PlayerChips += f2.a;
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

        private void PlayersPowerInitializing()
        {
            player.PlayerPower = 0;
            firstBot.PlayerPower = 0;
            secondBot.PlayerPower = 0;
            thirdBot.PlayerPower = 0;
            forthBot.PlayerPower = 0;
            fifthBot.PlayerPower = 0;
        }

        private void NotBotTurn()
        {
            firstBotTurn = false;
            secondBotTurn = false;
            thirdBotTurn = false;
            forthBotTurn = false;
            fifthBotTurn = false;
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

        //Initializing players call
        private void PlayerCallInitializing()
        {
            player.PlayerCall = 0;
            firstBot.PlayerCall = 0;
            secondBot.PlayerCall = 0;
            thirdBot.PlayerCall = 0;
            forthBot.PlayerCall = 0;
            fifthBot.PlayerCall = 0;
        }

        //Initializing players raise
        private void PlayerRaiseInitializing()
        {
            player.PlayerRaise = 0;
            firstBot.PlayerRaise = 0;
            secondBot.PlayerRaise = 0;
            thirdBot.PlayerRaise = 0;
            forthBot.PlayerRaise = 0;
            fifthBot.PlayerRaise = 0;
        }

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

            double playerPower = player.PlayerPower;
            double firstBotPower = firstBot.PlayerPower;
            double secondBotPower = secondBot.PlayerPower;
            double thirdBotPower = thirdBot.PlayerPower;
            double forthBotPower = forthBot.PlayerPower;
            double fifthBotPower = fifthBot.PlayerPower;

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

            Winner(player.PlayerType, player.PlayerPower, "Player", player.PlayerChips, fixedLast);
            Winner(firstBot.PlayerType, firstBotPower, "Bot 1", firstBot.PlayerChips, fixedLast);
            Winner(secondBot.PlayerType, secondBotPower, "Bot 2", secondBot.PlayerChips, fixedLast);
            Winner(thirdBot.PlayerType, thirdBotPower, "Bot 3", thirdBot.PlayerChips, fixedLast);
            Winner(forthBot.PlayerType, forthBotPower, "Bot 4", forthBot.PlayerChips, fixedLast);
            Winner(fifthBot.PlayerType, fifthBotPower, "Bot 5", fifthBot.PlayerChips, fixedLast);
        }
        void AI(int c1, int c2, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }
            if (sFTurn)
            {
                Holder[c1].Visible = false;
                Holder[c2].Visible = false;
            }
        }
        private void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 20, 25);
        }
        private void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }
        private void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }
        private void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }
        private void ThreeOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
        }
        private void Straight(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
        }
        private void Flush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fCall, fRaise);
        }
        private void FullHouse(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
        }
        private void FourOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fkCall, fkRaise);
            }
        }
        private void StraightFlush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sfCall, sfRaise);
            }
        }

        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            raising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }
        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            raising = false;
        }
        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            raising = false;
            sTurn = false;
            sChips -= call;
            sStatus.Text = "Call " + call;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
        }
        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(raise);
            sStatus.Text = "raise " + raise;
            textBoxPot.Text = (int.Parse(textBoxPot.Text) + Convert.ToInt32(raise)).ToString();
            call = Convert.ToInt32(raise);
            raising = true;
            sTurn = false;
        }
        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        private void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (call <= 0)
            {
                Check(ref sTurn, sStatus);
            }
            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(sChips, n))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
                if (rnd == 2)
                {
                    if (call <= RoundN(sChips, n1))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (rnd == 3)
            {
                if (raise == 0)
                {
                    raise = call * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (raise <= RoundN(sChips, n))
                    {
                        raise = call * 2;
                        Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }
        private void PH(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (call <= 0)
                {
                    Check(ref sTurn, sStatus);
                }
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n) && call <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= RoundN(sChips, n) && raise >= (RoundN(sChips, n)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= (RoundN(sChips, n)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n - rnd) && call <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= RoundN(sChips, n - rnd) && raise >= (RoundN(sChips, n - rnd)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (raise <= (RoundN(sChips, n - rnd)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    raise = RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }
        void Smooth(ref int botChips, ref bool botTurn, ref bool botFTurn, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (call >= RoundN(botChips, n))
                {
                    if (botChips > call)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= call)
                    {
                        raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
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
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        raise = call * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }
            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }

        #region UserInterface
        // Have to make class UserInterface
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
        private void Update_Tick(object sender, object e)
        {
            if (playerChips <= 0)
            {
                textBoxPlayerChips.Text = "Player Chips: 0";
            }
            if (firstBot.PlayerChips <= 0)
            {
                textBoxFirstBotChips.Text = "1st Bot Chips: 0";
            }
            if (secondBot.PlayerChips <= 0)
            {
                textBoxSecondBotChips.Text = "2nd Bot Chips: 0";
            }
            if (thirdBot.PlayerChips <= 0)
            {
                textBoxThirdBotChips.Text = "3rd Bot Chips: 0";
            }
            if (forthBot.PlayerChips <= 0)
            {
                textBoxForthBotChips.Text = "4th Bot Chips: 0";
            }
            if (fifthBot.PlayerChips <= 0)
            {
                textBoxFifthBotChips.Text = "5th Bot Chips: 0";
            }

            textBoxPlayerChips.Text = "Player Chips: " + playerChips.ToString();
            textBoxFirstBotChips.Text = "1st Bot Chips: " + firstBot.PlayerChips.ToString();
            textBoxSecondBotChips.Text = "2nd Bot Chips: " + secondBot.PlayerChips.ToString();
            textBoxThirdBotChips.Text = "3rd Bot Chips: " + thirdBot.PlayerChips.ToString();
            textBoxForthBotChips.Text = "4th Bot Chips: " + forthBot.PlayerChips.ToString();
            textBoxFifthBotChips.Text = "5th Bot Chips: " + fifthBot.PlayerChips.ToString();

            if (playerChips <= 0)
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
            if (playerChips >= call)
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
            if (playerChips <= 0)
            {
                buttonRaise.Enabled = false;
            }
            int parsedValue;

            if (textBoxRaise.Text != "" && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (playerChips <= int.Parse(textBoxRaise.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "raise";
                }
            }
            if (playerChips < call)
            {
                buttonRaise.Enabled = false;
            }
        }
        private async void BotFold_Click(object sender, EventArgs e)
        {
            playerStatus.Text = "Fold";
            playerTurn = false;
            playerFoldedTurn = true;
            await Turns();
        }
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
        private async void BotCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldedTurn);

            if (playerChips >= call)
            {
                playerChips -= call;
                textBoxPlayerChips.Text = "playerChips : " + playerChips.ToString();
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
            else if (playerChips <= call && call > 0)
            {
                textBoxPot.Text = (int.Parse(textBoxPot.Text) + playerChips).ToString();
                playerStatus.Text = "All in " + playerChips;
                playerChips = 0;
                textBoxPlayerChips.Text = "playerChips : " + playerChips.ToString();
                playerTurn = false;
                buttonFold.Enabled = false;
                playerCall = playerChips;
            }
            await Turns();
        }
        private async void BotRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, playerFoldedTurn);

            int parsedValue;
            if (textBoxRaise.Text != "" && int.TryParse(textBoxRaise.Text, out parsedValue))
            {
                if (playerChips > call)
                {
                    if (raise * 2 > int.Parse(textBoxRaise.Text))
                    {
                        textBoxRaise.Text = (raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (playerChips >= int.Parse(textBoxRaise.Text))
                        {
                            call = int.Parse(textBoxRaise.Text);
                            raise = int.Parse(textBoxRaise.Text);
                            playerStatus.Text = "raise " + call.ToString();
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + call).ToString();
                            buttonCall.Text = "Call";
                            playerChips -= int.Parse(textBoxRaise.Text);
                            raising = true;
                            last = 0;
                            playerRaise = Convert.ToInt32(raise);
                        }
                        else
                        {
                            call = playerChips;
                            raise = playerChips;
                            textBoxPot.Text = (int.Parse(textBoxPot.Text) + playerChips).ToString();
                            playerStatus.Text = "raise " + call.ToString();
                            playerChips = 0;
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
        private void BotAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAdd.Text == "") { }
            else
            {
                playerChips += int.Parse(textBoxAdd.Text);
                firstBot.PlayerChips += int.Parse(textBoxAdd.Text);
                secondBot.PlayerChips += int.Parse(textBoxAdd.Text);
                thirdBot.PlayerChips += int.Parse(textBoxAdd.Text);
                forthBot.PlayerChips += int.Parse(textBoxAdd.Text);
                fifthBot.PlayerChips += int.Parse(textBoxAdd.Text);
            }
            textBoxPlayerChips.Text = "playerChips : " + playerChips.ToString();
        }
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
        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            width = this.Width;
            height = this.Height;
        }
        #endregion
    }
}