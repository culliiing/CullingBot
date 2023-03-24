using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CullingBot
{
    public class Poll
    {
        public static List<Poll> polls = new List<Poll>();

        int id;
        public int Id { get { return id; } }
        public string? Title;
        public string? Description;
        int yesVotes;
        int noVotes;

        public Poll(int id, string? title = null, string? description = null)
        {
            this.id = id;
            this.Title = title;
            this.Description = description;

            this.yesVotes = 0;
            this.noVotes = 0;
        }

        public static Poll GetNewPoll(string? title = null, string? description = null)
        {
            var poll = new Poll(polls.Count + 1, title, description);

            polls.Add(poll);

            return poll;
        }

        public static Embed GetPollEmbed(int pollId, bool? yes = null)
        {
            var poll = GetPoll(pollId);
            Color embedColor;

            if (yes == true)
                poll.yesVotes += 1;
            else if(yes == false)
                poll.noVotes += 1;

            if (poll.yesVotes > poll.noVotes)
                embedColor = Color.Green;
            else if (poll.yesVotes < poll.noVotes)
                embedColor = Color.Red;
            else
                embedColor = Color.LightGrey;

            return new EmbedBuilder()
                .WithTitle(poll.Title)
                .WithColor(embedColor)
                .AddField("yes", poll.yesVotes, true)
                .AddField("no", poll.noVotes, true)
                .Build();
        }

        public static Poll GetPoll(int pollId)
        {
            foreach (Poll poll in Poll.polls)
                if (poll.Id == pollId)
                    return poll;

            throw new Exception("Requested poll doesn't exist.");
        }
    }
}
