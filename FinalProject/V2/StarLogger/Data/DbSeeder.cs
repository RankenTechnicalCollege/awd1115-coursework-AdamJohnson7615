using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StarLogger.Models;

namespace StarLogger.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider service)
        {
            var context = service.GetService<ApplicationDbContext>();
            var userManager = service.GetService<UserManager<User>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // Database Checker
            context.Database.EnsureCreated();

            // User Roles
            if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("User")) await roleManager.CreateAsync(new IdentityRole("User"));

            // User seed data
            if (await userManager.FindByEmailAsync("admin@starlogger.com") == null)
            {
                var admin = new User
                {
                    UserName = "StarCommander",
                    Email = "admin@starlogger.com",
                    EmailConfirmed = true,
                    ProfilePictureUrl = "https://ui-avatars.com/api/?name=Admin&background=random"
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (await userManager.FindByEmailAsync("user@starlogger.com") == null)
            {
                var user = new User
                {
                    UserName = "StarGamer",
                    Email = "user@starlogger.com",
                    EmailConfirmed = true,
                    ProfilePictureUrl = "https://ui-avatars.com/api/?name=StarGamer&background=0D8ABC&color=fff"
                };
                await userManager.CreateAsync(user, "User123!");
                await userManager.AddToRoleAsync(user, "User");
            }

            var retroUser = await CreateUser(userManager, "RetroKing", "retro@starlogger.com", "User123!", "https://i.ibb.co/nsHZ7CjS/mario.png");
            var cozyUser = await CreateUser(userManager, "CozyCat", "cozy@starlogger.com", "User123!", "https://i.ibb.co/Rp42wm1S/1242683-full.png");
            var speedUser = await CreateUser(userManager, "Speedster", "speed@starlogger.com", "User123!", "https://i.ibb.co/qLcYDD1H/12cdbd466772300ff6f5d16b99e9d240.jpg");


            var cf7 = await userManager.FindByEmailAsync("cartoonfan7@gmail.com");
            if (cf7 == null)
            {
                cf7 = new User
                {
                    UserName = "CartoonFan7",
                    Email = "cartoonfan7@gmail.com",
                    EmailConfirmed = true,
                    ProfilePictureUrl = "https://i.ibb.co/Sw9ZptL7/images.jpg"
                };
                await userManager.CreateAsync(cf7, "SpongeBob!14");
                await userManager.AddToRoleAsync(cf7, "User");
            }

            // Helpers
            async Task AddGameLog(User user, string title, string platform, string igdbId, string coverUrl, CompletionStatus status, int rating = 0)
            {
                // Ensure Game Exists
                var game = await context.Games.FirstOrDefaultAsync(g => g.Title == title);
                if (game == null)
                {
                    game = new Game { Title = title, Platform = platform, IgdbId = igdbId, CoverImageUrl = coverUrl };
                    context.Games.Add(game);
                    await context.SaveChangesAsync();
                }

                // Add to User's Library
                if (!context.UserGames.Any(ug => ug.UserId == user.Id && ug.GameId == game.Id))
                {
                    context.UserGames.Add(new UserGame
                    {
                        UserId = user.Id,
                        GameId = game.Id,
                        Status = status,
                        Rating = rating,
                        DateAdded = DateTime.Now.AddDays(-new Random().Next(1, 100))
                    });
                }
            }

            async Task AddPost(User user, string gameTitle, string content, int likes, List<(User author, string text)> comments)
            {
                var game = await context.Games.FirstOrDefaultAsync(g => g.Title == gameTitle);
                if (game != null)
                {
                    if (!context.Posts.Any(p => p.UserId == user.Id && p.Content == content))
                    {
                        var post = new Post
                        {
                            UserId = user.Id,
                            GameId = game.Id,
                            Content = content,
                            Type = PostType.StatusUpdate,
                            DatePosted = DateTime.Now.AddHours(-new Random().Next(1, 72)),
                            LikeCount = likes
                        };
                        context.Posts.Add(post);
                        await context.SaveChangesAsync();

                        foreach (var c in comments)
                        {
                            context.Comments.Add(new Comment
                            {
                                PostId = post.Id,
                                UserId = c.author.Id,
                                Content = c.text,
                                DatePosted = post.DatePosted.AddMinutes(new Random().Next(5, 120))
                            });
                        }
                    }
                }
            }

            // Some bonus data to make the site look lively when presenting

            await AddGameLog(cf7, "Nickelodeon All-Star Brawl", "PC", "119388", "https://images.igdb.com/igdb/image/upload/t_cover_big/co4upo.jpg", CompletionStatus.CurrentlyPlaying, 7);
            await AddGameLog(cf7, "Nicktoons Unite!", "PS2", "15001", "https://images.igdb.com/igdb/image/upload/t_cover_big/co55ni.jpg", CompletionStatus.CurrentlyPlaying, 0);
            await AddGameLog(cf7, "SpongeBob SquarePants: Battle for Bikini Bottom", "GameCube", "2369", "https://images.igdb.com/igdb/image/upload/t_cover_big/co262n.jpg", CompletionStatus.Finished, 8);
            await AddGameLog(cf7, "Nickelodeon Kart Racers 3: Grand Prix", "Switch", "216885", "https://images.igdb.com/igdb/image/upload/t_cover_big/co5frp.jpg", CompletionStatus.Finished, 6);
            await AddGameLog(cf7, "Epic Mickey", "Wii", "345246", "https://images.igdb.com/igdb/image/upload/t_cover_big/co6omo.jpg", CompletionStatus.Finished, 8);
            await AddGameLog(cf7, "Nickelodeon All-Star Brawl 2", "PS5", "259206", "https://images.igdb.com/igdb/image/upload/t_cover_big/co756t.jpg", CompletionStatus.Finished, 9);
            await AddGameLog(cf7, "SpongeBob SquarePants: The Cosmic Shake", "PS4", "216533", "https://images.igdb.com/igdb/image/upload/t_cover_big/co3ub9.jpg", CompletionStatus.Finished, 0);
            await AddGameLog(cf7, "The Fairly OddParents: Clash with the Anti-World", "GBA", "18001", "https://images.igdb.com/igdb/image/upload/t_cover_big/coa0nb.jpg", CompletionStatus.Finished, 6);
            await AddGameLog(cf7, "Sonic Colors", "Wii", "2268", "https://images.igdb.com/igdb/image/upload/t_cover_big/co4wdr.jpg", CompletionStatus.CurrentlyPlaying, 5);
            await AddGameLog(cf7, "Mario Kart Wii", "Wii", "191437", "https://images.igdb.com/igdb/image/upload/t_cover_big/co214e.jpg", CompletionStatus.Finished, 8);
            await AddGameLog(cf7, "Super Smash Bros. for Nintendo 3DS", "3DS", "4656", "https://images.igdb.com/igdb/image/upload/t_cover_big/co1wvk.jpg", CompletionStatus.Finished, 8);
            await AddGameLog(cf7, "Street Fighter II", "Arcade", "12", "https://images.igdb.com/igdb/image/upload/t_cover_big/co55et.jpg", CompletionStatus.CurrentlyPlaying, 9);
            await AddGameLog(cf7, "Luigi's Mansion: Dark Moon", "3DS", "2476", "https://images.igdb.com/igdb/image/upload/t_cover_big/co3vjj.jpg", CompletionStatus.CurrentlyPlaying, 7);
            await AddGameLog(cf7, "Cartoon Network: Battle Crashers", "PS4", "25795", "https://images.igdb.com/igdb/image/upload/t_cover_big/co22v8.jpg", CompletionStatus.Finished, 1);

            await AddPost(cf7, "Mario Kart Wii", "Finally got all the characters, that took forever!", 12,
                new List<(User, string)> { (speedUser, "Rosalina is the hardest unlock for sure. GG!"), (retroUser, "Are you using the Wii Wheel or Nunchuk?") });

            await AddPost(cf7, "Nickelodeon All-Star Brawl", "Wow, this is like Smash Brothers, but with Nickelodeon? This is kinda cool.", 8,
                new List<(User, string)> { (cozyUser, "Cute game! I wanna play as SpongeBob.. Do they have any characters from FOP?"), (speedUser, "The wavedashing mechanics are actually legit. Hit me up on Discord and we can run some matches, I main Aang.") });

            await AddPost(cf7, "Cartoon Network: Battle Crashers", "I want my money back!", 3,
                new List<(User, string)> { (retroUser, "We warned you... licensed games are a gamble.") });

            await AddPost(cf7, "The Fairly OddParents: Clash with the Anti-World", "Pulled out my Game Boy Advance to revisit this childhood classic!", 15,
                new List<(User, string)> { (retroUser, "GBA pixel art ages like fine wine."), (speedUser, "I forgot this existed!") });


            await AddGameLog(retroUser, "Chrono Trigger", "SNES", "17001", "https://images.igdb.com/igdb/image/upload/t_cover_big/co3plw.jpg", CompletionStatus.Finished, 10);
            await AddGameLog(retroUser, "Castlevania: Symphony of the Night", "PS1", "17002", "https://images.igdb.com/igdb/image/upload/t_cover_big/co53m8.jpg", CompletionStatus.Finished, 10);
            await AddGameLog(retroUser, "Tetris", "GB", "17003", "https://images.igdb.com/igdb/image/upload/t_cover_big/co2ufl.jpg", CompletionStatus.CurrentlyPlaying, 9);

            await AddPost(retroUser, "Chrono Trigger", "Just finished another playthrough. The Magus battle theme is still the best track in gaming history.", 22,
                new List<(User, string)> { (cf7, "I really need to play this. Is the Steam version okay?"), (cozyUser, "Frog is my favorite character! ^-^") });

            await AddPost(retroUser, "Tetris", "Hit 999,999 score on GameBoy today. My thumbs are completely numb.", 45,
                new List<(User, string)> { (speedUser, "Insane reaction times!"), (cf7, "Legend.") });

            await AddGameLog(cozyUser, "Animal Crossing: New Horizons", "Switch", "18001", "https://images.igdb.com/igdb/image/upload/t_cover_big/co3wls.jpg", CompletionStatus.CurrentlyPlaying, 10);
            await AddGameLog(cozyUser, "Stardew Valley", "PC", "11133", "https://images.igdb.com/igdb/image/upload/t_cover_big/coa93h.jpg", CompletionStatus.Finished, 10);
            await AddGameLog(cozyUser, "The Sims 4", "PC", "18002", "https://images.igdb.com/igdb/image/upload/t_cover_big/co3h3l.jpg", CompletionStatus.CurrentlyPlaying, 8);

            await AddPost(cozyUser, "Animal Crossing: New Horizons", "Sherb moved into my island today! He is absolutely precious.", 18,
                new List<(User, string)> { (cf7, "I have Raymond on my island! Apparently he's the most popular villager."), (retroUser, "I still prefer the GameCube version's rude villagers lol.") });

            await AddPost(cozyUser, "Stardew Valley", "Finally married Abigail. We are going to explore the mines together!", 14,
                new List<(User, string)> { (retroUser, "Pixel art romance at its finest.") });

            await AddGameLog(speedUser, "Sonic Frontiers", "PS5", "19001", "https://images.igdb.com/igdb/image/upload/t_cover_big/co5p52.jpg", CompletionStatus.Finished, 8);
            await AddGameLog(speedUser, "Forza Horizon 5", "Xbox", "19002", "https://images.igdb.com/igdb/image/upload/t_cover_big/co3ofx.jpg", CompletionStatus.CurrentlyPlaying, 9);
            await AddGameLog(speedUser, "F-Zero GX", "GameCube", "19003", "https://images.igdb.com/igdb/image/upload/t_cover_big/co525x.jpg", CompletionStatus.Finished, 10);

            await AddPost(speedUser, "Sonic Frontiers", "The sense of speed in the open zones is actually pretty good. Don't listen to the haters.", 9,
                new List<(User, string)> { (cf7, "This game didn't sit right with me."), (retroUser, "Adventure 2 Battle or bust.") });

            await AddPost(speedUser, "F-Zero GX", "Just cleared story mode on Very Hard. I think my controller is broken now.", 30,
                new List<(User, string)> { (retroUser, "That game is notoriously difficult. Respect."), (cf7, "Nintendo needs to bring this series back!") });

            await context.SaveChangesAsync();
        }

        private static async Task<User> CreateUser(UserManager<User> um, string name, string email, string password, string picUrl)
        {
            var user = await um.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = name,
                    Email = email,
                    EmailConfirmed = true,
                    ProfilePictureUrl = picUrl
                };
                await um.CreateAsync(user, password);
                await um.AddToRoleAsync(user, "User");
            }
            return user;
        }
    }
}