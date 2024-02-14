namespace InsurgencyWeapons.Projectiles.Grenades
{
    internal class MK2ExplosiveNPC : GrenadeBase
    {
        public override string Texture => "InsurgencyWeapons/Projectiles/Grenades/MK2Explosive";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            NPCProj = true;
            FuseTime = 4f;
            base.SetDefaults();
        }
    }
}