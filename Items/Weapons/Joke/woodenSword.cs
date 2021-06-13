using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class woodenSword: ModItem
	{

		int curCombo = 0;
		int timeNotUsed = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wood Sword");
			Tooltip.SetDefault("A toy sword made from wood");
		}

		public override void SetDefaults()
		{
			item.damage = 5;
			item.melee = true;
			item.width = 26;
			item.height = 26;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 20;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("woodenStick"), 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override bool CanUseItem(Player player)
		{
			item.useTime = item.useAnimation;
			item.reuseDelay = 0;

			switch (curCombo)
            {
				case 0:
					item.useStyle = ItemUseStyleID.SwingThrow;
					item.knockBack = 3;
					break;
				case 1:
					item.useStyle = ItemUseStyleID.Stabbing;
					item.knockBack = 4;
					break;
				case 2:
					item.useStyle = ItemUseStyleID.SwingThrow;
					item.knockBack = 3;
					break;
			}

			timeNotUsed = 0;
			curCombo = (curCombo >= 2) ? 0 : curCombo + 1;

			return true;
        }

        public override void UpdateInventory(Player player)
        {

			timeNotUsed++;
            if (timeNotUsed > item.useTime*4)
            {
				curCombo = 0;
            }

        }

        public override bool? CanHitNPC(Player player, NPC target)
        {
			return (target.life > item.damage*2);
        }

        public override bool CanHitPvp(Player player, Player target)
		{
			return (target.statLife > item.damage * 2);
		}

    }
}
