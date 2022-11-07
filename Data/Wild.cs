﻿using PokemonRPG.Enums;
using Type = PokemonRPG.Enums.Type;

namespace PokemonRPG.Data;

public class Wild : Pokemon
{
    private Wild(string name, int hp, int attack, int defense, int speed, int special, Type type1, Type type2,
        int expYield, IEnumerable<Move> moves, int growthRate, int level) : base(name, hp, attack, defense,
        speed, special, type1, type2, expYield, moves, growthRate, level)
    {
    }

    public MoveClass GetRandomMove()
    {
        var damagingMoves = Moves.Where(move => move.Category == MoveCategory.Physical && move.Power > 0).ToArray();
        return damagingMoves.Length > 0
            ? damagingMoves[Random.Shared.Next(0, damagingMoves.Length)]
            : MoveClass.GetMove(Move.Struggle);
    }

    public static Wild GetRandom(int level)
    {
        var values = Enum.GetValues<WildPokemon>();
        var wildPokemon = (WildPokemon)values.GetValue(Random.Shared.Next(values.Length))!;
        return wildPokemon switch
        {
            // WildPokemon.Bulbasaur=>new Wild("Bulbizarre",45,49,49,45,65,Type.Grass,Type.Poison,45,64,new List<Move>{Move.Tackle,Move.Growl},3,level),
            // WildPokemon.Ivysaur=>new Wild("Herbizarre",60,62,63,60,80,Type.Grass,Type.Poison,45,141,new List<Move>{Move.Tackle,Move.Growl,Move.LeechSeed},3,level),
            // WildPokemon.Venusaur=>new Wild("Florizarre",80,82,83,80,100,Type.Grass,Type.Poison,45,208,new List<Move>{Move.Tackle,Move.Growl,Move.LeechSeed,Move.VineWhip},3,level),
            // WildPokemon.Charmander=>new Wild("Salamèche",39,52,43,65,50,Type.Fire,Type.Fire,45,65,new List<Move>{Move.Scratch,Move.Growl},3,level),
            // WildPokemon.Charmeleon=>new Wild("Reptincel",58,64,58,80,65,Type.Fire,Type.Fire,45,142,new List<Move>{Move.Scratch,Move.Growl,Move.Ember},3,level),
            // WildPokemon.Charizard=>new Wild("Dracaufeu",78,84,78,100,85,Type.Fire,Type.Flying,45,209,new List<Move>{Move.Scratch,Move.Growl,Move.Ember,Move.Leer},3,level),
            // WildPokemon.Squirtle=>new Wild("Carapuce",44,48,65,43,50,Type.Water,Type.Water,45,66,new List<Move>{Move.Tackle,Move.TailWhip},3,level),
            // WildPokemon.Wartortle=>new Wild("Carabaffe",59,63,80,58,65,Type.Water,Type.Water,45,143,new List<Move>{Move.Tackle,Move.TailWhip,Move.Bubble},3,level),
            // WildPokemon.Blastoise=>new Wild("Tortank",79,83,100,78,85,Type.Water,Type.Water,45,210,new List<Move>{Move.Tackle,Move.TailWhip,Move.Bubble,Move.WaterGun},3,level),
            WildPokemon.Caterpie => new Wild("Chenipan", 45, 30, 35, 45, 20, Type.Bug, Type.Bug, 53,
                new List<Move> { Move.Tackle, Move.StringShot }, 0, level),
            WildPokemon.Metapod => new Wild("Chrysacier", 50, 20, 55, 30, 25, Type.Bug, Type.Bug, 72,
                new List<Move> { Move.Harden }, 0, level),
            // WildPokemon.Butterfree=>new Wild("Papilusion",60,45,50,70,80,Type.Bug,Type.Flying,45,160,new List<Move>{Move.Confusion},0,level),
            WildPokemon.Weedle => new Wild("Aspicot", 40, 35, 30, 50, 20, Type.Bug, Type.Poison, 52,
                new List<Move> { Move.PoisonSting, Move.StringShot }, 0, level),
            WildPokemon.Kakuna => new Wild("Coconfort", 45, 25, 50, 35, 25, Type.Bug, Type.Poison, 71,
                new List<Move> { Move.Harden }, 0, level),
            // WildPokemon.Beedrill=>new Wild("Dardargnan",65,80,40,75,45,Type.Bug,Type.Poison,45,159,new List<Move>{Move.FuryAttack},0,level),
            WildPokemon.Pidgey => new Wild("Roucool", 40, 45, 40, 56, 35, Type.Normal, Type.Flying, 55,
                new List<Move> { Move.Gust }, 3, level),
            WildPokemon.Pidgeotto => new Wild("Roucoups", 63, 60, 55, 71, 50, Type.Normal, Type.Flying, 113,
                new List<Move> { Move.Gust, Move.SandAttack }, 3, level),
            // WildPokemon.Pidgeot=>new Wild("Roucarnage",83,80,75,91,70,Type.Normal,Type.Flying,45,172,new List<Move>{Move.Gust,Move.SandAttack,Move.QuickAttack},3,level),
            WildPokemon.Rattata => new Wild("Rattata", 30, 56, 35, 72, 25, Type.Normal, Type.Normal, 57,
                new List<Move> { Move.Tackle, Move.TailWhip }, 0, level),
            WildPokemon.Raticate => new Wild("Rattatac", 55, 81, 60, 97, 50, Type.Normal, Type.Normal, 116,
                new List<Move> { Move.Tackle, Move.TailWhip, Move.QuickAttack }, 0, level),
            WildPokemon.Spearow => new Wild("Piafabec", 40, 60, 30, 70, 31, Type.Normal, Type.Flying, 58,
                new List<Move> { Move.Peck, Move.Growl }, 0, level),
            WildPokemon.Fearow => new Wild("Rapasdepic", 65, 90, 65, 100, 61, Type.Normal, Type.Flying, 162,
                new List<Move> { Move.Peck, Move.Growl, Move.Leer }, 0, level),
            WildPokemon.Ekans => new Wild("Abo", 35, 60, 44, 55, 40, Type.Poison, Type.Poison, 62,
                new List<Move> { Move.Wrap, Move.Leer }, 0, level),
            WildPokemon.Arbok => new Wild("Arbok", 60, 85, 69, 80, 65, Type.Poison, Type.Poison, 147,
                new List<Move> { Move.Wrap, Move.Leer, Move.PoisonSting }, 0, level),
            WildPokemon.Pikachu => new Wild("Pikachu", 35, 55, 30, 90, 50, Type.Electric, Type.Electric, 82,
                new List<Move> { Move.Thundershock, Move.Growl }, 0, level),
            WildPokemon.Raichu => new Wild("Raichu", 60, 90, 55, 100, 90, Type.Electric, Type.Electric, 122,
                new List<Move> { Move.Thundershock, Move.Growl, Move.ThunderWave }, 0, level),
            WildPokemon.Sandshrew => new Wild("Sabelette", 50, 75, 85, 40, 30, Type.Ground, Type.Ground, 93,
                new List<Move> { Move.Scratch }, 0, level),
            WildPokemon.Sandslash => new Wild("Sablaireau", 75, 100, 110, 65, 55, Type.Ground, Type.Ground, 163,
                new List<Move> { Move.Scratch, Move.SandAttack }, 0, level),
            WildPokemon.NidoranF => new Wild("Nidoran♀", 55, 47, 52, 41, 40, Type.Poison, Type.Poison, 59,
                new List<Move> { Move.Growl, Move.Tackle }, 3, level),
            WildPokemon.Nidorina => new Wild("Nidorina", 70, 62, 67, 56, 55, Type.Poison, Type.Poison, 117,
                new List<Move> { Move.Growl, Move.Tackle, Move.Scratch }, 3, level),
            // WildPokemon.Nidoqueen=>new Wild("Nidoqueen",90,82,87,76,75,Type.Poison,Type.Ground,45,194,new List<Move>{Move.Tackle,Move.Scratch,Move.TailWhip,Move.BodySlam},3,level),
            WildPokemon.NidoranM => new Wild("Nidoran♂", 46, 57, 40, 50, 40, Type.Poison, Type.Poison, 60,
                new List<Move> { Move.Leer, Move.Tackle }, 3, level),
            WildPokemon.Nidorino => new Wild("Nidorino", 61, 72, 57, 65, 55, Type.Poison, Type.Poison, 118,
                new List<Move> { Move.Leer, Move.Tackle, Move.HornAttack }, 3, level),
            // WildPokemon.Nidoking=>new Wild("Nidoking",81,92,77,85,75,Type.Poison,Type.Ground,45,195,new List<Move>{Move.Tackle,Move.HornAttack,Move.PoisonSting,Move.Thrash},3,level),
            WildPokemon.Clefairy => new Wild("Mélofée", 70, 45, 48, 35, 60, Type.Normal, Type.Normal, 68,
                new List<Move> { Move.Pound, Move.Growl }, 4, level),
            // WildPokemon.Clefable=>new Wild("Mélodelfe",95,70,73,60,85,Type.Normal,Type.Normal,25,129,new List<Move>{Move.Sing,Move.Doubleslap,Move.Minimize,Move.Metronome},4,level),
            WildPokemon.Vulpix => new Wild("Goupix", 38, 41, 40, 65, 65, Type.Fire, Type.Fire, 63,
                new List<Move> { Move.Ember, Move.TailWhip }, 0, level),
            // WildPokemon.Ninetales=>new Wild("Feunard",73,76,75,100,100,Type.Fire,Type.Fire,75,178,new List<Move>{Move.Ember,Move.TailWhip,Move.QuickAttack,Move.Roar},0,level),
            WildPokemon.Jigglypuff => new Wild("Rondoudou", 115, 45, 20, 20, 25, Type.Normal, Type.Normal, 76,
                new List<Move> { Move.Sing }, 4, level),
            WildPokemon.Wigglytuff => new Wild("Grodoudou", 140, 70, 45, 45, 50, Type.Normal, Type.Normal, 109,
                new List<Move> { Move.Sing, Move.Disable, Move.DefenseCurl, Move.Doubleslap }, 4, level),
            WildPokemon.Zubat => new Wild("Nosferapti", 40, 45, 35, 55, 40, Type.Poison, Type.Flying, 54,
                new List<Move> { Move.LeechLife }, 0, level),
            WildPokemon.Golbat => new Wild("Nosferalto", 75, 80, 70, 90, 75, Type.Poison, Type.Flying, 171,
                new List<Move> { Move.LeechLife, Move.Screech, Move.Bite }, 0, level),
            WildPokemon.Oddish => new Wild("Mystherbe", 45, 50, 55, 30, 75, Type.Grass, Type.Poison, 78,
                new List<Move> { Move.Absorb }, 3, level),
            WildPokemon.Gloom => new Wild("Ortide", 60, 65, 70, 40, 85, Type.Grass, Type.Poison, 132,
                new List<Move> { Move.Absorb, Move.Poisonpowder, Move.StunSpore }, 3, level),
            // WildPokemon.Vileplume=>new Wild("Rafflesia",75,80,85,50,100,Type.Grass,Type.Poison,45,184,new List<Move>{Move.StunSpore,Move.SleepPowder,Move.Acid,Move.PetalDance},3,level),
            WildPokemon.Paras => new Wild("Paras", 35, 70, 55, 25, 55, Type.Bug, Type.Grass, 70,
                new List<Move> { Move.Scratch }, 0, level),
            WildPokemon.Parasect => new Wild("Parasect", 60, 95, 80, 30, 80, Type.Bug, Type.Grass, 128,
                new List<Move> { Move.Scratch, Move.StunSpore, Move.LeechLife }, 0, level),
            WildPokemon.Venonat => new Wild("Mimitoss", 60, 55, 50, 45, 40, Type.Bug, Type.Poison, 75,
                new List<Move> { Move.Tackle, Move.Disable }, 0, level),
            WildPokemon.Venomoth => new Wild("Aéromite", 70, 65, 60, 90, 90, Type.Bug, Type.Poison, 138,
                new List<Move> { Move.Tackle, Move.Disable, Move.Poisonpowder, Move.LeechLife }, 0, level),
            WildPokemon.Diglett => new Wild("Taupiqueur", 10, 55, 25, 95, 45, Type.Ground, Type.Ground, 81,
                new List<Move> { Move.Scratch }, 0, level),
            WildPokemon.Dugtrio => new Wild("Triopikeur", 35, 80, 50, 120, 70, Type.Ground, Type.Ground, 153,
                new List<Move> { Move.Scratch, Move.Growl, Move.Dig }, 0, level),
            WildPokemon.Meowth => new Wild("Miaouss", 40, 45, 35, 90, 40, Type.Normal, Type.Normal, 69,
                new List<Move> { Move.Scratch, Move.Growl }, 0, level),
            // WildPokemon.Persian=>new Wild("Persian",65,70,60,115,65,Type.Normal,Type.Normal,90,148,new List<Move>{Move.Scratch,Move.Growl,Move.Bite,Move.Screech},0,level),
            WildPokemon.Psyduck => new Wild("Psykokwak", 50, 52, 48, 55, 50, Type.Water, Type.Water, 80,
                new List<Move> { Move.Scratch }, 0, level),
            WildPokemon.Golduck => new Wild("Akwakwak", 80, 82, 78, 85, 80, Type.Water, Type.Water, 174,
                new List<Move> { Move.Scratch, Move.TailWhip, Move.Disable }, 0, level),
            WildPokemon.Mankey => new Wild("Férosinge", 40, 80, 35, 70, 35, Type.Fighting, Type.Fighting, 74,
                new List<Move> { Move.Scratch, Move.Leer }, 0, level),
            // WildPokemon.Primeape=>new Wild("Colossinge",65,105,60,95,60,Type.Fighting,Type.Fighting,75,149,new List<Move>{Move.Scratch,Move.Leer,Move.KarateChop,Move.FurySwipes},0,level),
            WildPokemon.Growlithe => new Wild("Caninos", 55, 70, 45, 60, 50, Type.Fire, Type.Fire, 91,
                new List<Move> { Move.Bite, Move.Roar }, 5, level),
            // WildPokemon.Arcanine=>new Wild("Arcanin",90,110,80,95,80,Type.Fire,Type.Fire,75,213,new List<Move>{Move.Roar,Move.Ember,Move.Leer,Move.TakeDown},5,level),
            // WildPokemon.Poliwag=>new Wild("Ptitard",40,50,40,90,40,Type.Water,Type.Water,255,77,new List<Move>{Move.Bubble},3,level),
            // WildPokemon.Poliwhirl=>new Wild("Têtarte",65,65,65,90,50,Type.Water,Type.Water,120,131,new List<Move>{Move.Bubble,Move.Hypnosis,Move.WaterGun},3,level),
            // WildPokemon.Poliwrath=>new Wild("Tartard",90,85,95,70,70,Type.Water,Type.Fighting,45,185,new List<Move>{Move.Hypnosis,Move.WaterGun,Move.Doubleslap,Move.BodySlam},3,level),
            WildPokemon.Abra => new Wild("Abra", 25, 20, 15, 90, 105, Type.Psychic, Type.Psychic, 73,
                new List<Move> { Move.Teleport }, 3, level),
            WildPokemon.Kadabra => new Wild("Kadabra", 40, 35, 30, 105, 120, Type.Psychic, Type.Psychic, 145,
                new List<Move> { Move.Teleport, Move.Confusion, Move.Disable }, 3, level),
            // WildPokemon.Alakazam=>new Wild("Alakazam",55,50,45,120,135,Type.Psychic,Type.Psychic,50,186,new List<Move>{Move.Teleport,Move.Confusion,Move.Disable},3,level),
            WildPokemon.Machop => new Wild("Machoc", 70, 80, 50, 35, 35, Type.Fighting, Type.Fighting, 88,
                new List<Move> { Move.KarateChop }, 3, level),
            WildPokemon.Machoke => new Wild("Machopeur", 80, 100, 70, 45, 50, Type.Fighting, Type.Fighting, 146,
                new List<Move> { Move.KarateChop, Move.LowKick, Move.Leer }, 3, level),
            // WildPokemon.Machamp=>new Wild("Mackogneur",90,130,80,55,65,Type.Fighting,Type.Fighting,45,193,new List<Move>{Move.KarateChop,Move.LowKick,Move.Leer},3,level),
            WildPokemon.Bellsprout => new Wild("Chétiflor", 50, 75, 35, 40, 70, Type.Grass, Type.Poison, 84,
                new List<Move> { Move.VineWhip, Move.Growth }, 3, level),
            WildPokemon.Weepinbell => new Wild("Boustiflor", 65, 90, 50, 55, 85, Type.Grass, Type.Poison, 151,
                new List<Move> { Move.VineWhip, Move.Growth, Move.Wrap }, 3, level),
            // WildPokemon.Victreebel=>new Wild("Empiflor",80,105,65,70,100,Type.Grass,Type.Poison,45,191,new List<Move>{Move.SleepPowder,Move.StunSpore,Move.Acid,Move.RazorLeaf},3,level),
            WildPokemon.Tentacool => new Wild("Tentacool", 40, 40, 35, 70, 100, Type.Water, Type.Poison, 105,
                new List<Move> { Move.Acid }, 5, level),
            // WildPokemon.Tentacruel=>new Wild("Tentacruel",80,70,65,100,120,Type.Water,Type.Poison,60,205,new List<Move>{Move.Acid,Move.Supersonic,Move.Wrap},5,level),
            WildPokemon.Geodude => new Wild("Racaillou", 40, 80, 100, 20, 30, Type.Rock, Type.Ground, 86,
                new List<Move> { Move.Tackle }, 3, level),
            WildPokemon.Graveler => new Wild("Gravalanch", 55, 95, 115, 35, 45, Type.Rock, Type.Ground, 134,
                new List<Move> { Move.Tackle, Move.DefenseCurl }, 3, level),
            // WildPokemon.Golem=>new Wild("Grolem",80,110,130,45,55,Type.Rock,Type.Ground,45,177,new List<Move>{Move.Tackle,Move.DefenseCurl},3,level),
            WildPokemon.Ponyta => new Wild("Ponyta", 50, 85, 55, 90, 65, Type.Fire, Type.Fire, 152,
                new List<Move> { Move.Ember }, 0, level),
            // WildPokemon.Rapidash=>new Wild("Galopa",65,100,70,105,80,Type.Fire,Type.Fire,60,192,new List<Move>{Move.Ember,Move.TailWhip,Move.Stomp,Move.Growl},0,level),
            WildPokemon.Slowpoke => new Wild("Ramoloss", 90, 65, 65, 15, 40, Type.Water, Type.Psychic, 99,
                new List<Move> { Move.Confusion }, 0, level),
            WildPokemon.Slowbro => new Wild("Flagadoss", 95, 75, 110, 30, 80, Type.Water, Type.Psychic, 164,
                new List<Move> { Move.Confusion, Move.Disable, Move.Headbutt }, 0, level),
            WildPokemon.Magnemite => new Wild("Magnéti", 25, 35, 70, 45, 95, Type.Electric, Type.Electric, 89,
                new List<Move> { Move.Tackle }, 0, level),
            WildPokemon.Magneton => new Wild("Magnéton", 50, 60, 95, 70, 120, Type.Electric, Type.Electric, 161,
                new List<Move> { Move.Tackle, Move.Sonicboom, Move.Thundershock }, 0, level),
            // WildPokemon.Farfetchd=>new Wild("Canarticho",52,65,55,60,58,Type.Normal,Type.Flying,45,94,new List<Move>{Move.Peck,Move.SandAttack},0,level),
            WildPokemon.Doduo => new Wild("Doduo", 35, 85, 45, 75, 35, Type.Normal, Type.Flying, 96,
                new List<Move> { Move.Peck }, 0, level),
            WildPokemon.Dodrio => new Wild("Dodrio", 60, 110, 70, 100, 60, Type.Normal, Type.Flying, 158,
                new List<Move> { Move.Peck, Move.Growl, Move.FuryAttack }, 0, level),
            WildPokemon.Seel => new Wild("Otaria", 65, 45, 55, 45, 70, Type.Water, Type.Water, 100,
                new List<Move> { Move.Headbutt }, 0, level),
            WildPokemon.Dewgong => new Wild("Lamantine", 90, 70, 80, 70, 95, Type.Water, Type.Ice, 176,
                new List<Move> { Move.Headbutt, Move.Growl, Move.AuroraBeam }, 0, level),
            WildPokemon.Grimer => new Wild("Tadmorv", 80, 80, 50, 25, 40, Type.Poison, Type.Poison, 90,
                new List<Move> { Move.Pound, Move.Disable }, 0, level),
            WildPokemon.Muk => new Wild("Grotadmorv", 105, 105, 75, 50, 65, Type.Poison, Type.Poison, 157,
                new List<Move> { Move.Pound, Move.Disable, Move.PoisonGas }, 0, level),
            WildPokemon.Shellder => new Wild("Kokiyas", 30, 65, 100, 40, 45, Type.Water, Type.Water, 97,
                new List<Move> { Move.Tackle, Move.Withdraw }, 5, level),
            // WildPokemon.Cloyster=>new Wild("Crustabri",50,95,180,70,85,Type.Water,Type.Ice,60,203,new List<Move>{Move.Withdraw,Move.Supersonic,Move.Clamp,Move.AuroraBeam},5,level),
            WildPokemon.Gastly => new Wild("Fantominus", 30, 35, 30, 80, 100, Type.Ghost, Type.Poison, 95,
                new List<Move> { Move.Lick, Move.ConfuseRay, Move.NightShade }, 3, level),
            WildPokemon.Haunter => new Wild("Spectrum", 45, 50, 45, 95, 115, Type.Ghost, Type.Poison, 126,
                new List<Move> { Move.Lick, Move.ConfuseRay, Move.NightShade }, 3, level),
            // WildPokemon.Gengar=>new Wild("Ectoplasma",60,65,60,110,130,Type.Ghost,Type.Poison,45,190,new List<Move>{Move.Lick,Move.ConfuseRay,Move.NightShade},3,level),
            WildPokemon.Onix => new Wild("Onix", 35, 45, 160, 70, 30, Type.Rock, Type.Ground, 108,
                new List<Move> { Move.Tackle, Move.Screech }, 0, level),
            WildPokemon.Drowzee => new Wild("Soporifik", 60, 48, 45, 42, 90, Type.Psychic, Type.Psychic, 102,
                new List<Move> { Move.Pound, Move.Hypnosis }, 0, level),
            WildPokemon.Hypno => new Wild("Hypnomade", 85, 73, 70, 67, 115, Type.Psychic, Type.Psychic, 165,
                new List<Move> { Move.Pound, Move.Hypnosis, Move.Disable, Move.Confusion }, 0, level),
            WildPokemon.Krabby => new Wild("Krabby", 30, 105, 90, 50, 25, Type.Water, Type.Water, 115,
                new List<Move> { Move.Bubble, Move.Leer }, 0, level),
            WildPokemon.Kingler => new Wild("Krabboss", 55, 130, 115, 75, 50, Type.Water, Type.Water, 206,
                new List<Move> { Move.Bubble, Move.Leer, Move.Vicegrip }, 0, level),
            WildPokemon.Voltorb => new Wild("Voltorbe", 40, 30, 50, 100, 55, Type.Electric, Type.Electric, 103,
                new List<Move> { Move.Tackle, Move.Screech }, 0, level),
            WildPokemon.Electrode => new Wild("Électrode", 60, 50, 70, 140, 80, Type.Electric, Type.Electric, 150,
                new List<Move> { Move.Tackle, Move.Screech, Move.Sonicboom }, 0, level),
            WildPokemon.Exeggcute => new Wild("Noeunoeuf", 60, 40, 80, 40, 60, Type.Grass, Type.Psychic, 98,
                new List<Move> { Move.Barrage, Move.Hypnosis }, 5, level),
            // WildPokemon.Exeggutor=>new Wild("Noadkoko",95,95,85,55,125,Type.Grass,Type.Psychic,45,212,new List<Move>{Move.Barrage,Move.Hypnosis},5,level),
            WildPokemon.Cubone => new Wild("Osselait", 50, 50, 95, 35, 40, Type.Ground, Type.Ground, 87,
                new List<Move> { Move.BoneClub, Move.Growl }, 0, level),
            WildPokemon.Marowak => new Wild("Ossatueur", 60, 80, 110, 45, 50, Type.Ground, Type.Ground, 124,
                new List<Move> { Move.BoneClub, Move.Growl, Move.Leer, Move.FocusEnergy }, 0, level),
            // WildPokemon.Hitmonlee=>new Wild("Kicklee",50,120,53,87,35,Type.Fighting,Type.Fighting,45,139,new List<Move>{Move.DoubleKick,Move.Meditate},0,level),
            // WildPokemon.Hitmonchan=>new Wild("Tygnon",50,105,79,76,35,Type.Fighting,Type.Fighting,45,140,new List<Move>{Move.CometPunch,Move.Agility},0,level),
            // WildPokemon.Lickitung=>new Wild("Excelangue",90,55,75,30,60,Type.Normal,Type.Normal,45,127,new List<Move>{Move.Wrap,Move.Supersonic},0,level),
            WildPokemon.Koffing => new Wild("Smogo", 40, 65, 95, 35, 60, Type.Poison, Type.Poison, 114,
                new List<Move> { Move.Tackle, Move.Smog }, 0, level),
            WildPokemon.Weezing => new Wild("Smogogo", 65, 90, 120, 60, 85, Type.Poison, Type.Poison, 173,
                new List<Move> { Move.Tackle, Move.Smog, Move.Sludge }, 0, level),
            WildPokemon.Rhyhorn => new Wild("Rhinocorne", 80, 85, 95, 25, 30, Type.Ground, Type.Rock, 135,
                new List<Move> { Move.HornAttack }, 5, level),
            WildPokemon.Rhydon => new Wild("Rhinoféros", 105, 130, 120, 40, 45, Type.Ground, Type.Rock, 204,
                new List<Move> { Move.HornAttack, Move.Stomp, Move.TailWhip, Move.FuryAttack }, 5, level),
            WildPokemon.Chansey => new Wild("Leveinard", 250, 5, 5, 50, 105, Type.Normal, Type.Normal, 255,
                new List<Move> { Move.Pound, Move.Doubleslap }, 4, level),
            WildPokemon.Tangela => new Wild("Saquedeneu", 65, 55, 115, 60, 100, Type.Grass, Type.Grass, 166,
                new List<Move> { Move.Constrict, Move.Bind }, 0, level),
            WildPokemon.Kangaskhan => new Wild("Kangourex", 105, 95, 80, 90, 40, Type.Normal, Type.Normal, 175,
                new List<Move> { Move.CometPunch, Move.Rage }, 0, level),
            WildPokemon.Horsea => new Wild("Hypotrempe", 30, 40, 70, 60, 70, Type.Water, Type.Water, 83,
                new List<Move> { Move.Bubble }, 0, level),
            WildPokemon.Seadra => new Wild("Hypocéan", 55, 65, 95, 85, 95, Type.Water, Type.Water, 155,
                new List<Move> { Move.Bubble, Move.Smokescreen }, 0, level),
            // WildPokemon.Goldeen=>new Wild("Poissirène",45,67,60,63,50,Type.Water,Type.Water,225,111,new List<Move>{Move.Peck,Move.TailWhip},0,level),
            // WildPokemon.Seaking=>new Wild("Poissoroy",80,92,65,68,80,Type.Water,Type.Water,60,170,new List<Move>{Move.Peck,Move.TailWhip,Move.Supersonic},0,level),
            WildPokemon.Staryu => new Wild("Stari", 30, 45, 55, 85, 70, Type.Water, Type.Water, 106,
                new List<Move> { Move.Tackle }, 5, level),
            // WildPokemon.Starmie=>new Wild("Staross",60,75,85,115,100,Type.Water,Type.Psychic,60,207,new List<Move>{Move.Tackle,Move.WaterGun,Move.Harden},5,level),
            // WildPokemon.MrMime=>new Wild("M. Mime",40,45,65,90,100,Type.Psychic,Type.Psychic,45,136,new List<Move>{Move.Confusion,Move.Barrier},0,level),
            WildPokemon.Scyther => new Wild("Insécateur", 70, 110, 80, 105, 55, Type.Bug, Type.Flying, 187,
                new List<Move> { Move.QuickAttack }, 0, level),
            // WildPokemon.Jynx=>new Wild("Lippoutou",65,50,35,95,95,Type.Ice,Type.Psychic,45,137,new List<Move>{Move.Pound,Move.LovelyKiss},0,level),
            WildPokemon.Electabuzz => new Wild("Élektek", 65, 83, 57, 105, 85, Type.Electric, Type.Electric, 156,
                new List<Move> { Move.QuickAttack, Move.Leer }, 0, level),
            WildPokemon.Magmar => new Wild("Magmar", 65, 95, 57, 93, 85, Type.Fire, Type.Fire, 167,
                new List<Move> { Move.Ember }, 0, level),
            WildPokemon.Pinsir => new Wild("Scarabrute", 65, 125, 100, 85, 55, Type.Bug, Type.Bug, 200,
                new List<Move> { Move.Vicegrip }, 5, level),
            WildPokemon.Tauros => new Wild("Tauros", 75, 100, 95, 110, 70, Type.Normal, Type.Normal, 211,
                new List<Move> { Move.Tackle }, 5, level),
            // WildPokemon.Magikarp=>new Wild("Magicarpe",20,10,55,80,20,Type.Water,Type.Water,255,20,new List<Move>{Move.Splash},5,level),
            // WildPokemon.Gyarados=>new Wild("Léviator",95,125,79,81,100,Type.Water,Type.Flying,45,214,new List<Move>{Move.Bite,Move.DragonRage,Move.Leer,Move.HydroPump},5,level),
            // WildPokemon.Lapras=>new Wild("Lokhlass",130,85,80,60,95,Type.Water,Type.Ice,45,219,new List<Move>{Move.WaterGun,Move.Growl},5,level),
            WildPokemon.Ditto => new Wild("Métamorph", 48, 48, 48, 48, 48, Type.Normal, Type.Normal, 61,
                new List<Move> { Move.Transform }, 0, level),
            // WildPokemon.Eevee=>new Wild("Évoli",55,55,50,55,65,Type.Normal,Type.Normal,45,92,new List<Move>{Move.Tackle,Move.SandAttack},0,level),
            // WildPokemon.Vaporeon=>new Wild("Aquali",130,65,60,65,110,Type.Water,Type.Water,45,196,new List<Move>{Move.Tackle,Move.SandAttack,Move.QuickAttack,Move.WaterGun},0,level),
            // WildPokemon.Jolteon=>new Wild("Voltali",65,65,60,130,110,Type.Electric,Type.Electric,45,197,new List<Move>{Move.Tackle,Move.SandAttack,Move.QuickAttack,Move.Thundershock},0,level),
            // WildPokemon.Flareon=>new Wild("Pyroli",65,130,60,65,110,Type.Fire,Type.Fire,45,198,new List<Move>{Move.Tackle,Move.SandAttack,Move.QuickAttack,Move.Ember},0,level),
            // WildPokemon.Porygon=>new Wild("Porygon",65,60,70,40,75,Type.Normal,Type.Normal,45,130,new List<Move>{Move.Tackle,Move.Sharpen,Move.Conversion},0,level),
            // WildPokemon.Omanyte=>new Wild("Amonita",35,40,100,35,90,Type.Rock,Type.Water,45,120,new List<Move>{Move.WaterGun,Move.Withdraw},0,level),
            // WildPokemon.Omastar=>new Wild("Amonistar",70,60,125,55,115,Type.Rock,Type.Water,45,199,new List<Move>{Move.WaterGun,Move.Withdraw,Move.HornAttack},0,level),
            // WildPokemon.Kabuto=>new Wild("Kabuto",30,80,90,55,45,Type.Rock,Type.Water,45,119,new List<Move>{Move.Scratch,Move.Harden},0,level),
            // WildPokemon.Kabutops=>new Wild("Kabutops",60,115,105,80,70,Type.Rock,Type.Water,45,201,new List<Move>{Move.Scratch,Move.Harden,Move.Absorb},0,level),
            // WildPokemon.Aerodactyl=>new Wild("Ptéra",80,105,65,130,60,Type.Rock,Type.Flying,45,202,new List<Move>{Move.WingAttack,Move.Agility},5,level),
            // WildPokemon.Snorlax=>new Wild("Ronflex",160,110,65,30,65,Type.Normal,Type.Normal,25,154,new List<Move>{Move.Headbutt,Move.Amnesia,Move.Rest},5,level),
            // WildPokemon.Articuno=>new Wild("Artikodin",90,85,100,85,125,Type.Ice,Type.Flying,3,215,new List<Move>{Move.Peck,Move.IceBeam},5,level),
            // WildPokemon.Zapdos=>new Wild("Électhor",90,90,85,100,125,Type.Electric,Type.Flying,3,216,new List<Move>{Move.Thundershock,Move.DrillPeck},5,level),
            // WildPokemon.Moltres=>new Wild("Sulfura",90,100,90,90,125,Type.Fire,Type.Flying,3,217,new List<Move>{Move.Peck,Move.FireSpin},5,level),
            // WildPokemon.Dratini=>new Wild("Minidraco",41,64,45,50,50,Type.Dragon,Type.Dragon,45,67,new List<Move>{Move.Wrap,Move.Leer},5,level),
            // WildPokemon.Dragonair=>new Wild("Draco",61,84,65,70,70,Type.Dragon,Type.Dragon,45,144,new List<Move>{Move.Wrap,Move.Leer,Move.ThunderWave},5,level),
            // WildPokemon.Dragonite=>new Wild("Dracolosse",91,134,95,80,100,Type.Dragon,Type.Flying,45,218,new List<Move>{Move.Wrap,Move.Leer,Move.ThunderWave,Move.Agility},5,level),
            // WildPokemon.Mewtwo=>new Wild("Mewtwo",106,110,90,130,154,Type.Psychic,Type.Psychic,3,220,new List<Move>{Move.Confusion,Move.Disable,Move.Swift,Move.PsychicM},5,level),
            // WildPokemon.Mew=>new Wild("Mew",100,100,100,100,100,Type.Psychic,Type.Psychic,45,64,new List<Move>{Move.Pound},3,level),

            _ => throw new ArgumentOutOfRangeException(nameof(wildPokemon), wildPokemon, null)
        };
    }
}