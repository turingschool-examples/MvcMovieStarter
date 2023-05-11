using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MvcMovie.DataAccess;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MvcMovie.FeatureTests
{
    public class ReviewsControllerTests :IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ReviewsControllerTests(WebApplicationFactory<Program> factory)
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
            Movie youngFrankenstein = new Movie { Genre = "Comedy", Title = "Young Frankenstein" };
            context.Movies.Add(spaceballs);
            context.Movies.Add(youngFrankenstein);
            context.SaveChanges();
            Review review1 = new Review { Rating = 5, Content = "Better than Star Wars" };
            Review review2 = new Review { Rating = 4, Content = "Good. But, when will then be now?" };
            spaceballs.Reviews.Add(review1);
            spaceballs.Reviews.Add(review2);
            context.SaveChanges();

            var response = await client.GetAsync($"/Movies/{spaceballs.Id}/Reviews");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains(spaceballs.Title, html);
            Assert.Contains("5: Better than Star Wars", html);
            Assert.Contains("4: Good. But, when will then be now?", html);
            Assert.DoesNotContain(youngFrankenstein.Title, html);
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

        [Fact]
        public async Task Create_AddsReview_RedirectsToMovieReviewsIndex()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Movie spaceballs = new Movie { Genre = "Comedy", Title = "Spaceballs" };
            context.Movies.Add(spaceballs);
            context.SaveChanges();

            var formData = new Dictionary<string, string>
            {
                { "Rating", "5" },
                { "Content", "Better than Star Wars" }
            };

            var response = await client.PostAsync($"/movies/{spaceballs.Id}/reviews", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"/Movies/{spaceballs.Id}/reviews", response.RequestMessage.RequestUri.ToString());
            Assert.Contains("5: Better than Star Wars", html);
            Assert.DoesNotContain("4: Good. But, when will then be now?", html);
        }
    }
}
