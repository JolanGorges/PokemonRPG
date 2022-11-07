﻿using PokemonRPG.Enums;
using Type = PokemonRPG.Enums.Type;

namespace PokemonRPG.Data;

public class MoveClass
{
    private MoveClass(Move move, string name, int power, Type type, int accuracy, int powerPoints,
        bool highCriticalHit = false)
    {
        Move = move;
        Name = name;
        Power = power;
        Type = type;
        Accuracy = accuracy;
        MaxPowerPoints = PowerPoints = powerPoints;
        HighCriticalHit = highCriticalHit;
        // https://bulbapedia.bulbagarden.net/wiki/Status_move
        if (power == 0)
            Category = MoveCategory.Status;
        // https://bulbapedia.bulbagarden.net/wiki/Physical_move
        else if (type is Type.Normal or Type.Fighting or Type.Flying or Type.Poison or Type.Ground or Type.Rock
                 or Type.Bug or Type.Ghost)
            Category = MoveCategory.Physical;
        // https://bulbapedia.bulbagarden.net/wiki/Special_move
        else if (type is Type.Fire or Type.Water or Type.Grass or Type.Electric or Type.Psychic or Type.Ice
                 or Type.Dragon)
            Category = MoveCategory.Special;
    }

    public Move Move { get; }
    public string Name { get; }
    public int Power { get; }
    public Type Type { get; }
    public int Accuracy { get; }
    public int PowerPoints { get; set; }
    public int MaxPowerPoints { get; }
    public bool HighCriticalHit { get; }
    public MoveCategory Category { get; }

