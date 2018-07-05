using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace MatchingImagesGame
{
    public partial class Form1 : Form
    {
        bool clickable = true;
        int matches = 0;
        Label firstClicked = null;
        Label secondClicked = null;

        Random rnd = new Random();
        // All sets of images
        List<string> Icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z",
        };

        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AssignIconsToSquares()
        {
            foreach (Control control in tblpPlayGround.Controls)
            {
                // Convert to Label
                Label iconLabel = control as Label;
                
                if (iconLabel != null)
                {
                    // Assign random icon to each square
                    int randomNumber = rnd.Next(Icons.Count);
                    iconLabel.Text = Icons[randomNumber];

                    // Hide icons
                    iconLabel.ForeColor = iconLabel.BackColor;
                    // Remove icons from icon List
                    Icons.RemoveAt(randomNumber);
                }
            }
        }

        // Labels made clickable through addition of Click event

        private void label_click(object sender, EventArgs e)
        {
            if (clickable == true)
            {
                // Convert to Label
                Label clickedLabel = sender as Label;
                // Reveal icon of clicked square
                if (clickedLabel != null)
                {
                    if (clickedLabel.ForeColor == Color.Black)
                    {
                        return;
                    }

                    clickedLabel.ForeColor = Color.Black;

                    // Select a set of two squares and reveal them
                    if (firstClicked == null)
                    {
                        firstClicked = clickedLabel;
                        firstClicked.ForeColor = Color.Black;

                        return;
                    }

                    secondClicked = clickedLabel;
                    secondClicked.ForeColor = Color.Black;

                    CheckForWinner();

                    // If a set of two is equal, leave them visible
                    if (firstClicked.Text == secondClicked.Text)
                    {
                        // Empried and timer is not started
                        firstClicked = null;
                        secondClicked = null;
                        matches++;
                        return;
                    }

                    // Makes sure player can not click for one second
                    clickable = false;
                    // Timer is activated
                    timer.Start();
                }
            }
        }

        // Timer added
        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            // Hide again
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;
            // Emptied
            firstClicked = null;
            secondClicked = null;
            // Player can click again
            clickable = true;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tblpPlayGround.Controls)
            {
                Label iconLable = control as Label;

                if (iconLable != null)
                {
                    if (iconLable.ForeColor == iconLable.BackColor)
                    {
                        return;
                    }
                }

                if (matches == 7)
                {
                    PlayVictorySound();
                    MessageBox.Show("All matches found!", "Congratulations!");
                    Close();
                }
            }
        }

        // Menu Strips
        private void mstrpNewGame_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void mstrpExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mstrpInstructions_Click(object sender, EventArgs e)
        {
            OpenInstructions();
        }

        private void mstrpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Memory-type game by Ioan Krumov", "About");
        }

        // Instructions file
        private void OpenInstructions()
        {
            string fileName = "Instructions.txt";
            string projectPath = Directory.GetCurrentDirectory();

            if (File.Exists(projectPath + "\\" + fileName))
            {
                File.Delete(projectPath + "\\" + fileName);
            }

            List<string> Rules = new List<string>()
                {
                    "Game rules: ",
                    "This is a memory-based game.",
                    "You must connect all squares with the same image.",
                    "Once you click on two squares, the images of those squares will reveal.",
                    "If the images are the same - you have discovered a pair.",
                    "If they are not the same - they will be hidden again in a second.",
                    "You operate within two-click \"rounds\".",
                    "",
                    "Have Fun!",
                };

            File.WriteAllLines(projectPath + "\\" + fileName, Rules);
            System.Diagnostics.Process.Start(projectPath + "\\" + fileName);
        }

        // Sound
        public static void PlayVictorySound()
        {
            string soundPath = Directory.GetCurrentDirectory() + "\\Sounds" + "\\victory.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }
    }
}