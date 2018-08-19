using System.Collections;
using System.Collections.Generic;

// TODO: more params
public struct LevelInfo {

    public int maxLevel;
    public float lifeTime;
    public int maxChargeLevel;

    public LevelInfo(float lifeTime, int maxLevel, int maxChargeLevel)
    {
        this.lifeTime = lifeTime;
        this.maxLevel = maxLevel;
        this.maxChargeLevel = maxChargeLevel;
    }
}
