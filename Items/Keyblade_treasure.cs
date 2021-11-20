using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace KingdomTerrahearts.Items
{
	class Keyblade_treasure : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Trove");
			Tooltip.SetDefault("A pickaxe Keyblade" +
				"\nAllows you to mine faster than your current pickaxe" +
				"\nHas the same power as your current pickaxe");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.DamageType = ModContent.GetInstance<Extra.KeybladeDamage>();
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.pick = 55;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.Amethyst, 2)
			.AddIngredient(ItemID.Ruby, 2)
			.AddIngredient(ItemID.Sapphire, 2)
			.AddIngredient(ItemID.Diamond, 2)
			.AddIngredient(ItemID.Topaz, 2)
			.AddTile(TileID.Anvils)
			.Register();
		}

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {

            switch (pre)
            {
				case -3:
				case PrefixID.Keen:
				case PrefixID.Superior:
				case PrefixID.Forceful:
				case PrefixID.Broken:
				case PrefixID.Damaged:
				case PrefixID.Shoddy:
				case PrefixID.Hurtful:
				case PrefixID.Strong:
				case PrefixID.Unpleasant:
				case PrefixID.Weak:
				case PrefixID.Ruthless:
				case PrefixID.Godly:
				case PrefixID.Demonic:
				case PrefixID.Zealous:
					return true;
            }

            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(10))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SilverCoin);

			}

		}

        public override void UpdateInventory(Player player)
        {
			foreach (Item i in player.inventory)
			{
				if (i.pick >= Item.pick && i != Item)
				{
					Item.pick = i.pick + 5;
					Item.useTime =Item.useAnimation= Math.Max(i.useTime-1,1);
					Item.scale = Math.Max(i.scale, Item.scale);
				}
			}
            base.UpdateInventory(player);
        }

    }
}
