using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aletta_s_Kitchen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var tr = new ControlWriter(TextBoxConsole);

            Console.SetOut(tr);
            Console.SetError(tr);

            for (int i=0; i<BotHandler.genericPool.ingredients.Count(); i++)
            {
                comboBox1.Items.Add(BotHandler.genericPool.ingredients[i].name);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonBotStart_Click(object sender, EventArgs e)
        {
            BotHandler.bot.RunAsync();            
            ButtonBotStart.Enabled = false;
            ButtonBotStop.Enabled = true;
        }
        private void ButtonBotStop_Click(object sender, EventArgs e)
        {
            BotHandler.bot.StopBot();
            ButtonBotStart.Enabled = true;
            ButtonBotStop.Enabled = false;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            BotHandler.bot.StopBot();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListBoxFeedback.Items.Clear();

            ListBoxFeedback.Items.Add("General Game Statistics");
            ListBoxFeedback.Items.Add("");

            IngredientPool pool = BotHandler.genericPool;            

            ListBoxFeedback.Items.Add($"Total Ingredients: {pool.ingredients.Count}");

            ListBoxFeedback.Items.Add($"Common: {pool.ingredients.FindAll(x => x.rarity == GameRelated.Rarity.Common).Count()}");
            ListBoxFeedback.Items.Add($"Rare: {pool.ingredients.FindAll(x => x.rarity == GameRelated.Rarity.Rare).Count()}");
            ListBoxFeedback.Items.Add($"Epic: {pool.ingredients.FindAll(x => x.rarity == GameRelated.Rarity.Epic).Count()}");
            ListBoxFeedback.Items.Add($"Legendary: {pool.ingredients.FindAll(x => x.rarity == GameRelated.Rarity.Legendary).Count()}");

            ListBoxFeedback.Items.Add("");
            ListBoxFeedback.Items.Add("Type Breakdown:");

            foreach (Tribe type in Enum.GetValues(typeof(Tribe)))
            {                
                ListBoxFeedback.Items.Add($"{type}: {pool.ingredients.FindAll(x => x.tribe == type).Count}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListBoxFeedback.Items.Clear();
            if (comboBox1.SelectedItem == null)
            {
                ListBoxFeedback.Items.Add("Please select an ingredient from the dropdown menu.");
            }
            else
            {
                ListBoxFeedback.Items.Add(BotHandler.genericPool.ingredients[comboBox1.SelectedIndex].GetFullInfo());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListBoxFeedback.Items.Clear();

            ListBoxFeedback.Items.Add("\u200B".Length);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            ListBoxFeedback.Items.Clear();

            Game game = new Game();

            await game.player.kitchen.Restart(game);

            for (int i=0; i<game.player.kitchen.OptionsCount; i++)
            {
                ListBoxFeedback.Items.Add(game.player.kitchen.OptionAt(i).GetFullInfo());
            }

            ListBoxFeedback.Items.Add($"Next Ingredient: {game.player.kitchen.nextOption.GetFullInfo()}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ListBoxFeedback.Items.Clear();

            ListBoxFeedback.Items.Add("Running Games:");
            foreach (var game in BotHandler.playerGames)
            {
                ListBoxFeedback.Items.Add($"{game.Value.player.name}");
            }
        }
    }

    public class ControlWriter : TextWriter
    {
        private const int ConsoleCharLimit = 1500;

        private readonly TextBox textBox;
        public ControlWriter(TextBox tb)
        {
            this.textBox = tb;
        }

        public override void Write(string value)
        {
            if (this.textBox.InvokeRequired)
            {
                this.textBox.Invoke(new Action<string>(Write), new object[] { value });
            }
            else
            {
                this.textBox.Text += value + Environment.NewLine;
            }

            if (this.textBox.Text.Length > ConsoleCharLimit) this.textBox.Text = this.textBox.Text.Remove(0, this.textBox.Text.Length - ConsoleCharLimit);
        }
        public override void WriteLine(string value)
        {
            if (this.textBox.InvokeRequired)
            {
                this.textBox.Invoke(new Action<string>(Write), new object[] { value });
            }
            else
            {
                this.textBox.Text += value + Environment.NewLine;
            }

            if (this.textBox.Text.Length > ConsoleCharLimit) this.textBox.Text = this.textBox.Text.Remove(0, this.textBox.Text.Length - ConsoleCharLimit);
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
}
