﻿namespace Class_6.Services;

/// <summary>
///     This service interface only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public interface IFileService
{
    void Read();
    void Write(int movieId, string movieTitle, string genresString);
    void Display();
}
