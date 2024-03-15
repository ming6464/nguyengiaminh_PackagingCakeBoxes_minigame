using UnityEngine;

public class VFXScript : Singleton<VFXScript>
{

    [SerializeField]
    private ParticleSystem _particleSystem;

    public void PlayVfx(Vector3 postition)
    {
        transform.position = postition;
        if (_particleSystem)
        {
            _particleSystem.Emit(30);
        }
    }
}
