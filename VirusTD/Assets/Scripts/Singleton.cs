using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Singleton 
{
    public static MachinegunConcreteFactory machinegunFactory { get; } = new MachinegunConcreteFactory();
    public static BlockConcreteFactory blockFactory { get; } = new BlockConcreteFactory();
    public static ProjectileConcreteFactory projectileFactory { get; } = new ProjectileConcreteFactory();
}
