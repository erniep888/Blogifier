﻿using Core.Data;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Pages.Admin.Settings
{
    public class UsersModel : AdminPageModel
    {
        IUnitOfWork _db;

        [BindProperty]
        public IEnumerable<Author> Authors { get; set; }

        public UsersModel(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGet(int page = 1)
        {
            Author = await _db.Authors.GetItem(a => a.AppUserName == User.Identity.Name);

            if (!Author.IsAdmin)
                return RedirectToPage("../Shared/_Error", new { code = 403 });

            var pager = new Pager(page);
            Authors = await _db.Authors.GetItems(u => u.Created > DateTime.MinValue, pager);

            return Page();
        }
    }
}