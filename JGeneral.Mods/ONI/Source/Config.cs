using System;
using System.Collections.Generic;
using HarmonyLib;
using TUNING;
using UnityEngine;

public class Config : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        string id = Patches.Fix.ID;
        int width = 2;
        int height = 2;
        string anim = "sweep_bot_base_station_kanim";
        int hitpoints = 30;
        float construction_time = 40f;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        float[] construction_mass = new float[]
        {
            BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0] - SweepBotConfig.MASS
        };
        string[] refined_METALS = MATERIALS.REFINED_METALS;
        EffectorValues none = NOISE_POLLUTION.NONE;
        EffectorValues tier = BUILDINGS.DECOR.PENALTY.TIER1;
        EffectorValues noise = none;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, refined_METALS, melting_point, build_location_rule, tier, noise, 0.2f);
        buildingDef.ViewMode = OverlayModes.Power.ID;
        buildingDef.Floodable = false;
        buildingDef.AudioCategory = "Metal";
        buildingDef.RequiresPowerInput = true;
        buildingDef.EnergyConsumptionWhenActive = 300f;
        buildingDef.ExhaustKilowattsWhenActive = 0f;
        buildingDef.SelfHeatKilowattsWhenActive = 1f;
        buildingDef.OutputConduitType = ConduitType.Solid;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        Prioritizable.AddRef(go);
    }
    
    public override void DoPostConfigureComplete(GameObject go)
    {
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
    }
}