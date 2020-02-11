using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayPoker.Data
{
    public class SCRUMPokerException : Exception
    {
        public SCRUMPokerException(string Message)
            : base(Message)
        {

        }
    }

    public class UserService
    {
        private Participant participant;
        public Participant GetUser() => participant; 

        public Participant CreateUser(string name)
        {
            participant = new Participant()
            {
                Name = name
            };

            return participant;
        }
    }

    public class PokerService
    {
        public event Action OnGameChanged;
        private static Dictionary<string, Game> games = new Dictionary<string, Game>();

        public List<Game> ListGames() => games.Values.ToList();

        public void DeleteGame(Game game)
        {
            games.Remove(game.Subject);
            OnGameChanged?.Invoke();
        }

        public void AddGame(string gameName, Participant creator)
        {
            var game = new Game();
            game.OnGameStatusChanged += () => OnGameChanged?.Invoke();
            game.Subject = gameName;
            game.Facilitator = creator;
            games.Add(game.Subject, game);
            OnGameChanged?.Invoke();
        }

        public Game GetGame(string id)
        {
            return games[id];
        }
    }
}
