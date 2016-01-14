using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Poker
{
    public partial class PokerTable : Form
    {
        #region Variables
        ProgressBar progressBar = new ProgressBar();
        private const int CARDS_COUNT = 17;

        private int call = 500;
        private int foldedPlayers = 5;

        Panel playerPanel = new Panel();
        Panel firstBotPanel = new Panel();
        Panel secondBotPanel = new Panel();
        Panel thirdBotPanel = new Panel();
        Panel forthBotPanel = new Panel();
        Panel fifthBotPanel = new Panel();

        private int playerChips = 10000;
        private int firstBotChips = 10000;
        private int secondBotChips = 10000;
        private int thirdBotChips = 10000;
        private int forthBotChips = 10000;
        private int fifthBotChips = 10000;

        private double type;
        private double rounds = 0;
        private double Raise = 0;

        private double firstBotPower;
        private double secondBotPower;
        private double thirdBotPower;
        private double forthBotPower;
        private double fifthBotPower;
        private double playerPower = 0;

        private double playerType = -1;
        private double firstBotType = -1;
        private double secondBotType = -1;
        private double thirdBotType = -1;
        private double forthBotType = -1;
        private double fifthBotType = -1;

        private bool firstBotTrun = false;
        private bool secondBotTurn = false;

        private bool thirdBotTurn = false;

        private bool forthBotTurn = false;
        private bool fifthBotTurn = false;

        private bool firstBotFoldedTurn = false;
        private bool secondBotFoldedTurn = false;
        private bool thirdBotFoldedTurn = false;
        private bool forthBotFoldedTurn = false;
        private bool fifthBotFoldedTurn = false;

        private bool playerFolded;
        private bool firstBotFolded;
        private bool secondBotFolded;
        private bool thirdBotFolded;
        private bool forthBotFolded;
        private bool fifthBotFolded;
        private bool chipsAdded;
        private bool changed;

        private int playerCall = 0;
        private int firstBotCall = 0;
        private int secondBotCall = 0;
        private int thirdBotCall = 0;
        private int forthBotCall = 0;
        private int fifthBotCall = 0;

        private int playerRaise = 0;
        private int firstBotRaise = 0;
        private int secondBotRaise = 0;
        private int thirdBotRaise = 0;
        private int forthBotRaise = 0;
        private int fifthBotRaise = 0;

        private int height;
        private int width;

        private int winners = 0;

        private int Flop = 1;
        private int Turn = 2;
        private int River = 3;
        private int End = 4;
        int maxLeft = 6;

        private int lastPlayed = 123; //едва ли трябва да е 123
        private int raisedTurn = 1;

        List<bool?> bools = new List<bool?>();
        List<Type> Win = new List<Type>();
        List<string> CheckWinners = new List<string>();
        List<int> chips = new List<int>();

        private bool playerFoldedTurn = false;
        private bool playerTurn = true;
        private bool restart = false;
        bool raising = false;

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
        int[] reserve = new int[17];
        Image[] deck = new Image[52];
        PictureBox[] holder = new PictureBox[52];
        Timer timer = new Timer();
        Timer updates = new Timer();

        private int tick = 60; 

        private int i;// ???? to do

        private int bigBlind = 500;

        private int smallBlind = 250;
        //int up = 10000000; not sure if it`s needed
        int turnCount = 0;
        #endregion
        public PokerTable()
        {
            //bools.Add(playerFoldedTurn); bools.Add(firstBotFoldedTurn); bools.Add(secondBotFoldedTurn); bools.Add(thirdBotFoldedTurn); bools.Add(forthBotFoldedTurn); bools.Add(fifthBotFoldedTurn);
            call = this.bigBlind;
            MaximizeBox = false;
            MinimizeBox = false;
            this.updates.Start();
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            Shuffle();
            this.textBoxPot.Enabled = false;
            this.textBoxChips.Enabled = false;
            this.textBoxFirstBotChips.Enabled = false;
            this.textBoxSecondBoxChips.Enabled = false;
            this.textBoxThirdBotChips.Enabled = false;
            this.textBoxForthBotChips.Enabled = false;
            this.textBoxFifthBotChips.Enabled = false;
            this.textBoxChips.Text = "playerChips : " + this.playerChips.ToString();
            this.textBoxFirstBotChips.Text = "playerChips : " + this.firstBotChips.ToString();
            this.textBoxSecondBoxChips.Text = "playerChips : " + this.secondBotChips.ToString();
            this.textBoxThirdBotChips.Text = "playerChips : " + this.thirdBotChips.ToString();
            this.textBoxForthBotChips.Text = "playerChips : " + this.forthBotChips.ToString();
            this.textBoxFifthBotChips.Text = "playerChips : " + this.fifthBotChips.ToString();
            timer.Interval = (1 * 1 * 1000);
            timer.Tick += timer_Tick;
            this.updates.Interval = (1 * 1 * 100);
            this.updates.Tick += Update_Tick;
            this.textBoxBigBlind.Visible = true;
            this.textBoxSmallBlind.Visible = true;
            this.buttonBigBlind.Visible = true;
            this.buttonSmallBlind.Visible = true;
            this.textBoxBigBlind.Visible = true;
            this.textBoxSmallBlind.Visible = true;
            this.buttonBigBlind.Visible = true;
            this.buttonSmallBlind.Visible = true;
            this.textBoxBigBlind.Visible = false;
            this.textBoxSmallBlind.Visible = false;
            this.buttonBigBlind.Visible = false;
            this.buttonSmallBlind.Visible = false;
            this.textBoxRaise.Text = (this.bigBlind * 2).ToString();
        }
        async Task Shuffle()
        {
            bools.Add(this.playerFoldedTurn);
            bools.Add(this.firstBotFoldedTurn);
            bools.Add(this.secondBotFoldedTurn);
            bools.Add(this.thirdBotFoldedTurn);
            bools.Add(this.forthBotFoldedTurn);
            bools.Add(this.fifthBotFoldedTurn);
            this.buttonCall.Enabled = false;
            this.buttonRaise.Enabled = false;
            this.buttonFold.Enabled = false;
            this.buttonCheck.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");
            int horizontal = 580, vertical = 480;
            Random random = new Random();

            for ( i = ImgLocation.Length; i > 0; i--)
            {
                int j = random.Next(i);
                var k = ImgLocation[j];
                ImgLocation[j] = ImgLocation[i - 1];
                ImgLocation[i - 1] = k;
            }

            for (i = 0; i < CARDS_COUNT; i++)
            {

                this.deck[i] = Image.FromFile(ImgLocation[i]);
                var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
                foreach (var ch in charsToRemove)
                {
                    ImgLocation[i] = ImgLocation[i].Replace(ch, string.Empty);
                }
                this.reserve[i] = int.Parse(ImgLocation[i]) - 1;
                this.holder[i] = new PictureBox();
                this.holder[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.holder[i].Height = 130;
                this.holder[i].Width = 80;
                this.Controls.Add(this.holder[i]);
                this.holder[i].Name = "pb" + i.ToString();
                await Task.Delay(200);
                #region Throwing Cards
                if (i < 2)
                {
                    if (this.holder[0].Tag != null)
                    {
                        this.holder[1].Tag = this.reserve[1];
                    }
                    this.holder[0].Tag = this.reserve[0];
                    this.holder[i].Image = this.deck[i];
                    this.holder[i].Anchor = (AnchorStyles.Bottom);
                    //holder[i].Dock = DockStyle.Top;
                    this.holder[i].Location = new Point(horizontal, vertical);
                    horizontal += this.holder[i].Width;
                    this.Controls.Add(this.playerPanel);
                    this.playerPanel.Location = new Point(this.holder[0].Left - 10, this.holder[0].Top - 10);
                    this.playerPanel.BackColor = Color.DarkBlue;
                    this.playerPanel.Height = 150;
                    this.playerPanel.Width = 180;
                    this.playerPanel.Visible = false;
                }
                if (this.firstBotChips > 0)
                {
                    foldedPlayers--;
                    if (i >= 2 && i < 4)
                    {
                        if (this.holder[2].Tag != null)
                        {
                            this.holder[3].Tag = this.reserve[3];
                        }
                        this.holder[2].Tag = this.reserve[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }
                        check = true;
                        this.holder[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        this.holder[i].Image = backImage;
                        //holder[i].Image = deck[i];
                        this.holder[i].Location = new Point(horizontal, vertical);
                        horizontal += this.holder[i].Width;
                        this.holder[i].Visible = true;
                        this.Controls.Add(this.firstBotPanel);
                        this.firstBotPanel.Location = new Point(this.holder[2].Left - 10, this.holder[2].Top - 10);
                        this.firstBotPanel.BackColor = Color.DarkBlue;
                        this.firstBotPanel.Height = 150;
                        this.firstBotPanel.Width = 180;
                        this.firstBotPanel.Visible = false;
                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }
                if (this.secondBotChips > 0)
                {
                    foldedPlayers--;
                    if (i >= 4 && i < 6)
                    {
                        if (this.holder[4].Tag != null)
                        {
                            this.holder[5].Tag = this.reserve[5];
                        }
                        this.holder[4].Tag = this.reserve[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }
                        check = true;
                        this.holder[i].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        this.holder[i].Image = backImage;
                        //holder[i].Image = deck[i];
                        this.holder[i].Location = new Point(horizontal, vertical);
                        horizontal += this.holder[i].Width;
                        this.holder[i].Visible = true;
                        this.Controls.Add(this.secondBotPanel);
                        this.secondBotPanel.Location = new Point(this.holder[4].Left - 10, this.holder[4].Top - 10);
                        this.secondBotPanel.BackColor = Color.DarkBlue;
                        this.secondBotPanel.Height = 150;
                        this.secondBotPanel.Width = 180;
                        this.secondBotPanel.Visible = false;
                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }
                if (this.thirdBotChips > 0)
                {
                    foldedPlayers--;
                    if (i >= 6 && i < 8)
                    {
                        if (this.holder[6].Tag != null)
                        {
                            this.holder[7].Tag = this.reserve[7];
                        }
                        this.holder[6].Tag = this.reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }
                        check = true;
                        this.holder[i].Anchor = (AnchorStyles.Top);
                        this.holder[i].Image = backImage;
                        //holder[card].Image = deck[card];
                        this.holder[i].Location = new Point(horizontal, vertical);
                        horizontal += this.holder[i].Width;
                        this.holder[i].Visible = true;
                        this.Controls.Add(this.thirdBotPanel);
                        this.thirdBotPanel.Location = new Point(this.holder[6].Left - 10, this.holder[6].Top - 10);
                        this.thirdBotPanel.BackColor = Color.DarkBlue;
                        this.thirdBotPanel.Height = 150;
                        this.thirdBotPanel.Width = 180;
                        this.thirdBotPanel.Visible = false;
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }
                if (this.forthBotChips > 0)
                {
                    foldedPlayers--;
                    if (i >= 8 && i < 10)
                    {
                        if (this.holder[8].Tag != null)
                        {
                            this.holder[9].Tag = this.reserve[9];
                        }
                        this.holder[8].Tag = this.reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }
                        check = true;
                        this.holder[i].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        this.holder[i].Image = backImage;
                        //holder[i].Image = deck[i];
                        this.holder[i].Location = new Point(horizontal, vertical);
                        horizontal += this.holder[i].Width;
                        this.holder[i].Visible = true;
                        this.Controls.Add(this.forthBotPanel);
                        this.forthBotPanel.Location = new Point(this.holder[8].Left - 10, this.holder[8].Top - 10);
                        this.forthBotPanel.BackColor = Color.DarkBlue;
                        this.forthBotPanel.Height = 150;
                        this.forthBotPanel.Width = 180;
                        this.forthBotPanel.Visible = false;
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }
                if (this.fifthBotChips > 0)
                {
                    foldedPlayers--;
                    if (i >= 10 && i < 12)
                    {
                        if (this.holder[10].Tag != null)
                        {
                            this.holder[11].Tag = this.reserve[11];
                        }
                        this.holder[10].Tag = this.reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }
                        check = true;
                        this.holder[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        this.holder[i].Image = backImage;
                        //holder[i].Image = deck[i];
                        this.holder[i].Location = new Point(horizontal, vertical);
                        horizontal += this.holder[i].Width;
                        this.holder[i].Visible = true;
                        this.Controls.Add(this.fifthBotPanel);
                        this.fifthBotPanel.Location = new Point(this.holder[10].Left - 10, this.holder[10].Top - 10);
                        this.fifthBotPanel.BackColor = Color.DarkBlue;
                        this.fifthBotPanel.Height = 150;
                        this.fifthBotPanel.Width = 180;
                        this.fifthBotPanel.Visible = false;
                        if (i == 11)
                        {
                            check = false;
                        }
                    }
                }
                if (i >= 12)
                {
                    this.holder[12].Tag = this.reserve[12];
                    if (i > 12) this.holder[13].Tag = this.reserve[13];
                    if (i > 13) this.holder[14].Tag = this.reserve[14];
                    if (i > 14) this.holder[15].Tag = this.reserve[15];
                    if (i > 15)
                    {
                        this.holder[16].Tag = this.reserve[16];

                    }
                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }
                    check = true;
                    if (this.holder[i] != null)
                    {
                        this.holder[i].Anchor = AnchorStyles.None;
                        this.holder[i].Image = backImage;
                        //holder[i].Image = deck[i];
                        this.holder[i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
                #endregion
                if (this.firstBotChips <= 0)
                {
                    this.firstBotFoldedTurn = true;
                    this.holder[2].Visible = false;
                    this.holder[3].Visible = false;
                }
                else
                {
                    this.firstBotFoldedTurn = false;
                    if (i == 3)
                    {
                        if (this.holder[3] != null)
                        {
                            this.holder[2].Visible = true;
                            this.holder[3].Visible = true;
                        }
                    }
                }
                if (this.secondBotChips <= 0)
                {
                    this.secondBotFoldedTurn = true;
                    this.holder[4].Visible = false;
                    this.holder[5].Visible = false;
                }
                else
                {
                    this.secondBotFoldedTurn = false;
                    if (i == 5)
                    {
                        if (this.holder[5] != null)
                        {
                            this.holder[4].Visible = true;
                            this.holder[5].Visible = true;
                        }
                    }
                }
                if (this.thirdBotChips <= 0)
                {
                    this.thirdBotFoldedTurn = true;
                    this.holder[6].Visible = false;
                    this.holder[7].Visible = false;
                }
                else
                {
                    this.thirdBotFoldedTurn = false;
                    if (i == 7)
                    {
                        if (this.holder[7] != null)
                        {
                            this.holder[6].Visible = true;
                            this.holder[7].Visible = true;
                        }
                    }
                }
                if (this.forthBotChips <= 0)
                {
                    this.forthBotFoldedTurn = true;
                    this.holder[8].Visible = false;
                    this.holder[9].Visible = false;
                }
                else
                {
                    this.forthBotFoldedTurn = false;
                    if (i == 9)
                    {
                        if (this.holder[9] != null)
                        {
                            this.holder[8].Visible = true;
                            this.holder[9].Visible = true;
                        }
                    }
                }
                if (this.fifthBotChips <= 0)
                {
                    this.fifthBotFoldedTurn = true;
                    this.holder[10].Visible = false;
                    this.holder[11].Visible = false;
                }
                else
                {
                    this.fifthBotFoldedTurn = false;
                    if (i == 11)
                    {
                        if (this.holder[11] != null)
                        {
                            this.holder[10].Visible = true;
                            this.holder[11].Visible = true;
                        }
                    }
                }
                if (i == 16)
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
            if (i == 17)
            {
                this.buttonRaise.Enabled = true;
                this.buttonCall.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonFold.Enabled = true;
            }
        }
        async Task Turns()
        {
            #region Rotating
            if (!this.playerFoldedTurn)
            {
                if (this.playerTurn)
                {
                    FixCall(this.playerStatus, ref this.playerCall, ref this.playerRaise, 1);
                    //MessageBox.Show("Player's Turn");
                    pbTimer.Visible = true;
                    pbTimer.Value = 1000;
                     this.tick = 60;
                    //up = 10000000;
                    timer.Start();
                    this.buttonRaise.Enabled = true;
                    this.buttonCall.Enabled = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonFold.Enabled = true;
                    turnCount++;
                    FixCall(this.playerStatus, ref this.playerCall, ref this.playerRaise, 2);
                }
            }
            if (this.playerFoldedTurn || !this.playerTurn)
            {
                await AllIn();
                if (this.playerFoldedTurn && !this.playerFolded)
                {
                    if (this.buttonCall.Text.Contains("All in") == false || this.buttonRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        this.playerFolded = true;
                    }
                }
                await CheckRaise(0, 0);
                pbTimer.Visible = false;
                this.buttonRaise.Enabled = false;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                timer.Stop();
                this.firstBotTrun = true;
                if (!this.firstBotFoldedTurn)
                {
                    if (this.firstBotTrun)
                    {
                        FixCall(this.firstBotStatus, ref this.firstBotCall, ref this.firstBotRaise, 1);
                        FixCall(this.firstBotStatus, ref this.firstBotCall, ref this.firstBotRaise, 2);
                        Rules(2, 3, "Bot 1", ref firstBotType, ref this.firstBotPower, this.firstBotFoldedTurn);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, ref this.firstBotChips, ref this.firstBotTrun, ref  this.firstBotFoldedTurn, this.firstBotStatus, 0, this.firstBotPower, firstBotType);
                        turnCount++;
                        this.lastPlayed = 1;
                        this.firstBotTrun = false;
                        this.secondBotTurn = true;
                    }
                }
                if (this.firstBotFoldedTurn && !this.firstBotFolded)
                {
                    bools.RemoveAt(1);
                    bools.Insert(1, null);
                    maxLeft--;
                    this.firstBotFolded = true;
                }
                if (this.firstBotFoldedTurn || !this.firstBotTrun)
                {
                    await CheckRaise(1, 1);
                    this.secondBotTurn = true;
                }
                if (!this.secondBotFoldedTurn)
                {
                    if (this.secondBotTurn)
                    {
                        FixCall(this.secondBotStatus, ref this.secondBotCall, ref this.secondBotRaise, 1);
                        FixCall(this.secondBotStatus, ref this.secondBotCall, ref this.secondBotRaise, 2);
                        Rules(4, 5, "Bot 2", ref this.secondBotType, ref this.secondBotPower, this.secondBotFoldedTurn);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, ref this.secondBotChips, ref this.secondBotTurn, ref  this.secondBotFoldedTurn, this.secondBotStatus, 1, this.secondBotPower, this.secondBotType);
                        turnCount++;
                        this.lastPlayed = 2;
                        this.secondBotTurn = false;
                        thirdBotTurn = true;
                    }
                }
                if (this.secondBotFoldedTurn && !this.secondBotFolded)
                {
                    bools.RemoveAt(2);
                    bools.Insert(2, null);
                    maxLeft--;
                    this.secondBotFolded = true;
                }
                if (this.secondBotFoldedTurn || !this.secondBotTurn)
                {
                    await CheckRaise(2, 2);
                    thirdBotTurn = true;
                }
                if (!this.thirdBotFoldedTurn)
                {
                    if (thirdBotTurn)
                    {
                        FixCall(this.thirdBotStatus, ref this.thirdBotCall, ref this.thirdBotRaise, 1);
                        FixCall(this.thirdBotStatus, ref this.thirdBotCall, ref this.thirdBotRaise, 2);
                        Rules(6, 7, "Bot 3", ref this.thirdBotType, ref this.thirdBotPower, this.thirdBotFoldedTurn);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, ref this.thirdBotChips, ref thirdBotTurn, ref  this.thirdBotFoldedTurn, this.thirdBotStatus, 2, this.thirdBotPower, this.thirdBotType);
                        turnCount++;
                        this.lastPlayed = 3;
                        thirdBotTurn = false;
                        forthBotTurn = true;
                    }
                }
                if (this.thirdBotFoldedTurn && !this.thirdBotFolded)
                {
                    bools.RemoveAt(3);
                    bools.Insert(3, null);
                    maxLeft--;
                    this.thirdBotFolded = true;
                }
                if (this.thirdBotFoldedTurn || !thirdBotTurn)
                {
                    await CheckRaise(3, 3);
                    forthBotTurn = true;
                }
                if (!this.forthBotFoldedTurn)
                {
                    if (forthBotTurn)
                    {
                        FixCall(this.forthBotStatus, ref this.forthBotCall, ref this.forthBotRaise, 1);
                        FixCall(this.forthBotStatus, ref this.forthBotCall, ref this.forthBotRaise, 2);
                        Rules(8, 9, "Bot 4", ref this.forthBotType, ref this.forthBotPower, this.forthBotFoldedTurn);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, ref this.forthBotChips, ref forthBotTurn, ref  this.forthBotFoldedTurn, this.forthBotStatus, 3, this.forthBotPower, this.forthBotType);
                        turnCount++;
                        this.lastPlayed = 4;
                        forthBotTurn = false;
                        this.fifthBotTurn = true;
                    }
                }
                if (this.forthBotFoldedTurn && !this.forthBotFolded)
                {
                    bools.RemoveAt(4);
                    bools.Insert(4, null);
                    maxLeft--;
                    this.forthBotFolded = true;
                }
                if (this.forthBotFoldedTurn || !forthBotTurn)
                {
                    await CheckRaise(4, 4);
                    this.fifthBotTurn = true;
                }
                if (!this.fifthBotFoldedTurn)
                {
                    if (this.fifthBotTurn)
                    {
                        FixCall(this.fifthBotStatus, ref this.fifthBotCall, ref this.fifthBotRaise, 1);
                        FixCall(this.fifthBotStatus, ref this.fifthBotCall, ref this.fifthBotRaise, 2);
                        Rules(10, 11, "Bot 5", ref this.fifthBotType, ref this.fifthBotPower, this.fifthBotFoldedTurn);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, ref this.fifthBotChips, ref this.fifthBotTurn, ref  this.fifthBotFoldedTurn, this.fifthBotStatus, 4, this.fifthBotPower, this.fifthBotType);
                        turnCount++;
                        this.lastPlayed = 5;
                        this.fifthBotTurn = false;
                    }
                }
                if (this.fifthBotFoldedTurn && !this.fifthBotFolded)
                {
                    bools.RemoveAt(5);
                    bools.Insert(5, null);
                    maxLeft--;
                    this.fifthBotFolded = true;
                }
                if (this.fifthBotFoldedTurn || !this.fifthBotTurn)
                {
                    await CheckRaise(5, 5);
                    this.playerTurn = true;
                }
                if (this.playerFoldedTurn && !this.playerFolded)
                {
                    if (this.buttonCall.Text.Contains("All in") == false || this.buttonRaise.Text.Contains("All in") == false)
                    {
                        bools.RemoveAt(0);
                        bools.Insert(0, null);
                        maxLeft--;
                        this.playerFolded = true;
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

        void Rules(int c1, int c2, string currentText, ref double current, ref double Power, bool foldedTurn)
        {
            if (c1 == 0 && c2 == 1)
            {
            }
            if (!foldedTurn || c1 == 0 && c2 == 1 && this.playerStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = this.reserve[c1];
                Straight[1] = this.reserve[c2];
                Straight1[0] = Straight[2] = this.reserve[12];
                Straight1[1] = Straight[3] = this.reserve[13];
                Straight1[2] = Straight[4] = this.reserve[14];
                Straight1[3] = Straight[5] = this.reserve[15];
                Straight1[4] = Straight[6] = this.reserve[16];
                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(Straight); Array.Sort(st1); Array.Sort(st2); Array.Sort(st3); Array.Sort(st4);
                #endregion
                for (i = 0; i < 16; i++)
                {
                    if (this.reserve[i] == int.Parse(this.holder[c1].Tag.ToString()) && this.reserve[i + 1] == int.Parse(this.holder[c2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1

                        rPairFromHand(ref current, ref Power);

                        #region Pair or Two Pair from Table current = 2 || 0
                        rPairTwoPair(ref current, ref Power);
                        #endregion

                        #region Two Pair current = 2
                        rTwoPair(ref current, ref Power);
                        #endregion

                        #region Three of a kind current = 3
                        rThreeOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight current = 4
                        rStraight(ref current, ref Power, Straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        rFlush(ref current, ref Power, ref vf, Straight1);
                        #endregion

                        #region Full House current = 6
                        rFullHouse(ref current, ref Power, ref done, Straight);
                        #endregion

                        #region Four of a Kind current = 7
                        rFourOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        rStraightFlush(ref current, ref Power, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        rHighCard(ref current, ref Power);
                        #endregion
                    }
                }
            }
        }
        private void rStraightFlush(ref double current, ref double Power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        Power = (st1.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        Power = (st1.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        Power = (st2.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        Power = (st2.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        Power = (st3.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        Power = (st3.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        Power = (st4.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        Power = (st4.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rFourOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        current = 7;
                        Power = (Straight[j] / 4) * 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        Power = 13 * 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rFullHouse(ref double current, ref double Power, ref bool done, int[] Straight)
        {
            if (current >= -1)
            {
                type = Power;
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
                                Power = 13 * 2 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                Power = fh.Max() / 4 * 2 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 6 });
                                sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }
                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }
                if (current != 6)
                {
                    Power = type;
                }
            }
        }
        private void rFlush(ref double current, ref double Power, ref bool vf, int[] Straight1)
        {
            if (current >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f1[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reserve[i] / 4 < f1.Max() / 4 && this.reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 4)//different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f1[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 && this.reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (this.reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 5)
                {
                    if (this.reserve[i] % 4 == f1[0] % 4 && this.reserve[i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reserve[i + 1] % 4 == f1[0] % 4 && this.reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reserve[i] / 4 < f1.Min() / 4 && this.reserve[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        Power = f1.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f2[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reserve[i] / 4 < f2.Max() / 4 && this.reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f2[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 && this.reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (this.reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (this.reserve[i] % 4 == f2[0] % 4 && this.reserve[i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reserve[i + 1] % 4 == f2[0] % 4 && this.reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reserve[i] / 4 < f2.Min() / 4 && this.reserve[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        Power = f2.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f3[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reserve[i] / 4 < f3.Max() / 4 && this.reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f3[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 && this.reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (this.reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (this.reserve[i] % 4 == f3[0] % 4 && this.reserve[i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reserve[i + 1] % 4 == f3[0] % 4 && this.reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reserve[i] / 4 < f3.Min() / 4 && this.reserve[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        Power = f3.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (this.reserve[i] % 4 == this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f4[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reserve[i] / 4 < f4.Max() / 4 && this.reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (this.reserve[i] % 4 != this.reserve[i + 1] % 4 && this.reserve[i] % 4 == f4[0] % 4)
                    {
                        if (this.reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (this.reserve[i + 1] % 4 != this.reserve[i] % 4 && this.reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (this.reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reserve[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (this.reserve[i] % 4 == f4[0] % 4 && this.reserve[i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reserve[i + 1] % 4 == f4[0] % 4 && this.reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = this.reserve[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reserve[i] / 4 < f4.Min() / 4 && this.reserve[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        Power = f4.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (this.reserve[i] / 4 == 0 && this.reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reserve[i + 1] / 4 == 0 && this.reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (this.reserve[i] / 4 == 0 && this.reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reserve[i + 1] / 4 == 0 && this.reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (this.reserve[i] / 4 == 0 && this.reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reserve[i + 1] / 4 == 0 && this.reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (this.reserve[i] / 4 == 0 && this.reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reserve[i + 1] / 4 == 0 && this.reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rStraight(ref double current, ref double Power, int[] Straight)
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
                            Power = op.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            Power = op[j + 4] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }
                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 4 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rThreeOfAKind(ref double current, ref double Power, int[] Straight)
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
                            Power = 13 * 3 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }
        private void rTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (this.reserve[i] / 4 != this.reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (this.reserve[i] / 4 == this.reserve[tc] / 4 && this.reserve[i + 1] / 4 == this.reserve[tc - k] / 4 ||
                                    this.reserve[i + 1] / 4 == this.reserve[tc] / 4 && this.reserve[i] / 4 == this.reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (this.reserve[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (this.reserve[i] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reserve[i + 1] / 4 != 0 && this.reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.reserve[i] / 4) * 2 + (this.reserve[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
        private void rPairTwoPair(ref double current, ref double Power)
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
                            if (this.reserve[tc] / 4 == this.reserve[tc - k] / 4)
                            {
                                if (this.reserve[tc] / 4 != this.reserve[i] / 4 && this.reserve[tc] / 4 != this.reserve[i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (this.reserve[i] / 4) * 2 + 13 * 4 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (this.reserve[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reserve[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.reserve[tc] / 4) * 2 + (this.reserve[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.reserve[tc] / 4) * 2 + (this.reserve[i] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (this.reserve[i] / 4 > this.reserve[i + 1] / 4)
                                        {
                                            if (this.reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + this.reserve[i] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = this.reserve[tc] / 4 + this.reserve[i] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (this.reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + this.reserve[i + 1] + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = this.reserve[tc] / 4 + this.reserve[i + 1] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
        private void rPairFromHand(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (this.reserve[i] / 4 == this.reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (this.reserve[i] / 4 == 0)
                        {
                            current = 1;
                            Power = 13 * 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            Power = (this.reserve[i + 1] / 4) * 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (this.reserve[i + 1] / 4 == this.reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reserve[i + 1] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + this.reserve[i] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (this.reserve[i + 1] / 4) * 4 + this.reserve[i] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                    if (this.reserve[i] / 4 == this.reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reserve[i] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + this.reserve[i + 1] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (this.reserve[tc] / 4) * 4 + this.reserve[i + 1] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }
        private void rHighCard(ref double current, ref double Power)
        {
            if (current == -1)
            {
                if (this.reserve[i] / 4 > this.reserve[i + 1] / 4)
                {
                    current = -1;
                    Power = this.reserve[i] / 4;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    Power = this.reserve[i + 1] / 4;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (this.reserve[i] / 4 == 0 || this.reserve[i + 1] / 4 == 0)
                {
                    current = -1;
                    Power = 13;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(double current, double Power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (this.holder[j].Visible)
                    this.holder[j].Image = this.deck[j];
            }
            if (current == sorted.Current)
            {
                if (Power == sorted.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
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
                    if (CheckWinners.Contains("Player"))
                    {
                        this.playerChips += int.Parse(this.textBoxPot.Text) / winners;
                        this.textBoxChips.Text = this.playerChips.ToString();
                        //playerPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBotChips += int.Parse(this.textBoxPot.Text) / winners;
                        this.textBoxFirstBotChips.Text = this.firstBotChips.ToString();
                        //firstBotPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBotChips += int.Parse(this.textBoxPot.Text) / winners;
                        this.textBoxSecondBoxChips.Text = this.secondBotChips.ToString();
                        //secondBotPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBotChips += int.Parse(this.textBoxPot.Text) / winners;
                        this.textBoxThirdBotChips.Text = this.thirdBotChips.ToString();
                        //thirdBotPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        this.forthBotChips += int.Parse(this.textBoxPot.Text) / winners;
                        this.textBoxForthBotChips.Text = this.forthBotChips.ToString();
                        //forthBotPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBotChips += int.Parse(this.textBoxPot.Text) / winners;
                        this.textBoxFifthBotChips.Text = this.fifthBotChips.ToString();
                        //fifthBotPanel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        this.playerChips += int.Parse(this.textBoxPot.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBotChips += int.Parse(this.textBoxPot.Text);
                        //await Finish(1);
                        //firstBotPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBotChips += int.Parse(this.textBoxPot.Text);
                        //await Finish(1);
                        //secondBotPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBotChips += int.Parse(this.textBoxPot.Text);
                        //await Finish(1);
                        //thirdBotPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        this.forthBotChips += int.Parse(this.textBoxPot.Text);
                        //await Finish(1);
                        //forthBotPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBotChips += int.Parse(this.textBoxPot.Text);
                        //await Finish(1);
                        //fifthBotPanel.Visible = true;
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
                        Raise = 0;
                        call = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!this.playerFoldedTurn)
                            this.playerStatus.Text = "";
                        if (!this.firstBotFoldedTurn)
                            this.firstBotStatus.Text = "";
                        if (!this.secondBotFoldedTurn)
                            this.secondBotStatus.Text = "";
                        if (!this.thirdBotFoldedTurn)
                            this.thirdBotStatus.Text = "";
                        if (!this.forthBotFoldedTurn)
                            this.forthBotStatus.Text = "";
                        if (!this.fifthBotFoldedTurn)
                            this.fifthBotStatus.Text = "";
                    }
                }
            }
            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.holder[j].Image != this.deck[j])
                    {
                        this.holder[j].Image = this.deck[j];
                        this.playerCall = 0; this.playerRaise = 0;
                        this.firstBotCall = 0; this.firstBotRaise = 0;
                        this.secondBotCall = 0; this.secondBotRaise = 0;
                        this.thirdBotCall = 0; this.thirdBotRaise = 0;
                        this.forthBotCall = 0; this.forthBotRaise = 0;
                        this.fifthBotCall = 0; this.fifthBotRaise = 0;
                    }
                }
            }
            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.holder[j].Image != this.deck[j])
                    {
                        this.holder[j].Image = this.deck[j];
                        this.playerCall = 0; this.playerRaise = 0;
                        this.firstBotCall = 0; this.firstBotRaise = 0;
                        this.secondBotCall = 0; this.secondBotRaise = 0;
                        this.thirdBotCall = 0; this.thirdBotRaise = 0;
                        this.forthBotCall = 0; this.forthBotRaise = 0;
                        this.fifthBotCall = 0; this.fifthBotRaise = 0;
                    }
                }
            }
            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.holder[j].Image != this.deck[j])
                    {
                        this.holder[j].Image = this.deck[j];
                        this.playerCall = 0; this.playerRaise = 0;
                        this.firstBotCall = 0; this.firstBotRaise = 0;
                        this.secondBotCall = 0; this.secondBotRaise = 0;
                        this.thirdBotCall = 0; this.thirdBotRaise = 0;
                        this.forthBotCall = 0; this.forthBotRaise = 0;
                        this.fifthBotCall = 0; this.fifthBotRaise = 0;
                    }
                }
            }
            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!this.playerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref this.playerType, ref this.playerPower, this.playerFoldedTurn);
                }
                if (!this.firstBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref firstBotType, ref this.firstBotPower, this.firstBotFoldedTurn);
                }
                if (!this.secondBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref this.secondBotType, ref this.secondBotPower, this.secondBotFoldedTurn);
                }
                if (!this.thirdBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref this.thirdBotType, ref this.thirdBotPower, this.thirdBotFoldedTurn);
                }
                if (!this.forthBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref this.forthBotType, ref this.forthBotPower, this.forthBotFoldedTurn);
                }
                if (!this.fifthBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref this.fifthBotType, ref this.fifthBotPower, this.fifthBotFoldedTurn);
                }
                Winner(this.playerType, this.playerPower, "Player", this.playerChips, fixedLast);
                Winner(firstBotType, this.firstBotPower, "Bot 1", this.firstBotChips, fixedLast);
                Winner(this.secondBotType, this.secondBotPower, "Bot 2", this.secondBotChips, fixedLast);
                Winner(this.thirdBotType, this.thirdBotPower, "Bot 3", this.thirdBotChips, fixedLast);
                Winner(this.forthBotType, this.forthBotPower, "Bot 4", this.forthBotChips, fixedLast);
                Winner(this.fifthBotType, this.fifthBotPower, "Bot 5", this.fifthBotChips, fixedLast);
                restart = true;
                this.playerTurn = true;
                this.playerFoldedTurn = false;
                this.firstBotFoldedTurn = false;
                this.secondBotFoldedTurn = false;
                this.thirdBotFoldedTurn = false;
                this.forthBotFoldedTurn = false;
                this.fifthBotFoldedTurn = false;
                if (this.playerChips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        this.playerChips = f2.a;
                        this.firstBotChips += f2.a;
                        this.secondBotChips += f2.a;
                        this.thirdBotChips += f2.a;
                        this.forthBotChips += f2.a;
                        this.fifthBotChips += f2.a;
                        this.playerFoldedTurn = false;
                        this.playerTurn = true;
                        this.buttonRaise.Enabled = true;
                        this.buttonFold.Enabled = true;
                        this.buttonCheck.Enabled = true;
                        this.buttonRaise.Text = "Raise";
                    }
                }
                this.playerPanel.Visible = false; this.firstBotPanel.Visible = false; this.secondBotPanel.Visible = false; this.thirdBotPanel.Visible = false; this.forthBotPanel.Visible = false; this.fifthBotPanel.Visible = false;
                this.playerCall = 0; this.playerRaise = 0;
                this.firstBotCall = 0; this.firstBotRaise = 0;
                this.secondBotCall = 0; this.secondBotRaise = 0;
                this.thirdBotCall = 0; this.thirdBotRaise = 0;
                this.forthBotCall = 0; this.forthBotRaise = 0;
                this.fifthBotCall = 0; this.fifthBotRaise = 0;
                this.lastPlayed = 0;
                call = this.bigBlind;
                Raise = 0;
                ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bools.Clear();
                rounds = 0;
                this.playerPower = 0; this.playerType = -1;
                type = 0; this.firstBotPower = 0; this.secondBotPower = 0; this.thirdBotPower = 0; this.forthBotPower = 0; this.fifthBotPower = 0;
                firstBotType = -1; this.secondBotType = -1; this.thirdBotType = -1; this.forthBotType = -1; this.fifthBotType = -1;
                this.chips.Clear();
                CheckWinners.Clear();
                winners = 0;
                Win.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    this.holder[os].Image = null;
                    this.holder[os].Invalidate();
                    this.holder[os].Visible = false;
                }
                this.textBoxPot.Text = "0";
                this.playerStatus.Text = "";
                await Shuffle();
                await Turns();
            }
        }
        void FixCall(Label status, ref int cCall, ref int cRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }
                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }
                    if (status.Text.Contains("Check"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cRaise != Raise && cRaise <= Raise)
                    {
                        call = Convert.ToInt32(Raise) - cRaise;
                    }
                    if (cCall != call || cCall <= call)
                    {
                        call = call - cCall;
                    }
                    if (cRaise == Raise && Raise > 0)
                    {
                        call = 0;
                        this.buttonCall.Enabled = false;
                        this.buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }
        async Task AllIn()
        {
            #region All in
            if (this.playerChips <= 0 && !this.chipsAdded)
            {
                if (this.playerStatus.Text.Contains("Raise"))
                {
                    this.chips.Add(this.playerChips);
                    this.chipsAdded = true;
                }
                if (this.playerStatus.Text.Contains("Call"))
                {
                    this.chips.Add(this.playerChips);
                    this.chipsAdded = true;
                }
            }
            this.chipsAdded = false;
            if (this.firstBotChips <= 0 && !this.firstBotFoldedTurn)
            {
                if (!this.chipsAdded)
                {
                    this.chips.Add(this.firstBotChips);
                    this.chipsAdded = true;
                }
                this.chipsAdded = false;
            }
            if (this.secondBotChips <= 0 && !this.secondBotFoldedTurn)
            {
                if (!this.chipsAdded)
                {
                    this.chips.Add(this.secondBotChips);
                    this.chipsAdded = true;
                }
                this.chipsAdded = false;
            }
            if (this.thirdBotChips <= 0 && !this.thirdBotFoldedTurn)
            {
                if (!this.chipsAdded)
                {
                    this.chips.Add(this.thirdBotChips);
                    this.chipsAdded = true;
                }
                this.chipsAdded = false;
            }
            if (this.forthBotChips <= 0 && !this.forthBotFoldedTurn)
            {
                if (!this.chipsAdded)
                {
                    this.chips.Add(this.forthBotChips);
                    this.chipsAdded = true;
                }
                this.chipsAdded = false;
            }
            if (this.fifthBotChips <= 0 && !this.fifthBotFoldedTurn)
            {
                if (!this.chipsAdded)
                {
                    this.chips.Add(this.fifthBotChips);
                    this.chipsAdded = true;
                }
            }
            if (this.chips.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                this.chips.Clear();
            }
            #endregion

            var abc = bools.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = bools.IndexOf(false);
                if (index == 0)
                {
                    this.playerChips += int.Parse(this.textBoxPot.Text);
                    this.textBoxChips.Text = this.playerChips.ToString();
                    this.playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    this.firstBotChips += int.Parse(this.textBoxPot.Text);
                    this.textBoxChips.Text = this.firstBotChips.ToString();
                    this.firstBotPanel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    this.secondBotChips += int.Parse(this.textBoxPot.Text);
                    this.textBoxChips.Text = this.secondBotChips.ToString();
                    this.secondBotPanel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    this.thirdBotChips += int.Parse(this.textBoxPot.Text);
                    this.textBoxChips.Text = this.thirdBotChips.ToString();
                    this.thirdBotPanel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    this.forthBotChips += int.Parse(this.textBoxPot.Text);
                    this.textBoxChips.Text = this.forthBotChips.ToString();
                    this.forthBotPanel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    this.fifthBotChips += int.Parse(this.textBoxPot.Text);
                    this.textBoxChips.Text = this.fifthBotChips.ToString();
                    this.fifthBotPanel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    this.holder[j].Visible = false;
                }
                await Finish(1);
            }
            this.chipsAdded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= End)
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
            this.playerPanel.Visible = false; this.firstBotPanel.Visible = false; this.secondBotPanel.Visible = false; this.thirdBotPanel.Visible = false; this.forthBotPanel.Visible = false; this.fifthBotPanel.Visible = false;
            call = this.bigBlind; Raise = 0;
            foldedPlayers = 5;
            type = 0; rounds = 0; this.firstBotPower = 0; this.secondBotPower = 0; this.thirdBotPower = 0; this.forthBotPower = 0; this.fifthBotPower = 0; this.playerPower = 0; this.playerType = -1; Raise = 0;
            firstBotType = -1; this.secondBotType = -1; this.thirdBotType = -1; this.forthBotType = -1; this.fifthBotType = -1;
            this.firstBotTrun = false; this.secondBotTurn = false; thirdBotTurn = false; forthBotTurn = false; this.fifthBotTurn = false;
            this.firstBotFoldedTurn = false; this.secondBotFoldedTurn = false; this.thirdBotFoldedTurn = false; this.forthBotFoldedTurn = false; this.fifthBotFoldedTurn = false;
            this.playerFolded = false; this.firstBotFolded = false; this.secondBotFolded = false; this.thirdBotFolded = false; this.forthBotFolded = false; this.fifthBotFolded = false;
            this.playerFoldedTurn = false; this.playerTurn = true; restart = false; raising = false;
            this.playerCall = 0; this.firstBotCall = 0; this.secondBotCall = 0; this.thirdBotCall = 0; this.forthBotCall = 0; this.fifthBotCall = 0; this.playerRaise = 0; this.firstBotRaise = 0; this.secondBotRaise = 0; this.thirdBotRaise = 0; this.forthBotRaise = 0; this.fifthBotRaise = 0;
            height = 0; width = 0; winners = 0; Flop = 1; Turn = 2; River = 3; End = 4; maxLeft = 6;
            this.lastPlayed = 123; raisedTurn = 1;
            bools.Clear();
            CheckWinners.Clear();
            this.chips.Clear();
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            this.textBoxPot.Text = "0";
             this.tick = 60; 
            //up = 10000000;
            turnCount = 0;
            this.playerStatus.Text = "";
            this.firstBotStatus.Text = "";
            this.secondBotStatus.Text = "";
            this.thirdBotStatus.Text = "";
            this.forthBotStatus.Text = "";
            this.fifthBotStatus.Text = "";
            if (this.playerChips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    this.playerChips = f2.a;
                    this.firstBotChips += f2.a;
                    this.secondBotChips += f2.a;
                    this.thirdBotChips += f2.a;
                    this.forthBotChips += f2.a;
                    this.fifthBotChips += f2.a;
                    this.playerFoldedTurn = false;
                    this.playerTurn = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonFold.Enabled = true;
                    this.buttonCheck.Enabled = true;
                    this.buttonRaise.Text = "Raise";
                }
            }
            ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                this.holder[os].Image = null;
                this.holder[os].Invalidate();
                this.holder[os].Visible = false;
            }
            await Shuffle();
            //await Turns();
        }
        void FixWinners()
        {
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!this.playerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref this.playerType, ref this.playerPower, this.playerFoldedTurn);
            }
            if (!this.firstBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref firstBotType, ref this.firstBotPower, this.firstBotFoldedTurn);
            }
            if (!this.secondBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref this.secondBotType, ref this.secondBotPower, this.secondBotFoldedTurn);
            }
            if (!this.thirdBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref this.thirdBotType, ref this.thirdBotPower, this.thirdBotFoldedTurn);
            }
            if (!this.forthBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref this.forthBotType, ref this.forthBotPower, this.forthBotFoldedTurn);
            }
            if (!this.fifthBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref this.fifthBotType, ref this.fifthBotPower, this.fifthBotFoldedTurn);
            }
            Winner(this.playerType, this.playerPower, "Player", this.playerChips, fixedLast);
            Winner(firstBotType, this.firstBotPower, "Bot 1", this.firstBotChips, fixedLast);
            Winner(this.secondBotType, this.secondBotPower, "Bot 2", this.secondBotChips, fixedLast);
            Winner(this.thirdBotType, this.thirdBotPower, "Bot 3", this.thirdBotChips, fixedLast);
            Winner(this.forthBotType, this.forthBotPower, "Bot 4", this.forthBotChips, fixedLast);
            Winner(this.fifthBotType, this.fifthBotPower, "Bot 5", this.fifthBotChips, fixedLast);
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
                this.holder[c1].Visible = false;
                this.holder[c2].Visible = false;
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
            this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + call).ToString();
        }
        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(Raise);
            sStatus.Text = "Raise " + Raise;
            this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + Convert.ToInt32(Raise)).ToString();
            call = Convert.ToInt32(Raise);
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
                if (Raise == 0)
                {
                    Raise = call * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (Raise <= RoundN(sChips, n))
                    {
                        Raise = call * 2;
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
                    if (Raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n) && call <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n) && Raise >= (RoundN(sChips, n)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= (RoundN(sChips, n)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
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
                    if (Raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n - rnd) && call <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n - rnd) && Raise >= (RoundN(sChips, n - rnd)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= (RoundN(sChips, n - rnd)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
                if (call <= 0)
                {
                    Raise = RoundN(sChips, r - rnd);
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
                        this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (botChips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        Raise = call * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }
            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }

        #region UI
        private async void timer_Tick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                this.playerFoldedTurn = true;
                await Turns();
            }
            if (this.tick > 0)
            {
                this.tick--;
                pbTimer.Value = (this.tick / 6) * 100;
            }
        }
        private void Update_Tick(object sender, object e)
        {
            if (this.playerChips <= 0)
            {
                this.textBoxChips.Text = "playerChips : 0";
            }
            if (this.firstBotChips <= 0)
            {
                this.textBoxFirstBotChips.Text = "playerChips : 0";
            }
            if (this.secondBotChips <= 0)
            {
                this.textBoxSecondBoxChips.Text = "playerChips : 0";
            }
            if (this.thirdBotChips <= 0)
            {
                this.textBoxThirdBotChips.Text = "playerChips : 0";
            }
            if (this.forthBotChips <= 0)
            {
                this.textBoxForthBotChips.Text = "playerChips : 0";
            }
            if (this.fifthBotChips <= 0)
            {
                this.textBoxFifthBotChips.Text = "playerChips : 0";
            }
            this.textBoxChips.Text = "playerChips : " + this.playerChips.ToString();
            this.textBoxFirstBotChips.Text = "playerChips : " + this.firstBotChips.ToString();
            this.textBoxSecondBoxChips.Text = "playerChips : " + this.secondBotChips.ToString();
            this.textBoxThirdBotChips.Text = "playerChips : " + this.thirdBotChips.ToString();
            this.textBoxForthBotChips.Text = "playerChips : " + this.forthBotChips.ToString();
            this.textBoxFifthBotChips.Text = "playerChips : " + this.fifthBotChips.ToString();
            if (this.playerChips <= 0)
            {
                this.playerTurn = false;
                this.playerFoldedTurn = true;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                this.buttonCheck.Enabled = false;
            }
            //if (up > 0)
            //{
            //    up--;
            //}
            if (this.playerChips >= call)
            {
                this.buttonCall.Text = "Call " + call.ToString();
            }
            else
            {
                this.buttonCall.Text = "All in";
                this.buttonRaise.Enabled = false;
            }
            if (call > 0)
            {
                this.buttonCheck.Enabled = false;
            }
            if (call <= 0)
            {
                this.buttonCheck.Enabled = true;
                this.buttonCall.Text = "Call";
                this.buttonCall.Enabled = false;
            }
            if (this.playerChips <= 0)
            {
                this.buttonRaise.Enabled = false;
            }
            int parsedValue;

            if (this.textBoxRaise.Text != "" && int.TryParse(this.textBoxRaise.Text, out parsedValue))
            {
                if (this.playerChips <= int.Parse(this.textBoxRaise.Text))
                {
                    this.buttonRaise.Text = "All in";
                }
                else
                {
                    this.buttonRaise.Text = "Raise";
                }
            }
            if (this.playerChips < call)
            {
                this.buttonRaise.Enabled = false;
            }
        }
        private async void bFold_Click(object sender, EventArgs e)
        {
            this.playerStatus.Text = "Fold";
            this.playerTurn = false;
            this.playerFoldedTurn = true;
            await Turns();
        }
        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                this.playerTurn = false;
                this.playerStatus.Text = "Check";
            }
            else
            {
                //playerStatus.Text = "All in " + playerChips;

                this.buttonCheck.Enabled = false;
            }
            await Turns();
        }
        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref this.playerType, ref this.playerPower, this.playerFoldedTurn);
            if (this.playerChips >= call)
            {
                this.playerChips -= call;
                this.textBoxChips.Text = "playerChips : " + this.playerChips.ToString();
                if (this.textBoxPot.Text != "")
                {
                    this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + call).ToString();
                }
                else
                {
                    this.textBoxPot.Text = call.ToString();
                }
                this.playerTurn = false;
                this.playerStatus.Text = "Call " + call;
                this.playerCall = call;
            }
            else if (this.playerChips <= call && call > 0)
            {
                this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + this.playerChips).ToString();
                this.playerStatus.Text = "All in " + this.playerChips;
                this.playerChips = 0;
                this.textBoxChips.Text = "playerChips : " + this.playerChips.ToString();
                this.playerTurn = false;
                this.buttonFold.Enabled = false;
                this.playerCall = this.playerChips;
            }
            await Turns();
        }
        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref this.playerType, ref this.playerPower, this.playerFoldedTurn);
            int parsedValue;
            if (this.textBoxRaise.Text != "" && int.TryParse(this.textBoxRaise.Text, out parsedValue))
            {
                if (this.playerChips > call)
                {
                    if (Raise * 2 > int.Parse(this.textBoxRaise.Text))
                    {
                        this.textBoxRaise.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (this.playerChips >= int.Parse(this.textBoxRaise.Text))
                        {
                            call = int.Parse(this.textBoxRaise.Text);
                            Raise = int.Parse(this.textBoxRaise.Text);
                            this.playerStatus.Text = "Raise " + call.ToString();
                            this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + call).ToString();
                            this.buttonCall.Text = "Call";
                            this.playerChips -= int.Parse(this.textBoxRaise.Text);
                            raising = true;
                            this.lastPlayed = 0;
                            this.playerRaise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            call = this.playerChips;
                            Raise = this.playerChips;
                            this.textBoxPot.Text = (int.Parse(this.textBoxPot.Text) + this.playerChips).ToString();
                            this.playerStatus.Text = "Raise " + call.ToString();
                            this.playerChips = 0;
                            raising = true;
                            this.lastPlayed = 0;
                            this.playerRaise = Convert.ToInt32(Raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }
            this.playerTurn = false;
            await Turns();
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            if (this.textBoxAdd.Text == "") { }
            else
            {
                this.playerChips += int.Parse(this.textBoxAdd.Text);
                this.firstBotChips += int.Parse(this.textBoxAdd.Text);
                this.secondBotChips += int.Parse(this.textBoxAdd.Text);
                this.thirdBotChips += int.Parse(this.textBoxAdd.Text);
                this.forthBotChips += int.Parse(this.textBoxAdd.Text);
                this.fifthBotChips += int.Parse(this.textBoxAdd.Text);
            }
            this.textBoxChips.Text = "playerChips : " + this.playerChips.ToString();
        }
        private void bOptions_Click(object sender, EventArgs e)
        {
            this.textBoxBigBlind.Text = this.bigBlind.ToString();
            this.textBoxSmallBlind.Text = this.smallBlind.ToString();
            if (this.textBoxBigBlind.Visible == false)
            {
                this.textBoxBigBlind.Visible = true;
                this.textBoxSmallBlind.Visible = true;
                this.buttonBigBlind.Visible = true;
                this.buttonSmallBlind.Visible = true;
            }
            else
            {
                this.textBoxBigBlind.Visible = false;
                this.textBoxSmallBlind.Visible = false;
                this.buttonBigBlind.Visible = false;
                this.buttonSmallBlind.Visible = false;
            }
        }
        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.textBoxSmallBlind.Text.Contains(",") || this.textBoxSmallBlind.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                this.textBoxSmallBlind.Text = this.smallBlind.ToString();
                return;
            }
            if (!int.TryParse(this.textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.textBoxSmallBlind.Text = this.smallBlind.ToString();
                return;
            }
            if (int.Parse(this.textBoxSmallBlind.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                this.textBoxSmallBlind.Text = this.smallBlind.ToString();
            }
            if (int.Parse(this.textBoxSmallBlind.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(this.textBoxSmallBlind.Text) >= 250 && int.Parse(this.textBoxSmallBlind.Text) <= 100000)
            {
                this.smallBlind = int.Parse(this.textBoxSmallBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (this.textBoxBigBlind.Text.Contains(",") || this.textBoxBigBlind.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                this.textBoxBigBlind.Text = this.bigBlind.ToString();
                return;
            }
            if (!int.TryParse(this.textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                this.textBoxSmallBlind.Text = this.bigBlind.ToString();
                return;
            }
            if (int.Parse(this.textBoxBigBlind.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                this.textBoxBigBlind.Text = this.bigBlind.ToString();
            }
            if (int.Parse(this.textBoxBigBlind.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(this.textBoxBigBlind.Text) >= 500 && int.Parse(this.textBoxBigBlind.Text) <= 200000)
            {
                this.bigBlind = int.Parse(this.textBoxBigBlind.Text);
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