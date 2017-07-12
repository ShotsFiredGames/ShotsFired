using UnityEngine;
using System.Collections;

namespace RootMotion.Demos {
	
	/// <summary>
	/// Emitting smoke for the mech spider
	/// </summary>
	public class MechSpiderParticles: MonoBehaviour {
		
		public MechSpiderController mechSpiderController;
		
		private ParticleSystem particles;
		
		void Start() {
			particles = (ParticleSystem)GetComponent(typeof(ParticleSystem));
		}
		
		void Update() {
			// Smoke
			float inputMag = mechSpiderController.inputVector.magnitude;
			
			float emissionRate = Mathf.Clamp(inputMag * 50, 30, 50);
			
			#if (UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
			particles.emissionRate = emissionRate;
			#else
			var emission = particles.emission;
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);
#endif

            var main = particles.main;
			main.startColor = new Color (main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, Mathf.Clamp(inputMag, 0.4f, 1f));
		}
	}
}
