﻿using Class_6.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Class_6.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{
    private readonly IFileService _fileService;
    public MainService(IFileService fileService)
    {
        _fileService = fileService;
        // don't want to create this dependency here
        //_fileService = new FileService();
    }

    public void Invoke()
    {
        string choice;
        do
        {
            Console.WriteLine("1) Add Movie");
            Console.WriteLine("2) Display All Movies");
            Console.WriteLine("X) Quit");
            choice = Console.ReadLine();

            // Logic would need to exist to validate inputs and data prior to writing to the file
            // You would need to decide where this logic would reside.
            // Is it part of the FileService or some other service?
            if (choice == "1")
            {
                // Add Movie
                // ask user to input movie title
                Console.WriteLine("Enter the movie title");
                // input title
                string movieTitle = Console.ReadLine();
                // check for duplicate title
                List<string> LowerCaseMovieTitles = _fileService.MovieTitles.ConvertAll(t => t.ToLower());

                // The below code is equivalent to the lambda expression (ConvertAll) above
                //foreach (var mTitle in _fileService.MovieTitles)
                //{
                //    LowerCaseMovieTitles.Add(mTitle.ToLower());
                //}

                if (LowerCaseMovieTitles.Contains(movieTitle.ToLower()))
                {
                    Console.WriteLine("That movie has already been entered");
                    //logger.Info("Duplicate movie title {Title}", movieTitle);
                }
                else
                {
                    // generate movie id - use max value in MovieIds + 1
                    int movieId = _fileService.MovieIds.Max() + 1;
                    // input genres
                    List<string> genres = new List<string>();
                    string genre;
                    do
                    {
                        // ask user to enter genre
                        Console.WriteLine("Enter genre (or done to quit)");
                        // input genre
                        genre = Console.ReadLine();
                        // if user enters "done"
                        // or does not enter a genre do not add it to list
                        if (genre != "done" && genre.Length > 0)
                        {
                            genres.Add(genre);
                        }
                    } while (genre != "done");
                    // specify if no genres are entered
                    if (genres.Count == 0)
                    {
                        genres.Add("(no genres listed)");
                    }
                    // use "|" as delimeter for genres
                    string genresString = string.Join("|", genres);
                    // if there is a comma(,) in the title, wrap it in quotes
                    movieTitle = movieTitle.IndexOf(',') != -1 ? $"\"{movieTitle}\"" : movieTitle;
                    // display movie id, title, genres
                    Console.WriteLine($"{movieId},{movieTitle},{genresString}");

                    // ask the user for the values to write
                    //_fileService.Write(movieId, movieTitle, genresString);

                    var movie = new Movie();
                    movie.Id = movieId;
                    movie.Title = movieTitle;
                    movie.Genres = genresString;

                    _fileService.Write(movie);
                }
            }
            else if (choice == "2")
            {
                _fileService.Read();
                _fileService.Display();
            }
        }
        while (choice != "X");
    }
}
