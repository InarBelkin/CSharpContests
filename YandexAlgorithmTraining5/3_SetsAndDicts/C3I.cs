using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Yandex5._3;

public static class C3I
{
    // private static Regex RegMatch = new Regex(@"""([a-zA-Z]+)"" - ""([a-zA-Z]+)"" ([0-9]+):([0-9]+)");
    private static Regex RegMatch = new Regex(@"(""[a-zA-Z ]+"") - (""[a-zA-Z ]+"") ([0-9]+):([0-9]+)");
    private static Regex RegPlayerGoal = new Regex(@"([a-zA-Z ]+) ([0-9]+)'");
    private static Regex GoalsOnMinuteReg = new Regex("Goals on minute [0-9]+ by ([a-zA-Z ]+)");
    private static Regex GoalsOnFirstMinutesReg = new Regex("Goals on first [0-9]+ minutes by ([a-zA-Z ]+)");
    private static Regex GoalsOnLastMinutesReg = new Regex("Goals on last [0-9]+ minutes by ([a-zA-Z ]+)");

    private static Db db = new Db();

    public static void Solution()
    {
        using var file = new StreamReader(@"F:\Projects\Textes\input.txt");
        var requests = new List<string>();

        while (file.ReadLine() is var s && s is not null)
        {
            if (s.StartsWith('"'))
            {
                ReadMatch(s, file);
            }
            else
            {
                requests.Add(s);
                ReadRequest(s.Trim());
            }
        }
    }

    static void ReadMatch(string sBegin, StreamReader reader)
    {
        var regResult = RegMatch.Match(sBegin);

        var command1Name = regResult.Groups[1].Value;
        var command2Name = regResult.Groups[2].Value;

        var command1Goals = int.Parse(regResult.Groups[3].Value);
        var command2Goals = int.Parse(regResult.Groups[4].Value);

        var team1 = db.TryAddTeam(command1Name);
        var team2 = db.TryAddTeam(command2Name);

        var match = db.TryAddMatch(command1Name, command2Name, command1Goals, command2Goals);

        for (int i = 0; i < command1Goals; i++)
        {
            var regResultP = RegPlayerGoal.Match(reader.ReadLine());
            var playerName = regResultP.Groups[1].Value;
            var minute = int.Parse(regResultP.Groups[2].Value);
            db.AddPlayerGoal(match, team1, playerName, minute);
        }

        for (int i = 0; i < command2Goals; i++)
        {
            var regResultP = RegPlayerGoal.Match(reader.ReadLine());
            var playerName = regResultP.Groups[1].Value;
            var minute = int.Parse(regResultP.Groups[2].Value);
            db.AddPlayerGoal(match, team2, playerName, minute);
        }

        db.AddOpen(match);
    }

    static void ReadRequest(string request)
    {
        var words = request.Trim().Split().ToList();
        if (request.StartsWith("Total goals for"))
        {
            var nameOfTeam = request.Substring(16);
            db.Teams.TryGetValue(nameOfTeam, out var team);
            if (team is not null && db.GoalsPerTeam.TryGetValue(team, out var goalsList))
            {
                Console.WriteLine($"{goalsList.Count}");
            }
            else
            {
                Console.WriteLine("0");
            }
        }
        else if (request.StartsWith("Mean goals per game for"))
        {
            var teamName = request.Substring(24);
            var matches = db.MatchesPerTeam[db.Teams[teamName]];
            var goals = db.GoalsPerTeam[db.Teams[teamName]];
            Console.WriteLine(
                ((double)goals.Count / (double)matches.Count).ToString("0.0###############",
                    CultureInfo.InvariantCulture));
        }
        else if (request.StartsWith("Total goals by"))
        {
            var playerName = request.Substring(15);
            if (db.Players.TryGetValue(playerName, out var player))
            {
                Console.WriteLine(db.GoalsPerPlayer[player].Count);
            }
            else
            {
                Console.WriteLine(0);
            }
        }
        else if (request.StartsWith("Mean goals per game by"))
        {
            var playerName = request.Substring(23);
            var player = db.Players[playerName];
            var matches = db.MatchesPerTeam[player.Team];
            var goals = db.GoalsPerPlayer[player];
            Console.WriteLine(
                ((double)goals.Count / (double)matches.Count).ToString("0.0###############",
                    CultureInfo.InvariantCulture));
        }
        else if (request.StartsWith("Goals on minute"))
        {
            var playerName = GoalsOnMinuteReg.Match(request).Groups[1].Value;
            if (db.Players.TryGetValue(playerName, out var player))
            {
                var minute = int.Parse(words[3]);
                Console.WriteLine(db.GoalsOnMinutePerPlayer[player][minute]);
            }
            else
            {
                Console.WriteLine(0);
            }
        }
        else if (request.StartsWith("Goals on first"))
        {
            var playerName = GoalsOnFirstMinutesReg.Match(request).Groups[1].Value;
            if (db.Players.TryGetValue(playerName, out var player))
            {
                var minute = int.Parse(words[3]);
                var result = db.GoalsOnMinutePerPlayer[player][1..(minute + 1)].Sum();
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(0);
            }
        }
        else if (request.StartsWith("Goals on last"))
        {
            var playerName = GoalsOnLastMinutesReg.Match(request).Groups[1].Value;
            if (db.Players.TryGetValue(playerName, out var player))
            {
                var minute = int.Parse(words[3]);
                var result = db.GoalsOnMinutePerPlayer[player][(91 - minute)..].Sum();
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine(0);
            }
        }
        else if (request.StartsWith("Score opens by"))
        {
            var hsName = request.Substring(15);
            if (db.Teams.TryGetValue(hsName, out var team))
            {
                if (db.TeamOpensCount.TryGetValue(team, out var cnt))
                {
                    Console.WriteLine($"{cnt}");
                }
                else
                {
                    Console.WriteLine(0);
                }
            }
            else if (db.Players.TryGetValue(hsName, out var player))
            {
                if (db.PlayersOpensCount.TryGetValue(player, out var cnt))
                {
                    Console.WriteLine($"{cnt}");
                }
                else
                {
                    Console.WriteLine(0);
                }
            }
            else
            {
                Console.WriteLine(0);
            }
        }
    }
}

