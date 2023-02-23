using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]

        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -------------------- INDEX -------------------- //

        public IActionResult Index()
        {
            return View();
        }

        // -------------------- DETAILS -------------------- //

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product")
            };

            return View(OrderVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                // Retrieve a list of all products and includes category and covertype
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                orderHeaders = _unitOfWork.OrderHeader.GetAll(
                    u=>u.ApplicationUserId==claim.Value, includeProperties: "ApplicationUser");
            }
            

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u=>u.PaymentStaus==SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStaus == SD.StatusInProcess);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStaus == SD.StatusApproved);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStaus == SD.StatusShipped);
                    break;
                default:
                    break;
            }

            // Return a Json containting the list
            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}