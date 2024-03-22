using InsurgencyWeapons.Helpers;
using Terraria.Localization;

namespace InsurgencyWeapons
{
    public class InsurgencyCustomSetBonusModPlayer : ModPlayer
    {
        public bool revolverSet;

        public override void ResetEffects()
        {
            revolverSet = false;
            base.ResetEffects();
        }
    }

    public class InsurgencyCustomSetBonus : GlobalItem
    {
        private InsurgencyCustomSetBonusModPlayer GetSetBonusModPlayer(Player player) => player.GetModPlayer<InsurgencyCustomSetBonusModPlayer>();

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.ObsidianHelm && body.type == ItemID.ObsidianShirt && legs.type == ItemID.ObsidianPants)
                return "INSObsidianRevolver";
            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "INSObsidianRevolver")
            {
                GetSetBonusModPlayer(player).revolverSet = true;
                player.setBonus += "\n" + Language.GetTextValue("Mods.InsurgencyWeapons.SetBonuses.RevolverSet");
            }
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if ((Insurgency.Pistols.Contains(item.type) || Insurgency.Revolvers.Contains(item.type)) && GetSetBonusModPlayer(player).revolverSet)
                damage *= 2;
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if ((Insurgency.Pistols.Contains(item.type) || Insurgency.Revolvers.Contains(item.type)) && GetSetBonusModPlayer(player).revolverSet)
                crit += 25;
        }
    }
}