using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRazorPages.Models;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreRazorPages.Pages.Orders
{
    public class DisplayModel : PageModel
    {
        private readonly IOrderData _orderData;
        private readonly IFoodData _foodData;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public OrderUpdateModel OrderUpdateModel { get; set; }

        public OrderModel Order { get; set; }
        public string ItemPurchased { get; set; }

        public DisplayModel(IOrderData orderData, IFoodData foodData)
        {
            _orderData = orderData;
            _foodData = foodData;
        }
        public async Task<IActionResult> OnGet()
        {
            Order = await _orderData.GetOrderById(Id);

            if (Order != null)
            {
                //shortcut instead of getting by individual Id
                var food = await _foodData.GetFood();

                ItemPurchased = food.Where(x => x.Id == Order.FoodId).FirstOrDefault()?.Title;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
                return Page();

            await _orderData.UpdateOrderName(OrderUpdateModel.Id, OrderUpdateModel.OrderName);

            return RedirectToPage("./Display", new { OrderUpdateModel.Id });
        }
    }
}