    public double GetEffectiveness(Type pokemonType)
    {
        return Type switch
        {
            Type.Ghost when pokemonType == Type.Psychic => 0,
            Type.Ghost when pokemonType == Type.Normal => 0,
            Type.Ground when pokemonType == Type.Flying => 0,
            Type.Electric when pokemonType == Type.Ground => 0,
            Type.Fighting when pokemonType == Type.Ghost => 0,
            Type.Normal when pokemonType == Type.Ghost => 0,
            Type.Rock when pokemonType == Type.Fighting => 0.5,
            Type.Bug when pokemonType == Type.Fighting => 0.5,
            Type.Electric when pokemonType == Type.Electric => 0.5,
            Type.Flying when pokemonType == Type.Electric => 0.5,
            Type.Fighting when pokemonType == Type.Psychic => 0.5,
            Type.Fighting when pokemonType == Type.Poison => 0.5,
            Type.Poison when pokemonType == Type.Poison => 0.5,
            Type.Grass when pokemonType == Type.Poison => 0.5,
            Type.Electric when pokemonType == Type.Dragon => 0.5,
            Type.Grass when pokemonType == Type.Dragon => 0.5,
            Type.Water when pokemonType == Type.Dragon => 0.5,
            Type.Fire when pokemonType == Type.Dragon => 0.5,
            Type.Fighting when pokemonType == Type.Flying => 0.5,
            Type.Grass when pokemonType == Type.Flying => 0.5,
            Type.Bug when pokemonType == Type.Flying => 0.5,
            Type.Poison when pokemonType == Type.Ground => 0.5,
            Type.Rock when pokemonType == Type.Ground => 0.5,
            Type.Poison when pokemonType == Type.Ghost => 0.5,
            Type.Bug when pokemonType == Type.Ghost => 0.5,
            Type.Electric when pokemonType == Type.Grass => 0.5,
            Type.Ground when pokemonType == Type.Grass => 0.5,
            Type.Grass when pokemonType == Type.Grass => 0.5,
            Type.Water when pokemonType == Type.Grass => 0.5,
            Type.Water when pokemonType == Type.Water => 0.5,
            Type.Fire when pokemonType == Type.Water => 0.5,
            Type.Ice when pokemonType == Type.Water => 0.5,
            Type.Poison when pokemonType == Type.Rock => 0.5,
            Type.Normal when pokemonType == Type.Rock => 0.5,
            Type.Flying when pokemonType == Type.Rock => 0.5,
            Type.Fire when pokemonType == Type.Rock => 0.5,
            Type.Grass when pokemonType == Type.Fire => 0.5,
            Type.Fire when pokemonType == Type.Fire => 0.5,
            Type.Bug when pokemonType == Type.Fire => 0.5,
            Type.Fighting when pokemonType == Type.Bug => 0.5,
            Type.Ground when pokemonType == Type.Bug => 0.5,
            Type.Grass when pokemonType == Type.Bug => 0.5,
            Type.Ice when pokemonType == Type.Ice => 0.5,
            Type.Psychic when pokemonType == Type.Psychic => 0.5,
            Type.Psychic when pokemonType == Type.Fighting => 2,
            Type.Flying when pokemonType == Type.Fighting => 2,
            Type.Ground when pokemonType == Type.Electric => 2,
            Type.Bug when pokemonType == Type.Psychic => 2,
            Type.Psychic when pokemonType == Type.Poison => 2,
            Type.Ground when pokemonType == Type.Poison => 2,
            Type.Bug when pokemonType == Type.Poison => 2,
            Type.Dragon when pokemonType == Type.Dragon => 2,
            Type.Ice when pokemonType == Type.Dragon => 2,
            Type.Fighting when pokemonType == Type.Normal => 2,
            Type.Electric when pokemonType == Type.Flying => 2,
            Type.Rock when pokemonType == Type.Flying => 2,
            Type.Ice when pokemonType == Type.Flying => 2,
            Type.Grass when pokemonType == Type.Ground => 2,
            Type.Water when pokemonType == Type.Ground => 2,
            Type.Ice when pokemonType == Type.Ground => 2,
            Type.Ghost when pokemonType == Type.Ghost => 2,
            Type.Poison when pokemonType == Type.Grass => 2,
            Type.Flying when pokemonType == Type.Grass => 2,
            Type.Fire when pokemonType == Type.Grass => 2,
            Type.Bug when pokemonType == Type.Grass => 2,
            Type.Ice when pokemonType == Type.Grass => 2,
            Type.Electric when pokemonType == Type.Water => 2,
            Type.Grass when pokemonType == Type.Water => 2,
            Type.Fighting when pokemonType == Type.Rock => 2,
            Type.Ground when pokemonType == Type.Rock => 2,
            Type.Grass when pokemonType == Type.Rock => 2,
            Type.Water when pokemonType == Type.Rock => 2,
            Type.Ground when pokemonType == Type.Fire => 2,
            Type.Water when pokemonType == Type.Fire => 2,
            Type.Rock when pokemonType == Type.Fire => 2,
            Type.Poison when pokemonType == Type.Bug => 2,
            Type.Flying when pokemonType == Type.Bug => 2,
            Type.Rock when pokemonType == Type.Bug => 2,
            Type.Fire when pokemonType == Type.Bug => 2,
            Type.Fighting when pokemonType == Type.Ice => 2,
            Type.Rock when pokemonType == Type.Ice => 2,
            Type.Fire when pokemonType == Type.Ice => 2,
            _ => 1
        };
    }

