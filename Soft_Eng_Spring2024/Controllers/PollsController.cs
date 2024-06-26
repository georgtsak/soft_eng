﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Poll.ToListAsync());
        }

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

            return View(poll);
        }

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
        public async Task<IActionResult> Create([Bind("Id,Title,StartDate,FinishDate,Votes")] Poll poll , string[] pollColumn)
        {
            if (ModelState.IsValid)
            {   
                poll.Votes=ColumnsToVotes(pollColumn);
                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        // GET: Polls/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartDate,FinishDate,Votes")] Poll poll)
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

        // GET: Polls/Delete/5
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

        private string ColumnsToVotes(string[] columns)
        {
            Dictionary<string,int> votesDict=new Dictionary<string, int>();
            foreach (var column in columns) { votesDict.Add(column, 0);}
            string serialized=JsonSerializer.Serialize(votesDict);
            return serialized;
        }

        private Dictionary<string,int> VotesToDict(string Votes)
        {
            Dictionary<string,int> votesDict = JsonSerializer.Deserialize<Dictionary<string,int>>(Votes);
            return votesDict;
        }


    }
}