using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class Keyblade_woodenStick : Keyblade
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Stick");
            Tooltip.SetDefault("'Roxas... that's a stick'" +
                "\nYes this is that same stick" +
                "\nDon't question it");
        }

		public override void SetDefaults()
		{
			item.damage = 1;
			item.melee = true;
			item.width = 26;
			item.height = 26;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 10;

			SaveAtributes();
			magic = keyMagic.fire;
			magicCost = 1;
			keyTransformations = new keyTransformation[] {  };
			transSprites = new string[] {};
			formChanges = new keyDriveForm[] { };
			animationTimes = new int[] { 10, 10 };
			keyLevel = -100;
			keySummon = summonType.mushu;
		}

        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(this,2);
			recipe.SetResult(ItemID.Wood);
			recipe.AddRecipe();
        }

        public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.fire;
			comboMax = 1;
			keySummon = summonType.mushu;
		}
	}
}