public class Db
{
    public readonly Dictionary<string, Team> Teams = new Dictionary<string, Team>();
    public readonly Dictionary<string, Player> Players = new Dictionary<string, Player>();

    public readonly List<Match> Matches = new List<Match>();
    public readonly List<Goal> Goals = new List<Goal>();

    public readonly Dictionary<Team, List<Goal>> GoalsPerTeam = new Dictionary<Team, List<Goal>>();
    public readonly Dictionary<Team, List<Match>> MatchesPerTeam = new Dictionary<Team, List<Match>>();
    public readonly Dictionary<Player, List<Goal>> GoalsPerPlayer = new Dictionary<Player, List<Goal>>();
    public readonly Dictionary<Player, int[]> GoalsOnMinutePerPlayer = new Dictionary<Player, int[]>();
    public readonly Dictionary<Match, List<Goal>> GoalsPerMatch = new Dictionary<Match, List<Goal>>();
    public readonly Dictionary<Team, int> TeamOpensCount = new Dictionary<Team, int>();
    public readonly Dictionary<Player, int> PlayersOpensCount = new Dictionary<Player, int>();

    public Team TryAddTeam(string teamName)
    {
        this.Teams.TryGetValue(teamName, out var team);
        if (team is null)
        {
            team = new Team(teamName);
            this.Teams[teamName] = team;
        }

        return team;
    }

    public Player TryAddPlayer(string playerName, Team team)
    {
        this.Players.TryGetValue(playerName, out var player);
        if (player is null)
        {
            player = new Player(playerName, team);
            this.Players.Add(playerName, player);
        }

        return player;
    }

    public Match TryAddMatch(string sideA, string sideB, int goalsA, int goalsB)
    {
        var match = new Match(Teams[sideA], Teams[sideB], goalsA, goalsB);
        Matches.Add(match);
        this.MatchesPerTeam.TryAdd(Teams[sideA], new List<Match>());
        this.MatchesPerTeam[Teams[sideA]].Add(match);
        this.MatchesPerTeam.TryAdd(Teams[sideB], new List<Match>());
        this.MatchesPerTeam[Teams[sideB]].Add(match);
        return match;
    }

    public Goal AddGoal(Match Match, Player Author, int Minute)
    {
        var goal = new Goal(Match, Author, Minute);
        this.Goals.Add(goal);

        this.GoalsPerTeam.TryAdd(Author.Team, new List<Goal>());
        this.GoalsPerTeam[Author.Team].Add(goal);

        this.GoalsPerPlayer.TryAdd(Author, new List<Goal>());
        this.GoalsPerPlayer[Author].Add(goal);

        if (!this.GoalsOnMinutePerPlayer.ContainsKey(Author))
        {
            this.GoalsOnMinutePerPlayer[Author] = new int[91];
        }

        this.GoalsOnMinutePerPlayer[Author][Minute]++;

        this.GoalsPerMatch.TryAdd(Match, new List<Goal>());
        this.GoalsPerMatch[Match].Add(goal);

        return goal;
    }

    public void AddPlayerGoal(Match match, Team team, string PlayerName, int Minute)
    {
        var player = this.TryAddPlayer(PlayerName, team);
        AddGoal(match, player, Minute);
    }

    public void AddOpen(Match match)
    {
        if (this.GoalsPerMatch.TryGetValue(match, out var goals) && goals.Count > 0)
        {
            var firstGoal = this.GoalsPerMatch[match].OrderBy(g => g.Minute).First();
            var firstPlayer = firstGoal.Author;
            this.PlayersOpensCount.TryAdd(firstPlayer, 0);
            this.PlayersOpensCount[firstPlayer]++;
            this.TeamOpensCount.TryAdd(firstPlayer.Team, 0);
            this.TeamOpensCount[firstPlayer.Team]++;
        }
    }
}

public record Team(string Name);

public record Player(string Name, Team Team);

public class Match
{
    public Match(Team SideA, Team SideB, int GoalsA, int GoalsB)
    {
        this.SideA = SideA;
        this.SideB = SideB;
        this.GoalsA = GoalsA;
        this.GoalsB = GoalsB;
    }

    public Team SideA { get; init; }
    public Team SideB { get; init; }
    public int GoalsA { get; init; }
    public int GoalsB { get; init; }
}

public record Goal(Match Match, Player Author, int Minute);