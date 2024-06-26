﻿using Admin_panel.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Admin_panel.Controllers
{
    public class WebController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly Applicationdbcontext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WebController(Applicationdbcontext dbcontext, IWebHostEnvironment webHostEnvironment, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);

            return View(profile);
        }
        [HttpPost]
        [ActionName("Profile")]
        public async Task<IActionResult> Profile2(ApplicationUser user)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);
            profile.first_name = user.first_name;
            profile.last_name = user.last_name;
            profile.UserName = user.first_name;
            profile.PhoneNumber = user.PhoneNumber;
            profile.City = user.City;
            profile.Country = user.Country;
            profile.Town = user.Town;
            TempData["msg2"] = "Profile Updated..!!";


            _dbcontext.ApplicationUsers.Update(profile);
            await _dbcontext.SaveChangesAsync();


            return RedirectToAction("Profile", "Web");
        }
        public async Task<IActionResult> ChangeEmail()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ApplicationUser user)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);
            if (user.Email == null)
            {
                TempData["msg"] = "fill both fields ..!";
                return RedirectToAction("ChangeEmail", "Web");
            }
            else if (profile.Email != user.Email)
            {
                TempData["msg"] = "Email is not correct ..!";
                return RedirectToAction("ChangeEmail", "Web");
            }
            var check = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.new_email);
            if (check != null)
            {
                TempData["msg"] = "This Email is already exist.";
                return RedirectToAction("ChangeEmail", "Web");
            }
            else
            {
                profile.NormalizedUserName = user.new_email.ToUpper();
                profile.Email = user.new_email;
                profile.NormalizedEmail = user.new_email.ToUpper();

                _dbcontext.ApplicationUsers.Update(profile);
                var result = await _dbcontext.SaveChangesAsync();


                TempData["msg"] = "Your Email is changed now.";
                await _signInManager.SignOutAsync();

                return LocalRedirect("/Identity/Account/Login2");
            }


        }
        [HttpGet]
        public async Task<IActionResult> Shop(int id, int page,string name)
        {
            
           
            if (page == 0)
            {
                page = 1;
                
            }
            ViewBag.pages = page;
            var plist1 = _dbcontext.Products.Count();
            var plist2 = _dbcontext.Products.Where(a => a.p_category == id).Count();
            var result_per_page = 5;
           
           
            var query = _dbcontext.Products.Include(x => x.p_cat).Include(s => s.p_spmart).Where(s => s.p_name.Contains(name) || name == null).AsNoTracking().OrderBy(s => s.p_name);
            var query2 = _dbcontext.Products.Include(x => x.p_cat).Include(s => s.p_spmart).Where(a => a.p_category == id).Where(s=>s.p_name.Contains(name) ||name==null).AsNoTracking().OrderBy(s => s.p_name);
            if (id == 0)
            {
                ViewBag.total_pages1 = (int)Math.Ceiling(plist1 / (double)result_per_page);
                
                Product pr = new Product()
                {
                    products = await PagingList.CreateAsync(query, result_per_page, page),
                    categories = await _dbcontext.Categories.ToListAsync(),
                    p_category = id
                    
                
                };
                foreach (var item in pr.categories)
                {
                    item.p_count = _dbcontext.Products.Where(x => x.p_category == item.cat_id).Count();
                }
                return View(pr);
            }
            else
            {
                ViewBag.total_pages1 = (int)Math.Ceiling(plist2 / (double)result_per_page);

                Product pr = new Product()
                {
                    products = await PagingList.CreateAsync(query2, result_per_page, page),
                    categories = await _dbcontext.Categories.ToListAsync(),
                    p_category = id


                };
                foreach (var item in pr.categories)
                {
                    item.p_count = _dbcontext.Products.Where(x => x.p_category == item.cat_id).Count();
                }
                return View(pr);
            }




        }
        public Product product { get; set; }
        public Review ProductReview { get; set; }
        public async Task<IActionResult> PDetail(int id)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null)
            {
                product = new Product()
                {
                    p_product = await _dbcontext.Products.Include(x => x.p_cat).Include(s => s.p_spmart).FirstOrDefaultAsync(a => a.p_id == id),
                    ProductReview = new()
                };
                product.ProductReview.Rlist = await _dbcontext.PReviews.OrderByDescending(s => s.r_date).Include(x => x.user).Where(y => y.prd_id == id).ToListAsync();

                return View(product);
            }
            else
            {
                product = new Product()
                {
                    p_product = await _dbcontext.Products.Include(x => x.p_cat).Include(s => s.p_spmart).FirstOrDefaultAsync(a => a.p_id == id),
                    p_cart = await _dbcontext.CartProducts.Where(x => x.User.Id == claims.Value).FirstOrDefaultAsync(s => s.Productid.p_id == id),
                    ProductReview = new()
                };
                product.ProductReview.Rlist = await _dbcontext.PReviews.OrderByDescending(s=>s.r_date).Include(x => x.user).Where(y => y.prd_id == id).ToListAsync();

                return View(product);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PReview(Product pr, int id)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null)
            {
                TempData["msg"] = "Please Login to post a review.";
                return LocalRedirect("~/Web/PDetail/" + id);
            }
            else
            {
                Review prw = new()
                {
                    pr_msg = pr.ProductReview.pr_msg,
                    user_id = claims.Value,
                    prd_id = id,
                    r_date=DateTime.Now
                };



                await _dbcontext.PReviews.AddAsync(prw);
                await _dbcontext.SaveChangesAsync();
                TempData["msg2"] = "Thanks for your review :)";
                return LocalRedirect("~/Web/PDetail/" + id);

            }

        }
        public async Task<IActionResult> Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact2(Contact con)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claims != null)
            {
                var user = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);
                con.c_name = user.first_name;
                con.c_email = user.Email;
                await _dbcontext.AddAsync(con);
                await _dbcontext.SaveChangesAsync();
                TempData["msg2"] = "Thank You. Your Message has been sent to our team.";
                return RedirectToAction("Contact", "Web");
            }
            else
            {
                await _dbcontext.AddAsync(con);
                await _dbcontext.SaveChangesAsync();
                TempData["msg2"] = "Thank You. Your Message has been sent to our team.";
                return RedirectToAction("Contact", "Web");
            }
            
        }
    }
}
