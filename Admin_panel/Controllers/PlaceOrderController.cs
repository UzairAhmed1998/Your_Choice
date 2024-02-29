using Admin_panel.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System.Security.Claims;

namespace Admin_panel.Controllers
{
    public class PlaceOrderController : Controller
    {
        private readonly Applicationdbcontext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PlaceOrderController(Applicationdbcontext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        public double sumcart(double quantity, double price)
        {
            return price * quantity;
        }
        [BindProperty]
        public SummaryVM SummaryVM { get; set; }
        public Order Order { get; set; }
        public async Task<IActionResult> Checkout()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            SummaryVM = new SummaryVM
            {
                listCart = await _dbcontext.CartProducts.Include(x => x.Productid).Where(x => x.users_id == claims.Value).ToListAsync(),
                Order = new()
            };
            if (SummaryVM.listCart.Count()==0)
            {
                return LocalRedirect("~/Web/Index");
            }
            foreach (var item in SummaryVM.listCart)
            {
                item.cart_price = sumcart(item.Quantity, item.Productid.p_price);
                item.cart_total = item.cart_total + item.cart_price;
            }
            SummaryVM.Order.app_user = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);

            SummaryVM.Order.first_name = SummaryVM.Order.app_user.first_name.ToUpper();
            SummaryVM.Order.last_name = SummaryVM.Order.app_user.last_name.ToUpper();
            SummaryVM.Order.Contact_No = SummaryVM.Order.app_user.PhoneNumber;
            SummaryVM.Order.City = SummaryVM.Order.app_user.City;
            SummaryVM.Order.Town = SummaryVM.Order.app_user.Town;
            SummaryVM.Order.Country = SummaryVM.Order.app_user.Country;
            SummaryVM.Order.st_rate = 150;
            return View(SummaryVM);
        }
        [AutoValidateAntiforgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> OrderPlace()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

            SummaryVM.listCart = await _dbcontext.CartProducts.Include(x => x.Productid).Where(x => x.users_id == claims.Value).ToListAsync();
              
            
            foreach (var item in SummaryVM.listCart)
            {
                item.cart_price = sumcart(item.Quantity, item.Productid.p_price);
                item.cart_total = item.cart_total + item.cart_price;
                if (SummaryVM.Order.online=="online")
                {
                    if (item.cart_total < 140) { 
                    TempData["msg"] = "For Online Order minimum Rs 140/- shopping required.";
                    return LocalRedirect("~/PlaceOrder/Checkout");
                    }
                }
            }
            //SummaryVM.Order.first_name = SummaryVM.Order.first_name;
            //SummaryVM.Order.last_name = SummaryVM.Order.last_name.ToUpper();
            //SummaryVM.Order.Contact_No = SummaryVM.Order.Contact_No;
            //SummaryVM.Order.City = SummaryVM.Order.City;
            //SummaryVM.Order.Town = SummaryVM.Order.Town;
            //SummaryVM.Order.Country = SummaryVM.Order.Country;
            //SummaryVM.Order.Order_total = SummaryVM.Order.Order_total;
            
            SummaryVM.Order.payment_status = StaticDetails.Payment_Status_Pending;
            SummaryVM.Order.Order_status = StaticDetails.Order_Status_Pending;
            SummaryVM.Order.Order_date = System.DateTime.Now;
            SummaryVM.Order.user_id = claims.Value;
            if (SummaryVM.Order.cod == "cod") { 
            SummaryVM.Order.payment_type = SummaryVM.Order.cod;
            }
            else if (SummaryVM.Order.online == "online")
            {
                SummaryVM.Order.payment_type = SummaryVM.Order.online;
            }
            else
            {
                TempData["msg"] = "choose any one payment option.";
                return LocalRedirect("~/PlaceOrder/Checkout");
            }
            await _dbcontext.Orders.AddAsync(SummaryVM.Order);
            await _dbcontext.SaveChangesAsync();

            foreach (var item in SummaryVM.listCart)
            {
                OrderDetail ord = new OrderDetail()
                {
                    order_id = SummaryVM.Order.Order_id,
                    product_id = item.Product_id,
                    Count = item.Quantity,
                    price = item.cart_price

                };
                await _dbcontext.OrderDetails.AddAsync(ord);
                await _dbcontext.SaveChangesAsync();

            }
            if (SummaryVM.Order.cod == "cod")
            {
                return LocalRedirect("~/PlaceOrder/OrderConfirmation/" + SummaryVM.Order.Order_id);
            }
            else
            {
                var domain = "https://localhost:7038/";
                var option = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"PlaceOrder/OrderConfirmation?id={SummaryVM.Order.Order_id}",
                    CancelUrl = domain + $"Web/Index"
                };
                foreach (var item in SummaryVM.listCart)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {

                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Productid.p_price * 100),
                            Currency = "PKR",
                            ProductData = new SessionLineItemPriceDataProductDataOptions

                            {
                                Name = item.Productid.p_name

                            },
                        },
                        Quantity = item.Quantity

                    };

                    option.LineItems.Add(sessionLineItem);
                }
                var service = new SessionService();
                Session session = service.Create(option);
                SummaryVM.Order.Session_id = session.Id;
                SummaryVM.Order.Payment_intentid = session.PaymentIntentId;
                await _dbcontext.SaveChangesAsync();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
        }
        public async Task<IActionResult>OrderConfirmation(int id)
        {
            Order order = await _dbcontext.Orders.FirstOrDefaultAsync(x => x.Order_id == id);
            if (order == null)
            {
              
                return LocalRedirect("~/Web/Index");
            }
            else if (order.payment_type == "cod")
            {
                List<CartProduct> cart = await _dbcontext.CartProducts.Where(u => u.users_id == order.user_id).ToListAsync();
                _dbcontext.CartProducts.RemoveRange(cart);
                await _dbcontext.SaveChangesAsync();
                return View(id);
                
            }
            else { 
            var service = new SessionService();
            Session session = service.Get(order.Session_id);
            if (session.PaymentStatus.ToLower() == "paid")
            {
               
                order.payment_status=StaticDetails.Payment_Status_Success;
                _dbcontext.Orders.Update(order);
                await _dbcontext.SaveChangesAsync();
            }
            List<CartProduct> cart = await _dbcontext.CartProducts.Where(u => u.users_id == order.user_id).ToListAsync();
            _dbcontext.CartProducts.RemoveRange(cart);
            await _dbcontext.SaveChangesAsync();
            return View(id);
            }
        }
    }
}
