# Aletta's Kitchen
Aletta's Kitchen is a singleplayer game about cooking the greatest dishes. You have a kitchen with ingredients that give points and you have to pick the best
combinations to maximize the score of your dish. This is a discord bot that allows playing the game through discord.

# A note about running the bot
Unfortunately, due to the changes in the Discord API, the bot cannot be run in its currnent states. The game logic can still be extracted and ran separately.
If you want to try out the game online, you can do so [here](http://maysick.com/alettaskitchen/) (the web port was not done by me)

# How to play
The goal of the game is to get as many points as possible and last as long as possible. You are presented with a kitchen of 5 ingredients. Each of them has a
point score and a type (also referred to as tribe), such as Fruit, Vegetable... Dragon? There are 7 types of ingredients that each have their own identity and
strategy. To create a dish, you pick ingredients from the kitchen (up to 3) and then cook them, which gives you their combined points. Every 5 rounds you're asked
to fulfil a points quota, which grows bigger over time.

## Ingredient types
- Beasts - They synergise with giving permanent buffs to each other and utilising the buffs.
- Demons - They are very big, but have downsides or other effects that drastically affect the kitchen.
- Dragons - They are big ingredients, often requiring special conditions to be added to your dish.
- Elementals - They care about making consecutive dishes that contain elementals and keeping the chain for as long as possible.
- Fruits - They are destructive and provide ways to quickly clear your kitchen if it fills with bad ingredient.
- Murlocs - They love creating full dishes of themselves and creating many smaller Murlocs.
- Vegetables - They have effects that trigger while in the kitchen, not in the dish.

There are also ingredients of no type. They often have unique abilities.

## Gamemodes

There are two primary gamemodes available:
- Regular (endless)
- Limited - Only 5 of the 7 ingredient types will be available in the kitchen.
