using Terraria.ModLoader;
using Terraria;

namespace KingdomTerrahearts
{
	public class KingdomTerrahearts : Mod
	{

		public KingdomTerrahearts instance;

		internal interface ILoadable
		{
			void Load();
			void Unload();
		}

		public KingdomTerrahearts()
		{

			instance = this;
			

		}

		public override void Unload()
		{
			instance = null;
		}

		public override void Load()
		{
			if (!Main.dedServ)
			{
				AddEquipTexture(null, EquipType.Legs, "orgCoatLegs", "KingdomTerrahearts/Items/Armor/orgCoat_Legs");
				AddEquipTexture(new Items.Armor.orgCoatHead(), null, EquipType.Head, "orgCoatHead", "KingdomTerrahearts/Items/Armor/orgCoat_Head");
				AddEquipTexture(new Items.Armor.orgCoatBody(), null, EquipType.Body, "orgCoatBody", "KingdomTerrahearts/Items/Armor/orgCoat_Body", "KingdomTerrahearts/Items/Armor/orgCoat_Arms");
				AddEquipTexture(new Items.Armor.orgCoatLegs(), null, EquipType.Legs, "orgCoatLegs", "KingdomTerrahearts/Items/Armor/orgCoat_Legs");
			}
		}


	}
}