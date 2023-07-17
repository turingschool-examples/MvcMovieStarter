using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MvcMovie.DataAccess;
using MvcMovie.Models;
using NuGet.Frameworks;

namespace MvcMovie.FeatureTests
{
    public class MovieControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MovieControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private MvcMovieContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MvcMovieContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new MvcMovieContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewWithMovies()
        {
            //Arrange
            //Set up the client
            var client = _factory.CreateClient();
            //Get Spaceballs and Young Frankenstein in the DB
            var context = GetDbContext();
            context.Movies.Add(new Movie { Genre = "Comedy", Title = "Spaceballs" });
            context.Movies.Add(new Movie { Genre = "Comedy", Title = "Young Frankenstein" });
            context.SaveChanges();

            //Act
            // Make a get request to /movies and save the raw response
            var response = await client.GetAsync("/movies");
            //Turn the raw response into a string
            var html = await response.Content.ReadAsStringAsync();

            //Assert
            //Assert that the status is success
            response.EnsureSuccessStatusCode();
            //Assert that the page contains spaceballs
            Assert.Contains("Spaceballs", html);
            //Assert that the page contains young frankenstein
            Assert.Contains("Young Frankenstein", html);
            //Assert that Elf is not in the page
            Assert.DoesNotContain("Elf", html);
        }
    }
}
