﻿using System;
using System.Collections.Generic;
using System.IO;
using Class_6.Models;
using Microsoft.Extensions.Logging;

namespace Class_6.Services;

/// <summary>
///     This concrete service and method only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public class FileService : IFileService
{
    private ILogger<IFileService> _logger;
    private string _fileName;

    // these should not be here
    public List<int> MovieIds { get; set; }
    public List<string> MovieTitles { get; set; }
    public List<string> MovieGenres { get; set; }

    #region constructors
    // default constructor
    public FileService()
    {

    }

    // constructor
    public FileService(int myInt)
    {
        Console.WriteLine($"constructor value {myInt}");
    }

    public FileService(string myString)
    {
        Console.WriteLine($"constructor value {myString}");

    }
    #endregion

    public FileService(ILogger<IFileService> logger)
    {
        _logger = logger;
        logger.LogInformation("Here is some information");

        _fileName = $"{Environment.CurrentDirectory}/movies.csv";

        MovieIds = new List<int>();
        MovieTitles = new List<string>();
        MovieGenres = new List<string>();
    }

    public void Read()
    {
        _logger.LogInformation("Reading");
        Console.WriteLine("*** I am reading");

        // create parallel lists of movie details
        // lists must be used since we do not know number of lines of data
        //List<UInt64> MovieIds = new List<UInt64>();
        //List<string> MovieTitles = new List<string>();
        //List<string> MovieGenres = new List<string>();
        // to populate the lists with data, read from the data file
        try
        {
            StreamReader sr = new StreamReader(_fileName);
            // first line contains column headers
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                // first look for quote(") in string
                // this indicates a comma(,) in movie title
                int idx = line.IndexOf('"');
                if (idx == -1)
                {
                    // no quote = no comma in movie title
                    // movie details are separated with comma(,)
                    string[] movieDetails = line.Split(',');
                    // 1st array element contains movie id
                    MovieIds.Add(int.Parse(movieDetails[0]));
                    // 2nd array element contains movie title
                    MovieTitles.Add(movieDetails[1]);
                    // 3rd array element contains movie genre(s)
                    // replace "|" with ", "
                    MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                }
                else
                {
                    // quote = comma in movie title
                    // extract the movieId
                    MovieIds.Add(int.Parse(line.Substring(0, idx - 1)));
                    // remove movieId and first quote from string
                    line = line.Substring(idx + 1);
                    // find the next quote
                    idx = line.IndexOf('"');
                    // extract the movieTitle
                    MovieTitles.Add(line.Substring(0, idx));
                    // remove title and last comma from the string
                    line = line.Substring(idx + 2);
                    // replace the "|" with ", "
                    MovieGenres.Add(line.Replace("|", ", "));
                }
            }
            // close file when done
            sr.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        _logger.LogInformation("Movies in file {Count}", MovieIds.Count);
    }

    public void Write(Movie movie)
    {
        Console.WriteLine("*** I am writing");

        StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine($"{movie.Id},{movie.Title},{movie.Genres}");
        sw.Close();

        // add movie details to Lists
        MovieIds.Add(movie.Id);
        MovieTitles.Add(movie.Title);
        MovieGenres.Add(movie.Genres);
        // log transaction
        _logger.LogInformation("Movie id {Id} added", movie.Id);

    }

    public void Display()
    {
        // Display All Movies
        // loop thru Movie Lists
        for (int i = 0; i < MovieIds.Count; i++)
        {
            // display movie details
            Console.WriteLine($"Id: {MovieIds[i]}");
            Console.WriteLine($"Title: {MovieTitles[i]}");
            Console.WriteLine($"Genre(s): {MovieGenres[i]}");
            Console.WriteLine();
        }
    }
}
