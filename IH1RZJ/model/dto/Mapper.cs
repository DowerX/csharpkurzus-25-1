using IH1RZJ.Model.DTO.Json;

namespace IH1RZJ.Model.DTO;

public static class Mapper
{
  #region ToDTO
  public static AppearanceJsonDTO ToDTO(this Appearance appearance) => new AppearanceJsonDTO
  {
    ID = appearance.ID,
    MovieID = appearance.MovieID,
    PersonID = appearance.PersonID,
    Role = appearance.Role
  };

  public static MovieJsonDTO ToDTO(this Movie movie) => new MovieJsonDTO
  {
    ID = movie.ID,
    Title = movie.Title,
    Description = movie.Description,
    ReleaseDate = movie.ReleaseDate
  };

  public static PersonJsonDTO ToDTO(this Person person) => new PersonJsonDTO
  {
    ID = person.ID,
    Name = person.Name,
    Birthday = person.Birthday,
    Death = person.Death,
    Bio = person.Bio
  };

  public static ReviewJsonDTO ToDTO(this Review review) => new ReviewJsonDTO
  {
    ID = review.ID,
    UserID = review.UserID,
    MovieID = review.MovieID,
    Score = review.Score
  };
  #endregion

  #region ToDomain
  public static Appearance ToDomain(this AppearanceJsonDTO appearance) => new Appearance
  {
    ID = appearance.ID,
    MovieID = appearance.MovieID,
    PersonID = appearance.PersonID,
    Role = appearance.Role
  };

  public static Movie ToDomain(this MovieJsonDTO movie) => new Movie
  {
    ID = movie.ID,
    Title = movie.Title,
    Description = movie.Description,
    ReleaseDate = movie.ReleaseDate
  };

  public static Person ToDomain(this PersonJsonDTO person) => new Person
  {
    ID = person.ID,
    Name = person.Name,
    Birthday = person.Birthday,
    Death = person.Death,
    Bio = person.Bio
  };

  public static Review ToDomain(this ReviewJsonDTO review) => new Review
  {
    ID = review.ID,
    UserID = review.UserID,
    MovieID = review.MovieID,
    Score = review.Score
  };
  #endregion
}