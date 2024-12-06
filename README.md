# kodehode - fullstack - model and linq

## Notes
This project performs a *tiny* level of normalisation on the dataset. This can be replicated with by *invoking* the script in [normalise.rs](./normalise.rs). An unusual script file as it [requires rust-script](https://rust-script.org/) to function.

This does the following:
- Ensure all fields/keys which can exist, do exist, even if they are `null`ed.
- Assign a monotonically increasing UUIDv7 value in the same order as each entry was initially read from the dataset.

### reasoning
At the time this excercise was started, `.net 9.0` was not a stable release, the inclusion of UUIDv7 in the standard library is a [very recent addition](https://learn.microsoft.com/en-us/dotnet/api/system.guid.createversion7).

## dataset
the dataset was collected from https://raw.githubusercontent.com/prust/wikipedia-movie-data/master/movies.json using `curl https://raw.githubusercontent.com/prust/wikipedia-movie-data/master/movies.json --output movies.json`