using Terraria.ModLoader;

namespace InsurgencyWeapons.Items
{
    internal abstract class AmmoItem : ModItem
    { }

    /// <summary>
    /// Main class
    /// </summary>
    internal abstract class WeaponUtils : ModItem
    { }

    internal abstract class AssaultRifle : WeaponUtils
    { }

    internal abstract class BattleRifle : WeaponUtils
    { }

    internal abstract class Carbine : WeaponUtils
    { }

    internal abstract class Rifle : WeaponUtils
    { }

    internal abstract class Shotgun : WeaponUtils
    { }

    internal abstract class SniperRifle : WeaponUtils
    { }

    internal abstract class SubMachineGun : WeaponUtils
    { }

    internal abstract class Launcher : WeaponUtils
    { }

    internal abstract class Pistol : WeaponUtils
    { }

    internal abstract class Revolver : WeaponUtils
    { }

    internal abstract class Grenade : WeaponUtils
    { }
}