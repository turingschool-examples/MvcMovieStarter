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
    }
}