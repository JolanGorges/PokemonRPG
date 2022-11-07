using PokemonRPG.Enums;
using Type = PokemonRPG.Enums.Type;

namespace PokemonRPG.Data;

public class Starter : Pokemon
{
    private StarterPokemon _currentEvolution;

    private Starter(StarterPokemon currentEvolution, string name, int hp, int attack, int defense, int speed,
        int special, Type type1, Type type2,
        IEnumerable<Move> moves, int growthRate) : base(name, hp, attack, defense,
        speed, special, type1, type2, 0, moves, growthRate, 5)
    {
        _currentEvolution = currentEvolution;
    }

    public void CheckEvolutionsAndMoves()
    {
        var moves = Moves.Select(x => x.Move).ToArray();
        switch (_currentEvolution)
        {
            case StarterPokemon.Bulbasaur:
                if (Level >= 13 && !moves.Contains(Move.VineWhip))
                    Moves.Add(MoveClass.GetMove(Move.VineWhip));
                if (Level >= 16)
                {
                    Name = "Herbizarre";
                    Hp.Base = 60;
                    Attack.Base = 62;
                    Defense.Base = 63;
                    Speed.Base = 60;
                    Special.Base = 80;
                    _currentEvolution = StarterPokemon.Ivysaur;
                }

                break;
            case StarterPokemon.Ivysaur:
                if (Level >= 30 && !moves.Contains(Move.RazorLeaf))
                    Moves.Add(MoveClass.GetMove(Move.RazorLeaf));
                if (Level >= 32)
                {
                    Name = "Florizarre";
                    Hp.Base = 80;
                    Attack.Base = 82;
                    Defense.Base = 83;
                    Speed.Base = 80;
                    Special.Base = 100;
                    _currentEvolution = StarterPokemon.Venusaur;
                }

                break;
            case StarterPokemon.Venusaur:
                break;
            case StarterPokemon.Charmander:
                if (Level >= 16)
                {
                    Name = "Reptincel";
                    Hp.Base = 58;
                    Attack.Base = 64;
                    Defense.Base = 58;
                    Speed.Base = 80;
                    Special.Base = 65;
                    _currentEvolution = StarterPokemon.Charmeleon;
                }

                break;
            case StarterPokemon.Charmeleon:
                if (Level >= 24 && !moves.Contains(Move.Rage))
                    Moves.Add(MoveClass.GetMove(Move.Rage));
                if (Level >= 33 && !moves.Contains(Move.Slash))
                    Moves.Add(MoveClass.GetMove(Move.Slash));
                if (Level >= 36)
                {
                    Name = "Dracaufeu";
                    Hp.Base = 78;
                    Attack.Base = 84;
                    Defense.Base = 78;
                    Speed.Base = 100;
                    Special.Base = 85;
                    _currentEvolution = StarterPokemon.Charizard;
                }

                break;
            case StarterPokemon.Charizard:
                break;
            case StarterPokemon.Squirtle:
                if (Level >= 16)
                {
                    Name = "Carabaffe";
                    Hp.Base = 59;
                    Attack.Base = 63;
                    Defense.Base = 80;
                    Speed.Base = 58;
                    Special.Base = 65;
                    _currentEvolution = StarterPokemon.Wartortle;
                }

                break;
            case StarterPokemon.Wartortle:
                if (Level >= 24 && !moves.Contains(Move.Bite))
                    Moves.Add(MoveClass.GetMove(Move.Bite));
                if (Level >= 36)
                {
                    Name = "Carabaffe";
                    Hp.Base = 79;
                    Attack.Base = 83;
                    Defense.Base = 100;
                    Speed.Base = 78;
                    Special.Base = 85;
                    _currentEvolution = StarterPokemon.Blastoise;
                }

                break;
            case StarterPokemon.Blastoise:
                if (Level >= 42 && !moves.Contains(Move.SkullBash))
                    Moves.Add(MoveClass.GetMove(Move.SkullBash));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public static Starter GetPokemon(StarterPokemon starter)
    {
        return starter switch
        {
            StarterPokemon.Bulbasaur => new Starter(starter, "Bulbizarre", 45, 49, 49, 45, 65, Type.Grass, Type.Poison,
                new List<Move> { Move.Tackle, Move.Growl }, 3),
            StarterPokemon.Charmander => new Starter(starter, "Salamèche", 39, 52, 43, 65, 50, Type.Fire, Type.Fire,
                new List<Move> { Move.Scratch, Move.Growl }, 3),
            StarterPokemon.Squirtle => new Starter(starter, "Carapuce", 44, 48, 65, 43, 50, Type.Water, Type.Water,
                new List<Move> { Move.Tackle, Move.TailWhip }, 3),
            _ => throw new ArgumentOutOfRangeException(nameof(starter), starter, null)
        };
    }
}