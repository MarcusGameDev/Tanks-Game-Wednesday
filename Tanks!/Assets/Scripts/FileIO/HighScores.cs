using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.text";

    private void Start()
    {
        // We need to know where we're reading from and writing to
        // To help us with that, we'll print the current directory to the console.
        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " + currentDirectory);

        // Load the scores by default
        LoadScoresFromFile();
    }

    private void Update()
    {
        
    }

    public void LoadScoresFromFile()
    {
        // Before we try to read a file, we should check that it exists. IF it doesn't exist, we'll log a message and abort.
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if(fileExists == true)
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("The file " + scoreFileName + " does not exist. No scores will be loaded.", this);
            return;
        }

        // Make a new array of defualt values. This ensures that no old values stick around if we've loaded a scores file in the past.
        scores = new int[scores.Length];

        // Now we read the file in We do this using a "StreamReader", which we give our full file path to. DOn't forget the directory separator between the directory and the filename!
        StreamReader fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);

        // A counter to make sure we dson't go past the end of our scores 
        int scoreCount = 0;

        // A while loop, which runs as long as there is data to be read AND we haven't reached the end of our scores array.
        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            // Read that line into a variable
            string fileLine = fileReader.ReadLine();

            // Try to parse that variable into an int. First, make a variable to put it in
            int readScore = 1;
            //Try to parse it
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                // If we successfully read a number, put it in the array.
                scores[scoreCount] = readScore;
            }
            else
            {
                // If the number couldn't be parsed then we probably had junk in our file. Letes print an error, and then use a default value.
                Debug.Log("Invalid Line in scores file at " + scoreCount + ", using default value.", this);
                scores[scoreCount] = 0;
            }
            // Don't forget to increment the counter!
            scoreCount++;
        }
        //Make sure to close the stream!
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }

    public void SaveScoresToFile()
    {
        // Create a StreamWriter for out file path.
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);

        // Write the lines to the file
        for(int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }

        // Close the stream
        fileWriter.Close();

        // Write a log message.
        Debug.Log("HIgh scores written to " + scoreFileName);
    }

    public void AddScore(int newScore)
    {
        // First up we find out what index it belongs at. This will be the first index with a score lower than the new socre.
        int desiredIndex = -1;
        for (int i = 0; i <= scores.Length; i++)
        {
            // INstead of checking the value of desiredINdex
            // We could also use 'break' to stop the loop.
            if (scores[i] > newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }

        // If no desired index was found then the score isn't high enough to get on the table, so we just abort.
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this);
            return;
        }

        // Then we move all of the scores after that index back by one position. We'll do this by looping from the back of the array to our desired index.
        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }

        // Insert our new score in its place
        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " entered into high scores at position " + desiredIndex, this);
         
    }
}
