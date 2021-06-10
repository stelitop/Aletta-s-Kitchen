using Aletta_s_Kitchen.BotRelated;
using Aletta_s_Kitchen.GameRelated.Gamemodes;
using Aletta_s_Kitchen.GameRelated.GoalTypes;
using Aletta_s_Kitchen.GameRelated.IngredientRelated;
using Aletta_s_Kitchen.GameRelated.IngredientRelated.EffectRelated;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aletta_s_Kitchen.GameRelated
{
    public partial class Game
    {        
        public int curRound;
        public IngredientPool pool;
        public Player player;        

        public GameState gameState;

        public List<string> feedback;
        public GoalGenerator goalGenerator;
        public WinCondition winCondition;

        public DiscordMessage UIMessage;
        public ulong playerId;

        private int gamemodeChoicePage;
        private int tutorialPage;

        public Game()
        {
            this.curRound = 1;
            this.pool = new IngredientPool();
            this.player = new Player();
            this.gameState = GameState.None;

            this.feedback = new List<string>();

            this.goalGenerator = new SquareIncrGoalGenerator();
            this.winCondition = new WinCondition(x => false);

            this.UIMessage = null;
            this.playerId = 0;

            this.gamemodeChoicePage = 1;
            this.tutorialPage = 0;
        }

        public async Task Start(Gamemode gamemode) => await this.Start(BotHandler.genericPool, gamemode);
        public async Task Start(IngredientPool pool, Gamemode gamemode)
        { 
            this.curRound = 1;            
            this.pool = new IngredientPool(pool);
            this.player = new Player();

            await gamemode.ApplyGamemodeSettings(this);

            if (!(gamemode is Gamemode.TutorialGamemode)) await this.player.kitchen.Restart(this);
        }

        public async Task NextRound()
        {
            this.curRound++;

            for (int i = 0; i < this.player.kitchen.OptionsCount; i++)
            {
                if (this.player.kitchen.OptionAt(i) == null) continue;

                EffectArgs args = new EffectArgs.TimerArgs(EffectType.Timer, i);
                await Effect.CallEffects(this.player.kitchen.OptionAt(i).effects, EffectType.Timer, this.player.kitchen.OptionAt(i), this, args);
            }

            await this.player.kitchen.FillEmptySpots(this);            

            this.gameState = GameState.PickFromKitchen;            

            if (this.goalGenerator.CurrentGoal(this).round == this.curRound)
            {
                this.gameState = GameState.BeforeQuota;

                if (this.goalGenerator.CurrentGoal(this).IsGoalFulfilled(this))
                {
                    await this.CheckQuota();
                }
            }
        }

        public async Task CheckQuota()
        {
            var curGoal = this.goalGenerator.CurrentGoal(this);

            if (!curGoal.IsGoalFulfilled(this))
            {
                await this.EndGame();
            }
            else
            {
                this.feedback.Add("You fulfilled the quota!");
                this.gameState = GameState.PickFromKitchen;
            }
        }

        public async Task EndGame()
        {
            BotHandler.SetUserState(this.playerId, UserState.Idle);

            this.gameState = GameState.GameOverLoss;
            if (this.winCondition.Check(this)) this.gameState = GameState.GameOverWin;

            await this.UIMessage.ModifyAsync((await this.GetUIEmbed()).Build()).ConfigureAwait(false);

            if (BotHandler.playerGames.ContainsKey(this.playerId)) BotHandler.playerGames.Remove(this.playerId);
        }

        private void FormatCardTexts(List<string> cardTexts)
        {
            const int lineLength = 18;

            //Item1 = number of lines, Item2 = chars on last line
            List<int> totalLines = new List<int>();
            List<int> lastLineLen = new List<int>();

            int maxLines = 0;

            for (int i=0; i<cardTexts.Count; i++)
            {
                string[] words = cardTexts[i].Split(' ');

                totalLines.Add(1);
                lastLineLen.Add(0);

                foreach (var word in words)
                {
                    if (lastLineLen.Last() + word.Length > lineLength)
                    {
                        totalLines[totalLines.Count-1]++;
                        lastLineLen[lastLineLen.Count - 1] = 0;
                    }

                    lastLineLen[lastLineLen.Count - 1] += word.Length;
                    if (lastLineLen.Last() < lineLength && word != words.Last()) lastLineLen[lastLineLen.Count - 1]++;
                }

                if (maxLines < totalLines.Last()) maxLines = totalLines.Last();
            }

            for (int i=0; i<cardTexts.Count; i++)
            {
                if (totalLines[i] < maxLines)
                {
                    for (int j = 0; j < lineLength - lastLineLen[i]; j++) cardTexts[i] += " ";

                    cardTexts[i] += "\u200B";

                    while (maxLines - totalLines[i] > 1)
                    {
                        totalLines[i]++;
                        cardTexts[i] += "\u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B";
                    }
                    cardTexts[i] += "\u200B ";

                    //while (maxLines - totalLines[i] > 0)
                    //{
                    //    totalLines[i]++;
                    //    cardTexts[i] += "\n";
                    //}
                }
                cardTexts[i] += "\u200B";
            }
        }
       
        public async Task<DiscordEmbedBuilder> GetUIEmbed()
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"{this.player.name}'s Kitchen",
                Color = DiscordColor.Azure,
            };

            if (this.winCondition.Check(this) && this.gameState != GameState.GameOverLoss && this.gameState != GameState.GameOverWin) await this.EndGame();

            if (this.gameState == GameState.Loading)
            {
                embed.Description = $"Preparing your Kitchen. This may take a while... :hourglass_flowing_sand:";
                return embed;
            }
            if (this.gameState == GameState.GameOverLoss || this.gameState == GameState.GameOverWin)
            {
                embed.Color = DiscordColor.Gray;

                string emptyDesc = "\u200B";

                for (int i = 0; i < 50; i++) emptyDesc += " \u200B";

                embed.AddField("\u200B", emptyDesc, true);
                if (this.gameState == GameState.GameOverLoss) embed.AddField("\u200B", "```\u200B    Game Over!```", true);
                else embed.AddField("\u200B", "```\u200B     You Win!```", true);
                embed.AddField("\u200B", emptyDesc, true);

                embed.AddField("\u200B", $"```fix\n\u200BYou've finished the game with a score of {this.player.curPoints}p and lasted {this.curRound} rounds! To play again, use a!play.```");

                return embed;
            }
            if (this.gameState == GameState.ChooseGamemode)
            {
                embed.AddField("\u200B", "\u200B", true);
                embed.AddField("Choose a Gamemode", "\u200B", true);
                embed.AddField("\u200B", "\u200B", true);

                List<string> formatDescriptions = new List<string>();
                Game.gamemodes.ForEach(x => formatDescriptions.Add(x.description));                
                this.FormatCardTexts(formatDescriptions);

                List<KeyValuePair<string, string>> gamemodeCards = new List<KeyValuePair<string, string>>();

                for (int i=0; i<formatDescriptions.Count && i < Game.gamemodes.Count; i++)
                {                    
                    gamemodeCards.Add(new KeyValuePair<string, string>(Game.gamemodes[i].title, formatDescriptions[i]));
                }

                for (int i=0; i<gamemodeCards.Count; i++)
                {
                    embed.AddField($"{BotHandler.IntToEmojis(i+1)} {gamemodeCards[i].Key}", $"```{gamemodeCards[i].Value} ```", true);
                }

                embed.AddField("\u200B", "\u200B", true);

                return embed;
            }                        

            if (BotHandler.GetUserState(this.playerId) == UserState.Tutorial && this.tutorialPage < Game.tutorialPages.Count)
            {
                var mem = this.feedback;

                this.feedback = Game.tutorialPages[this.tutorialPage].feedback;
                var embedRet = this.GetPlayerUI(Game.tutorialPages[this.tutorialPage].shownElements);

                this.feedback = mem;

                return embedRet;
            }

            return this.GetPlayerUI();
        }


        public class PlayerUIElements
        {
            public bool kitchenStats, quota, kitchenIngredients, nextIngredient, dish, gameEvents, instructions;

            public PlayerUIElements()
            {
                kitchenStats = quota = kitchenIngredients = nextIngredient = dish = gameEvents = instructions = false;
            }

            public PlayerUIElements(bool mode)
            {
                kitchenStats = quota = kitchenIngredients = nextIngredient = dish = gameEvents = instructions = mode;
            }
        }

        public DiscordEmbedBuilder GetPlayerUI() => this.GetPlayerUI(new PlayerUIElements(true));        
        public DiscordEmbedBuilder GetPlayerUI(PlayerUIElements shownElements)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Title = $"{this.player.name}'s Kitchen",
                Color = DiscordColor.Azure,
            };

            embed.Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "To end early, type a!endgame. For a new game interface, type a!newui."
            };

            //Kitchen Stats
            if (shownElements.kitchenStats)
            {
                string kitchentStatsString = $"Current Score: {BotHandler.IntToEmojis(this.player.curPoints)}p\nCurrent Round: {this.curRound}";
                if (this.player.curPoints >= 1000) kitchentStatsString = $"Current Score:\n{BotHandler.IntToEmojis(this.player.curPoints)}p\nCurrent Round: {this.curRound}";

                if (this.goalGenerator.CurrentGoal(this).round - this.curRound == 1)
                {
                    kitchentStatsString += "\n***(quota next turn!)***";
                }


                embed.AddField("Kitchen Stats", kitchentStatsString, true);
            }
            else
            {
                embed.AddField("\u200B\n\n\u200B", "\u200B", true);
            }

            if (shownElements.kitchenIngredients)
            {
                embed.AddField("\u200B", "\u200B\n\n\n**The Kitchen**\n\u200B", true);
            }
            else
            {
                embed.AddField("\u200B", "\u200B\n\n\n\n\u200B", true);
            }

            //Quota
            if (shownElements.quota)
            {
                try
                {
                    Goal goal = this.goalGenerator.CurrentGoal(this);

                    if (goal.round == this.curRound && this.gameState != GameState.BeforeQuota)
                    {
                        this.curRound++;
                        goal = this.goalGenerator.CurrentGoal(this);
                        this.curRound--;
                    }

                    embed.AddField($"Next Quota: Round {goal.round}", goal.GetDescription(this), true);
                }
                catch (Exception)
                {
                    embed.AddField("There's no next quota.", "\u200B", true);
                }
            }
            else
            {
                embed.AddField("\u200B", "\u200B", true);
            }

            //Prepare all card text to be the same size
            var kitchen = this.player.kitchen.GetAllIngredients();

            List<string> cardTexts = new List<string>();

            for (int i = 0; i < kitchen.Count && i < 5; i++)
            {
                if (kitchen[i] == null)
                {
                    cardTexts.Add("");
                    continue;
                }
                cardTexts.Add(kitchen[i].GetDescriptionText(this, GameLocation.Kitchen));
            }

            Ingredient nextIngr = this.player.kitchen.nextOption;
            cardTexts.Add(nextIngr.GetDescriptionText(this, GameLocation.NextIngredient));

            for (int i = 0; i < 3; i++)
            {
                if (this.player.hand.IngredientAt(i) == null)
                {
                    cardTexts.Add("");
                    continue;
                }
                cardTexts.Add(this.player.hand.IngredientAt(i).GetDescriptionText(this, GameLocation.Hand));
            }

            //cardTexts.Add("");

            this.FormatCardTexts(cardTexts);

            //string emptyText = cardTexts.Last();
            //cardTexts.RemoveAt(cardTexts.Count - 1);

            string emptyText = "\u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B \u200B";

            //Prepare all card text to be the same size

            // The kitchen field
            if (shownElements.kitchenIngredients)
            {
                for (int i = 0; i < kitchen.Count && i < 5; i++)
                {
                    if (kitchen[i] == null)
                    {
                        embed.AddField($"Empty Kitchen Slot #{i + 1}", "\u200B", true);

                        continue;
                    }

                    string kitchenTitle = kitchen[i].GetTitleText();

                    if ((this.gameState == GameState.PickFromKitchen && this.player.hand.OptionsCount < 3) || this.gameState == GameState.Tutorial)
                    {
                        kitchenTitle = kitchen[i].GetTitleText(i + 1);
                        //kitchenTitle += $" - {BotHandler.numToEmoji[i + 1]}";
                    }

                    //if (i >= 3) kitchenTitle = $"\u200B\n\n{kitchenTitle}";

                    //string kitchenDesc = kitchen[i].GetDescriptionText(this, GameLocation.Kitchen);
                    //if (kitchenDesc.Equals(string.Empty)) kitchenDesc = "\u200B";
                    string kitchenDesc = cardTexts[i];

                    if (kitchen[i].glowLocation == GameLocation.Kitchen || kitchen[i].glowLocation == GameLocation.Any)
                    {
                        if (kitchen[i].GlowCondition(this, i))
                        {
                            kitchenDesc = $"fix\n{kitchenDesc}";
                        }
                    }

                    kitchenDesc = $"```{kitchenDesc} ```";

                    embed.AddField(kitchenTitle, kitchenDesc, true);
                }

                for (int i = kitchen.Count; i < 5; i++)
                {
                    embed.AddField($"\u200B\n\nKitchen Slot {i + 1}", "(empty)", true);
                }
            }
            else
            {
                for (int i=0; i<5; i++)
                {
                    embed.AddField("\u200B\n\u200B", emptyText, true);
                }
            }
            // The kitchen field

            // The field about the next ingredient coming
            if (shownElements.nextIngredient)
            {
                //string nextTitle = "__Next in the Kitchen__\n\n";
                string nextTitle = nextIngr.GetTitleText();

                string nextDesc = cardTexts[5];

                if (nextIngr.glowLocation == GameLocation.NextIngredient || nextIngr.glowLocation == GameLocation.Any)
                {
                    try
                    {
                        if (nextIngr.GlowCondition(this, -1))
                        {
                            nextDesc = $"fix\n{nextDesc}";
                        }
                    }
                    catch (Exception) { }
                }

                nextDesc = "```" + nextDesc + " ```";
                nextDesc += "\n**\u200B \u200B \u200B \u200B \u200B<Next in the Kitchen>**";
                embed.AddField(nextTitle, nextDesc, true);
            }
            else
            {
                embed.AddField("\u200B\n\u200B", emptyText, true);
            }
            // The field about the next ingredient coming

            //The Dish
            if (shownElements.dish)
            {
                // The Dish title
                embed.AddField("\u200B", "\u200B", true);
                embed.AddField("\u200B", "**The Dish**", true);
                embed.AddField("\u200B", "\u200B", true);
                // The Dish title

                // The hand field
                for (int i = 0; i < this.player.hand.OptionsCount && i < 3; i++)
                {
                    string handTitle = string.Empty;
                    string handDesc = string.Empty;

                    if (this.player.hand.IngredientAt(i) == null)
                    {
                        //if (this.gameState == GameState.ChooseInHandForIngredient) handTitle = $"Empty Dish Slot {BotHandler.numToEmoji[i+1]}";
                        //else
                        handTitle = $"Empty Dish Slot #{i + 1}";
                        embed.AddField(handTitle, "\u200B", true);
                        continue;
                    }

                    handTitle = this.player.hand.IngredientAt(i).GetTitleText();

                    //if (this.gameState == GameState.ChooseInHandForIngredient) handTitle += $" - {BotHandler.numToEmoji[i + 1]}";


                    handDesc = cardTexts[i + 6];

                    if (this.player.hand.IngredientAt(i).glowLocation == GameLocation.Hand || this.player.hand.IngredientAt(i).glowLocation == GameLocation.Any)
                    {
                        if (this.player.hand.IngredientAt(i).GlowCondition(this, i))
                        {
                            handDesc = $"fix\n{handDesc}";
                        }
                    }


                    handDesc = $"```{handDesc} ```";
                    embed.AddField(handTitle, handDesc, true);
                }

                for (int i = this.player.hand.OptionsCount; i < 3; i++)
                {
                    string handTitle = string.Empty;
                    string handDesc = string.Empty;

                    handTitle = $"Empty Dish Slot #{i + 1}";
                    embed.AddField(handTitle, "\u200B", true);
                    continue;
                }
            }
            else
            {
                for (int i=0; i<3; i++)
                {
                    embed.AddField("\u200B\n\u200B", emptyText, true);
                }
            }
            // The hand field

            // Feedback messages from Ingredients
            if (shownElements.gameEvents)
            {
                string feedbackMsg = string.Empty;

                for (int i = 0; i < this.feedback.Count; i++)
                {
                    feedbackMsg += $":asterisk: {this.feedback[i]}\n";
                }

                if (!(this.feedback.Count == 0 || feedbackMsg.Equals(string.Empty)))
                {
                    embed.AddField("Game Events", feedbackMsg);
                }
            }
            // Feedback messages from Ingredients

            // Instructions on what buttons to click
            if (shownElements.instructions)
            {
                string instrTitle = string.Empty, instrDescription = string.Empty;
                switch (this.gameState)
                {
                    case GameState.PickFromKitchen:
                    case GameState.Tutorial:
                        instrTitle = "Pick an ingredient to add to your dish or cook your dish.";
                        if (this.player.hand.OptionsCount < 3 && this.player.hand.OptionsCount < this.player.hand.handLimit) instrDescription = ":one::two::three::four::five: - Pick an ingredient.\n:fork_knife_plate: - Cook your dish.";
                        else instrDescription = ":fork_knife_plate: - Cook your dish.";
                        break;
                    //case GameState.ChooseInHandForIngredient:
                    //    instrTitle = "Choose where to put your ingredient.";
                    //    instrDescription = ":one::two::three: - Place on this spot.\n:no_entry_sign: - Cancel picking an ingredient.";
                    //    break;
                    case GameState.BeforeQuota:
                        instrTitle = "Quota this round! Do you want to cook your dish before the quota?";
                        //instrDescription = ":fork_knife_plate: - Cook your dish.\n:no_entry_sign: - Proceed without cooking.";
                        instrDescription = ":fork_knife_plate: - Cook your dish.";
                        break;
                    default:
                        instrTitle = "Instructions";
                        instrDescription = "Instructions Description";
                        break;
                }

                embed.AddField(instrTitle, instrDescription);
            }
            // Instructions on what buttons to click

            return embed;
        }

        /*
         * 0-4 - buttons from 1 to 5
         * 5 - cook
         * 6 - cancel
         */
        public async Task ProcessButtonPress(DiscordUser user, int emojiIndex)
        {
            switch (this.gameState)
            {
                case GameState.PickFromKitchen:
                    {
                        if (emojiIndex < 5)
                        {
                            if (!this.player.kitchen.OptionAt(emojiIndex).CanBeBought(this, emojiIndex))
                            {
                                this.feedback.Clear();
                                this.feedback.Add($"{this.player.kitchen.OptionAt(emojiIndex).name} can't be picked currently!");
                                break;
                            }

                            await this.player.kitchen.PickIngredient(this, emojiIndex);
                        }
                        else if (emojiIndex == 5)
                        {
                            await this.player.hand.Cook(this);
                        }
                        else if (emojiIndex == 6 && this.tutorialPage == Game.tutorialPages.Count)
                        {
                            this.tutorialPage--;
                            this.gameState = GameState.Tutorial;
                        }
                        break;
                    }
                case GameState.BeforeQuota:
                    {
                        if (emojiIndex >= 5)
                        {
                            if (emojiIndex == 5)
                            {
                                await this.player.hand.Cook(this);
                            }
                            else if (emojiIndex == 6) { }

                            await this.CheckQuota();
                        }

                        break;
                    }
                case GameState.ChooseGamemode:
                    {
                        if (emojiIndex < 5)
                        {
                            int choice = (this.gamemodeChoicePage - 1) * 5 + emojiIndex;
                            if (choice >= Game.gamemodes.Count) break;

                            this.gameState = GameState.PickFromKitchen;
                            this.feedback.Add($"You've picked {Game.gamemodes[choice].title} as the gamemode!");

                            await this.Start(Game.gamemodes[choice]);
                            this.player.name = user.Username;
                            this.playerId = user.Id;
                        }
                        break;
                    }
                case GameState.Tutorial:
                    {
                        if (emojiIndex == 6)
                        {
                            this.tutorialPage--;
                            if (this.tutorialPage < 0) this.tutorialPage = 0;
                        }
                        else if (emojiIndex == 7)
                        {
                            this.tutorialPage++;
                            if (this.tutorialPage >= Game.tutorialPages.Count)
                            {
                                this.tutorialPage = Game.tutorialPages.Count;
                                this.gameState = GameState.PickFromKitchen;
                            }
                        }

                        break;
                    }
                default:
                    break;
            }


            await this.UIMessage.ModifyAsync((await this.GetUIEmbed()).Build()).ConfigureAwait(false);            

            if (this.gameState == GameState.GameOverLoss) await this.EndGame();
        }

        public void RestOfGameBuff(RoGCondition condition, RoGBuff buff, bool includeHand = true)
        {
            foreach (var ingr in this.player.kitchen.GetAllNonNullIngredients())
            {
                if (condition(ingr)) buff(ingr);
            }

            if (includeHand)
                foreach (var ingr in this.player.hand.GetAllIngredients())
                {
                    if (ingr == null) continue;
                    if (condition(ingr)) buff(ingr);
                }   

            if (this.player.kitchen.nextOption != null)
            {
                if (condition(this.player.kitchen.nextOption)) buff(this.player.kitchen.nextOption);
            }

            foreach (var ingr in this.pool.ingredients)
            {
                if (ingr == null) continue;
                if (condition(ingr)) buff(ingr);
            }

            foreach (var ingr in this.pool.tokens)
            {
                if (ingr == null) continue;
                if (condition(ingr)) buff(ingr);
            }
        }

        public delegate bool RoGCondition(Ingredient ingr);
        public delegate void RoGBuff(Ingredient ingr);


        public static readonly List<Gamemode> gamemodes = new List<Gamemode>
        {
            new Gamemode.TutorialGamemode(),
            //new Gamemode.AnonymousMethodGamemode("Easy", "Reach 100 points. For those new to deckbuilders.", game => { game.goalGenerator = new SquareIncrGoalGenerator(); game.winCondition = new WinCondition(x => x.player.curPoints >= 100); }),
            //new Gamemode.AnonymousMethodGamemode("Medium", "Reach 300 points. For those new to Aletta's Kitchen.", game => { game.goalGenerator = new SquareIncrGoalGenerator(); game.winCondition = new WinCondition(x => x.player.curPoints >= 300); }),
            //new Gamemode.AnonymousMethodGamemode("Hard", "Reach 500 points. For those looking for a challenge.", game => { game.goalGenerator = new SquareIncrGoalGenerator(); game.winCondition = new WinCondition(x => x.player.curPoints >= 500); }),
            new Gamemode.AnonymousMethodGamemode("Play the game", "Let's get cooking!", game => { game.goalGenerator = new SquareIncrGoalGenerator(); game.winCondition = new WinCondition(x => false); }),
        };

        private struct TutorialPage
        {
            public List<string> feedback;
            public PlayerUIElements shownElements;

            public TutorialPage(List<string> feedback, PlayerUIElements shownElements)
            {
                this.feedback = feedback;
                this.shownElements = shownElements;
            }
        }

        private static readonly List<TutorialPage> tutorialPages = new List<TutorialPage>
        {
            new TutorialPage(new List<string>{ "Welcome to the tutorial! Let’s teach you the way around the kitchen.", "In this field you will receive info on the elements of the interface and how to use them.", "You can navigate through the tutorial with the :arrow_left: and :arrow_right: buttons." }, new PlayerUIElements{ gameEvents = true}),
            new TutorialPage(new List<string>{ "This is your current total score and round."}, new PlayerUIElements{ gameEvents = true, kitchenStats = true}),
            new TutorialPage(new List<string>{ "This is the score quota. If you fail it, you're out of the game!", "The quota increases each time you complete it."}, new PlayerUIElements{ gameEvents = true, quota = true}),
            new TutorialPage(new List<string>{ "This is your kitchen!", "Your kitchen can only fit 5 ingredients at a time.", "On the bottom right of your kitchen is the next ingredient to enter your kitchen.", "Each ingredient comes with points, signified as 1p, or 2p, etc.", "The blue number is the number of their slot in the kitchen. More on that later."}, new PlayerUIElements{ gameEvents = true, kitchenIngredients = true, nextIngredient = true}),
            new TutorialPage(new List<string>{ "This is where you prepare your dish.", "Your dish can only fit 3 ingredients at a time.", "You’ll need to pick ingredients from the kitchen to create your dish."}, new PlayerUIElements{ gameEvents = true, dish = true}),
            new TutorialPage(new List<string>{ "This is how you’ll pick ingredients from the kitchen.", "The numbers on your buttons correspond to the number of the ingredient in the kitchen. Eg. When you click 5, the 5th ingredient in the kitchen is picked.", "Whenever you pick an ingredient, you advance a round and become closer to the quota.", "If you're not sure what to do, you can always check the Instructions below."}, new PlayerUIElements{ gameEvents = true, instructions = true}),
            new TutorialPage(new List<string>{ "Finally, the field you've been reading all this info from.", "This is where things happening in the game will be communicated to you.", "You can expect everything random or not obvious what's happening to be informed here.", "When you next click :arrow_right:, you can try playing the game! It will go on until you reach 75 points.", "You can always go back with the arrows if you want to reread something."}, new PlayerUIElements{ gameEvents = true}),
        };
    }
}
