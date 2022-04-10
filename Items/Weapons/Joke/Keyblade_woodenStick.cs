using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class Keyblade_woodenStick : KeybladeBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Stick");
            Tooltip.SetDefault("'Roxas... that's a stick'" +
                "\nYes this is that same stick" +
                "\nDon't question it");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 1;
			Item.width = 26;
			Item.height = 26;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 1;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 10;

			SaveAtributes();
			magic = keyMagic.fire;
			guardType = keybladeBlockingType.none;
			magicCost = 1;
			keyTransformations = new keyTransformation[] {  };
			transSprites = new string[] {""};
			formChanges = new keyDriveForm[] { };
			animationTimes = new int[] { 10, 10 };
			keyLevel = -100;
			keySummon = summonType.mushu;
			keybladeElement = keyType.fire;
			comboMax = 1;
		}

        public override void ChangeKeybladeValues()
		{
			magic = keyMagic.fire;
			guardType = keybladeBlockingType.none;
			magicCost = 1;
			keyTransformations = new keyTransformation[] { };
			transSprites = new string[] { "" };
			formChanges = new keyDriveForm[] { };
			animationTimes = new int[] { 10, 10 };
			keyLevel = -100;
			keySummon = summonType.mushu;
			keybladeElement = keyType.fire;
			comboMax = 1;
		}
	}
}
