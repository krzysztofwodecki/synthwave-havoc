using UnityEngine;

namespace TopDownShooter.Interactive.Player
{
    public class PlayerWeapon : BaseWeapon
    {
        [Header("Player-specific fields")]
        [SerializeReference] protected CameraController _camera;
        [SerializeField] protected float _cameraShakeForce = 0.5f;

        public override bool TryShoot()
        {
            var didShoot = base.TryShoot();

            if (didShoot)
            {
                _camera.AddShake(_cameraShakeForce * Random.insideUnitCircle);
            }

            return didShoot;
        }
    }
}