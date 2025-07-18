using AircraftController;
using AircraftController.AircraftAI;
using Common;
using Locomotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class AircraftInstaller : MonoInstaller
    {
        [SerializeField]
        private AircraftMovementData movementData;
        [SerializeField]
        private Rigidbody rigidbody;
        [SerializeField]
        private Transform aircraftTransform;
        [SerializeField]
        private bool isAIControlled = true;

        public override void InstallBindings()
        {
            Container.Bind<Team>().FromInstance(Team.Blue).AsSingle();

            Container.BindInterfacesAndSelfTo<Aircraft>().AsSingle()
                .WithArguments(movementData, aircraftTransform, rigidbody, true, 100.0f, 80.0f);

            if(isAIControlled)
                Container.BindInterfacesAndSelfTo<AircraftAIController>().AsSingle().NonLazy();
            else 
                Container.BindInterfacesAndSelfTo<AircraftPlayerController>().AsSingle().NonLazy();
        }
    }
}
