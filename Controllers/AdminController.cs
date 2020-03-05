using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndyBooks.Models;
using IndyBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndyBooks.Controllers
{
    public class AdminController : Controller
    {
        private IndyBooksDataContext _db;
        public AdminController(IndyBooksDataContext db) { _db = db; }

        [HttpGet]
        public IActionResult CreateBook()
        {
            var bookVM = new AddBookViewModel();
            bookVM.Writers = _db.Writers;
            //TODO: Populate a new AddBookViewModel object with a complete set of Writers
            //      and send it on to the View "AddBook"
         
            return View("AddBook", bookVM);
        }

        [HttpPost]
        public IActionResult CreateBook(AddBookViewModel bookVM)
        {
            //TODO: Build the Writer object using the parameter
            Writer author = new Writer(); 
            if (bookVM.AuthorId <= 0)
            {
                
                    author.Name= bookVM.Name;
            }
            else
            {

            }
            

            //TODO: Build the Book using the parameter data and your newly created author.
            Book book; 

            //TODO: Add author and book to their DbSets; SaveChanges
           

            //Shows the book using the Index View 
            return RedirectToAction("Index", new { id = bookVM.Id });
        }

        [HttpGet]
        public IActionResult Index(long id)
        {
            IQueryable<Book> books = _db.Books.Include(b => b.Author);//.Where(b => b.Id == id).OrderBy(b => b.SKU);*/
            if(id > 0)
            {
                return View("SearchResults", books.Where(b => b.Id == id));
            }
            else
            {
                                return View("SearchResults", books.OrderBy(b => b.SKU));
            }
            //TODO: filter books by the id (if passed an id as its Route Parameter),
            //     otherwise use the entire collection of Books, ordered by SKU.



        }

         //TODO: Write a method to take a book id, and load book and author info
         //      into the ViewModel for the AddBook View
         [HttpGet]


        [HttpGet]
        public IActionResult DeleteBook(long id)
        {
            //TODO: Remove the Book associated with the given id number; Save Changes


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search() { return View(); }
        [HttpPost]
        public IActionResult Search(SearchViewModel search)
        {

            IQueryable<Book> foundBooks = _db.Books; 


            if (search.Title != null)
            {
                foundBooks = foundBooks
                            .Where(b => b.Title.Contains(search.Title))
                            .OrderBy(b => b.Author.Name)
                            ;
            }


            if (search.AuthorName != null)
            {

                foundBooks = foundBooks
                            .Include(b => b.Author)
                            .Where(b => b.Author.Name.Contains(search.AuthorName, StringComparison.CurrentCulture))
                            ;
            }

            if (search.MinPrice > 0 && search.MaxPrice > 0)
            {
                foundBooks = foundBooks
                            .Where(b => b.Price >= search.MinPrice && b.Price <= search.MaxPrice)
                            .OrderByDescending(b=>b.Price)
                            ;
            }

            if (search.MinPrice == 0 && search.MaxPrice > 0)
            {
                decimal max = _db.Books.Max(b => b.Price);
                foundBooks = foundBooks
                            .Where(b => b.Price == max)
                            ;
            }

            return View("SearchResults", foundBooks);
        }

    }
}
