using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Custom
{
    public class Keyblade_DukeFish: Keyblade
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duke's Boublefang");
			Tooltip.SetDefault("Fangs, sharknados and sharks included");
		}

		public override void SetDefaults()
		{
			item.damage = 430;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.scale = 0.85f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.holdStyle = 4;
			item.knockBack = 3;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useAnimation = item.useTime = 20;

			SaveAtributes();
			keyLevel = 4;
			magic = keyMagic.wind;
			projectileTime *= 5;
			keyTransformations = new keyTransformation[] { keyTransformation.staff, keyTransformation.shield };
			transSprites = new string[] { "Items/Weapons/Keyblade_demonite", "Items/Weapons/Keyblade_destiny" };
			formChanges = new keyDriveForm[] { keyDriveForm.element };
			animationTimes = new int[] { 7, 5, 10 };
			keySummon = summonType.dumbo;
		}

		public override void ChangeKeybladeValues()
		{
			keybladeElement = keyType.dark;
			comboMax = 4;
			keySummon = summonType.dumbo;
		}

	}
}
