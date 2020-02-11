using System;
using System.Collections.Generic;
using System.Text;


namespace PlayPoker.Data
{
    public class Game
    {
        public event Action OnGameChanged;
        public event Action OnGameStatusChanged;
        private List<Participant> _participants = new List<Participant>();
        private List<PokerVote> _votes = new List<PokerVote>();
        private bool _finishedVoting;
        private Participant _facilitator = null;

        public Participant Facilitator {
            get { return _facilitator; }
            set { _facilitator = value; }
        }

        public bool FinishedVoting
        {
            get { return _finishedVoting; }
            set { _finishedVoting = value; }
        }

        public List<PokerVote> Votes {
            get { return _votes; }
        }
        private string _subject;

        public List<Participant> Participants {
            get
            {
                return _participants;
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public void AddParticipant( Participant participant )
        {
            if (FinishedVoting)
            {
                throw new SCRUMPokerException("Game is finished, no new participants are allowed");
            }
            if (Participants.Count == 0)
            {
                Participants.Add(participant);
                Facilitator = participant;
                return;
            }
            Participant gameParticipant;
            for (int i = 0; i < Participants.Count; i++)
            {
                gameParticipant = Participants[i];
                if (gameParticipant.Name.Equals(participant.Name))
                {
                    Participants[i] = participant;
                    return;
                }
            }
            Participants.Add(participant);
            OnGameChanged?.Invoke();
        }
		public PokerVote GetParticipantVote( Participant participant )
        {
            foreach (PokerVote vote in Votes)
            {
                if (vote.VoteParticipant.Name.Equals(participant.Name))
                {
                    return vote;
                }
            }
            return null;
        }
		private PokerVote GetLowestVote ()
        {
            if (Votes.Count == 0)
            {
                return null;
            }
            PokerVote result = Votes[0];

            foreach (PokerVote vote in Votes)
            {
                if (vote.VoteCard < result.VoteCard)
                {
                    result = vote;
                }
            }
            return result;
        }
		private PokerVote GetHighestVote()
        {
            if (Votes.Count==0)
            {
                return null;
            }
            PokerVote result = Votes[0];

            foreach (PokerVote vote in Votes)
            {
                if (vote.VoteCard > result.VoteCard)
                {
                    result = vote;
                }
            }
            return result;
        }

        public string GameResult()
        {
            PokerVote highest = GetHighestVote();
            PokerVote lowest = GetLowestVote();
            if (lowest==null)
            {
                return "Nobody voted, please restart game.";
            }
            if (lowest.VoteCard == PokerCard.cCoffee)
            {
                return "Somebody needs some coffee. Please restart the game.";
            }
            if ((highest.VoteCard - lowest.VoteCard) <= 1)
            {
                return "<b><font size=\"+3\">"+((int)highest.VoteCard).ToString()+"</font></b> story points";
            }
            else
            {
                StringBuilder SB = new StringBuilder();
                foreach (PokerVote vote in Votes)
                {
                    if (vote.VoteCard.Equals(highest.VoteCard) | (vote.VoteCard.Equals(lowest.VoteCard)))
                    {
                        SB.Append(vote.VoteParticipant.Name + ", ");
                    }                    
                }
                if (SB.Length > 0)
                {
                    return SB.ToString().Substring(0,SB.Length-1)+" please explain your estimates.";
                }

            }
            return "Indeterminate result";
        }

        public bool HasVoted( Participant participant )
        {
            return GetParticipantVote(participant) != null;
        }

        public void SendVote( PokerVote vote )
        {
            if (!HasVoted(vote.VoteParticipant))
            {
                Votes.Add(vote);
                OnGameChanged?.Invoke();
            }
            else
            {
                throw new SCRUMPokerException("Participant already voted");
            }
        }

        public void ToggleRoom()
        {
            FinishedVoting = !FinishedVoting;
            OnGameChanged?.Invoke();
            OnGameStatusChanged?.Invoke();
        }
    }
}
