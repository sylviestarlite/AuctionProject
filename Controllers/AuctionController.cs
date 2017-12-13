using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CBT.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CBT.Controllers
{
    public class AuctionController : Controller
    {
        private Context _context;
        private User ActiveUser 
        {
            get{ return _context.Users.Where(u => u.UserId == HttpContext.Session.GetInt32("id")).FirstOrDefault();}
        }
        public AuctionController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if(ActiveUser == null)
                return RedirectToAction("Index","Home");
            ViewBag.User = ActiveUser;
            Dashboard dashData = new Dashboard
            {
                Auctions = _context.Auctions.OrderBy(a => a.Date).ToList(),
                User = ActiveUser
            };
            List<Auction> Auctions = _context.Auctions.Include(a => a.Bids).ToList();
            List<Auction> Expired = new List<Auction>();
            foreach(var a in Auctions)
            {
                if(a.Date.Subtract(DateTime.Now).Days < 0)
                {
                    Expired.Add(a);
                }
            }
            foreach(var a in Expired)
            {
                if(a.Bids.Count == 0 || a.Paid == true)
                {
                    continue;
                }
                else
                {
                    User Bidder = _context.Users.SingleOrDefault(u => u.UserId == a.Bids[a.Bids.Count-1].UserId);
                    Bidder.Wallet -= a.Bid;

                    User Seller = _context.Users.SingleOrDefault(u => u.UserId == a.UserId);
                    Seller.Wallet += a.Bid;

                    a.Paid = true;

                    _context.SaveChanges();
                }
            }
        return View(dashData);
        }

        public IActionResult Create()
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ViewAuction model)
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "Home");
            if(ModelState.IsValid)
            {
                Auction auction = new Auction
                {
                    Name = model.Name,
                    Description = model.Description,
                    Date = model.Date,
                    Bid = model.StartingBid,
                    Seller = ActiveUser.FirstName,
                    Paid = false,
                    UserId = ActiveUser.UserId
                };
                _context.Auctions.Add(auction);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        public IActionResult Bid(int id, int amount)
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "Home");
            Auction RetrievedAuction = _context.Auctions.SingleOrDefault(a => a.AuctionId == id);
            if(amount < RetrievedAuction.Bid)
            {
                ViewBag.Error = "Bid must be higher than current highest bid!";
                return View("Show",_context.Auctions.SingleOrDefault(a => a.AuctionId == id));
            }
            if(amount > ActiveUser.Wallet)
            {
                ViewBag.Error = "You don't have enough money for that bid!";
                return View("Show",_context.Auctions.SingleOrDefault(a => a.AuctionId == id));
            }
            Bid bid = new Bid
            {
                UserId = ActiveUser.UserId,
                AuctionId = id
            };
            _context.Bids.Add(bid);
            RetrievedAuction.Bid = amount;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Show(int id)
        {
            if(HttpContext.Session.GetInt32("id") == null)
               return RedirectToAction("Index");
            return View("Show",_context.Auctions.SingleOrDefault(a => a.AuctionId == id));
        }

        public IActionResult Delete(int id)
        {
            if(ActiveUser == null)
                return RedirectToAction("Index", "Home");
            Auction toDelete = _context.Auctions.SingleOrDefault(a => a.AuctionId == id);
            if (_context.Auctions.Where(a => a.AuctionId == id).SingleOrDefault().UserId == ActiveUser.UserId)
            {
                _context.Auctions.Remove(toDelete);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}