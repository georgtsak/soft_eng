using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Soft_Eng_Spring2024.Data;
using Soft_Eng_Spring2024.Models;

namespace Soft_Eng_Spring2024.Controllers
{
    public class PollsController : Controller
    {
        private readonly DataContext _context;

        public PollsController(DataContext context)
        {
            _context = context;
        }

        // GET: Polls
        [Authorize(Policy = "MustBeAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Poll.ToListAsync());
        }

        public void addVotes(List<Poll> polls)
        {
            Random rnd = new Random();
            foreach (Poll poll in polls)
            {
                var v = DeserializeVotes(poll.Votes);
                foreach (var key in v.Keys)
                {
                    v[key] += rnd.Next(20, 50);
                }
                poll.Votes = SerializeVotes(v);
                _context.Update(poll);
            }
            _context.SaveChangesAsync();
        }

        [Authorize(Policy = "MustBeAdmin")]
        // GET: Polls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Poll
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poll == null)
            {
                return NotFound();
            }
            //TempData["pollDict"] = DeserializeVotes(poll.Votes);
            return View(poll);
        }

        public async Task<IActionResult> Details2()
        {
            return View(await _context.Poll.ToListAsync());
        }

        [Authorize(Policy = "AdminOrProfessor")]
        // GET: Polls/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Polls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrProfessor")]
        public async Task<IActionResult> Create([Bind("Id,Title,StartDate,FinishDate,Votes,Voters")] Poll poll , string[] pollColumn)
        {
            if (ModelState.IsValid)
            {   
                poll.Votes=SerializeVotes(pollColumn);
                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        // GET: Polls/Edit/5
        [Authorize(Policy = "AdminOrProfessor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Poll.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        // POST: Polls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrProfessor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartDate,FinishDate,Votes,Voters")] Poll poll)
        {
            if (id != poll.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        [Authorize]
        public async Task<IActionResult> Vote(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Poll.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            TempData["pollDict"] = DeserializeVotes(poll.Votes);
            return View(poll);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(int? id,string vote)
        {
            if (id == null || vote==null)
            {
                return NotFound();
            }
            var poll = await _context.Poll.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    var votesDict = DeserializeVotes(poll.Votes);
                    var uId = Int32.Parse(User.FindFirst("uid").Value);
                    if (poll.Voters.Contains(uId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        votesDict[vote]++;
                        poll.Voters.Add(uId);
                        poll.Votes=SerializeVotes(votesDict);
                        _context.Update(poll);
                        await _context.SaveChangesAsync();
                    }                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        // GET: Polls/Delete/5
        [Authorize(Policy = "AdminOrProfessor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Poll
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // POST: Polls/Delete/5
        [Authorize(Policy = "AdminOrProfessor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poll = await _context.Poll.FindAsync(id);
            if (poll != null)
            {
                _context.Poll.Remove(poll);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PollExists(int id)
        {
            return _context.Poll.Any(e => e.Id == id);
        }

        private string SerializeVotes(string[] columns)
        {
            Dictionary<string,int> votesDict=new Dictionary<string, int>();
            foreach (var column in columns) { votesDict.Add(column, 0);}
            string serialized=JsonSerializer.Serialize(votesDict);
            return serialized;
        }

        private string SerializeVotes(Dictionary<String,int> dict)
        {
            string serialized = JsonSerializer.Serialize(dict);
            return serialized;
        }

        private Dictionary<string,int> DeserializeVotes(string Votes)
        {
            Dictionary<string,int> votesDict = JsonSerializer.Deserialize<Dictionary<string,int>>(Votes);
            return votesDict;
        }


    }
}
