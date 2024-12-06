using System.Text;
using datasett.Context;

namespace datasett;

class Program
{
    static void Main(string[] args)
    {
        var context = new Context.Context();
        // these are all by value copies, not by ref.
        // As this short excercise does not perform permanent mutations, the additonal copies are superflous for this program
        var moviesSelect = context.Movies;
        var moviesSelectMany = context.Movies;
        var moviesGroupBy = context.Movies;

        // filter
        var moviesWithinTimespan =
            from movie in moviesSelect.AsEnumerable()
            where movie.Year is >= 1960 and < 1970
            select movie;

        // flatten
        var everyCreditedCastMember = moviesSelectMany.SelectMany(
            (movie) =>
            {
                return movie.Cast;
            }
        );

        // sub-arrays by group
        var moviesByYear = from movie in moviesGroupBy.AsEnumerable() group movie by movie.Year;

        Context.Context.SaveQueryResult(moviesWithinTimespan, prettyPrint: true);
        Context.Context.SaveQueryResult(everyCreditedCastMember, prettyPrint: true);
        Context.Context.SaveQueryResult(moviesByYear, prettyPrint: true);
    }
}
