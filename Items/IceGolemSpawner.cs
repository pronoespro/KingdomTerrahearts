using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.Items
{
    public class IceGolemSpawner:ModItem
    {

        public override string Texture => "Terraria/Images/Item_2161";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Golem Spawner");
            Tooltip.SetDefault("A core made from pure ice" +
                "\nNot charged with ice power" +
                "\nSummons an Ice Golem");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 40;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool alreadySpawned = NPC.AnyNPCs(NPCID.IceGolem);
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, NPCID.IceGolem);
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Snowball, 15)
            .AddIngredient(ModContent.ItemType<Materials.frostStone>(),10)
            .AddTile(TileID.WorkBenches)
            .Register();
        }

    }
}
