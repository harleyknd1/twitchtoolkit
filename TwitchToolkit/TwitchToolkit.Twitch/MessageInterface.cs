using RimWorld;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Utilities;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Twitch;

public class MessageInterface : TwitchInterfaceBase
{
	public MessageInterface(Game game)
	{
	}

	public override void ParseMessage(ITwitchMessage twitchMessage)
	{
		if (twitchMessage.Message.ToLower() == "!easteregg")
		{
			switch (twitchMessage.Username.ToLower())
			{
			case "hodlhodl":
				EasterEgg.ExecuteHodlEasterEgg();
				break;
			case "saschahi":
				EasterEgg.ExecuteSaschahiEasterEgg();
				break;
			case "sirrandoo":
				EasterEgg.ExecuteSirRandooEasterEgg();
				break;
			case "nry_chan":
				EasterEgg.ExecuteNryEasterEgg();
                break;
			case "kogayis":
				EasterEgg.ExecuteYiskahEasterEgg();
                break;
			case "labrat616":
                EasterEgg.ExecuteLabratEasterEgg();
                break;

            }
		}
		else if (twitchMessage.Message.ToLower() == "!credz")
        {
            switch (twitchMessage.Username.ToLower())
            {
				case "hodlhodl":
                case "saschahi":
				case "sirrandoo":
				case "nry_chan":
                    string text = "If you're reading this letter it means one of the TTK devs has visited your stream.\r\n Keep up the good stream and remember to have fun!";
                    Find.LetterStack.ReceiveLetter((TaggedString)("TTK Dev visited!"), (TaggedString)(text), LetterDefOf.PositiveEvent);
                    break;
            }
            
        }
		if (!ToolkitCoreSettings.forceWhispers || twitchMessage.WhisperMessage != null)
		{
			if (Helper.ModActive)
			{
				CommandsHandler.CheckCommand(twitchMessage);
			}
			if (VoteHandler.voteActive && int.TryParse(twitchMessage.Message, out var voteId))
			{
				VoteHandler.currentVote.RecordVote(Viewers.GetViewer(twitchMessage.Username).id, voteId - 1);
			}
		}
	}
}
