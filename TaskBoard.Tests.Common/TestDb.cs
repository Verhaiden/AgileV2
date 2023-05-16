using System;

using TaskBoard.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskBoard.Tests.Common
{
    public class TestDb
    {
        private string uniqueDbName;

        public TestDb()
        {
            this.uniqueDbName = "TaskBoard-TestDb-" + DateTime.Now.Ticks;
            this.SeedDatabase();
        }

        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase(this.uniqueDbName);
            return new ApplicationDbContext(optionsBuilder.Options, false);
        }

        public Board OpenBoard { get; private set; }

        public Board InProgressBoard { get; private set; }

        public Board DoneBoard { get; private set; }

        public User GuestUser { get; private set; }

        public Task CSSTask { get; private set; }

        public User UserMaria { get; private set; }

        public Task EditTask { get; private set; }

        private void SeedDatabase()
        {
            var dbContext = this.CreateDbContext();
            var userStore = new UserStore<User>(dbContext);
            var hasher = new PasswordHasher<User>();
            var normalizer = new UpperInvariantLookupNormalizer();
            var userManager = new UserManager<User>(
                userStore, null, hasher, null, null, normalizer, null, null, null);

            // tworzenie tablic
            this.OpenBoard = new Board()
            {
                Id = 1,
                Name = "Nowe zadanie"
            };
            dbContext.Add(this.OpenBoard);

            this.InProgressBoard = new Board()
            {
                Id = 2,
                Name = "W trakcie realizacji"
            };
            dbContext.Add(this.InProgressBoard);

            this.DoneBoard = new Board()
            {
                Id = 3,
                Name = "Gotowe"
            };
            dbContext.Add(this.DoneBoard);

            // tworzenie GuestUser
            this.GuestUser = new User()
            {
                UserName = "gosc",
                NormalizedUserName = "gosc",
                Email = "gosc@mail.com",
                NormalizedEmail = "gosc@mail.com",
                FirstName = "gosc",
                LastName = "gosc",
            };
            userManager.CreateAsync(this.GuestUser, this.GuestUser.UserName).Wait();

            //   GuestUser jest właścicielem CSSTask
            this.CSSTask = new Task()
            {
                Title = "poprawić CSS",
                Description = "Poprawić wyglą strony",
                CreatedOn = DateTime.Now.AddDays(-200),
                OwnerId = this.GuestUser.Id,
                BoardId = this.OpenBoard.Id
            };
            dbContext.Add(this.CSSTask);

            // tworzenie UserMaria
            this.UserMaria = new User()
            {
                UserName = "maria",
                Email = "maria@gmail.com",
                FirstName = "Maria",
                LastName = "Green",
            };
            userManager.CreateAsync(this.UserMaria, this.UserMaria.UserName).Wait();

            //   UserMaria jest właścicielem EditTask
            this.EditTask = new Task()
            {
                Id = 5,
                Title = "poprawić funkcje",
                Description = "dodać edycje zadań",
                CreatedOn = DateTime.Now.AddDays(-20),
                BoardId = this.OpenBoard.Id,
                OwnerId = this.UserMaria.Id
            };
            dbContext.Add(this.EditTask);

            dbContext.SaveChanges();
        }
    }
}
