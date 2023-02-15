using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -------------------- INDEX -------------------- //

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();

            return View(objCoverTypeList);
        }

        // -------------------- EDIT -------------------- //

        // GET

        public IActionResult Upsert(int? id)
        {
        ProductVM productVM = new()
        {
            Product = new(),

            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),

            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
        };
            
            if (id == null || id == 0)
            {
                // Create product

                // Pass data to the Upsert View w/ Viewbag
                //ViewBag.CategoryList = CategoryList;

                // Pass data to the Upsert View w/ Viewdata
                //ViewData["CoverTypeList"] = CoverTypeList;

                return View(productVM);
            }
            else
            {
                // Update product
            }

            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefaul(u=>u.Id == id); 

            return View(productVM);
        }

        // POST

        [HttpPost] // Method attribute
        [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            // Validation
            if (ModelState.IsValid) 
            {
                // Update object to CoverType
                //_unitOfWork.CoverType.Update(obj);

                // Save on db
                _unitOfWork.Save();

                // Success message
                TempData["success"] = "CoverType updated successfully";

                // Redirect to Index
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        // -------------------- DELETE -------------------- //

        // GET

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefaul(u=>u.Id == id); // return first instance -> if no exists return NULL

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(CoverTypeFromDbFirst);
        }

        // POST

        [HttpPost, ActionName("Delete")] // Method attribute + define action name("CoverType/Delete/id")
    [ValidateAntiForgeryToken] // CSRF (Cross-Site Request Forgery) 

        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefaul(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            // Remove object to Categories
            _unitOfWork.CoverType.Remove(obj);

            // Save on db
            _unitOfWork.Save();

            // Success message
            TempData["success"] = "CoverType deleted successfully";

            // Redirect to Index
            return RedirectToAction("Index");
        }
    }
