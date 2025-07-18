using AircraftController;
using AircraftController.AircraftAI;
using Common;
using FormationSystem;
using System;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers {
    public class TestInstaller : MonoInstaller
    {
        public GameObject AircraftPrefab;

        public override void InstallBindings()
        {
            //Install aircraft 
            InstallFormationManager();
        }

        void InstallFormationManager()
        {
            Container.Bind<Formation>().To<ArrowHead>().AsSingle();

            Container.BindFactory<AircraftMonoBehaviour, AircraftMonoBehaviour.Factory>()
                .FromComponentInNewPrefab(AircraftPrefab)
                .WithGameObjectName("Aircraft")
                .UnderTransformGroup("Aircrafts");
        }
    }
}
