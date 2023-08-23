namespace WeaponSystem
{
    public interface IProjectileFactory
    {
        public ITransform GetProjectile();
        public IHomingProjectile GetHomingProjectile();
    }
}
