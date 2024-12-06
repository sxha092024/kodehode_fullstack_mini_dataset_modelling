#!/bin/env rust-script
//! ```cargo
//! [dependencies]
//! uuid = { version = "1.11.0", features =  [ "v7" ] }
//! serde_json = "1.0.133"
//! serde = { version = "1.0.215", features = [ "derive" ] }
//! ```
use serde::{Deserialize, Serialize};
use uuid::Uuid;

use std::fs::File;
use std::io::{BufReader, BufWriter};

#[derive(Deserialize, Debug, Clone)]
struct Movie {
    title: String,
    year: i32,
    cast: Vec<String>,
    genres: Vec<String>,
    href: Option<String>,
    extract: Option<String>,
    thumbnail: Option<String>,
    thumbnail_width: Option<i32>,
    thumbnail_height: Option<i32>,
}

#[derive(Serialize, Debug, Clone)]
struct ProcessedMovie {
    id: String,
    title: String,
    year: i32,
    cast: Vec<String>,
    genres: Vec<String>,
    href: Option<String>,
    extract: Option<String>,
    thumbnail: Option<String>,
    thumbnail_width: Option<i32>,
    thumbnail_height: Option<i32>,
}

impl From<Movie> for ProcessedMovie {
    fn from(movie: Movie) -> Self {
        Self {
            id: format!("{}", Uuid::now_v7()),
            title: movie.title,
            year: movie.year,
            cast: movie.cast,
            genres: movie.genres,
            href: movie.href,
            extract: movie.extract,
            thumbnail: movie.thumbnail,
            thumbnail_width: movie.thumbnail_width,
            thumbnail_height: movie.thumbnail_height,
        }
    }
}

fn main() {
    let json_dataset = File::open("./movies.json").expect("Could not open `movies.json`");
    let reader = BufReader::new(json_dataset);
    let unprocessed_dataset: Vec<Movie> =
        serde_json::from_reader(reader).expect("Could not parse JSON data");
    println!("unprocessed movies: {}", unprocessed_dataset.len());
    let mut normalised_dataset = vec![];
    for movie in unprocessed_dataset {
        let normalised = ProcessedMovie::from(movie);
        normalised_dataset.push(normalised);
    }
    println!("processed movies: {}", normalised_dataset.len());
    let normalised_file =
        File::create("./processed.json").expect("Could not create file `processed.json`");
    let writer = BufWriter::new(normalised_file);
    serde_json::to_writer(writer, &mut normalised_dataset)
        .expect("Could not re-serialse normalised data");
}
