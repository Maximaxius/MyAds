using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyAds.Contexts;
using MyAds.Models;
using MyAds.ViewModels;
using System.Security.Cryptography;

namespace MyAds.Controllers
{
    [Authorize]
    public class AdvertisementController : Controller
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public AdvertisementController(
            ApplicationContext applicationContext,
            UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> AllAdvertisement()
        {
            var users = await _applicationContext.Users.ToListAsync();
            var Ads = await _applicationContext.Ads
                .ToListAsync();
            var Comments = await _applicationContext.Comments.ToListAsync();
            var userAds = Ads
                .OrderByDescending(ad => ad.CreationTime)
                .ToList();

            var list = new List<AddViewModel>();
            foreach (var userAd in userAds)
            {
                var AddViewModel = new AddViewModel
                {
                    CreationTime = userAd.CreationTime,
                    Name = userAd.Name,
                    Description = userAd.Description,
                    Id = userAd.Id,
                    UserId = userAd.UserId,
                    Type=userAd.Type,
                    //Photo=userAd.Photo
                };
                list.Add(AddViewModel);
            }

            var viewModel = new AllAdvertisementViewModel
            {
                User = users,
                Comments = Comments,
                Ads = list
            };

            return View(viewModel);

            DbSet<User> GetUsers()
            {
                return _applicationContext.Users;
            }
        }


        [HttpGet]
        public async Task<IActionResult> UserAdvertisement()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var Ads = await _applicationContext.Ads
                .Where(ad => ad.UserId == user.Id)
                .ToListAsync();
            var userAds = Ads
                .OrderByDescending(ad => ad.CreationTime)
                .ToList();

            var list = new List<AddViewModel>();
            foreach (var userAd in userAds)
            {
                var AddViewModel = new AddViewModel
                {
                    CreationTime = userAd.CreationTime,
                    Name = userAd.Name,
                    Description = userAd.Description,
                    Type = userAd.Type,
                    Id = userAd.Id,
                };
                list.Add(AddViewModel);
            }

            var viewModel = new UserAdvertisementViewModel
            {
                Ads = list
            };

            return View(viewModel);
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var log = new Advertisement
                {
                    UserId = user.Id,
                    Name = model.Name,
                    Description = model.Description,
                    CreationTime = DateTime.Now,
                    Type = model.Type,
                };

                //if (model.Photo != null)
                //{
                //    byte[] imageData = null;
                //    using (var binaryReader = new BinaryReader(model.Photo.OpenReadStream()))
                //    {
                //        imageData = binaryReader.ReadBytes((int)model.Photo.Length);
                //    }

                //    log.Photo = imageData;
                //}

                await _applicationContext.Ads.AddAsync(log);
                await _applicationContext.SaveChangesAsync();

                return Redirect("UserAdvertisement");
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult AddCom()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCom(int id, AddComViewModels model)
        {
            var selectedAd = await _applicationContext.Ads.FirstOrDefaultAsync(ad => ad.Id == id);

            if (selectedAd == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var log = new Comment
                {
                    AdId = selectedAd.Id,
                    Com = model.Com,
                    CreationTime = DateTime.Now,
                };

                await _applicationContext.Comments.AddAsync(log);
                await _applicationContext.SaveChangesAsync();

                return RedirectToAction("AllAdvertisement");
            }

            return View(model);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var Ads = await _applicationContext.Ads.FirstOrDefaultAsync(ad => ad.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (Ads.UserId == user.Id)
            {
                if (Ads != null)
                {
                    _applicationContext.Ads.Remove(Ads);
                    await _applicationContext.SaveChangesAsync();
                }
            }
            return RedirectToAction("UserAdvertisement");
        }




        [HttpGet]
        public async Task<IActionResult> EditAd(int? id)
        {
            Advertisement ads = await _applicationContext.Ads.FirstOrDefaultAsync(ad => ad.Id == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (ads.UserId == user.Id) { 
                if (ads != null)
                {
                    return View(ads);
                }
            }
            return RedirectToAction("UserAdvertisement");
        }

        [HttpPost]
        public async Task<IActionResult> EditAd(Advertisement ads)
        {
            
            _applicationContext.Entry(ads).State = EntityState.Modified;
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("UserAdvertisement");
        }
    }
}
