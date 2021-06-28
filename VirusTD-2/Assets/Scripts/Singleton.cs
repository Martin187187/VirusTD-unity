using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Singleton 
{
    public static MachinegunConcreteFactory machinegunFactory { get; } = new MachinegunConcreteFactory();
    public static BlockConcreteFactory blockFactory { get; } = new BlockConcreteFactory();
    public static ProjectileConcreteFactory projectileFactory { get; } = new ProjectileConcreteFactory();
    //public static StepTerrainCharacteristic standardTerrain { get; } = new StepTerrainCharacteristic(0.4f, 2f, new Color(1,1,1), 0, 2f, 2, 4, 0);



}
