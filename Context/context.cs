using System.Text.Json;
using datasett.Models;

namespace datasett.Context;

public class Context
{
    private protected List<Movie> _movies { get; set; }

    // We don't neccessarily want to perform mutations on our *actual* data,
    // so we do a by value copy with an in place array construction and destructuring arrays into values
    // This indirection means that any mutations done to the list handed out does not affect the internal state
    // However, to support *intentional* mutation we also have a setter accessory whose implementation *does*,
    // overwrite the internal state.
    // The question is, why do we do this? Because C# does not have a strict enough type system to define aliasing references
    // OR the mutation of a reference at a type or field level. See: Rust language's `mut` keyword.
    public List<Movie> Movies
    {
        get { return [.. _movies]; }
        set { _movies = value; }
    }

    public Context()
    {
        _movies = LoadMovies().ToList();
    }

    ~Context()
    {
        WriteMovies(_movies);
    }

    static IEnumerable<Movie> LoadMovies()
    {
        try
        {
            var jsonString = File.ReadAllText("./processed.json");
            var movies = JsonSerializer.Deserialize<List<Movie>>(jsonString);
            if (!(movies is null))
            {
                return movies;
            }
            else
            {
                throw new Exception("Unable to parse movies from document");
            }
        }
        catch (FileNotFoundException exc)
        {
            Console.WriteLine(exc);
            Console.WriteLine(
                "`./processed.json` could not be found, have you executed pre-process.rs?"
            );
            Environment.Exit(-1);
            throw;
        }
        catch (Exception exc)
        {
            // TODO: Swap to a logging framework
            Console.WriteLine(exc);
            throw;
        }
    }

    static void WriteMovies(IEnumerable<Movie> movies, bool prettyPrint = false)
    {
        WriteObjectToUniqueFile("movies.json", movies, prettyPrint);
    }

    public static void SaveQueryResult<T>(IEnumerable<T> query, bool prettyPrint = false)
    {
        WriteObjectToUniqueFile("query.json", query, prettyPrint);
    }

    /// <summary>
    /// Serialise an object as a JSON document to a unique path, prefixed by a UUIDv7-
    /// </summary>
    /// <param name="fileName">Filename, must contain file extension.</param>
    /// <param name="obj">object to serialise to json</param>
    /// <exception cref="NotImplementedException"></exception>
    static void WriteObjectToUniqueFile(string fileName, object obj, bool prettyPrint = false)
    {
        try
        {
            var uuidv7 = Guid.CreateVersion7();
            using (var fs = File.Create($"./{uuidv7}-{fileName}"))
            {
                if (prettyPrint)
                {
                    JsonSerializer.Serialize(
                        fs,
                        obj,
                        new JsonSerializerOptions { WriteIndented = true }
                    );
                }
                else
                {
                    JsonSerializer.Serialize(fs, obj);
                }
                return;
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc);
            throw;
        }
    }
}
