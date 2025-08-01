using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashsvp
{
    public class AudioSystem : MonoBehaviour
    {
        public AudioSource engineSound;
        public AudioSource GearSound;
        [Range(0, 1)]
        public float minPitch;
        [Range(1, 3)]
        public float maxPitch;
        private GearSystem gearSystem;

        private SimcadeVehicleController SimcadeVehicleController;

        private void Start()
        {
            gearSystem = GetComponent<GearSystem>();
            SimcadeVehicleController = GetComponent<SimcadeVehicleController>();
        }

        private void FixedUpdate()
        {
            SoundManager();
        }

        private void SoundManager()
        {
            float speed = gearSystem.carSpeed;
            float enginePitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(speed) / gearSystem.gearSpeeds[Mathf.Clamp(gearSystem.currentGear, 0, 4)]);
            
            if (SimcadeVehicleController.vehicleIsGrounded)
            {
                engineSound.pitch = Mathf.MoveTowards(engineSound.pitch, enginePitch, 0.02f);
            }

            if (Mathf.Abs(SimcadeVehicleController.accelerationInput) > 0.1f)
            {
                engineSound.volume = Mathf.MoveTowards(engineSound.volume, 1, 0.01f);
            }
            else
            {
                engineSound.volume = Mathf.MoveTowards(engineSound.volume, 0.5f, 0.01f);
            }


        }
    }
}
