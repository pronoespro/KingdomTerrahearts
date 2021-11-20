using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public abstract class ComboWeaponBase : ModItem
	{

		public int curCombo = 0;
		public int timeNotUsed = 0;
		public bool canKill = false;

		public override bool CanUseItem(Player player)
		{
			Item.useTime = Item.useAnimation;
			Item.reuseDelay = 0;

			switch (curCombo)
			{
				case 0:
					Item.useStyle = ItemUseStyleID.Swing;
					Item.knockBack = 3;
					break;
				case 1:
					Item.useStyle = ItemUseStyleID.Thrust;
					Item.knockBack = 4;
					break;
				case 2:
					Item.useStyle = ItemUseStyleID.Swing;
					Item.knockBack = 3;
					break;
			}

			timeNotUsed = 0;
			curCombo = (curCombo >= 2) ? 0 : curCombo + 1;

			return true;
		}

		public override void UpdateInventory(Player player)
		{

			timeNotUsed++;
			if (timeNotUsed > Item.useTime * 4)
			{
				curCombo = 0;
			}

		}

		public override bool? CanHitNPC(Player player, NPC target)
		{
			Vector2 dirOfTarget =target.Center- player.Center;
			if (Vector2.Dot(dirOfTarget,new Vector2(player.direction,0))>0 && MathHelp.Magnitude(dirOfTarget)<(Item.width+Item.height)/2)
			{
				return (target.life > Item.damage * 2) || canKill;
			}
			return false;
		}


	}
}
