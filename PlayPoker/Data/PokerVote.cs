namespace PlayPoker.Data
{
	public class PokerVote {
        private Participant _voteParticipant;

        public Participant VoteParticipant {
            get { return _voteParticipant; }
            set { _voteParticipant = value; }
        }
        PokerCard _voteCard;

        public PokerCard VoteCard {
            get { return _voteCard; }
            set { _voteCard = value; }
        }
	}
}
