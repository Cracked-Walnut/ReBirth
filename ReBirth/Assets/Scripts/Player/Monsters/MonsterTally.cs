using UnityEngine;

/*
Purpose:
    To handle kill counts related to Monsters.
Last Edited:
    01-12-23.
*/
public class MonsterTally : MonoBehaviour {

    private int _totalMonsterTally,
        _ghoulTally,
        _spitterTally,
        _summonerTally;

    public int GetTotalMonsterTally() { return _totalMonsterTally; }
    public int GetGhoulTally() { return _ghoulTally; }

    public string IncrementMonsterTally(string _monsterTag) {
        if (_monsterTag == "Ghoul") 
            _ghoulTally++;
        else if (_monsterTag == "Spitter")
            _spitterTally++;
        else if (_monsterTag == "Summoner")
            _summonerTally++;
            
        _totalMonsterTally++;

        return "Ghoul: " + _ghoulTally + "\nSpitter: " + _spitterTally + "\nSummoner: " + _summonerTally + "\nTotal: " + _totalMonsterTally;
    }

}
