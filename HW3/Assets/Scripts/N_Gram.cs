using RPS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Represents the frequency data of moves for a specific n-gram sequence
public class KeyDataRecord
{
    public Dictionary<string, int> counts = new Dictionary<string, int>(); // Tracks the frequency of each move (rock, paper, scissors)
    public int total = 0; // Total occurrences for this sequence

    // Constructor initializes counts dictionary with default values for each move type
    public KeyDataRecord()
    {
        counts.Add("s", 0); // Frequency for "scissors"
        counts.Add("r", 0); // Frequency for "rock"
        counts.Add("p", 0); // Frequency for "paper"
        total = 0;
    }
}

// Implements the n-gram model to predict the player's next move based on past sequences
public class N_Gram
{
    public Dictionary<string, KeyDataRecord> data = new Dictionary<string, KeyDataRecord>(); // Holds frequency data for each n-gram sequence
    string currentRecord = ""; // Tracks the most recent sequence of moves
    int nValue = 4; // The length of sequences to track (n-gram size)

    // Registers a new sequence in the n-gram data
    public void register(string sequence)
    {
        char action = sequence[sequence.Length - 1]; // The last move in the sequence
        string key = sequence.Remove(sequence.Length - 1, 1); // The sequence key, excluding the latest move

        // Ensure the key exists in data, and add it if not
        if (!data.ContainsKey(key))
        {
            data[key] = new KeyDataRecord();
        }

        // Increment the count for the specific move (action) in this sequence
        data[key].counts[action.ToString()] += 1;
        data[key].total += 1;
    }

    // Logs the player's input move and updates the current sequence
    public void LogInput(char s)
    {
        // Append the new move to the current sequence, removing the oldest move if necessary
        if (currentRecord.Length < nValue)
        {
            currentRecord += s;
        }
        else
        {
            currentRecord = currentRecord.Remove(0, 1) + s;
        }

        // Only register the sequence when it has reached the required nValue length
        if (currentRecord.Length == nValue)
        {
            register(currentRecord);
        }
    }

    // Predicts the player's most likely next move based on past patterns
    public char GetMostLikly()
    {
        // Return a random move if the current record is empty (i.e., no data to predict)
        if (currentRecord.Length == 0)
            return RockPaperScissors.RandomMove();

        // Use the current sequence excluding the latest move as the key
        string key = currentRecord.Remove(0, 1);
        if (!data.ContainsKey(key))
            return RockPaperScissors.RandomMove();

        // Retrieve the recorded data for this sequence
        KeyDataRecord keyData = data[key];

        char bestMove = RockPaperScissors.RandomMove(); // Default to a random move
        int highestValue = 0;

        // Determine which move has the highest frequency in this sequence
        if (keyData.counts["s"] > highestValue)
        {
            bestMove = 's';
            highestValue = keyData.counts["s"];
        }
        if (keyData.counts["p"] > highestValue)
        {
            bestMove = 'p';
            highestValue = keyData.counts["p"];
        }
        if (keyData.counts["r"] > highestValue)
        {
            bestMove = 'r';
            highestValue = keyData.counts["r"];
        }

        return bestMove; // Return the move with the highest probability
    }
}
