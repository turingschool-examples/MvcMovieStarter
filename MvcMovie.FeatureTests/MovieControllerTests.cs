using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MvcMovie.DataAccess;
using MvcMovie.Models;


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
            var context = GetDbContext();
            context.Movies.Add(new Movie { Genre = "Comedy", Title = "Spaceballs" });
            context.Movies.Add(new Movie { Genre = "Comedy", Title = "Young Frankenstein" });
            context.SaveChanges();

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/Movies");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Spaceballs", html);
            Assert.Contains("Young Frankenstein", html);

            // Make sure it does not hit actual database
            Assert.DoesNotContain("Elf", html);

        }

        /*
         As a User
            When I visit '/movies/1/edit'
            Than I see a form to edit the movie
            And I see that the Title and Genre for that movie
                are pre-populated in the form.
         */
        [Fact]
        public async Task Edit_DisplayFormPrePopulated()
        {
            // Arrange
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Movie spaceballs = new Movie { Genre = "Comedy", Title = "Spaceballs" };
            context.Movies.Add(spaceballs);
            context.SaveChanges();

            // Act
            var response = await client.GetAsync($"/Movies/{spaceballs.Id}/edit");
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains(spaceballs.Title, html);
            Assert.Contains(spaceballs.Genre, html);
            Assert.Contains("Edit Movie", html);
            Assert.Contains("form method=\"post\"", html);
            Assert.Contains($"action=\"/movies/{spaceballs.Id}\"", html);
        }

        [Fact]
        public async Task Update_SavesChangesToMovie()
        {
            // Arrange
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Movie movie = new Movie { Title = "Goofy", Genre = "Comedy" };
            context.Movies.Add(movie);
            context.SaveChanges();

            var formData = new Dictionary<string, string>
            {
                { "Title", "Goofy" },
                { "Genre", "Documentary" }
            };

            // Act
            var response = await client.PostAsync($"/movies/{movie.Id}", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("Goofy", html);
            Assert.Contains("Documentary", html);
            Assert.DoesNotContain("Comedy", html);
        }
    }
}