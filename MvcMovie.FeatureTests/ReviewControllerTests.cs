using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MvcMovie.DataAccess;
using MvcMovie.Models;

namespace MvcMovie.FeatureTests
{

    public class ReviewControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ReviewControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task Index_ReturnsViewWithReviews()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Movie spaceballs = new Movie { Genre = "Comedy", Title = "Spaceballs" };
            Review review1 = new Review { Rating = 5, Content = "Better than Star Wars" };
            Review review2 = new Review { Rating = 4, Content = "Good. But, when will then be now?" };
            spaceballs.Reviews.Add(review1);
            spaceballs.Reviews.Add(review2);
            Movie youngFrankenstein = new Movie { Genre = "Comedy", Title = "Young Frankenstein" };
            Review review3 = new Review { Rating = 3, Content = "Not as good as Spaceballs" };
            context.Movies.Add(spaceballs);
            context.Movies.Add(youngFrankenstein);
            context.SaveChanges();

            var response = await client.GetAsync($"/Movies/{spaceballs.Id}/Reviews");
            //Make sure the route exists!
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var html = await response.Content.ReadAsStringAsync();

            //Make sure the page contains the correct info
            Assert.Contains(spaceballs.Title, html);
            Assert.Contains(review1.Content, html);
            Assert.Contains(review2.Content, html);
            //Make sure the page does not contain info for other movies, or reviews!
            Assert.DoesNotContain(youngFrankenstein.Title, html);
            Assert.DoesNotContain(review3.Content, html);
        }

        [Fact]
        public async Task Index_ReturnsViewWithLinkToNewForm()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Movie spaceballs = new Movie { Genre = "Comedy", Title = "Spaceballs" };
            context.Movies.Add(spaceballs);
            context.SaveChanges();

            var response = await client.GetAsync($"/Movies/{spaceballs.Id}/Reviews");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains($"<a href='/movies/{spaceballs.Id}/reviews/new'>", html);
        }

        [Fact]
        public async Task New_ReturnsViewWithForm()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Movie spaceballs = new Movie { Genre = "Comedy", Title = "Spaceballs" };
            context.Movies.Add(spaceballs);
            context.SaveChanges();

            var response = await client.GetAsync($"/Movies/{spaceballs.Id}/Reviews/New");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"<form method='post' action='/movies/{spaceballs.Id}/reviews'", html);
        }
    }

}