    public static MoveClass GetMove(Move move)
    {
        return move switch
        {
            Move.Pound => new MoveClass(move, "Écras'Face", 40, Type.Normal, 100, 35),
            Move.KarateChop => new MoveClass(move, "Poing-Karaté", 50, Type.Normal, 100, 25, true),
            Move.Doubleslap => new MoveClass(move, "Torgnoles", 15, Type.Normal, 85, 10),
            Move.CometPunch => new MoveClass(move, "Poing Comète", 18, Type.Normal, 85, 15),
            // Move.MegaPunch => new MoveClass(move, "Ultimapoing", Effect.NoAdditionalEffect, 80, Type.Normal, 85, 20),
            // Move.PayDay => new MoveClass(move, "Jackpot", Effect.PayDayEffect, 40, Type.Normal, 100, 20),
            // Move.FirePunch => new MoveClass(move, "Poing de Feu", Effect.BurnSideEffect1, 75, Type.Fire, 100, 15),
            // Move.IcePunch => new MoveClass(move, "Poing-Glace", Effect.FreezeSideEffect, 75, Type.Ice, 100, 15),
            // Move.Thunderpunch => new MoveClass(move, "Poing-Éclair", Effect.ParalyzeSideEffect1, 75, Type.Electric, 100, 15),
            Move.Scratch => new MoveClass(move, "Griffe", 40, Type.Normal, 100, 35),
            Move.Vicegrip => new MoveClass(move, "Force Poigne", 55, Type.Normal, 100, 30),
            // Move.Guillotine => new MoveClass(move, "Guillotine", Effect.OhkoEffect, 1, Type.Normal, 30, 5),
            // Move.RazorWind => new MoveClass(move, "Coupe-Vent", Effect.ChargeEffect, 80, Type.Normal, 75, 10),
            // Move.SwordsDance => new MoveClass(move, "Danse-Lames", Effect.AttackUp2Effect, 0, Type.Normal, 100, 30),
            // Move.Cut => new MoveClass(move, "Coupe", Effect.NoAdditionalEffect, 50, Type.Normal, 95, 30),
            Move.Gust => new MoveClass(move, "Tornade", 40, Type.Normal, 100, 35),
            // Move.WingAttack => new MoveClass(move, "Cru-Aile", Effect.NoAdditionalEffect, 35, Type.Flying, 100, 35),
            // Move.Whirlwind => new MoveClass(move, "Cyclone", Effect.SwitchAndTeleportEffect, 0, Type.Normal, 85, 20),
            // Move.Fly => new MoveClass(move, "Vol", Effect.FlyEffect, 70, Type.Flying, 95, 15),
            Move.Bind => new MoveClass(move, "Étreinte", 15, Type.Normal, 75, 20),
            // Move.Slam => new MoveClass(move, "Souplesse", Effect.NoAdditionalEffect, 80, Type.Normal, 75, 20),
            Move.VineWhip => new MoveClass(move, "Fouet Lianes", 35, Type.Grass, 100, 10),
            Move.Stomp => new MoveClass(move, "Écrasement", 65, Type.Normal, 100, 20),
            // Move.DoubleKick => new MoveClass(move, "Double Pied", Effect.AttackTwiceEffect, 30, Type.Fighting, 100, 30),
            // Move.MegaKick => new MoveClass(move, "Ultimawashi", Effect.NoAdditionalEffect, 120, Type.Normal, 75, 5),
            // Move.JumpKick => new MoveClass(move, "Pied Sauté", Effect.JumpKickEffect, 70, Type.Fighting, 95, 25),
            // Move.RollingKick => new MoveClass(move, "Mawashi Geri", Effect.FlinchSideEffect2, 60, Type.Fighting, 85, 15),
            Move.SandAttack => new MoveClass(move, "Jet de Sable", 0, Type.Normal, 100, 15),
            Move.Headbutt => new MoveClass(move, "Coup d'Boule", 70, Type.Normal, 100, 15),
            Move.HornAttack => new MoveClass(move, "Koud'Korne", 65, Type.Normal, 100, 25),
            Move.FuryAttack => new MoveClass(move, "Furie", 15, Type.Normal, 85, 20),
            // Move.HornDrill => new MoveClass(move, "Empal'Korne", Effect.OhkoEffect, 1, Type.Normal, 30, 5),
            Move.Tackle => new MoveClass(move, "Charge", 35, Type.Normal, 95, 35),
            // Move.BodySlam => new MoveClass(move, "Plaquage", Effect.ParalyzeSideEffect2, 85, Type.Normal, 100, 15),
            Move.Wrap => new MoveClass(move, "Ligotage", 15, Type.Normal, 85, 20),
            // Move.TakeDown => new MoveClass(move, "Bélier", Effect.RecoilEffect, 90, Type.Normal, 85, 20),
            // Move.Thrash => new MoveClass(move, "Mania", Effect.ThrashPetalDanceEffect, 90, Type.Normal, 100, 20),
            // Move.DoubleEdge => new MoveClass(move, "Damoclès", Effect.RecoilEffect, 100, Type.Normal, 100, 15),
            Move.TailWhip => new MoveClass(move, "Mimi-Queue", 0, Type.Normal, 100, 30),
            Move.PoisonSting => new MoveClass(move, "Dard-Venin", 15, Type.Poison, 100, 35),
            // Move.Twineedle => new MoveClass(move, "Double-Dard", Effect.TwineedleEffect, 25, Type.Bug, 100, 20),
            // Move.PinMissile => new MoveClass(move, "Dard-Nuée", Effect.TwoToFiveAttacksEffect, 14, Type.Bug, 85, 20),
            Move.Leer => new MoveClass(move, "Groz'Yeux", 0, Type.Normal, 100, 30),
            Move.Bite => new MoveClass(move, "Morsure", 60, Type.Normal, 100, 25),
            Move.Growl => new MoveClass(move, "Rugissement", 0, Type.Normal, 100, 40),
            Move.Roar => new MoveClass(move, "Hurlement", 0, Type.Normal, 100, 20),
            Move.Sing => new MoveClass(move, "Berceuse", 0, Type.Normal, 55, 15),
            // Move.Supersonic => new MoveClass(move, "Ultrason", Effect.ConfusionEffect, 0, Type.Normal, 55, 20),
            Move.Sonicboom => new MoveClass(move, "Sonicboom", 1, Type.Normal, 90, 20),
            Move.Disable => new MoveClass(move, "Entrave", 0, Type.Normal, 55, 20),
            Move.Acid => new MoveClass(move, "Acide", 40, Type.Poison, 100, 30),
            Move.Ember => new MoveClass(move, "Flammèche", 40, Type.Fire, 100, 25),
            // Move.Flamethrower => new MoveClass(move, "Lance-Flammes", Effect.BurnSideEffect1, 95, Type.Fire, 100, 15),
            // Move.Mist => new MoveClass(move, "Brume", Effect.MistEffect, 0, Type.Ice, 100, 30),
            // Move.WaterGun => new MoveClass(move, "Pistolet à O", Effect.NoAdditionalEffect, 40, Type.Water, 100, 25),
            // Move.HydroPump => new MoveClass(move, "Hydrocanon", Effect.NoAdditionalEffect, 120, Type.Water, 80, 5),
            // Move.Surf => new MoveClass(move, "Surf", Effect.NoAdditionalEffect, 95, Type.Water, 100, 15),
            // Move.IceBeam => new MoveClass(move, "Laser Glace", Effect.FreezeSideEffect, 95, Type.Ice, 100, 10),
            // Move.Blizzard => new MoveClass(move, "Blizzard", Effect.FreezeSideEffect, 120, Type.Ice, 90, 5),
            // Move.Psybeam => new MoveClass(move, "Rafale Psy", Effect.ConfusionSideEffect, 65, Type.Psychic, 100, 20),
            // Move.Bubblebeam => new MoveClass(move, "Bulles d'O", Effect.SpeedDownSideEffect, 65, Type.Water, 100, 20),
            Move.AuroraBeam => new MoveClass(move, "Onde Boréale", 65, Type.Ice, 100, 20),
            // Move.HyperBeam => new MoveClass(move, "Ultralaser", Effect.HyperBeamEffect, 150, Type.Normal, 90, 5),
            Move.Peck => new MoveClass(move, "Picpic", 35, Type.Flying, 100, 35),
            // Move.DrillPeck => new MoveClass(move, "Bec Vrille", Effect.NoAdditionalEffect, 80, Type.Flying, 100, 20),
            // Move.Submission => new MoveClass(move, "Sacrifice", Effect.RecoilEffect, 80, Type.Fighting, 80, 25),
            Move.LowKick => new MoveClass(move, "Balayage", 50, Type.Fighting, 90, 20),
            // Move.Counter => new MoveClass(move, "Riposte", Effect.NoAdditionalEffect, 1, Type.Fighting, 100, 20),
            // Move.SeismicToss => new MoveClass(move, "Frappe Atlas", Effect.SpecialDamageEffect, 1, Type.Fighting, 100, 20),
            // Move.Strength => new MoveClass(move, "Force", Effect.NoAdditionalEffect, 80, Type.Normal, 100, 15),
            Move.Absorb => new MoveClass(move, "Vol-Vie", 20, Type.Grass, 100, 20),
            // Move.MegaDrain => new MoveClass(move, "Méga-Sangsue", Effect.DrainHpEffect, 40, Type.Grass, 100, 10),
            // Move.LeechSeed => new MoveClass(move, "Vampigraine", Effect.LeechSeedEffect, 0, Type.Grass, 90, 10),
            Move.Growth => new MoveClass(move, "Croissance", 0, Type.Normal, 100, 40),
            Move.RazorLeaf => new MoveClass(move, "Tranch'Herbe", 55, Type.Grass, 95, 25, true),
            // Move.Solarbeam => new MoveClass(move, "Lance-Soleil", Effect.ChargeEffect, 120, Type.Grass, 100, 10),
            Move.Poisonpowder => new MoveClass(move, "Poudre Toxik", 0, Type.Poison, 75, 35),
            Move.StunSpore => new MoveClass(move, "Para-Spore", 0, Type.Grass, 75, 30),
            // Move.SleepPowder => new MoveClass(move, "Poudre Dodo", Effect.SleepEffect, 0, Type.Grass, 75, 15),
            // Move.PetalDance => new MoveClass(move, "Danse-Fleur", Effect.ThrashPetalDanceEffect, 70, Type.Grass, 100, 20),
            Move.StringShot => new MoveClass(move, "Sécrétion", 0, Type.Bug, 95, 40),
            // Move.DragonRage => new MoveClass(move, "Draco-Rage", Effect.SpecialDamageEffect, 1, Type.Dragon, 100, 10),
            // Move.FireSpin => new MoveClass(move, "Danse Flamme", Effect.TrappingEffect, 15, Type.Fire, 70, 15),
            Move.Thundershock => new MoveClass(move, "Éclair", 40, Type.Electric, 100, 30),
            // Move.Thunderbolt => new MoveClass(move, "Tonnerre", Effect.ParalyzeSideEffect1, 95, Type.Electric, 100, 15),
            Move.ThunderWave => new MoveClass(move, "Cage-Éclair", 0, Type.Electric, 100, 20),
            // Move.Thunder => new MoveClass(move, "Fatal-Foudre", Effect.ParalyzeSideEffect1, 120, Type.Electric, 70, 10),
            // Move.RockThrow => new MoveClass(move, "Jet-Pierres", Effect.NoAdditionalEffect, 50, Type.Rock, 65, 15),
            // Move.Earthquake => new MoveClass(move, "Séisme", Effect.NoAdditionalEffect, 100, Type.Ground, 100, 10),
            // Move.Fissure => new MoveClass(move, "Abîme", Effect.OhkoEffect, 1, Type.Ground, 30, 5),
            Move.Dig => new MoveClass(move, "Tunnel", 100, Type.Ground, 100, 10),
            // Move.Toxic => new MoveClass(move, "Toxik", Effect.PoisonEffect, 0, Type.Poison, 85, 10),
            Move.Confusion => new MoveClass(move, "Choc Mental", 50, Type.Psychic, 100, 25),
            // Move.PsychicM => new MoveClass(move, "Psyko", Effect.SpecialDownSideEffect, 90, Type.Psychic, 100, 10),
            Move.Hypnosis => new MoveClass(move, "Hypnose", 0, Type.Psychic, 60, 20),
            // Move.Meditate => new MoveClass(move, "Yoga", Effect.AttackUp1Effect, 0, Type.Psychic, 100, 40),
            // Move.Agility => new MoveClass(move, "Hâte", Effect.SpeedUp2Effect, 0, Type.Psychic, 100, 30),
            Move.QuickAttack => new MoveClass(move, "Vive-Attaque", 40, Type.Normal, 100, 30),
            Move.Rage => new MoveClass(move, "Frénésie", 20, Type.Normal, 100, 20),
            Move.Teleport => new MoveClass(move, "Téléport", 0, Type.Psychic, 100, 20),
            Move.NightShade => new MoveClass(move, "Ombre Nocturne", 0, Type.Ghost, 100, 15),
            // Move.Mimic => new MoveClass(move, "Copie", Effect.MimicEffect, 0, Type.Normal, 100, 10),
            Move.Screech => new MoveClass(move, "Grincement", 0, Type.Normal, 85, 40),
            // Move.DoubleTeam => new MoveClass(move, "Reflet", Effect.EvasionUp1Effect, 0, Type.Normal, 100, 15),
            // Move.Recover => new MoveClass(move, "Soin", Effect.HealEffect, 0, Type.Normal, 100, 20),
            Move.Harden => new MoveClass(move, "Armure", 0, Type.Normal, 100, 30),
            // Move.Minimize => new MoveClass(move, "Lilliput", Effect.EvasionUp1Effect, 0, Type.Normal, 100, 20),
            Move.Smokescreen => new MoveClass(move, "Brouillard", 0, Type.Normal, 100, 20),
            Move.ConfuseRay => new MoveClass(move, "Onde Folie", 0, Type.Ghost, 100, 10),
            Move.Withdraw => new MoveClass(move, "Repli", 0, Type.Water, 100, 40),
            Move.DefenseCurl => new MoveClass(move, "Boul'Armure", 0, Type.Normal, 100, 40),
            // Move.Barrier => new MoveClass(move, "Bouclier", Effect.DefenseUp2Effect, 0, Type.Psychic, 100, 30),
            // Move.LightScreen => new MoveClass(move, "Mur Lumière", Effect.LightScreenEffect, 0, Type.Psychic, 100, 30),
            // Move.Haze => new MoveClass(move, "Buée Noire", Effect.HazeEffect, 0, Type.Ice, 100, 30),
            // Move.Reflect => new MoveClass(move, "Protection", Effect.ReflectEffect, 0, Type.Psychic, 100, 20),
            Move.FocusEnergy => new MoveClass(move, "Puissance", 0, Type.Normal, 100, 30),
            // Move.Bide => new MoveClass(move, "Patience", Effect.BideEffect, 0, Type.Normal, 100, 10),
            // Move.Metronome => new MoveClass(move, "Métronome", Effect.MetronomeEffect, 0, Type.Normal, 100, 10),
            // Move.MirrorMove => new MoveClass(move, "Mimique", Effect.MirrorMoveEffect, 0, Type.Flying, 100, 20),
            // Move.Selfdestruct => new MoveClass(move, "Destruction", Effect.ExplodeEffect, 130, Type.Normal, 100, 5),
            // Move.EggBomb => new MoveClass(move, "Bomb'Œuf", Effect.NoAdditionalEffect, 100, Type.Normal, 75, 10),
            Move.Lick => new MoveClass(move, "Léchouille", 20, Type.Ghost, 100, 30),
            Move.Smog => new MoveClass(move, "Purédpois", 20, Type.Poison, 70, 20),
            Move.Sludge => new MoveClass(move, "Détritus", 65, Type.Poison, 100, 20),
            Move.BoneClub => new MoveClass(move, "Massd'Os", 65, Type.Ground, 85, 20),
            // Move.FireBlast => new MoveClass(move, "Déflagration", Effect.BurnSideEffect2, 120, Type.Fire, 85, 5),
            // Move.Waterfall => new MoveClass(move, "Cascade", Effect.NoAdditionalEffect, 80, Type.Water, 100, 15),
            // Move.Clamp => new MoveClass(move, "Claquoir", Effect.TrappingEffect, 35, Type.Water, 75, 10),
            // Move.Swift => new MoveClass(move, "Météores", Effect.SwiftEffect, 60, Type.Normal, 100, 20),
            Move.SkullBash => new MoveClass(move, "Coud'Krâne", 100, Type.Normal, 100, 15),
            // Move.SpikeCannon => new MoveClass(move, "Picanon", Effect.TwoToFiveAttacksEffect, 20, Type.Normal, 100, 15),
            Move.Constrict => new MoveClass(move, "Constriction", 10, Type.Normal, 100, 35),
            // Move.Amnesia => new MoveClass(move, "Amnésie", Effect.SpecialUp2Effect, 0, Type.Psychic, 100, 20),
            // Move.Kinesis => new MoveClass(move, "Télékinésie", Effect.AccuracyDown1Effect, 0, Type.Psychic, 80, 15),
            // Move.Softboiled => new MoveClass(move, "E-Coque", Effect.HealEffect, 0, Type.Normal, 100, 10),
            // Move.HiJumpKick => new MoveClass(move, "Pied Voltige", Effect.JumpKickEffect, 85, Type.Fighting, 90, 20),
            // Move.Glare => new MoveClass(move, "Regard Médusant", Effect.ParalyzeEffect, 0, Type.Normal, 75, 30),
            // Move.DreamEater => new MoveClass(move, "Dévorêve", Effect.DreamEaterEffect, 100, Type.Psychic, 100, 15),
            Move.PoisonGas => new MoveClass(move, "Gaz Toxik", 0, Type.Poison, 55, 40),
            Move.Barrage => new MoveClass(move, "Pilonnage", 15, Type.Normal, 85, 20),
            Move.LeechLife => new MoveClass(move, "Vampirisme", 20, Type.Bug, 100, 15),
            // Move.LovelyKiss => new MoveClass(move, "Grobisou", Effect.SleepEffect, 0, Type.Normal, 75, 10),
            // Move.SkyAttack => new MoveClass(move, "Piqué", Effect.ChargeEffect, 140, Type.Flying, 90, 5),
            Move.Transform => new MoveClass(move, "Morphing", 0, Type.Normal, 100, 10),
            Move.Bubble => new MoveClass(move, "Écume", 20, Type.Water, 100, 30),
            // Move.DizzyPunch => new MoveClass(move, "Uppercut", Effect.NoAdditionalEffect, 70, Type.Normal, 100, 10),
            // Move.Spore => new MoveClass(move, "Spore", Effect.SleepEffect, 0, Type.Grass, 100, 15),
            // Move.Flash => new MoveClass(move, "Flash", Effect.AccuracyDown1Effect, 0, Type.Normal, 70, 20),
            // Move.Psywave => new MoveClass(move, "Vague Psy", Effect.SpecialDamageEffect, 1, Type.Psychic, 80, 15),
            // Move.Splash => new MoveClass(move, "Trempette", Effect.SplashEffect, 0, Type.Normal, 100, 40),
            // Move.AcidArmor => new MoveClass(move, "Acidarmure", Effect.DefenseUp2Effect, 0, Type.Poison, 100, 40),
            // Move.Crabhammer => new MoveClass(move, "Pince-Masse", Effect.NoAdditionalEffect, 90, Type.Water, 85, 10, true),
            // Move.Explosion => new MoveClass(move, "Explosion", Effect.ExplodeEffect, 170, Type.Normal, 100, 5),
            // Move.FurySwipes => new MoveClass(move, "Combo-Griffe", Effect.TwoToFiveAttacksEffect, 18, Type.Normal, 80, 15),
            // Move.Bonemerang => new MoveClass(move, "Osmerang", Effect.AttackTwiceEffect, 50, Type.Ground, 90, 10),
            // Move.Rest => new MoveClass(move, "Repos", Effect.HealEffect, 0, Type.Psychic, 100, 10),
            // Move.RockSlide => new MoveClass(move, "Éboulement", Effect.NoAdditionalEffect, 75, Type.Rock, 90, 10),
            // Move.HyperFang => new MoveClass(move, "Croc de Mort", Effect.FlinchSideEffect1, 80, Type.Normal, 90, 15),
            // Move.Sharpen => new MoveClass(move, "Affûtage", Effect.AttackUp1Effect, 0, Type.Normal, 100, 30),
            // Move.Conversion => new MoveClass(move, "Conversion", Effect.ConversionEffect, 0, Type.Normal, 100, 30),
            // Move.TriAttack => new MoveClass(move, "Triplattaque", Effect.NoAdditionalEffect, 80, Type.Normal, 100, 10),
            // Move.SuperFang => new MoveClass(move, "Croc Fatal", Effect.SuperFangEffect, 1, Type.Normal, 90, 10),
            Move.Slash => new MoveClass(move, "Tranche", 70, Type.Normal, 100, 20, true),
            // Move.Substitute => new MoveClass(move, "Clonage", Effect.SubstituteEffect, 0, Type.Normal, 100, 10),
            Move.Struggle => new MoveClass(move, "Lutte", 50, Type.Normal, 100, 10),
            _ => throw new ArgumentOutOfRangeException(nameof(move), move, null)
        };
    }
}