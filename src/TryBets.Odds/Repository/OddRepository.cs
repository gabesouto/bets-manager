using TryBets.Odds.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;
using System.Data;

namespace TryBets.Odds.Repository;

public class OddRepository : IOddRepository
{
    protected readonly ITryBetsContext _context;
    public OddRepository(ITryBetsContext context)
    {
        _context = context;
    }

    public Match Patch(int MatchId, int TeamId, string BetValue)
    {
        var match = _context.Matches.Find(MatchId);
        if (match == null) throw new Exception("Match not found");

        if (TeamId != match.MatchTeamAId && TeamId != match.MatchTeamBId) throw new Exception("Team is not in this match");

        string BetValueConverted = BetValue.Replace(',', '.');
        decimal BetValueDecimal = decimal.Parse(BetValueConverted, CultureInfo.InvariantCulture);

        if (TeamId == match.MatchTeamAId)
        {
            match.MatchTeamAValue += BetValueDecimal;
        }
        else
        {
            match.MatchTeamBValue += BetValueDecimal;
        }

        decimal team = (TeamId == match.MatchTeamAId) ? match.MatchTeamAValue : match.MatchTeamBValue;


        _context.Matches.Update(match);
        _context.SaveChanges();

        return match;
    }

}