using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCV6
{
    public partial class Default : System.Web.UI.Page
    {
        //creates random value for program to use
        Random random = new Random();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //display reel values
                string[] reels = new string[] { spin1(),
                spin1(),
                spin1() };
                displayImages(reels);
                ViewState.Add("PlayersMoney", 100);
                displayBalance();
            }

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            int bet = 0;
            if (!int.TryParse(TextBox1.Text, out bet)) return;
            int winnings = pullLever(bet);
            //display bet and winnings
            displayResult(bet, winnings);
            adjust(bet, winnings);
            displayBalance();
        }
        //add winnings/losses to balance to get next balance
        private void adjust(int bet, int winnings)
        {
            int balance = int.Parse(ViewState["PlayersMoney"].ToString());
            balance -= bet;
            balance += winnings;
            ViewState["PlayersMoney"] = balance;
        }
        //display player balance
        private void displayBalance()
        {
            Label2.Text = String.Format("Player's balance: {0:C}", ViewState["PlayersMoney"]);
        }
        //display winnings/losses
        private void displayResult(int bet, int winnings)
        {
            if (winnings > 0)
            {
                Label1.Text = String.Format("You bet {0:C} and won {1:C}", bet, winnings);
            }
            else
                Label1.Text = String.Format("Sorry, you lost {0:C}", bet);
        }

        private int pullLever(int bet)
        {
            //code to generate array of  random pictures
            string[] reels = new string[] { spin1(),
                spin1(),
                spin1() };
            displayImages(reels);
            //logic for multiplier
            int multiplier = evalSpin(reels);
            return bet * multiplier;
        }

        private int evalSpin(string[] reels)
        {
            //1 bar then 0
            if (isBar(reels)) return 0;
            // three 7s then 100
            if (isJackpot(reels)) return 100;
            //one or more cherries then 2,3,4
            int multiplier = 0;
            if (isWinner(reels, out multiplier)) return multiplier;
            return 0;
        }
        //logic for winnings
        //if any bars
        private bool isBar(string[] reels)
        {
            if (reels[0] == "Bar"
                || reels[1] == "Bar"
                || reels[2] == "Bar")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //if 3 sevens
        private bool isJackpot(string[] reels)
        {
            if (reels[0] == "Seven"
            && reels[1] == "Seven"
            && reels[2] == "Seven")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //if 1, 2, or 3 cherries, then winner!
        private bool isWinner(string[] reels, out int multiplier)
        {
            multiplier = determineMultiplier(reels);
            if (multiplier > 0) return true;
            return false;
        }
        private int determineMultiplier(string[] reels)
        {
            //determine number of cherries
            int cherryCount = determineCherryCount(reels);
            if (cherryCount == 1) return 2;
            if (cherryCount == 2) return 3;
            if (cherryCount == 3) return 4;
            return 0;
        }
        private int determineCherryCount(string[] reels)
        {
            //if there are cherries; inc cherry count
            int cherryCount = 0;  //breakpoint line
            if (reels[0] == "Cherry") cherryCount++;
            if (reels[1] == "Cherry") cherryCount++;
            if (reels[2] == "Cherry") cherryCount++;
            return cherryCount;
        }
        private void displayImages(string[] reels)
        {
            //code to display random pictures
            Image1.ImageUrl = "/Images/" + reels[0] + ".png";
            Image2.ImageUrl = "/Images/" + reels[1] + ".png";
            Image3.ImageUrl = "/Images/" + reels[2] + ".png";

        }
        private string spin1()
        {
            string[] images = new string[]
            {   "Strawberry",
            "Bar",
            "Lemon",
            "Bell",
            "Clover",
            "Cherry",
            "Diamond",
            "Orange",
            "Seven",
            "HorseShoe",
            "Plum",
            "Watermelon"
            };
            return images[random.Next(11)];

        }
    }
}