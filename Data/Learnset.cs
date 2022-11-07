using PokemonRPG.Enums;

namespace PokemonRPG.Data;

public class Learnset
{
    private Learnset(int level, Move move)
    {
        Level = level;
        Move = move;
    }

    public int Level { get; }
    public Move Move { get; }
    
    public static Learnset[]? GetLearnset(Dex dex)
    {
        return dex switch
        {
            Dex.Bulbasaur => new Learnset[]
            {
                new(7, Move.LeechSeed), new(13, Move.VineWhip), new(20, Move.Poisonpowder), new(27, Move.RazorLeaf),
                new(34, Move.Growth), new(41, Move.SleepPowder), new(48, Move.Solarbeam)
            },
            Dex.Ivysaur => new Learnset[]
            {
                new(7, Move.LeechSeed), new(13, Move.VineWhip), new(22, Move.Poisonpowder), new(30, Move.RazorLeaf),
                new(38, Move.Growth), new(46, Move.SleepPowder), new(54, Move.Solarbeam)
            },
            Dex.Venusaur => new Learnset[]
            {
                new(7, Move.LeechSeed), new(13, Move.VineWhip), new(22, Move.Poisonpowder), new(30, Move.RazorLeaf),
                new(43, Move.Growth), new(55, Move.SleepPowder), new(65, Move.Solarbeam)
            },
            Dex.Charmander => new Learnset[]
            {
                new(9, Move.Ember), new(15, Move.Leer), new(22, Move.Rage), new(30, Move.Slash),
                new(38, Move.Flamethrower), new(46, Move.FireSpin)
            },
            Dex.Charmeleon => new Learnset[]
            {
                new(9, Move.Ember), new(15, Move.Leer), new(24, Move.Rage), new(33, Move.Slash),
                new(42, Move.Flamethrower), new(56, Move.FireSpin)
            },
            Dex.Charizard => new Learnset[]
            {
                new(9, Move.Ember), new(15, Move.Leer), new(24, Move.Rage), new(36, Move.Slash),
                new(46, Move.Flamethrower), new(55, Move.FireSpin)
            },
            Dex.Squirtle => new Learnset[]
            {
                new(8, Move.Bubble), new(15, Move.WaterGun), new(22, Move.Bite), new(28, Move.Withdraw),
                new(35, Move.SkullBash), new(42, Move.HydroPump)
            },
            Dex.Wartortle => new Learnset[]
            {
                new(8, Move.Bubble), new(15, Move.WaterGun), new(24, Move.Bite), new(31, Move.Withdraw),
                new(39, Move.SkullBash), new(47, Move.HydroPump)
            },
            Dex.Blastoise => new Learnset[]
            {
                new(8, Move.Bubble), new(15, Move.WaterGun), new(24, Move.Bite), new(31, Move.Withdraw),
                new(42, Move.SkullBash), new(52, Move.HydroPump)
            },
            Dex.Caterpie => null,
            Dex.Metapod => null,
            Dex.Butterfree => new Learnset[]
            {
                new(12, Move.Confusion), new(15, Move.Poisonpowder), new(16, Move.StunSpore), new(17, Move.SleepPowder),
                new(21, Move.Supersonic), new(26, Move.Whirlwind), new(32, Move.Psybeam)
            },
            Dex.Weedle => null,
            Dex.Kakuna => null,
            Dex.Beedrill => new Learnset[]
            {
                new(12, Move.FuryAttack), new(16, Move.FocusEnergy), new(20, Move.Twineedle), new(25, Move.Rage),
                new(30, Move.PinMissile), new(35, Move.Agility)
            },
            Dex.Pidgey => new Learnset[]
            {
                new(5, Move.SandAttack), new(12, Move.QuickAttack), new(19, Move.Whirlwind), new(28, Move.WingAttack),
                new(36, Move.Agility), new(44, Move.MirrorMove)
            },
            Dex.Pidgeotto => new Learnset[]
            {
                new(5, Move.SandAttack), new(12, Move.QuickAttack), new(21, Move.Whirlwind), new(31, Move.WingAttack),
                new(40, Move.Agility), new(49, Move.MirrorMove)
            },
            Dex.Pidgeot => new Learnset[]
            {
                new(5, Move.SandAttack), new(12, Move.QuickAttack), new(21, Move.Whirlwind), new(31, Move.WingAttack),
                new(44, Move.Agility), new(54, Move.MirrorMove)
            },
            Dex.Rattata => new Learnset[]
            {
                new(7, Move.QuickAttack), new(14, Move.HyperFang), new(23, Move.FocusEnergy), new(34, Move.SuperFang)
            },
            Dex.Raticate => new Learnset[]
            {
                new(7, Move.QuickAttack), new(14, Move.HyperFang), new(27, Move.FocusEnergy), new(41, Move.SuperFang)
            },
            Dex.Spearow => new Learnset[]
            {
                new(9, Move.Leer), new(15, Move.FuryAttack), new(22, Move.MirrorMove), new(29, Move.DrillPeck),
                new(36, Move.Agility)
            },
            Dex.Fearow => new Learnset[]
            {
                new(9, Move.Leer), new(15, Move.FuryAttack), new(25, Move.MirrorMove), new(34, Move.DrillPeck),
                new(43, Move.Agility)
            },
            Dex.Ekans => new Learnset[]
            {
                new(10, Move.PoisonSting), new(17, Move.Bite), new(24, Move.Glare), new(31, Move.Screech),
                new(38, Move.Acid)
            },
            Dex.Arbok => new Learnset[]
            {
                new(10, Move.PoisonSting), new(17, Move.Bite), new(27, Move.Glare), new(36, Move.Screech),
                new(47, Move.Acid)
            },
            Dex.Pikachu => new Learnset[]
            {
                new(9, Move.ThunderWave), new(16, Move.QuickAttack), new(26, Move.Swift), new(33, Move.Agility),
                new(43, Move.Thunder)
            },
            Dex.Raichu => null,
            Dex.Sandshrew => new Learnset[]
            {
                new(10, Move.SandAttack), new(17, Move.Slash), new(24, Move.PoisonSting), new(31, Move.Swift),
                new(38, Move.FurySwipes)
            },
            Dex.Sandslash => new Learnset[]
            {
                new(10, Move.SandAttack), new(17, Move.Slash), new(27, Move.PoisonSting), new(36, Move.Swift),
                new(47, Move.FurySwipes)
            },
            Dex.NidoranF => new Learnset[]
            {
                new(8, Move.Scratch), new(14, Move.PoisonSting), new(21, Move.TailWhip), new(29, Move.Bite),
                new(36, Move.FurySwipes), new(43, Move.DoubleKick)
            },
            Dex.Nidorina => new Learnset[]
            {
                new(8, Move.Scratch), new(14, Move.PoisonSting), new(23, Move.TailWhip), new(32, Move.Bite),
                new(41, Move.FurySwipes), new(50, Move.DoubleKick)
            },
            Dex.Nidoqueen => new Learnset[] { new(8, Move.Scratch), new(14, Move.PoisonSting), new(23, Move.BodySlam) },
            Dex.NidoranM => new Learnset[]
            {
                new(8, Move.HornAttack), new(14, Move.PoisonSting), new(21, Move.FocusEnergy), new(29, Move.FuryAttack),
                new(36, Move.HornDrill), new(43, Move.DoubleKick)
            },
            Dex.Nidorino => new Learnset[]
            {
                new(8, Move.HornAttack), new(14, Move.PoisonSting), new(23, Move.FocusEnergy), new(32, Move.FuryAttack),
                new(41, Move.HornDrill), new(50, Move.DoubleKick)
            },
            Dex.Nidoking => new Learnset[] { new(8, Move.HornAttack), new(14, Move.PoisonSting), new(23, Move.Thrash) },
            Dex.Clefairy => new Learnset[]
            {
                new(13, Move.Sing), new(18, Move.Doubleslap), new(24, Move.Minimize), new(31, Move.Metronome),
                new(39, Move.DefenseCurl), new(48, Move.LightScreen)
            },
            Dex.Clefable => null,
            Dex.Vulpix => new Learnset[]
            {
                new(16, Move.QuickAttack), new(21, Move.Roar), new(28, Move.ConfuseRay), new(35, Move.Flamethrower),
                new(42, Move.FireSpin)
            },
            Dex.Ninetales => null,
            Dex.Jigglypuff => new Learnset[]
            {
                new(9, Move.Pound), new(14, Move.Disable), new(19, Move.DefenseCurl), new(24, Move.Doubleslap),
                new(29, Move.Rest), new(34, Move.BodySlam), new(39, Move.DoubleEdge)
            },
            Dex.Wigglytuff => null,
            Dex.Zubat => new Learnset[]
            {
                new(10, Move.Supersonic), new(15, Move.Bite), new(21, Move.ConfuseRay), new(28, Move.WingAttack),
                new(36, Move.Haze)
            },
            Dex.Golbat => new Learnset[]
            {
                new(10, Move.Supersonic), new(15, Move.Bite), new(21, Move.ConfuseRay), new(32, Move.WingAttack),
                new(43, Move.Haze)
            },
            Dex.Oddish => new Learnset[]
            {
                new(15, Move.Poisonpowder), new(17, Move.StunSpore), new(19, Move.SleepPowder), new(24, Move.Acid),
                new(33, Move.PetalDance), new(46, Move.Solarbeam)
            },
            Dex.Gloom => new Learnset[]
            {
                new(15, Move.Poisonpowder), new(17, Move.StunSpore), new(19, Move.SleepPowder), new(28, Move.Acid),
                new(38, Move.PetalDance), new(52, Move.Solarbeam)
            },
            Dex.Vileplume => new Learnset[]
                { new(15, Move.Poisonpowder), new(17, Move.StunSpore), new(19, Move.SleepPowder) },
            Dex.Paras => new Learnset[]
            {
                new(13, Move.StunSpore), new(20, Move.LeechLife), new(27, Move.Spore), new(34, Move.Slash),
                new(41, Move.Growth)
            },
            Dex.Parasect => new Learnset[]
            {
                new(13, Move.StunSpore), new(20, Move.LeechLife), new(30, Move.Spore), new(39, Move.Slash),
                new(48, Move.Growth)
            },
            Dex.Venonat => new Learnset[]
            {
                new(24, Move.Poisonpowder), new(27, Move.LeechLife), new(30, Move.StunSpore), new(35, Move.Psybeam),
                new(38, Move.SleepPowder), new(43, Move.PsychicM)
            },
            Dex.Venomoth => new Learnset[]
            {
                new(24, Move.Poisonpowder), new(27, Move.LeechLife), new(30, Move.StunSpore), new(38, Move.Psybeam),
                new(43, Move.SleepPowder), new(50, Move.PsychicM)
            },
            Dex.Diglett => new Learnset[]
            {
                new(15, Move.Growl), new(19, Move.Dig), new(24, Move.SandAttack), new(31, Move.Slash),
                new(40, Move.Earthquake)
            },
            Dex.Dugtrio => new Learnset[]
            {
                new(15, Move.Growl), new(19, Move.Dig), new(24, Move.SandAttack), new(35, Move.Slash),
                new(47, Move.Earthquake)
            },
            Dex.Meowth => new Learnset[]
            {
                new(12, Move.Bite), new(17, Move.PayDay), new(24, Move.Screech), new(33, Move.FurySwipes),
                new(44, Move.Slash)
            },
            Dex.Persian => new Learnset[]
            {
                new(12, Move.Bite), new(17, Move.PayDay), new(24, Move.Screech), new(37, Move.FurySwipes),
                new(51, Move.Slash)
            },
            Dex.Psyduck => new Learnset[]
            {
                new(28, Move.TailWhip), new(31, Move.Disable), new(36, Move.Confusion), new(43, Move.FurySwipes),
                new(52, Move.HydroPump)
            },
            Dex.Golduck => new Learnset[]
            {
                new(28, Move.TailWhip), new(31, Move.Disable), new(39, Move.Confusion), new(48, Move.FurySwipes),
                new(59, Move.HydroPump)
            },
            Dex.Mankey => new Learnset[]
            {
                new(15, Move.KarateChop), new(21, Move.FurySwipes), new(27, Move.FocusEnergy),
                new(33, Move.SeismicToss), new(39, Move.Thrash)
            },
            Dex.Primeape => new Learnset[]
            {
                new(15, Move.KarateChop), new(21, Move.FurySwipes), new(27, Move.FocusEnergy),
                new(37, Move.SeismicToss), new(46, Move.Thrash)
            },
            Dex.Growlithe => new Learnset[]
            {
                new(18, Move.Ember), new(23, Move.Leer), new(30, Move.TakeDown), new(39, Move.Agility),
                new(50, Move.Flamethrower)
            },
            Dex.Arcanine => null,
            Dex.Poliwag => new Learnset[]
            {
                new(16, Move.Hypnosis), new(19, Move.WaterGun), new(25, Move.Doubleslap), new(31, Move.BodySlam),
                new(38, Move.Amnesia), new(45, Move.HydroPump)
            },
            Dex.Poliwhirl => new Learnset[]
            {
                new(16, Move.Hypnosis), new(19, Move.WaterGun), new(26, Move.Doubleslap), new(33, Move.BodySlam),
                new(41, Move.Amnesia), new(49, Move.HydroPump)
            },
            Dex.Poliwrath => new Learnset[] { new(16, Move.Hypnosis), new(19, Move.WaterGun) },
            Dex.Abra => null,
            Dex.Kadabra => new Learnset[]
            {
                new(16, Move.Confusion), new(20, Move.Disable), new(27, Move.Psybeam), new(31, Move.Recover),
                new(38, Move.PsychicM), new(42, Move.Reflect)
            },
            Dex.Alakazam => new Learnset[]
            {
                new(16, Move.Confusion), new(20, Move.Disable), new(27, Move.Psybeam), new(31, Move.Recover),
                new(38, Move.PsychicM), new(42, Move.Reflect)
            },
            Dex.Machop => new Learnset[]
            {
                new(20, Move.LowKick), new(25, Move.Leer), new(32, Move.FocusEnergy), new(39, Move.SeismicToss),
                new(46, Move.Submission)
            },
            Dex.Machoke => new Learnset[]
            {
                new(20, Move.LowKick), new(25, Move.Leer), new(36, Move.FocusEnergy), new(44, Move.SeismicToss),
                new(52, Move.Submission)
            },
            Dex.Machamp => new Learnset[]
            {
                new(20, Move.LowKick), new(25, Move.Leer), new(36, Move.FocusEnergy), new(44, Move.SeismicToss),
                new(52, Move.Submission)
            },
            Dex.Bellsprout => new Learnset[]
            {
                new(13, Move.Wrap), new(15, Move.Poisonpowder), new(18, Move.SleepPowder), new(21, Move.StunSpore),
                new(26, Move.Acid), new(33, Move.RazorLeaf), new(42, Move.Slam)
            },
            Dex.Weepinbell => new Learnset[]
            {
                new(13, Move.Wrap), new(15, Move.Poisonpowder), new(18, Move.SleepPowder), new(23, Move.StunSpore),
                new(29, Move.Acid), new(38, Move.RazorLeaf), new(49, Move.Slam)
            },
            Dex.Victreebel => new Learnset[]
                { new(13, Move.Wrap), new(15, Move.Poisonpowder), new(18, Move.SleepPowder) },
            Dex.Tentacool => new Learnset[]
            {
                new(7, Move.Supersonic), new(13, Move.Wrap), new(18, Move.PoisonSting), new(22, Move.WaterGun),
                new(27, Move.Constrict), new(33, Move.Barrier), new(40, Move.Screech), new(48, Move.HydroPump)
            },
            Dex.Tentacruel => new Learnset[]
            {
                new(7, Move.Supersonic), new(13, Move.Wrap), new(18, Move.PoisonSting), new(22, Move.WaterGun),
                new(27, Move.Constrict), new(35, Move.Barrier), new(43, Move.Screech), new(50, Move.HydroPump)
            },
            Dex.Geodude => new Learnset[]
            {
                new(11, Move.DefenseCurl), new(16, Move.RockThrow), new(21, Move.Selfdestruct), new(26, Move.Harden),
                new(31, Move.Earthquake), new(36, Move.Explosion)
            },
            Dex.Graveler => new Learnset[]
            {
                new(11, Move.DefenseCurl), new(16, Move.RockThrow), new(21, Move.Selfdestruct), new(29, Move.Harden),
                new(36, Move.Earthquake), new(43, Move.Explosion)
            },
            Dex.Golem => new Learnset[]
            {
                new(11, Move.DefenseCurl), new(16, Move.RockThrow), new(21, Move.Selfdestruct), new(29, Move.Harden),
                new(36, Move.Earthquake), new(43, Move.Explosion)
            },
            Dex.Ponyta => new Learnset[]
            {
                new(30, Move.TailWhip), new(32, Move.Stomp), new(35, Move.Growl), new(39, Move.FireSpin),
                new(43, Move.TakeDown), new(48, Move.Agility)
            },
            Dex.Rapidash => new Learnset[]
            {
                new(30, Move.TailWhip), new(32, Move.Stomp), new(35, Move.Growl), new(39, Move.FireSpin),
                new(47, Move.TakeDown), new(55, Move.Agility)
            },
            Dex.Slowpoke => new Learnset[]
            {
                new(18, Move.Disable), new(22, Move.Headbutt), new(27, Move.Growl), new(33, Move.WaterGun),
                new(40, Move.Amnesia), new(48, Move.PsychicM)
            },
            Dex.Slowbro => new Learnset[]
            {
                new(18, Move.Disable), new(22, Move.Headbutt), new(27, Move.Growl), new(33, Move.WaterGun),
                new(37, Move.Withdraw), new(44, Move.Amnesia), new(55, Move.PsychicM)
            },
            Dex.Magnemite => new Learnset[]
            {
                new(21, Move.Sonicboom), new(25, Move.Thundershock), new(29, Move.Supersonic),
                new(35, Move.ThunderWave), new(41, Move.Swift), new(47, Move.Screech)
            },
            Dex.Magneton => new Learnset[]
            {
                new(21, Move.Sonicboom), new(25, Move.Thundershock), new(29, Move.Supersonic),
                new(38, Move.ThunderWave), new(46, Move.Swift), new(54, Move.Screech)
            },
            Dex.Farfetchd => new Learnset[]
            {
                new(7, Move.Leer), new(15, Move.FuryAttack), new(23, Move.SwordsDance), new(31, Move.Agility),
                new(39, Move.Slash)
            },
            Dex.Doduo => new Learnset[]
            {
                new(20, Move.Growl), new(24, Move.FuryAttack), new(30, Move.DrillPeck), new(36, Move.Rage),
                new(40, Move.TriAttack), new(44, Move.Agility)
            },
            Dex.Dodrio => new Learnset[]
            {
                new(20, Move.Growl), new(24, Move.FuryAttack), new(30, Move.DrillPeck), new(39, Move.Rage),
                new(45, Move.TriAttack), new(51, Move.Agility)
            },
            Dex.Seel => new Learnset[]
            {
                new(30, Move.Growl), new(35, Move.AuroraBeam), new(40, Move.Rest), new(45, Move.TakeDown),
                new(50, Move.IceBeam)
            },
            Dex.Dewgong => new Learnset[]
            {
                new(30, Move.Growl), new(35, Move.AuroraBeam), new(44, Move.Rest), new(50, Move.TakeDown),
                new(56, Move.IceBeam)
            },
            Dex.Grimer => new Learnset[]
            {
                new(30, Move.PoisonGas), new(33, Move.Minimize), new(37, Move.Sludge), new(42, Move.Harden),
                new(48, Move.Screech), new(55, Move.AcidArmor)
            },
            Dex.Muk => new Learnset[]
            {
                new(30, Move.PoisonGas), new(33, Move.Minimize), new(37, Move.Sludge), new(45, Move.Harden),
                new(53, Move.Screech), new(60, Move.AcidArmor)
            },
            Dex.Shellder => new Learnset[]
            {
                new(18, Move.Supersonic), new(23, Move.Clamp), new(30, Move.AuroraBeam), new(39, Move.Leer),
                new(50, Move.IceBeam)
            },
            Dex.Cloyster => new Learnset[] { new(50, Move.SpikeCannon) },
            Dex.Gastly => new Learnset[] { new(27, Move.Hypnosis), new(35, Move.DreamEater) },
            Dex.Haunter => new Learnset[] { new(29, Move.Hypnosis), new(38, Move.DreamEater) },
            Dex.Gengar => new Learnset[] { new(29, Move.Hypnosis), new(38, Move.DreamEater) },
            Dex.Onix => new Learnset[]
            {
                new(15, Move.Bind), new(19, Move.RockThrow), new(25, Move.Rage), new(33, Move.Slam),
                new(43, Move.Harden)
            },
            Dex.Drowzee => new Learnset[]
            {
                new(12, Move.Disable), new(17, Move.Confusion), new(24, Move.Headbutt), new(29, Move.PoisonGas),
                new(32, Move.PsychicM), new(37, Move.Meditate)
            },
            Dex.Hypno => new Learnset[]
            {
                new(12, Move.Disable), new(17, Move.Confusion), new(24, Move.Headbutt), new(33, Move.PoisonGas),
                new(37, Move.PsychicM), new(43, Move.Meditate)
            },
            Dex.Krabby => new Learnset[]
            {
                new(20, Move.Vicegrip), new(25, Move.Guillotine), new(30, Move.Stomp), new(35, Move.Crabhammer),
                new(40, Move.Harden)
            },
            Dex.Kingler => new Learnset[]
            {
                new(20, Move.Vicegrip), new(25, Move.Guillotine), new(34, Move.Stomp), new(42, Move.Crabhammer),
                new(49, Move.Harden)
            },
            Dex.Voltorb => new Learnset[]
            {
                new(17, Move.Sonicboom), new(22, Move.Selfdestruct), new(29, Move.LightScreen), new(36, Move.Swift),
                new(43, Move.Explosion)
            },
            Dex.Electrode => new Learnset[]
            {
                new(17, Move.Sonicboom), new(22, Move.Selfdestruct), new(29, Move.LightScreen), new(40, Move.Swift),
                new(50, Move.Explosion)
            },
            Dex.Exeggcute => new Learnset[]
            {
                new(25, Move.Reflect), new(28, Move.LeechSeed), new(32, Move.StunSpore), new(37, Move.Poisonpowder),
                new(42, Move.Solarbeam), new(48, Move.SleepPowder)
            },
            Dex.Exeggutor => new Learnset[] { new(28, Move.Stomp) },
            Dex.Cubone => new Learnset[]
            {
                new(25, Move.Leer), new(31, Move.FocusEnergy), new(38, Move.Thrash), new(43, Move.Bonemerang),
                new(46, Move.Rage)
            },
            Dex.Marowak => new Learnset[]
            {
                new(25, Move.Leer), new(33, Move.FocusEnergy), new(41, Move.Thrash), new(48, Move.Bonemerang),
                new(55, Move.Rage)
            },
            Dex.Hitmonlee => new Learnset[]
            {
                new(33, Move.RollingKick), new(38, Move.JumpKick), new(43, Move.FocusEnergy), new(48, Move.HiJumpKick),
                new(53, Move.MegaKick)
            },
            Dex.Hitmonchan => new Learnset[]
            {
                new(33, Move.FirePunch), new(38, Move.IcePunch), new(43, Move.Thunderpunch), new(48, Move.MegaPunch),
                new(53, Move.Counter)
            },
            Dex.Lickitung => new Learnset[]
            {
                new(7, Move.Stomp), new(15, Move.Disable), new(23, Move.DefenseCurl), new(31, Move.Slam),
                new(39, Move.Screech)
            },
            Dex.Koffing => new Learnset[]
            {
                new(32, Move.Sludge), new(37, Move.Smokescreen), new(40, Move.Selfdestruct), new(45, Move.Haze),
                new(48, Move.Explosion)
            },
            Dex.Weezing => new Learnset[]
            {
                new(32, Move.Sludge), new(39, Move.Smokescreen), new(43, Move.Selfdestruct), new(49, Move.Haze),
                new(53, Move.Explosion)
            },
            Dex.Rhyhorn => new Learnset[]
            {
                new(30, Move.Stomp), new(35, Move.TailWhip), new(40, Move.FuryAttack), new(45, Move.HornDrill),
                new(50, Move.Leer), new(55, Move.TakeDown)
            },
            Dex.Rhydon => new Learnset[]
            {
                new(30, Move.Stomp), new(35, Move.TailWhip), new(40, Move.FuryAttack), new(48, Move.HornDrill),
                new(55, Move.Leer), new(64, Move.TakeDown)
            },
            Dex.Chansey => new Learnset[]
            {
                new(24, Move.Sing), new(30, Move.Growl), new(38, Move.Minimize), new(44, Move.DefenseCurl),
                new(48, Move.LightScreen), new(54, Move.DoubleEdge)
            },
            Dex.Tangela => new Learnset[]
            {
                new(29, Move.Absorb), new(32, Move.Poisonpowder), new(36, Move.StunSpore), new(39, Move.SleepPowder),
                new(45, Move.Slam), new(49, Move.Growth)
            },
            Dex.Kangaskhan => new Learnset[]
            {
                new(26, Move.Bite), new(31, Move.TailWhip), new(36, Move.MegaPunch), new(41, Move.Leer),
                new(46, Move.DizzyPunch)
            },
            Dex.Horsea => new Learnset[]
            {
                new(19, Move.Smokescreen), new(24, Move.Leer), new(30, Move.WaterGun), new(37, Move.Agility),
                new(45, Move.HydroPump)
            },
            Dex.Seadra => new Learnset[]
            {
                new(19, Move.Smokescreen), new(24, Move.Leer), new(30, Move.WaterGun), new(41, Move.Agility),
                new(52, Move.HydroPump)
            },
            Dex.Goldeen => new Learnset[]
            {
                new(19, Move.Supersonic), new(24, Move.HornAttack), new(30, Move.FuryAttack), new(37, Move.Waterfall),
                new(45, Move.HornDrill), new(54, Move.Agility)
            },
            Dex.Seaking => new Learnset[]
            {
                new(19, Move.Supersonic), new(24, Move.HornAttack), new(30, Move.FuryAttack), new(39, Move.Waterfall),
                new(48, Move.HornDrill), new(54, Move.Agility)
            },
            Dex.Staryu => new Learnset[]
            {
                new(17, Move.WaterGun), new(22, Move.Harden), new(27, Move.Recover), new(32, Move.Swift),
                new(37, Move.Minimize), new(42, Move.LightScreen), new(47, Move.HydroPump)
            },
            Dex.Starmie => null,
            Dex.MrMime => new Learnset[]
            {
                new(15, Move.Confusion), new(23, Move.LightScreen), new(31, Move.Doubleslap), new(39, Move.Meditate),
                new(47, Move.Substitute)
            },
            Dex.Scyther => new Learnset[]
            {
                new(17, Move.Leer), new(20, Move.FocusEnergy), new(24, Move.DoubleTeam), new(29, Move.Slash),
                new(35, Move.SwordsDance), new(42, Move.Agility)
            },
            Dex.Jynx => new Learnset[]
            {
                new(18, Move.Lick), new(23, Move.Doubleslap), new(31, Move.IcePunch), new(39, Move.BodySlam),
                new(47, Move.Thrash), new(58, Move.Blizzard)
            },
            Dex.Electabuzz => new Learnset[]
            {
                new(34, Move.Thundershock), new(37, Move.Screech), new(42, Move.Thunderpunch),
                new(49, Move.LightScreen), new(54, Move.Thunder)
            },
            Dex.Magmar => new Learnset[]
            {
                new(36, Move.Leer), new(39, Move.ConfuseRay), new(43, Move.FirePunch), new(48, Move.Smokescreen),
                new(52, Move.Smog), new(55, Move.Flamethrower)
            },
            Dex.Pinsir => new Learnset[]
            {
                new(25, Move.SeismicToss), new(30, Move.Guillotine), new(36, Move.FocusEnergy), new(43, Move.Harden),
                new(49, Move.Slash), new(54, Move.SwordsDance)
            },
            Dex.Tauros => new Learnset[]
            {
                new(21, Move.Stomp), new(28, Move.TailWhip), new(35, Move.Leer), new(44, Move.Rage),
                new(51, Move.TakeDown)
            },
            Dex.Magikarp => new Learnset[] { new(15, Move.Tackle) },
            Dex.Gyarados => new Learnset[]
            {
                new(20, Move.Bite), new(25, Move.DragonRage), new(32, Move.Leer), new(41, Move.HydroPump),
                new(52, Move.HyperBeam)
            },
            Dex.Lapras => new Learnset[]
            {
                new(16, Move.Sing), new(20, Move.Mist), new(25, Move.BodySlam), new(31, Move.ConfuseRay),
                new(38, Move.IceBeam), new(46, Move.HydroPump)
            },
            Dex.Ditto => null,
            Dex.Eevee => new Learnset[]
                { new(27, Move.QuickAttack), new(31, Move.TailWhip), new(37, Move.Bite), new(45, Move.TakeDown) },
            Dex.Vaporeon => new Learnset[]
            {
                new(27, Move.QuickAttack), new(31, Move.WaterGun), new(37, Move.TailWhip), new(40, Move.Bite),
                new(42, Move.AcidArmor), new(44, Move.Haze), new(48, Move.Mist), new(54, Move.HydroPump)
            },
            Dex.Jolteon => new Learnset[]
            {
                new(27, Move.QuickAttack), new(31, Move.Thundershock), new(37, Move.TailWhip),
                new(40, Move.ThunderWave), new(42, Move.DoubleKick), new(44, Move.Agility), new(48, Move.PinMissile),
                new(54, Move.Thunder)
            },
            Dex.Flareon => new Learnset[]
            {
                new(27, Move.QuickAttack), new(31, Move.Ember), new(37, Move.TailWhip), new(40, Move.Bite),
                new(42, Move.Leer), new(44, Move.FireSpin), new(48, Move.Rage), new(54, Move.Flamethrower)
            },
            Dex.Porygon => new Learnset[]
                { new(23, Move.Psybeam), new(28, Move.Recover), new(35, Move.Agility), new(42, Move.TriAttack) },
            Dex.Omanyte => new Learnset[]
                { new(34, Move.HornAttack), new(39, Move.Leer), new(46, Move.SpikeCannon), new(53, Move.HydroPump) },
            Dex.Omastar => new Learnset[]
                { new(34, Move.HornAttack), new(39, Move.Leer), new(44, Move.SpikeCannon), new(49, Move.HydroPump) },
            Dex.Kabuto => new Learnset[]
                { new(34, Move.Absorb), new(39, Move.Slash), new(44, Move.Leer), new(49, Move.HydroPump) },
            Dex.Kabutops => new Learnset[]
                { new(34, Move.Absorb), new(39, Move.Slash), new(46, Move.Leer), new(53, Move.HydroPump) },
            Dex.Aerodactyl => new Learnset[]
                { new(33, Move.Supersonic), new(38, Move.Bite), new(45, Move.TakeDown), new(54, Move.HyperBeam) },
            Dex.Snorlax => new Learnset[]
                { new(35, Move.BodySlam), new(41, Move.Harden), new(48, Move.DoubleEdge), new(56, Move.HyperBeam) },
            Dex.Articuno => new Learnset[] { new(51, Move.Blizzard), new(55, Move.Agility), new(60, Move.Mist) },
            Dex.Zapdos => new Learnset[] { new(51, Move.Thunder), new(55, Move.Agility), new(60, Move.LightScreen) },
            Dex.Moltres => new Learnset[] { new(51, Move.Leer), new(55, Move.Agility), new(60, Move.SkyAttack) },
            Dex.Dratini => new Learnset[]
            {
                new(10, Move.ThunderWave), new(20, Move.Agility), new(30, Move.Slam), new(40, Move.DragonRage),
                new(50, Move.HyperBeam)
            },
            Dex.Dragonair => new Learnset[]
            {
                new(10, Move.ThunderWave), new(20, Move.Agility), new(35, Move.Slam), new(45, Move.DragonRage),
                new(55, Move.HyperBeam)
            },
            Dex.Dragonite => new Learnset[]
            {
                new(10, Move.ThunderWave), new(20, Move.Agility), new(35, Move.Slam), new(45, Move.DragonRage),
                new(60, Move.HyperBeam)
            },
            Dex.Mewtwo => new Learnset[]
            {
                new(63, Move.Barrier), new(66, Move.PsychicM), new(70, Move.Recover), new(75, Move.Mist),
                new(81, Move.Amnesia)
            },
            Dex.Mew => new Learnset[]
                { new(10, Move.Transform), new(20, Move.MegaPunch), new(30, Move.Metronome), new(40, Move.PsychicM) },
            _ => throw new ArgumentOutOfRangeException(nameof(dex), dex, null)
        };
    }
}